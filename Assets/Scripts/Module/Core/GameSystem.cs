using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class GameSystem : Module, IUpdateEvent
    {
        protected override void OnEnable()
        {
            WorldManager.Instance.UnityEventMgr.Add(this);
        }

        protected override void OnDisable()
        {
            WorldManager.Instance.UnityEventMgr.Remove(this);
        }

        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(GameSystemData));
        }

        public override void Refresh(ObjectData objData, bool notMet = false)
        {
            var gameSystemData = objData.GetData<GameSystemData>() as GameSystemData;

            gameSystemData.clientFrame++;

            var deltaTime = Mathf.CeilToInt(Time.unscaledDeltaTime * 1000);
            gameSystemData.unscaleDeltaTime = deltaTime;
            gameSystemData.unscaleTime += deltaTime;

            WorldManager.Instance.TimerMgr.Update();
        }

        public void Update()
        {
            var objData = WorldManager.Instance.GameCore;

            Refresh(objData);
        }
    }
}
