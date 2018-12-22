using Data;

public partial class WorldManager : Singleton<WorldManager>
{
    ObjectData _gameCore;
    public ObjectData GameCore
    {
        get
        {
            if (_gameCore == null)
            {
                _gameCore = new ObjectData();
                _gameCore.AddData(new GameSystemData());

                var localizationData = new GameLocalizationData();
                localizationData.zone = "zh-cn";
                _gameCore.AddData(localizationData);

                _gameCore.AddData(new ResourcePreloadData());

                _gameCore.AddData(new PlayerBaseData());

                _objectDataList.Add(_gameCore);

                _gameCore.SetDirty();
            }

            return _gameCore;
        }
    }

    ObjectData _gameServer;
    public ObjectData GameServer
    {
        get
        {
            if (_gameServer == null)
            {
                _gameServer = new ObjectData();
                _gameServer.AddData(new GameNetworkData());
                _objectDataList.Add(_gameServer);

                _gameServer.SetDirty();
            }

            return _gameServer;
        }
    }

    ObjectData _item;
    public ObjectData Item
    {
        get
        {
            if (_item == null)
            {
                _item = new ObjectData();
                _item.AddData(new ItemInfoData());
                _objectDataList.Add(_item);

                _item.SetDirty();
            }

            return _item;
        }
    }

    ObjectData _player;
    public ObjectData Player
    {
        get
        {
            if (_player == null)
            {
                _player = new ObjectData();

                var actorData = new ActorData();
                actorData.actorId = 1;
                _player.AddData(actorData);

                _player.AddData(new ServerData());
                _player.AddData(new DirectionData());
                _player.AddData(new SpeedData());
                _player.AddData(new ResourceData());
                _player.AddData(new ResourceStateData());

                _player.AddData(new JoyStickData());
                _player.AddData(new ActorSyncData());

                _objectDataList.Add(_player);

                _player.SetDirty();
            }

            return _player;
        }
    }
}