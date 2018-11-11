using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;

public class TcpSocket : ISocketTool, IUpdateEvent
{
    const ushort CACHE_SIZE = 65535;
    const int WAIT_OUT_TIME = 5000;

    const int PACKAGE_LENGTH_WIDTH = 2;
    const int PROTOCOL_ID_WIDTH = 4;
    const int MESSAGE_LENGTH_WIDTH = 4;
    const int CUSTOM_WIDTH = 2;

    byte[] _headerPackage = new byte[PACKAGE_LENGTH_WIDTH];
    byte[] _headerAll = new byte[PROTOCOL_ID_WIDTH + MESSAGE_LENGTH_WIDTH + CUSTOM_WIDTH];

    List<byte> _threadSendByteList = new List<byte>();
    List<byte> _sendByteList = new List<byte>();
    List<Message> _sendMessageList = new List<Message>();
    List<Message> _cachedSendMessageList = new List<Message>();     // to use callback when receive the res message

    List<byte> _recvByteList = new List<byte>();
    List<Message> _recvMessageList = new List<Message>();
    byte[] _recvBytes = new byte[CACHE_SIZE];

    Thread _recvThread;
    Thread _sendThread;
    readonly object _lockRecvMessageListObj = new object();
    readonly object _lockSendMessageListObj = new object();
    readonly object _lockCachedSendMessageListObj = new object();

    Socket _socket;

    NotificationData _networkNotification = new NotificationData();

    public TcpSocket()
    {
        WorldManager.Instance.GetUnityEventTool().Add(this);

        _networkNotification.id = Constant.NOTIFICATION_TYPE_NETWORK;
        _networkNotification.mode = NotificationMode.ValueType;
        _networkNotification.state = NotificationStateType.None;
    }

