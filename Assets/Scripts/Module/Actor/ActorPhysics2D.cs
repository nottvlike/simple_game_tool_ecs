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
            var collider2D = Physics2D.OverlapBox(controllerData.groundCollider2D.transform.position,
                                                  controllerData.groundCollider2D.size, 0, LayerMask.GetMask(Constant.LAYER_GROUND_NAME));
            if (collider2D != null)
            {
                controllerData.isGround = true;
                LogUtil.I("controllerData ground name {0}", collider2D.gameObject.name);
            }
            else
            {
                controllerData.isGround = false;
            }

            LogUtil.I("controllerData.isGround {0}", controllerData.isGround);

            var physics2DData = objData.GetData<Physics2DData>();
            if (controllerData.isGround && physics2DData.force.x == 0 && physics2DData.force.y == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            var actorData = objData.GetData<ActorData>();
            var currentState = actorData.currentState;
            var forceY = physics2DData.force.y;
            if ((currentState & (int)ActorStateType.SkillDefault) == 0 && (currentState & (int)ActorStateType.SkillCustom) == 0)
            {
                forceY += -physics2DData.gravity;
            }

            var directionData = objData.GetData<DirectionData>();
            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
            var deltaX = directionData.direction.x * physics2DData.force.x * gameSystemData.unscaleDeltaTime;
            var deltaY = forceY * gameSystemData.unscaleDeltaTime;

            _movePosition.x = (float)deltaX / Constant.UNITY_UNIT_TO_GAME_UNIT;
            _movePosition.y = (float)deltaY / Constant.UNITY_UNIT_TO_GAME_UNIT;

            var rb2d = controllerData.rigidbody2D;
            rb2d.MovePosition(rb2d.position + _movePosition);
        }

        public static bool IsGround(ActorController2DData controllerData)
        {
            return controllerData.isGround;
        }
    }
}
