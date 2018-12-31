using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module
{
    public abstract class FixedUpdateModule : BaseUpdateModule, IFixedUpdateEvent
    {
        protected override void OnEnable()
        {
            WorldManager.Instance.UnityEventMgr.Add(this);
        }

        protected override void OnDisable()
        {
            WorldManager.Instance.UnityEventMgr.Remove(this);
        }

        void IFixedUpdateEvent.FixedUpdate()
        {
            for (var i = 0; i < _objectIdList.Count; ++i)
            {
                var objId = _objectIdList[i];

                var objData = WorldManager.Instance.GetObjectData(objId);
                Refresh(objData);
            }
        }
    }
}