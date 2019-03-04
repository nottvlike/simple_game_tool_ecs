using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class AttackCollider2D : MonoBehaviour
{
    Effect _effect;
    int _lastEnterTime;

    List<Buff> _buffList = new List<Buff>();

    public void Init(Effect effect)
    {
        _effect = effect;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var worldMgr = WorldManager.Instance;
        var gameSystemTime = worldMgr.GameCore.GetData<GameSystemData>();

        if (_lastEnterTime + _effect.duration < gameSystemTime.unscaleTime)
        {
            _lastEnterTime = gameSystemTime.unscaleTime;

            _buffList.Clear();

            var enemyBuffIdList = _effect.enemyBuffIdList;
            for (var i = 0; i < enemyBuffIdList.Length; i++)
            {
                var buffId = enemyBuffIdList[i];
                var buff = worldMgr.BuffConfig.GetBuff(buffId);
                _buffList.Add(buff);
            }

            Module.ActorAttack.Attack(gameObject, collision.gameObject, _buffList.ToArray());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var buffConfig = WorldManager.Instance.BuffConfig;
        _buffList.Clear();

        var enemyBuffIdList = _effect.enemyBuffIdList;
        for (var i = 0; i < enemyBuffIdList.Length; i++)
        {
            var buffId = enemyBuffIdList[i];
            var buff = buffConfig.GetBuff(buffId);
            if ((buff.buffAttribute & (int)BuffAttribute.NeedRemove) != 0)
            {
                _buffList.Add(buff);
            }
        }

        Module.ActorAttack.Clear(collision.gameObject, _buffList.ToArray());
    }
}
