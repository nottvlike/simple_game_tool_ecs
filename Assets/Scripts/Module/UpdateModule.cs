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
            WorldManager.Instance.GetUnityEventTool().Add(this);
        }

        protected override void OnDisable()
        {
            WorldManager.Instance.GetUnityEventTool().Remove(this);
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

                var objData = WorldManager.Instance.GetObjectData(objId);
                var resourceStateData = objData.GetData<Data.ResourceStateData>() as Data.ResourceStateData;
                if (!resourceStateData.isInstantiated)
                {
                    continue;
                }

                UpdateObject(objId, objData);
            }
        }

        protected abstract void UpdateObject(int objId, ObjectData obj);
    }
}
