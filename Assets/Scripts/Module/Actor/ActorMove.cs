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
            _requiredDataTypeList.Add(typeof(Physics2DData));
            _requiredDataTypeList.Add(typeof(SpeedData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(SpeedData);
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
                Stop(objData.ObjectId);
                return;
            }

            var physics2DData = objData.GetData<Physics2DData>();
            var positionData = objData.GetData<PositionData>();
            var positionY = ActorPhysics2D.GetActorFootY(positionData, physics2DData);
            if (positionData.ground.y != positionY)
            {
                speedData.speed = 0;
                return;
            }

            var actorData = objData.GetData<ActorData>();
            if (actorData.currentState != ActorStateType.Move)
            {
                actorData.currentState = ActorStateType.Move;
                objData.SetDirty(actorData);
            }

            var speed = speedData.speed;
            speedData.speed = speed - physics2DData.friction;
            if (speedData.speed <= 0)
            {
                speedData.speed = 0;
                physics2DData.force.x = 0;

                actorData.currentState = ActorStateType.Idle;
                objData.SetDirty(actorData);
            }
            else
            {
                physics2DData.force.x = speed;
            }
        }
    }
}