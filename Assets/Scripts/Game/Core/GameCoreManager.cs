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
                _gameCore.AddData<BattleResourceData>();
                _gameCore.AddData<BattleData>();
                _gameCore.AddData<ItemInfoData>();

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
                _gameServer.AddData<ServerData>();
                _objectDataList.Add(_gameServer);

                _gameServer.SetDirty();
            }

            return _gameServer;
        }
    }
}