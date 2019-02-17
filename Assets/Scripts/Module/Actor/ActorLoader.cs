using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ActorLoader : Module
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(Physics2DData));
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
            _requiredDataTypeList.Add(typeof(ActorController2DData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ResourceStateData) || data.GetType() == typeof(ResourceData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            var resourceData = objData.GetData<ResourceData>();
            if (!string.IsNullOrEmpty(resourceData.resource) && resourceStateData.isGameObject)
            {
                resourceStateData.isLoaded = WorldManager.Instance.ResourceMgr.IsResourceLoaded(resourceData.resource);
                if (!resourceStateData.isInstantiated)
                {
                    LoadResource(objData, resourceStateData, resourceData);
                }
            }
        }

        void LoadResource(ObjectData objData, ResourceStateData resourceStateData, ResourceData resourceData)
        {
            var worldMgr = WorldManager.Instance;
            worldMgr.ResourceMgr.LoadAsync(resourceData.resource, delegate (Object obj)
            {
                resourceStateData.isInstantiated = true;
                resourceData.gameObject = worldMgr.PoolMgr.GetGameObject(resourceData.resource, obj);
                resourceData.gameObject.name = resourceStateData.name;
                resourceData.gameObject.transform.position = resourceData.initialPosition;

                var controller = objData.GetData<ActorController2DData>();
                var rigidbody2D = resourceData.gameObject.GetComponent<Rigidbody2D>();
                controller.rigidbody2D = rigidbody2D;
                controller.root = resourceData.gameObject.transform;
                controller.foot = resourceData.gameObject.transform.Find("Foot");
                controller.positionY = Mathf.RoundToInt(controller.foot.position.y);

                controller.hurt = resourceData.gameObject.transform.Find("Hurt");
                var battleData = WorldManager.Instance.GameCore.GetData<BattleData>();
                battleData.hurtDictionary.Add(controller.hurt.gameObject, objData.ObjectId);

                controller.attack = resourceData.gameObject.transform.Find("Attack").gameObject;
                controller.attack.SetActive(false);
            });
        }

        static List<Data.Data> _dataList = new List<Data.Data>();
        public static void ReplaceActor(ObjectData objData, string actorName, string newResource, params Data.Data[] changedDataList)
        {
            var worldMgr = WorldManager.Instance;
            _dataList.Clear();

            var resourceStateData = objData.GetData<ResourceStateData>();
            resourceStateData.isGameObject = true;
            resourceStateData.isInstantiated = false;
            resourceStateData.name = actorName;

            var resourceData = objData.GetData<ResourceData>();
            if (resourceData.gameObject != null)
            {
                worldMgr.PoolMgr.ReleaseGameObject(resourceData.resource, resourceData.gameObject);
                resourceData.gameObject = null;
            }
            resourceData.resource = newResource;

            var controller = objData.GetData<ActorController2DData>();
            controller.rigidbody2D = null;
            controller.foot = null;

            var battleData = worldMgr.GameCore.GetData<BattleData>();
            if (controller.hurt)
            {
                battleData.hurtDictionary.Remove(controller.hurt.gameObject);
                controller.hurt = null;
            }

            objData.RemoveData<ActorFlyData>();
            objData.RemoveData<ActorDashData>();
            objData.RemoveData<ActorStressData>();

            var actorData = objData.GetData<ActorData>();
            var actorInfo = worldMgr.ActorConfig.Get(actorData.actorId);
            actorData.defaultSkill = actorInfo.defaultSkill;
            if (actorData.defaultSkill == SkillDefaultType.Fly)
            {
                objData.AddData<ActorFlyData>();
            }
            else if (actorData.defaultSkill == SkillDefaultType.Dash)
            {
                objData.AddData<ActorDashData>();
            }
            else if (actorData.defaultSkill == SkillDefaultType.Stress)
            {
                objData.AddData<ActorStressData>();
            }

            var actorAttributeData = objData.GetData<ActorAttributeData>();
            actorAttributeData.baseAttribute = actorInfo.attributeInfo;

            _dataList.Add(resourceStateData);
            _dataList.Add(resourceData);
            _dataList.Add(controller);
            _dataList.Add(actorData);
            _dataList.AddRange(changedDataList);

            objData.SetDirty(_dataList.ToArray());
        }
    }
}
