using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider2D : MonoBehaviour
{
    public int[] buffList;
    public int[] buffRemoveWhenExitList;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Module.ActorAttack.Attack(gameObject, collision.gameObject, buffList);
        Module.ActorAttack.Attack(gameObject, collision.gameObject, buffRemoveWhenExitList);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Module.ActorAttack.Clear(collision.gameObject, buffRemoveWhenExitList);
    }
}
