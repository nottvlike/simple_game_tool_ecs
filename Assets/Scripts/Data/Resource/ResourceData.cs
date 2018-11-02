using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class ResourceData : Data
    {
        public string name;
        public string resource;
        public GameObject gameObject;

        public override Data Clone()
        {
            var data = base.Clone() as ResourceData;
            data.name = String.Copy(this.name);
            return data;
        }
    }

    [Serializable]
    public class ResourcePoolData : Data
    {
        public string name;
        public bool isInUse;
    }

    [Serializable]
    public class ResourceStateData : Data
    {
        public bool isGameObject;
        public bool isLoaded;
        public bool isInstantiated;
    }
}
