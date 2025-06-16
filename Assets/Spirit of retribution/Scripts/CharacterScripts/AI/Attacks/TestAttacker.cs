using UnityEngine;
using System.Collections;
using WeaponSwitcherScript;
using CharacterAttackScript;
using AIController;
using WeaponScript;

public class TestAttacker : CharacterAttack
{
    //public TestAttacker(GameObject enemy) : base(enemy) { }


    public override void Execute()
    {
        _stoppingDistance = 10.0f;
        var enemyPosition = _enemy.transform.position;
        var currentPosition = gameObject.transform.position;
        float distanceToEnemy = Vector3.Distance(currentPosition, enemyPosition);
        if (distanceToEnemy >= 1.5f)
            FarAttack();

        else
            CloseAttack();
    }

    private void FarAttack()
    {
        //Debug.Log("Far attack");
    }

    private void CloseAttack()
    {
        //Debug.Log("Close attack");
    }
     
}
