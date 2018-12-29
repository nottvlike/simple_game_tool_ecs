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
            _requiredDataTypeList.Add(typeof(Collider2DData));
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

            var positionData = objData.GetData<PositionData>();
            var collider2DData = objData.GetData<Collider2DData>();
            var position = collider2DData.ground.position;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(position, -Vector3.up, 3, LayerMask.GetMask("Ground"));
            if (raycastHit2D)
            {
                positionData.ground.y = Mathf.CeilToInt(raycastHit2D.transform.parent.position.y * Constant.UNITY_UNIT_TO_GAME_UNIT);
            }

            var directionData = objData.GetData<DirectionData>();
            var direction = directionData.direction.x > 0 ? Vector3.right : Vector3.left;
            position = directionData.direction.x > 0 ? collider2DData.forward.position : collider2DData.back.position;
            raycastHit2D = Physics2D.Raycast(position, direction, 3, LayerMask.GetMask("Ground"));
            if (raycastHit2D)
            {
                positionData.forward.x = Mathf.CeilToInt(raycastHit2D.transform.parent.position.x * Constant.UNITY_UNIT_TO_GAME_UNIT);
            }
            else
            {
                positionData.forward.x = positionData.position.x + 1000;
            }

            var physics2DData = objData.GetData<Physics2DData>();
            var positionY = GetActorGroundY(positionData, physics2DData);
            if (positionY == positionData.ground.y && physics2DData.force.x == 0 && physics2DData.force.y == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            var forceY = physics2DData.force.y;
            if (positionY != positionData.ground.y)
            {
                forceY += -physics2DData.gravity;
            }

            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
            var deltaX = directionData.direction.x * physics2DData.force.x * gameSystemData.unscaleDeltaTime;
            var deltaY = forceY * gameSystemData.unscaleDeltaTime;

            var distanceY = Mathf.Abs(positionData.ground.y - positionY);
            if (distanceY != 0 && Mathf.Abs(deltaY) > distanceY)
            {
                deltaY = deltaY > 0 ? distanceY : -distanceY;
            }

            var positionX = GetActorForwardX(positionData, physics2DData, directionData);
            var distanceX = Mathf.Abs(positionData.forward.x - positionX);
            if (Mathf.Abs(deltaX) > distanceX)
            {
                deltaX = deltaX > 0 ? distanceX : -distanceX;
            }

            positionData.ground.x += deltaX;

            positionData.position.x += deltaX;
            positionData.position.y += deltaY;

            var resourceData = objData.GetData<ResourceData>();
            resourceData.gameObject.transform.Translate((float)deltaX / Constant.UNITY_UNIT_TO_GAME_UNIT, (float)deltaY / Constant.UNITY_UNIT_TO_GAME_UNIT, 0);
        }

        public static int GetActorGroundY(PositionData positionData, Physics2DData physics2DData)
        {
            return positionData.position.y - physics2DData.halfHeight;
        }

        public static int GetActorForwardX(PositionData positionData, Physics2DData physics2DData, DirectionData directionData)
        {
            return positionData.position.x + physics2DData.halfWidth * directionData.direction.x;
        }
    }
}
