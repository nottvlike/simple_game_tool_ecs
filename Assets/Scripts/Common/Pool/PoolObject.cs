using UnityEngine;

public abstract class PoolObject : BaseObject
{
    bool _isInUse = false;

    public string PoolName
    {
        get { return GetType().ToString(); }
    }

    public bool IsInUse
    {
        get { return _isInUse; }
        set
        {
            if (_isInUse == value)
                return;

            _isInUse = value;

            if (_resource)
            {
                ((GameObject)_resource).SetActive(_isInUse);
            }
        }
    }

    public PoolObject()
    : base()
    {
    }

    public PoolObject(int id)
    : base(id)
    {
    }

    public PoolObject(int id, string resourceName)
        : base(id, resourceName) 
    {
    }

    public override void Init()
    {
        if (_resource)
        {
            ((GameObject)_resource).SetActive(_isInUse);
        }
    }
}
