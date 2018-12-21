using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class ResourceData : Data
    {
        public string resource;
        public GameObject gameObject;
    }

    public class ResourcePoolData : Data
    {
        public string name;
        public bool isInUse;
    }

    [Serializable]
    public class ResourceStateData : Data
    {
        public string name;
        public bool isGameObject;
        public bool isLoaded;
        public bool isInstantiated;

        public override Data Clone()
        {
            var data = base.Clone() as ResourceStateData;
            data.name = string.Copy(name);
            return data;
        }
    }

    public class ResourcePreloadData : Data
    {
        public ResourcePreloadType preloadType;
        public int preloadCount;
    }
}
