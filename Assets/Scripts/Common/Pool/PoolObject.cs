class PoolObject : BaseObject, IPoolObject
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
                _resource.gameObject.SetActive(_isInUse);
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
        base.Init();

        if (_resource)
        {
            _resource.gameObject.SetActive(_isInUse);
        }
    }
}
