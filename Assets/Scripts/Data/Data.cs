using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public abstract class Data
    {
        public virtual Data Clone()
        {
            return (Data)MemberwiseClone();
        }
    }
}