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

        MakeNoise();


        Vector3 origin = ShootCenter.transform.position;
        Vector3 baseDirection = CalculateSpread();
        Debug.DrawRay(origin, baseDirection, Color.red, fireDistance);

        RaycastHit hit;

        if (!Physics.Raycast(origin, baseDirection, out hit, fireDistance))
            return;


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
