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
                _gameCore.AddData<GameSystemData>();

                var localizationData = _gameCore.AddData<GameLocalizationData>();
                localizationData.zone = "zh-cn";

                _gameCore.AddData<ResourcePreloadData>();
                _gameCore.AddData<PlayerBaseData>();

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
                _gameServer.AddData<GameNetworkData>();
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
                _item.AddData<ItemInfoData>();
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

                var actorData = _player.AddData<ActorData>();
                actorData.actorId = 1;

                _player.AddData<ActorController2DData>();
                _player.AddData<FollowCameraData>();
                _player.AddData<Physics2DData>();
                _player.AddData<ActorJumpData>();
                _player.AddData<ServerData>();
                _player.AddData<DirectionData>();
                _player.AddData<SpeedData>();
                _player.AddData<ResourceData>();
                _player.AddData<ResourceStateData>();
                _player.AddData<ClientJoyStickData>();
                _player.AddData<ServerJoyStickData>();
                _player.AddData<ActorSyncData>();

                _objectDataList.Add(_player);

                _player.SetDirty();
            }

            return _player;
        }
    }
}