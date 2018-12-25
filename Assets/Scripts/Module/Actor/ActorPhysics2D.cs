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

            var resourceData = objData.GetData<ResourceData>();
            var position = resourceData.gameObject.transform.position;

            var actorData = objData.GetData<ActorData>();
            RaycastHit2D raycastHit2D = Physics2D.Raycast(position, -Vector3.up, 3, LayerMask.GetMask("Ground"));
            if (raycastHit2D)
            {
                actorData.ground.y = raycastHit2D.transform.parent.position.y;
            }

            if (position.y == actorData.ground.y && actorData.force.x == 0 && actorData.force.y == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            var forceY = actorData.force.y;
            if (position.y != actorData.ground.y)
            {
                forceY += -actorData.gravity;
            }

            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
            var deltaTime = (float)gameSystemData.unscaleDeltaTime / Constant.SECOND_TO_MILLISECOND;

            var directionData = objData.GetData<DirectionData>();
            var deltaX = directionData.x * actorData.force.x * deltaTime;
            var deltaY = forceY * deltaTime;

            var distanceY = Mathf.Abs(actorData.ground.y - position.y);
            if (distanceY != 0 && Mathf.Abs(deltaY) > distanceY)
            {
                deltaY = deltaY > 0 ? distanceY : -distanceY;
            }

            resourceData.gameObject.transform.Translate(deltaX, deltaY, 0);
        }
    }
}
