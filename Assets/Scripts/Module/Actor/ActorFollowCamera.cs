using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorFollowCamera : Module, ITimerObject
    {
        TimerEvent _timer;
        Tweener _tweener;

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
            _timer = WorldManager.Instance.TimerMgr.AddEndLess(0, 0, this);
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

            if (_tweener != null && _tweener.IsPlaying)
            {
                return;
            }

            var resourceData = objData.GetData<ResourceData>();
            var transform = resourceData.gameObject.transform;

            var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            var maxWidth = Constant.SCREEN_WIDTH * Constant.MAX_FOLLOW_REGION / 2 ;
            var maxHeight = Constant.SCREEN_HEIGHT * Constant.MAX_FOLLOW_REGION / 2 ;

            var width = Mathf.Abs(screenPosition.x - Constant.SCREEN_WIDTH / 2);
            var height = Mathf.Abs(screenPosition.y - Constant.SCREEN_HEIGHT / 2);

            if (width > maxWidth || height > maxHeight)
            {
                var startPosition = Camera.main.transform.localPosition;
                var endPosition = transform.localPosition;
                endPosition.z = startPosition.z;
                _tweener = TweenerUtil.Move(Camera.main.gameObject, 0, Constant.CAMERA_FOLLOW_INTERVAL, startPosition, endPosition);
            }
        }

        public void Tick()
        {
            var player = WorldManager.Instance.Player;
            Refresh(player);
        }
    }
}
