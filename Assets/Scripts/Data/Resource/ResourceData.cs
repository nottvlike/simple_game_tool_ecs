using System;
using UnityEngine;

namespace Data
{
    public enum ResourceStateType
    {
        None,
        Load,
        Release,
    }

    public enum ResourceType
    {
        None,
        Actor,
        BattleItem
    }

    public enum ResourceCampType
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
        public ResourceType resourceType;
        public ResourceCampType campType;
        public ResourceStateType resourceStateType;

        public override void Clear()
        {
            name = string.Empty;
            isGameObject = false;
            isInstantiated = false;
            resourceType = ResourceType.None;
            campType = ResourceCampType.None;
            resourceStateType = ResourceStateType.None;
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
