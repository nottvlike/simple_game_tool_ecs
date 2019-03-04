using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    public ActorConfig ActorConfig
    {
        get;
        private set;
    }

    public JoyStickConfig JoyStickConfig
    {
        get;
        private set;
    }

    public PanelConfig PanelConfig
    {
        get;
        private set;
    }

    public ResourcePreloadConfig ResourcePreloadConfig
    {
        get;
        private set;
    }

    public LocalizationConfigGroup LocalizationConfigGroup
    {
        get;
        private set;
    }

    public BuffConfig BuffConfig
    {
        get;
        private set;
    }

    public BattleConfig BattleConfig
    {
        get;
        private set;
    }

    public BattleItemConfig BattleItemConfig
    {
        get;
        private set;
    }

    public SkillConfig SkillConfig
    {
        get;
        private set;
    }

    public void LoadConfig()
    {
        ResourceMgr.LoadAsync("MainConfigGroup", delegate (Object obj)
        {
            var configGroup = obj as ConfigGroup;

            ActorConfig = configGroup.Get<ActorConfig>();
            JoyStickConfig = configGroup.Get<JoyStickConfig>();
            LocalizationConfigGroup = configGroup.Get<LocalizationConfigGroup>();
            BuffConfig = configGroup.Get<BuffConfig>();
            BattleConfig = configGroup.Get<BattleConfig>();
            BattleItemConfig = configGroup.Get<BattleItemConfig>();
            SkillConfig = configGroup.Get<SkillConfig>();
        });
    }

    public void LoadPreloadConfig()
    {
        ResourceMgr.LoadAsync("PreloadConfigGroup", delegate(Object obj)
        {
            var configGroup = obj as ConfigGroup;
            ResourcePreloadConfig = configGroup.Get<ResourcePreloadConfig>();
            PanelConfig = configGroup.Get<PanelConfig>();

            var resourcePreloadData = GameCore.GetData<Data.ResourcePreloadData>();
            resourcePreloadData.preloadType = ResourcePreloadType.GameInit;
            resourcePreloadData.preloadCount = 0;

            GameCore.SetDirty(resourcePreloadData);
        });
    }
}
