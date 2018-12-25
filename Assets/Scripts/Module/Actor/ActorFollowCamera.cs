using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorFollowCamera : Module, ITimerObject
    {
        TimerEvent _timer;
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(FollowCameraData));
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return false;
        }

        protected override void OnEnable()
        {
            _timer = WorldManager.Instance.TimerMgr.AddEndLess(0, Constant.CAMERA_FOLLOW_INTERVAL, this);
        }

        protected override void OnDisable()
        {
            _timer.Clear();
            _timer = null;
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var resourceData = objData.GetData<ResourceData>();
            var endPosition = resourceData.gameObject.transform.localPosition;
            var startPosition = Camera.main.transform.localPosition;
            if (startPosition.x == endPosition.x && startPosition.y == endPosition.y)
            {
                return;
            }

            endPosition.z = startPosition.z;
            TweenerUtil.Move(Camera.main.gameObject, 0, Constant.CAMERA_FOLLOW_INTERVAL, startPosition, endPosition);
        }

        public void Tick()
        {
            for (var i = 0; i < _objectIdList.Count; i++)
            {
                var objId = _objectIdList[i];
                var objData = WorldManager.Instance.GetObjectData(objId);
                Refresh(objData);
            }
        }
    }
}
