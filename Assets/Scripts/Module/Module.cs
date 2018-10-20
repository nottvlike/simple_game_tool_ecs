using UnityEngine;
using System.Collections.Generic;
using System;

namespace Module{
    public abstract class Module
    {
        protected List<int> _objectIdList = new List<int>();

        public abstract bool IsBelong(List<Data.Data> dataList);

        public void OnIdListChanged()
        {
            _objectIdList.Clear();

            var objectIdList = ObjectData.GetModuleAddedObjectList(GetType());
            if (objectIdList != null)
            {
                _objectIdList.AddRange(objectIdList);
            }
        }
    }
}