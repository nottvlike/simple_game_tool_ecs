using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    ActorConfig _actorConfig;
    JoyStickConfig _joyStickConfig;
    PanelConfig _panelConfig;
    ResourcePreloadConfig _resourcePreloadConfig;
    LocalizationConfigGroup _localizationConfigGroup;
    BuffConfig _buffConfig;
    BattleConfig _battleConfig;
    BattleItemConfig _battleItemConfig;

    public ActorConfig ActorConfig
    {
        get { return _actorConfig; }
    }

    public JoyStickConfig JoyStickConfig
    {
        get { return _joyStickConfig; }
    }

    public PanelConfig PanelConfig
    {
        get { return _panelConfig; }
    }

    public ResourcePreloadConfig ResourcePreloadConfig
    {
        get { return _resourcePreloadConfig; }
    }

    public LocalizationConfigGroup LocalizationConfigGroup
    {
        get { return _localizationConfigGroup; }
    }

    public BuffConfig BuffConfig
    {
        get { return _buffConfig; }
    }

    public BattleConfig BattleConfig
    {
        get { return _battleConfig; }
    }

    public BattleItemConfig BattleItemConfig
    {
        get { return _battleItemConfig; }
    }

    public void LoadConfig()
    {
        ResourceMgr.LoadAsync("MainConfigGroup", delegate (Object obj)
        {
            var configGroup = obj as ConfigGroup;

            _actorConfig = configGroup.Get<ActorConfig>();
            _joyStickConfig = configGroup.Get<JoyStickConfig>();
            _localizationConfigGroup = configGroup.Get<LocalizationConfigGroup>();
            _buffConfig = configGroup.Get<BuffConfig>();
            _battleConfig = configGroup.Get<BattleConfig>();
            _battleItemConfig = configGroup.Get<BattleItemConfig>();
        });
    }

    public void LoadPreloadConfig()
    {
        ResourceMgr.LoadAsync("PreloadConfigGroup", delegate(Object obj)
        {
            var configGroup = obj as ConfigGroup;
            _resourcePreloadConfig = configGroup.Get<ResourcePreloadConfig>();
            _panelConfig = configGroup.Get<PanelConfig>();

            var resourcePreloadData = GameCore.GetData<Data.ResourcePreloadData>();
            resourcePreloadData.preloadType = ResourcePreloadType.GameInit;
            resourcePreloadData.preloadCount = 0;

            GameCore.SetDirty(resourcePreloadData);
        });
    }
}
