using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorMove : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorData));
            _requiredDataTypeList.Add(typeof(SpeedData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var speedData = objData.GetData<SpeedData>();
            if (speedData.speed == 0)
            {
                return;
            }

            speedData.speed = speedData.speed - speedData.friction;
            if (speedData.speed < 0)
            {
                speedData.speed = 0;
            }

            var actorData = objData.GetData<ActorData>();
            actorData.force.x = speedData.speed / (float)Constant.SPEED_BASE;
        }
    }
}