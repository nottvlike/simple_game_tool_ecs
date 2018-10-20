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
                _gameCore = new ObjectData(Constant.GAME_CORE_OBJECT_ID);
                _gameCore.AddData(new GameSystemData());
                _objectDataList.Add(_gameCore);

                _gameCore.RefreshModuleAddedObjectIdList();
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
                _gameServer = new ObjectData(Constant.GAME_SERVER_OBJECT_ID);
                _gameServer.AddData(new GameServerData());
                _objectDataList.Add(_gameServer);

                _gameServer.RefreshModuleAddedObjectIdList();
            }

            return _gameServer;
        }
    }
}