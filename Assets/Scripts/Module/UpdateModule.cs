using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public abstract class UpdateModule : Module, IUpdateEvent
    {
        public UpdateModule()
        {
            WorldManager.Instance.GetUnityEventTool().Add(this);
        }

        public override bool IsBelong(List<Data.Data> dataList)
        {
            return false;
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

                var obj = WorldManager.Instance.GetObject(objId);
                if (!obj.IsInit())
                {
                    continue;
                }

                UpdateObject(objId, obj);
            }
        }

        protected abstract void UpdateObject(int objId, BaseObject obj);
    }
}
