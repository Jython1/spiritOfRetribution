using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{

    private bool _canShoot;


    [SerializeField] private WeaponData weaponData;

    private void Start() {

        CharacterShoot.OnShoot += Shoot;
    }

    public void Shoot()
    {
        if(_canShoot != true)
        {
            Debug.Log("Shooting");
            _canShoot = false;
            Shoot();
            StartCoroutine(RecoilShooting());
        }
        
    }

    private IEnumerator RecoilShooting()
    {
        yield return new WaitForSeconds(weaponData.delayBetweenShots);
        _canShoot = true;
    }
    
}

