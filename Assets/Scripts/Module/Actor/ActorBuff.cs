using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorBuff : UpdateModule
    {
        public AttributeInfo tmpAttribute;

        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorAttributeData));
            _requiredDataTypeList.Add(typeof(ActorBuffData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ActorBuffData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var buffData = objData.GetData<ActorBuffData>();
            var buffList = buffData.buffList;
            if (buffList.Count == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            ResetAttributeInfo();

            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
            var attributeData = objData.GetData<ActorAttributeData>();
            var attribute = attributeData.baseAttribute;

            for (var i = 0; i < buffList.Count;)
            {
                var buff = buffList[i];

                if ((buff.time > 0 || buff.count > 0) &&
                    buff.lastUpdateTime + buff.interval < gameSystemData.unscaleTime)
                {
                    var value = 0;
                    if (buff.currentCount < buff.value.Length)
                    {
                        value = buff.value[buff.currentCount];
                    }
                    else
                    {
                        value = buff.value[0];
                    }

                    switch (buff.buffType)
                    {
                        case BuffType.NormalHurt:
                            {
                                var hurt = buff.valueType == ValueType.Normal ? value : attribute.hp * value;
                                if (hurt < 0)
                                {
                                    hurt += attribute.def;
                                }

                                tmpAttribute.hp += hurt;
                            }
                            break;
                    }

                    buff.currentCount++;

                    if (buff.time > 0)
                    {
                        buff.time = Mathf.Max(0, buff.time - gameSystemData.unscaleDeltaTime);
                    }

                    if (buff.count > 0)
                    {
                        buff.count--;
                    }

                    buff.lastUpdateTime = gameSystemData.unscaleTime;

                    buffList[i] = buff;
                }

                if (buff.time == 0 && buff.count == 0)
                {
                    buffList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            if (tmpAttribute.hp != 0 || tmpAttribute.mp != 0 || tmpAttribute.atk != 0 || tmpAttribute.def != 0)
            {
                attributeData.buffAttribute.hp = tmpAttribute.hp;
                attributeData.buffAttribute.mp = tmpAttribute.mp;
                attributeData.buffAttribute.atk = tmpAttribute.atk;
                attributeData.buffAttribute.def = tmpAttribute.def;

                UpdateActorAttributeInfo(attributeData);

                objData.SetDirty(attributeData);
            }
        }

        public void ResetAttributeInfo()
        {
            tmpAttribute.hp = 0;
            tmpAttribute.mp = 0;
            tmpAttribute.atk = 0;
            tmpAttribute.def = 0;
        }

        public static void UpdateActorAttributeInfo(ActorAttributeData attributeData)
        {
            var buffAttribute = attributeData.buffAttribute;

            attributeData.baseAttribute.hp += buffAttribute.hp;
            attributeData.baseAttribute.mp += buffAttribute.mp;
            attributeData.baseAttribute.atk += buffAttribute.atk;
            attributeData.baseAttribute.def += buffAttribute.def;
        }

        public static void Attack(GameObject hurt, int[] attackBuffList)
        {
            var battleData = WorldManager.Instance.GameCore.GetData<BattleData>();
            var objId = 0;
            if (battleData.hurtDictionary.TryGetValue(hurt, out objId))
            {
                var objData = WorldManager.Instance.GetObjectData(objId);
                var buffData = objData.GetData<ActorBuffData>();
                var buffList = buffData.buffList;

                var buffConfig = WorldManager.Instance.BuffConfig;
                for (var i = 0; i < attackBuffList.Length; i++)
                {
                    var buffId = attackBuffList[i];
                    var buff = buffConfig.Get(buffId);

                    buffList.Add(buff);
                }

                objData.SetDirty(buffData);
            }
        }
    }
}
