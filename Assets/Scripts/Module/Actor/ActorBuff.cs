using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorBuff : UpdateModule
    {
        public BaseAttributeInfo tmpAttribute;
        public ExtraAttributeInfo extraAttribute;

        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorData));
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
            if (buffList.Count == 0 || !CheckIfHasNeedRefreshBuff(buffList))
            {
                Stop(objData.ObjectId);
                return;
            }

            ResetAttributeInfo();

            var attributeData = objData.GetData<ActorAttributeData>();
            ResetExtraAttributeInfo(attributeData);

            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
            var actorData = objData.GetData<ActorData>();
            var isDirty = false;
            for (var i = 0; i < buffList.Count;)
            {
                var buff = buffList[i];

                if ((buff.time > 0 || buff.count > 0) &&
                    buff.lastUpdateTime + buff.interval < gameSystemData.unscaleTime)
                {
                    if (buff.count > 0)
                    {
                        isDirty = true;

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
                            case BuffType.NormalChangeHp:
                                {
                                    var hurt = buff.valueType == BuffValueType.Normal ? value : Mathf.FloorToInt(attributeData.baseAttribute.hp * value * Constant.PERCENT);
                                    if (hurt < 0)
                                    {
                                        hurt += attributeData.baseAttribute.def;
                                    }

                                    tmpAttribute.hp += hurt;
                                }
                                break;
                            case BuffType.ChangeHpMax:
                                {
                                    var actorInfo = WorldManager.Instance.ActorConfig.Get(actorData.actorId);
                                    extraAttribute.hpMax += value;

                                    var extraHp = (float)value / (actorInfo.attributeInfo.hp + attributeData.extraAttribute.hpMax) * attributeData.baseAttribute.hp;
                                    tmpAttribute.hp += Mathf.FloorToInt(extraHp);
                                }
                                break;
                        }

                        buff.count--;
                        buff.currentCount++;
                    }

                    if (buff.time > 0)
                    {
                        buff.time = Mathf.Max(0, buff.time - gameSystemData.unscaleDeltaTime);
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

            if (isDirty)
            {
                UpdateExtraAttributeInfo(attributeData, extraAttribute);

                UpdateBaseAttributeInfo(attributeData, tmpAttribute);

                objData.SetDirty(attributeData);
            }
        }

        public bool CheckIfHasNeedRefreshBuff(List<Buff> buffList)
        {
            for (var i = 0; i < buffList.Count; i++)
            {
                var buff = buffList[i];
                if (buff.count != 0 || buff.time > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void ResetAttributeInfo()
        {
            tmpAttribute.hp = 0;
            tmpAttribute.mp = 0;
            tmpAttribute.atk = 0;
            tmpAttribute.def = 0;
        }

        public void ResetExtraAttributeInfo(ActorAttributeData attributeData)
        {
            extraAttribute.hp = attributeData.extraAttribute.hp;
            extraAttribute.hpMax = attributeData.extraAttribute.hpMax;
            extraAttribute.mp = attributeData.extraAttribute.mp;
            extraAttribute.atk = attributeData.extraAttribute.atk;
            extraAttribute.def = attributeData.extraAttribute.def;
        }

        public static void UpdateBaseAttributeInfo(ActorAttributeData attributeData, BaseAttributeInfo attribute)
        {
            attributeData.baseAttribute.hp += attribute.hp;
            attributeData.baseAttribute.mp += attribute.mp;
            attributeData.baseAttribute.atk += attribute.atk;
            attributeData.baseAttribute.def += attribute.def;
        }

        public static void UpdateExtraAttributeInfo(ActorAttributeData attributeData, ExtraAttributeInfo attribute)
        {
            attributeData.extraAttribute.hp = attribute.hp;
            attributeData.extraAttribute.hpMax = attribute.hpMax;
            attributeData.extraAttribute.mp = attribute.mp;
            attributeData.extraAttribute.atk = attribute.atk;
            attributeData.extraAttribute.def = attribute.def;
        }

        public static void AddBuff(ActorBuffData buffData, int buffId)
        {
            var buffConfig = WorldManager.Instance.BuffConfig;
            var buff = buffConfig.Get(buffId);

            buffData.buffList.Add(buff);
        }

        public static void RemoveBuff(ObjectData objData, int buffId)
        {
            var worldMgr = WorldManager.Instance;

            var buffConfig = worldMgr.BuffConfig;
            var buff = buffConfig.Get(buffId);

            var actorData = objData.GetData<ActorData>();
            var actorInfo = worldMgr.ActorConfig.Get(actorData.actorId);

            var attributeData = objData.GetData<ActorAttributeData>();
            if (buff.time < 0)
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
                    case BuffType.ChangeHpMax:
                        {
                            var extraHp = (float)value / (actorInfo.attributeInfo.hp + attributeData.extraAttribute.hpMax) * attributeData.baseAttribute.hp;
                            attributeData.baseAttribute.hp -= Mathf.FloorToInt(extraHp);

                            attributeData.extraAttribute.hpMax -= value;
                        }
                        break;
                }
            }

            var buffData = objData.GetData<ActorBuffData>();
            var buffList = buffData.buffList;
            for (var i = 0; i < buffList.Count; i++)
            {
                var tmp = buffList[i];
                if (tmp.id == buffId)
                {
                    buffList.RemoveAt(i);
                    break;
                }
            }

            objData.SetDirty(buffData, attributeData);
        }

        public static void Attack(GameObject hurt, int[] attackBuffList)
        {
            var worldMgr = WorldManager.Instance;

            var battleData = worldMgr.GameCore.GetData<BattleData>();
            var objId = 0;
            if (battleData.hurtDictionary.TryGetValue(hurt, out objId))
            {
                var objData = worldMgr.GetObjectData(objId);
                var buffData = objData.GetData<ActorBuffData>();

                for (var i = 0; i < attackBuffList.Length; i++)
                {
                    var buffId = attackBuffList[i];
                    AddBuff(buffData, buffId);
                }

                objData.SetDirty(buffData);
            }
        }

        public static void Clear(GameObject hurt, int[] removeBuffList)
        {
            var worldMgr = WorldManager.Instance;

            var battleData = worldMgr.GameCore.GetData<BattleData>();
            var objId = 0;
            if (battleData.hurtDictionary.TryGetValue(hurt, out objId))
            {
                var objData = worldMgr.GetObjectData(objId);
                for (var i = 0; i < removeBuffList.Length; i++)
                {
                    var buffId = removeBuffList[i];
                    RemoveBuff(objData, buffId);
                }
            }
        }
    }
}
