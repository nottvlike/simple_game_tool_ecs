using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public abstract class Data : IPoolObject
    {
        public bool IsInUse
        {
            get;
            set;
        }

        public virtual void Clear()
        {
        }
    }
}