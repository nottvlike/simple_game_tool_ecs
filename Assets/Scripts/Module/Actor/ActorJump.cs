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
            _requiredDataTypeList.Add(typeof(PositionData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ActorJumpData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var jumpData = objData.GetData<ActorJumpData>();

            var currentJump = jumpData.currentJump;
            if (currentJump.y == 0 && currentJump.x == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            var actorData = objData.GetData<ActorData>();
            var positionData = objData.GetData<PositionData>();
            if (positionData.position.y == actorData.ground.y && jumpData.currentJump.y == 0)
            {
                jumpData.currentJump.x = 0;

                actorData.force.x = 0;
                actorData.force.y = 0;
            }
            else
            {
                jumpData.currentJump.y = currentJump.y - jumpData.friction;
                if (jumpData.currentJump.y < 0)
                {
                    jumpData.currentJump.y = 0;
                    actorData.force.y += -jumpData.friction;
                }
                else
                {
                    actorData.force.y = currentJump.y;
                }

                actorData.force.x = jumpData.currentJump.x;
            }
        }
    }
}
