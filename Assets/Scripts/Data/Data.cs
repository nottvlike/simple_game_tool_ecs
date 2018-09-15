using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Data
{
    public Data Clone()
    {
        return (Data)MemberwiseClone();
    }
}
