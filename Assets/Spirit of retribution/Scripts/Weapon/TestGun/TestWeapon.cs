using UnityEngine;
using WeaponScript;
using System.Collections;
using HealthScript;

public class TestWeapon : Weapon
{
    
    public override void Shoot()
    {

        _canShoot = false;
        StartCoroutine(Cooldown(0.1f));

        if (!isInfinityAmmo)
        ReduceAmmo(1);

        Vector3 origin = ShootCenter.transform.position; 
        Vector3 direction = ShootCenter.transform.forward;

        RaycastHit hit;

        if (!Physics.Raycast(origin, direction, out hit, 10f))
        return;

        Debug.DrawRay(origin, direction * 10f, Color.red, 10f);

        var enemyHealth = hit.collider.GetComponent<Health>();
        if (enemyHealth == null)
        return;

        enemyHealth.TakeDamage(10);


    }

    IEnumerator Cooldown(float cooldownval)
    {
        yield return new WaitForSeconds(cooldownval);
        _canShoot = true;
    }


}
