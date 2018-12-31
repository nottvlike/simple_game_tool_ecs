using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorPhysics2D : FixedUpdateModule
    {
        Vector3 _movePosition = Vector3.zero;

        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorControllerData));
            _requiredDataTypeList.Add(typeof(ActorData));
            _requiredDataTypeList.Add(typeof(Physics2DData));
            _requiredDataTypeList.Add(typeof(DirectionData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            var type = data.GetType();
            return type == typeof(Physics2DData) || type == typeof(DirectionData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var physics2DData = objData.GetData<Physics2DData>();
            var controllerData = objData.GetData<ActorControllerData>();
            if (IsGround(controllerData) && physics2DData.force.x == 0 && physics2DData.force.y == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            var actorData = objData.GetData<ActorData>();
            var currentState = actorData.currentState;
            var forceY = physics2DData.force.y;

            if (currentState != ActorStateType.SkillDefault && currentState != ActorStateType.SkillCustom)
            {
                forceY += -physics2DData.gravity;
            }

            var directionData = objData.GetData<DirectionData>();
            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
            var deltaX = directionData.direction.x * physics2DData.force.x * gameSystemData.unscaleDeltaTime;
            var deltaY = forceY * gameSystemData.unscaleDeltaTime;

            _movePosition.x = (float)deltaX / Constant.UNITY_UNIT_TO_GAME_UNIT;
            _movePosition.y = (float)deltaY / Constant.UNITY_UNIT_TO_GAME_UNIT;
            _movePosition.z = 0;

           controllerData.controller.Move(_movePosition); 
        }

        public static bool IsGround(ActorControllerData controllerData)
        {
            return controllerData.controller.isGrounded;
        }
    }
}
