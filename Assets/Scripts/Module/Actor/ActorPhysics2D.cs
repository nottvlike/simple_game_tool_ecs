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
            _requiredDataTypeList.Add(typeof(ActorData));
            _requiredDataTypeList.Add(typeof(DirectionData));
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            var type = data.GetType();
            return type == typeof(ActorData) || type == typeof(DirectionData) || type == typeof(SpeedData) || type == typeof(ActorJumpData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var actorData = objData.GetData<ActorData>();
            var resourceData = objData.GetData<ResourceData>();
            var position = resourceData.gameObject.transform.position;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(position, -Vector3.up, 3, LayerMask.GetMask("Ground"));
            if (raycastHit2D)
            {
                actorData.ground.y = Mathf.CeilToInt(raycastHit2D.transform.parent.position.y * Constant.UNITY_UNIT_TO_GAME_UNIT);
            }

            var positionData = objData.GetData<PositionData>();
            if (positionData.position.y == actorData.ground.y && actorData.force.x == 0 && actorData.force.y == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            var forceY = actorData.force.y;
            if (positionData.position.y != actorData.ground.y)
            {
                forceY += -actorData.gravity;
            }

            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
            var directionData = objData.GetData<DirectionData>();
            var deltaX = directionData.direction.x * actorData.force.x * gameSystemData.unscaleDeltaTime;
            var deltaY = forceY * gameSystemData.unscaleDeltaTime;

            var distanceY = Mathf.Abs(actorData.ground.y - positionData.position.y);
            if (distanceY != 0 && Mathf.Abs(deltaY) > distanceY)
            {
                deltaY = deltaY > 0 ? distanceY : -distanceY;
            }

            actorData.ground.x += deltaX;

            positionData.position.x += deltaX;
            positionData.position.y += deltaY;
            resourceData.gameObject.transform.Translate((float)deltaX / Constant.UNITY_UNIT_TO_GAME_UNIT, (float)deltaY / Constant.UNITY_UNIT_TO_GAME_UNIT, 0);
        }
    }
}
