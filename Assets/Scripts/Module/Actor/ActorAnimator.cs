using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ActorAnimator : Module
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorData));
            _requiredDataTypeList.Add(typeof(ActorController2DData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
 
            _requiredDataTypeList.Add(typeof(DirectionData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            var type = data.GetType();
            return type == typeof(ActorData) || type == typeof(DirectionData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var resourceData = objData.GetData<ResourceData>();

            var directionData = objData.GetData<DirectionData>();
            Vector3 scale = resourceData.gameObject.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * directionData.direction.x;
            resourceData.gameObject.transform.localScale = scale;

            var actorData = objData.GetData<ActorData>();
            if ((actorData.currentState & (int)ActorStateType.Hurt) != 0)
            {
                // play hurt animation;
            }
            else if ((actorData.currentState & (int)ActorStateType.SkillCustom) != 0)
            {
                // play custom skill animation;
            }
            else if ((actorData.currentState & (int)ActorStateType.SkillDefault) != 0)
            {
                // play default skill animation;
            }
            else if ((actorData.currentState & (int)ActorStateType.Jump) != 0)
            {
                // play jump animation;
            }
            else if ((actorData.currentState & (int)ActorStateType.Move) != 0)
            {
                // play move animation;
            }
            else
            {
                // play idle animation;
            }
        }
    }
}