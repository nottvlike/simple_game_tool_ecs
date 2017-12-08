using UnityEngine;
using System.Collections;
using System;

public class Module : IModule
{
    IObject _currentObject = null;

    public IObject CurrentObject
    {
        get { return _currentObject; }
        set
        {
            if (_currentObject == value)
                return;

            _currentObject = value;
        }
    }

    public virtual void OnAdd(IObject parent)
    {
    }

    public virtual void OnRemove(IObject parent)
    {
    }
}