    public void Init(string ip, int port)
    {
        if (_socket != null)
        {
            Close();
            _socket = null;
        }

        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            var address = IPAddress.Parse(ip);
            _socket.BeginConnect(ip, port, new AsyncCallback(delegate (IAsyncResult ar) {
                Socket socket = (Socket)ar.AsyncState;
                try
                {
                    socket.EndConnect(ar);

                    if (_recvThread == null)
                    {
                        _recvThread = new Thread(new ThreadStart(Receive));
                        _recvThread.IsBackground = true;
                    }

                    if (!_recvThread.IsAlive)
                    {
                        _recvThread.Start();
                    }
                }
                catch
                {
                    Close();
                }
            }), _socket);
        }
        catch
        {
            Close();
        }
    }

    public void Destroy()
    {
        Close();
    }

    void Send()
    {
        while (true)
        {
            if (!_socket.Connected)
            {
                Thread.Sleep(2000);
                continue;
            }

            _threadSendByteList.Clear();

            lock (_sendMessageList)
            {
                if (_sendMessageList.Count == 0)
                {
                    Thread.Sleep(200);
                    continue;
                }

                for (var i = 0; i < _sendMessageList.Count;)
                {
                    var message = _sendMessageList[i];
                    if (_threadSendByteList.Count + message.data.Length + PACKAGE_LENGTH_WIDTH <= CACHE_SIZE)
                    {
                        _threadSendByteList.AddRange(message.data);
                        _sendMessageList.Remove(message);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            var buf = _threadSendByteList.ToArray();
            var packageBytes = BitConverter.GetBytes((ushort)buf.Length);
            Array.Reverse(packageBytes);

            _threadSendByteList.Clear();
            _threadSendByteList.AddRange(packageBytes);
            _threadSendByteList.AddRange(buf);

            SocketError error;
            _socket.Send(_threadSendByteList.ToArray(), 0, _threadSendByteList.Count, SocketFlags.None, out error);
            if (error != SocketError.Success)
            {
                Debug.LogError(string.Format("Send byts error {0}!", error));
                Close();
                break;
            }
        }
    }

    void Receive()
    {
        while (true)
        {
            if (!_socket.Connected)
            {
                continue;
            }

            Array.Clear(_recvBytes, 0, _recvBytes.Length);

            SocketError error;
            int length = _socket.Receive(_recvBytes, 0, _recvBytes.Length, SocketFlags.None, out error);
            if (error == SocketError.Success)
            {
                var headLength = PACKAGE_LENGTH_WIDTH + PROTOCOL_ID_WIDTH + MESSAGE_LENGTH_WIDTH + CUSTOM_WIDTH;
                if (length >= headLength)
                {
                    UpackMessage(_recvBytes);
                }
                else
                {
                    Debug.LogError(string.Format("Receive byts should large than {0}!", headLength));
                }
            }
            else
            {
                Debug.LogError(string.Format("Receive byts error {0}!", error));
                Close();
                break;
            }
        }
    }

    void UpackMessage(byte[] bytes, bool hasPackageHead = true)
    {
        var offset = 0;
        var packageLength = bytes.Length;
        if (hasPackageHead)
        {
            // header是大端，特殊处理下
            Array.Clear(_headerPackage, 0, _headerPackage.Length);
            _headerPackage[0] = bytes[1];
            _headerPackage[1] = bytes[0];
            packageLength = BitConverter.ToUInt16(_headerPackage, 0);

            offset += PACKAGE_LENGTH_WIDTH;
        }

        var protocolId = BitConverter.ToUInt32(bytes, offset);
        offset += PROTOCOL_ID_WIDTH;

        var messageLength = BitConverter.ToInt32(bytes, offset);
        offset += MESSAGE_LENGTH_WIDTH + CUSTOM_WIDTH;

        var messageHeaderWidth = PROTOCOL_ID_WIDTH + MESSAGE_LENGTH_WIDTH + CUSTOM_WIDTH;
        if (messageLength > packageLength - messageHeaderWidth)
        {
            //message to long
            return;
        }

        _recvByteList.Clear();
        _recvByteList.AddRange(bytes);
        _recvByteList.RemoveRange(packageLength, bytes.Length - packageLength);
        _recvByteList.RemoveRange(0, offset);

        var leftLength = packageLength - messageHeaderWidth - messageLength;
        if (leftLength > 0)
        {
            _recvByteList.RemoveRange(_recvByteList.Count - leftLength, leftLength);
        }

        var message = new Message();
        message.data = _recvByteList.ToArray();
        message.resId = protocolId;

        lock (_lockCachedSendMessageListObj)
        {
            for (var i = 0; i < _cachedSendMessageList.Count; i++)
            {
                var cachedSendMessage = _cachedSendMessageList[i];
                if (cachedSendMessage.resId == protocolId)
                {
                    message.reqId = cachedSendMessage.reqId;
                    message.callback = cachedSendMessage.callback;
                    _cachedSendMessageList.Remove(cachedSendMessage);
                    break;
                }
            }
        }

        lock (_lockRecvMessageListObj)
        {
            _recvMessageList.Add(message);
        }

        if (leftLength > 0)
        {
            _recvByteList.Clear();
            _recvByteList.AddRange(bytes);
            _recvByteList.RemoveRange(packageLength, bytes.Length - packageLength);
            _recvByteList.RemoveRange(0, offset + messageLength);

            UpackMessage(_recvByteList.ToArray(), false);
        }
    }

    public void SendMessage(byte[] buf, uint reqId, uint resId = 0, ReceiveMessageDelegate callback = null)
    {
        var length = buf.Length + PROTOCOL_ID_WIDTH + MESSAGE_LENGTH_WIDTH + CUSTOM_WIDTH;
        if (length > CACHE_SIZE)
        {
            return;
        }

        Array.Clear(_headerAll, 0, _headerAll.Length);

        var Bytes = BitConverter.GetBytes(reqId);
        Buffer.BlockCopy(Bytes, 0, _headerAll, 0, Bytes.Length);

        var bufLengthBytes = BitConverter.GetBytes(buf.Length);
        Buffer.BlockCopy(bufLengthBytes, 0, _headerAll, PROTOCOL_ID_WIDTH, bufLengthBytes.Length);

        _sendByteList.Clear();
        _sendByteList.AddRange(_headerAll);
        _sendByteList.AddRange(buf);

        var message = new Message();
        message.data = _sendByteList.ToArray();
        message.reqId = reqId;
        message.resId = resId;
        message.callback = callback;

        lock (_lockSendMessageListObj)
        {
            _sendMessageList.Add(message);
        }

        if (resId != 0 && callback != null)
        {
            lock (_lockCachedSendMessageListObj)
            {
                _cachedSendMessageList.Add(message);
            }
        }

        if (_sendThread == null || !_sendThread.IsAlive)
        {
            _sendThread = new Thread(new ThreadStart(Send));
            _sendThread.IsBackground = true;
            _sendThread.Start();
        }
    }

    public void Update()
    {
        Tick();
    }

    void Tick()
    {
        Message[] messageList;
        lock (_lockRecvMessageListObj)
        {
            messageList = _recvMessageList.ToArray();
            _recvMessageList.Clear();
        }

        if (messageList.Length == 0)
        {
            return;
        }

        for (var i = 0; i < messageList.Length; i++)
        {
            var message = messageList[i];

            if (message.callback != null)
            {
                message.callback(message);
            }

            _networkNotification.data2 = message;
            _networkNotification.type = (int)message.resId;
            WorldManager.Instance.GetNotificationCenter().Notificate(_networkNotification);
        }
    }

    void CleanThread()
    {
        _recvByteList.Clear();
        _recvMessageList.Clear();
        if (_recvThread != null)
        {
            _recvThread.Abort();
            _recvThread = null;
        }

        _threadSendByteList.Clear();
        _sendByteList.Clear();
        _cachedSendMessageList.Clear();
        _sendMessageList.Clear();
        if (_sendThread != null)
        {
            _sendThread.Abort();
            _sendThread = null;
        }
    }

    public void Close()
    {
        CleanThread();

        if (_socket != null)
        {
            _socket.Close();
            _socket = null;
        }
    }
}
