using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : BaseObject {
    public override void Init()
    {
        _isInit = true;
    }

    public TestObject()
    : base()
    {

    }

    public TestObject(int id)
        : base(id)
    {
    }

    public TestObject(int id, string resourceName)
        : base(id, resourceName)
    {
    }
}
