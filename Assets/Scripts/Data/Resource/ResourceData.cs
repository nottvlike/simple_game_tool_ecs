using System;
using UnityEngine;

namespace Data
{
    public enum ResourceType
    {
        None,
        Creature,
        Config,
        UI
    }

    public enum CreatureStateType
    {
        None,
        Load,
        Release,
    }

    public enum CreatureType
    {
        None,
        Actor,
        BattleItem
    }

    public enum CreatureCampType
    {
        None,
        Player,
        Friend,
        Enemy,
        Neutral
    }

    [Serializable]
    public class ResourceData : Data
    {
        public string resource;
        public GameObject gameObject;
        public Vector3 initialPosition;

        public override void Clear()
        {
            resource = string.Empty;
            gameObject = null;
            initialPosition = Vector3.zero;
        }
    }

    [Serializable]
    public class ResourceStateData : Data
    {
        public string name;
        public bool isGameObject;
        public bool isInstantiated;

        public override void Clear()
        {
            name = string.Empty;
            isGameObject = false;
            isInstantiated = false;
        }
    }

    [Serializable]
    public class CreatureStateData : Data
    {
        public CreatureType type;
        public CreatureCampType campType;
        public CreatureStateType stateType;

        public override void Clear()
        {
            type = CreatureType.None;
            campType = CreatureCampType.None;
            stateType = CreatureStateType.None;
        }
    }

    public class ResourcePreloadData : Data
    {
        public ResourcePreloadType preloadType;
        public int preloadCount;

        public override void Clear()
        {
            preloadType = ResourcePreloadType.GameInit;
            preloadCount = 0;
        }
    }
}
