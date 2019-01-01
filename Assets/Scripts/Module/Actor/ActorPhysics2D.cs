using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorPhysics2D : FixedUpdateModule
    {
        Vector2 _movePosition = Vector2.zero;

        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorController2DData));
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

            var controllerData = objData.GetData<ActorController2DData>();
            var raycast2DHit = Physics2D.Raycast(controllerData.foot.position, -Vector2.up, 0.1f);
            if (raycast2DHit.collider != null)
            {
                controllerData.groundY = Mathf.RoundToInt(raycast2DHit.point.y * Constant.UNITY_UNIT_TO_GAME_UNIT);
            }
            else
            {
                controllerData.groundY = controllerData.positionY - 100;
            }

            var physics2DData = objData.GetData<Physics2DData>();
            var isGround = IsGround(controllerData);
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

            var distanceY = Mathf.Abs(controllerData.groundY - controllerData.positionY);
            if ((isGround && deltaY < 0) || !isGround && Mathf.Abs(deltaY) > distanceY)
            {
                deltaY = deltaY > 0 ? distanceY : -distanceY;
            }

            _movePosition.x = (float)deltaX / Constant.UNITY_UNIT_TO_GAME_UNIT;
            _movePosition.y = (float)deltaY / Constant.UNITY_UNIT_TO_GAME_UNIT;

            controllerData.positionY += deltaY;

            var rb2d = controllerData.rigidbody2D;
            rb2d.MovePosition(rb2d.position + _movePosition);
        }

        public static bool IsGround(ActorController2DData controllerData)
        {
            return controllerData.groundY == controllerData.positionY;
        }
    }
}
