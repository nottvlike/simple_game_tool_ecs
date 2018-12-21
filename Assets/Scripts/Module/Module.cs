using UnityEngine;
using System.Collections.Generic;
using System;

namespace Module{
    public abstract class Module
    {
        protected List<int> _objectIdList = new List<int>();

        protected List<Type> _requiredDataTypeList = new List<Type>();
        static List<Type> _tmpList = new List<Type>();

        public bool IsMeet(List<Data.Data> dataList)
        {
            _tmpList.Clear();

            for (var i = 0; i < dataList.Count; i++)
            {
                var data = dataList[i];
                var type = data.GetType();
                if (IsRequired(data) && _tmpList.IndexOf(type) == -1)
                {
                    _tmpList.Add(type);
                }
            }
            return _tmpList.Count > 0 && _tmpList.Count == _requiredDataTypeList.Count;
        }

        public bool IsRequired(Data.Data data)
        {
            return _requiredDataTypeList.IndexOf(data.GetType()) != -1;
        }

        protected abstract void InitRequiredDataType();

        public abstract void Refresh(ObjectData objData, bool notMet = false);

        public void Add(int objectDataId)
        {
            _objectIdList.Add(objectDataId);

            var objData = WorldManager.Instance.GetObjectData(objectDataId);
            Refresh(objData);
        }

        public void Remove(int objectDataId)
        {
            _objectIdList.Remove(objectDataId);

            var objData = WorldManager.Instance.GetObjectData(objectDataId);
            Refresh(objData, true);
        }

        public bool Contains(int objectDataId)
        {
            return _objectIdList.IndexOf(objectDataId) != -1;
        }

        public Module()
        {
            InitRequiredDataType();

            Enabled = true;
        }

        bool _enabled;
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (_enabled == value)
                {
                    return;
                }

                _enabled = value;

                if (_enabled)
                {
                    OnEnable();
                }
                else
                {
                    OnDisable();
                }
            }
        }
        
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
    }
}