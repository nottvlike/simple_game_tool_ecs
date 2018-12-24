using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorJump : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorData));
            _requiredDataTypeList.Add(typeof(ActorJumpData));
            _requiredDataTypeList.Add(typeof(DirectionData));
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var resourceData = objData.GetData<ResourceData>();
            var position = resourceData.gameObject.transform.position;

            var jumpData = objData.GetData<ActorJumpData>();
            var currentJump = jumpData.currentJump;
            if (jumpData.groundPosition.y != position.y || currentJump.y != 0)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(position, -Vector3.up, out raycastHit, 3, LayerMask.GetMask("Ground")))
                {
                    jumpData.groundPosition.y = raycastHit.transform.parent.position.y;
                }

                var jump = currentJump.y - jumpData.gravity;
                jumpData.currentJump.y = currentJump.y - jumpData.friction;
                if (jumpData.currentJump.y < 0)
                {
                    jumpData.currentJump.y = 0;
                    jumpData.fall += jumpData.friction;
                    jump += -jumpData.fall;
                }

                var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
                var deltaTime = (float)gameSystemData.unscaleDeltaTime / Constant.SECOND_TO_MILLISECOND;

                var directionData = objData.GetData<DirectionData>();
                var deltaX = directionData.x * currentJump.x * deltaTime;
                var deltaY = jump * deltaTime;

                var distanceY = Mathf.Abs(jumpData.groundPosition.y - position.y);
                if (distanceY != 0 && Mathf.Abs(deltaY) > distanceY)
                {
                    deltaY = deltaY > 0 ? distanceY : -distanceY;
                }

                resourceData.gameObject.transform.Translate(deltaX, deltaY, 0);
            }
        }
    }
}
