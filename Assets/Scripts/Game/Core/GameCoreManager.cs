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

                var localizationListConfig = ResourceMgr.Load("LocalizationConfigGroup") as LocalizationConfigGroup;
                LocalizationConfig[] localizationConfigList = localizationListConfig.localizationConfigList;
                for (var i = 0; i < localizationConfigList.Length; i++)
                {
                    var localizationConfig = localizationConfigList[i];
                    if (localizationConfig.zone == localizationData.zone)
                    {
                        localizationConfig.Init();
                        localizationData.currentConfig = localizationConfig;
                    }
                }

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
                _gameServer = new ObjectData();
                _gameServer.AddData(new GameNetworkData());
                _objectDataList.Add(_gameServer);

                _gameServer.RefreshModuleAddedObjectIdList();
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

                _item.RefreshModuleAddedObjectIdList();
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
                _player.AddData(new PlayerBaseData());
                _player.AddData(new ServerData());
                _objectDataList.Add(_player);

                _player.RefreshModuleAddedObjectIdList();
            }

            return _player;
        }
    }
}