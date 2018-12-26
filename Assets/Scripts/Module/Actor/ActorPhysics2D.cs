using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorPhysics2D : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(Physics2DData));
            _requiredDataTypeList.Add(typeof(PositionData));
            _requiredDataTypeList.Add(typeof(DirectionData));
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            var type = data.GetType();
            return type == typeof(Physics2DData) || type == typeof(DirectionData) || type == typeof(SpeedData) || type == typeof(ActorJumpData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var physics2DData = objData.GetData<Physics2DData>();
            var resourceData = objData.GetData<ResourceData>();
            var position = resourceData.gameObject.transform.position;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(position, -Vector3.up, 3, LayerMask.GetMask("Ground"));
            if (raycastHit2D)
            {
                physics2DData.ground.y = Mathf.CeilToInt(raycastHit2D.transform.parent.position.y * Constant.UNITY_UNIT_TO_GAME_UNIT);
            }

            var positionData = objData.GetData<PositionData>();
            if (positionData.position.y == physics2DData.ground.y && physics2DData.force.x == 0 && physics2DData.force.y == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            var forceY = physics2DData.force.y;
            if (positionData.position.y != physics2DData.ground.y)
            {
                forceY += -physics2DData.gravity;
            }

            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
            var directionData = objData.GetData<DirectionData>();
            var deltaX = directionData.direction.x * physics2DData.force.x * gameSystemData.unscaleDeltaTime;
            var deltaY = forceY * gameSystemData.unscaleDeltaTime;

            var distanceY = Mathf.Abs(physics2DData.ground.y - positionData.position.y);
            if (distanceY != 0 && Mathf.Abs(deltaY) > distanceY)
            {
                deltaY = deltaY > 0 ? distanceY : -distanceY;
            }

            physics2DData.ground.x += deltaX;

            positionData.position.x += deltaX;
            positionData.position.y += deltaY;
            resourceData.gameObject.transform.Translate((float)deltaX / Constant.UNITY_UNIT_TO_GAME_UNIT, (float)deltaY / Constant.UNITY_UNIT_TO_GAME_UNIT, 0);
        }
    }
}
