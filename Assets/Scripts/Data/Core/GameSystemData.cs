using System;
using FlatBuffers;

namespace Data
{
    public class GameSystemData : Data
    {
        public int systemTime;
        public int clientFrame;
    }

    public class GameNetworkData : Data
    {
        public FlatBufferBuilder builder = new FlatBufferBuilder(Constant.NETWORK_CACHE_SIZE);
        public GameCoeNotification notification = new GameCoeNotification();
        public int lastHeartBeatTime;
        public int serverTime;
    }
}