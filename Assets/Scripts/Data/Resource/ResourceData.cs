using System;
using UnityEngine;

namespace Data
{
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
