using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public abstract class UpdateModule : Module, IUpdateEvent
    {
        protected override void OnEnable()
        {
            WorldManager.Instance.UnityEventMgr.Add(this);
        }

        protected override void OnDisable()
        {
            WorldManager.Instance.UnityEventMgr.Remove(this);
        }

        void IUpdateEvent.Update()
        {
            if (_objectIdList.Count == 0)
            {
                return;
            }

            for (var i = 0; i < _objectIdList.Count; ++i)
            {
                var objId = _objectIdList[i];

                var objData = WorldManager.Instance.GetObjectData(objId);
                Refresh(objData);
            }
        }
    }
}
