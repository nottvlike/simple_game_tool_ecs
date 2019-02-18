using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public abstract class BaseUpdateModule : Module
    {
        public struct ModuleData
        {
            public int objId;
            public bool isStop;
        }

        ModuleData _defaultModuleData;
        List<ModuleData> _moduleDataList = new List<ModuleData>();

        public BaseUpdateModule()
        {
            _defaultModuleData.objId = -1;
        }

        public override void Add(int objectDataId)
        {
            var moduleData = new ModuleData();
            moduleData.objId = objectDataId;
            moduleData.isStop = false;
            _moduleDataList.Add(moduleData);

            base.Add(objectDataId);
        }

        public override void Remove(int objectDataId)
        {
            base.Remove(objectDataId);

            var index = GetModuleDataIndex(objectDataId);
            _moduleDataList.Remove(_moduleDataList[index]);

            Enabled = !CheckAllAreStop();
        }

        int GetModuleDataIndex(int objectDataId)
        {
            for (var i = 0; i < _moduleDataList.Count; i++)
            {
                var moduleData = _moduleDataList[i];
                if (moduleData.objId == objectDataId)
                {
                    return i;
                }
            }

            LogUtil.E("Failed to find module data " + objectDataId);
            return -1;
        }

        public override void SetDirty(ObjectData objData)
        {
            Start(objData.ObjectId);
        }

        void Start(int objectDataId)
        {
            var index = GetModuleDataIndex(objectDataId);
            var moduleData = _moduleDataList[index];
            if (moduleData.isStop)
            {
                moduleData.isStop = false;
                _moduleDataList[index] = moduleData;
            }

            Enabled = true;
        }

        protected void Stop(int objectDataId)
        {
            var index = GetModuleDataIndex(objectDataId);
            var moduleData = _moduleDataList[index];
            if (!moduleData.isStop)
            {
                moduleData.isStop = true;
                _moduleDataList[index] = moduleData;

                Enabled = !CheckAllAreStop();
            }
        }

        bool CheckAllAreStop()
        {
            if (_moduleDataList.Count == 0)
            {
                return true;
            }

            for (var i = 0; i < _moduleDataList.Count; i++)
            {
                if (!_moduleDataList[i].isStop)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public abstract class UpdateModule : BaseUpdateModule, IUpdateEvent
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
            for (var i = 0; i < _objectIdList.Count; ++i)
            {
                var objId = _objectIdList[i];

                var objData = WorldManager.Instance.GetObjectData(objId);
                Refresh(objData);
            }
        }
    }
}
