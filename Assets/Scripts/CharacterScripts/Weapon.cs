using UnityEngine;
using System.Collections;
using ShootingEvent;

namespace Gun
{
    public class Weapon : MonoBehaviour 
    {

        private bool _canShoot = true;
        [SerializeField] private WeaponData _weaponData;
        private float _currentAmmo;

        [SerializeField] private Transform _muzzle;


        private void OnEnable() {

            _currentAmmo = _weaponData.maxAmmo;
            CharacterShoot.OnShoot += Shoot;

        }

        private void OnDisable() {

            CharacterShoot.OnShoot -= Shoot;
        }

        void SetCurrentAmmo(int val)
        {
            _currentAmmo = val;
        }

        public void Shoot()
        {

            if(!_canShoot || _currentAmmo <= 0) return;

            Debug.Log(_currentAmmo);
            RaycastHit hit;
            if (Physics.Raycast(_muzzle.position, _muzzle.forward, out hit, _weaponData.distance))
            { 
                
                Debug.Log("Did Hit"); 
                Debug.DrawRay(_muzzle.position, _muzzle.forward * hit.distance, Color.green, 10f); 
            }

            else
            {
                Debug.DrawRay(_muzzle.position, _muzzle.forward * _weaponData.distance, Color.red, 10f);
            }
            
            _canShoot = false;
            _currentAmmo--;
            LimitAmmoVal();
            StartCoroutine(RecoilShooting());
            
        }


        public void LimitAmmoVal()
        {
            _currentAmmo = Mathf.Clamp(_currentAmmo, 0, _weaponData.maxAmmo);
        }

        private IEnumerator RecoilShooting()
        {
            yield return new WaitForSeconds(_weaponData.delayBetweenShots);
            _canShoot = true;
        }
        
    }


}
