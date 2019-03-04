using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module
{
    public abstract class LateUpdateModule : BaseUpdateModule, ILateUpdateEvent
    {
        protected override void OnEnable()
        {
            WorldManager.Instance.UnityEventMgr.Add(this);
        }

        protected override void OnDisable()
        {
            WorldManager.Instance.UnityEventMgr.Remove(this);
        }

        void ILateUpdateEvent.LateUpdate()
        {
            for (var i = 0; i < _moduleDataList.Count; ++i)
            {
                var moduleData = _moduleDataList[i];

                if (!moduleData.isStop)
                {
                    var objData = WorldManager.Instance.GetObjectData(moduleData.objId);
                    Refresh(objData);
                }
            }
        }
    }
}