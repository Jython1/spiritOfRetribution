using UnityEngine;
using WeaponSwitcherScript;
using WeaponScript;
using PlayerControllerScript;
using System;


namespace WeaponControllerScript
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Input")]
        public PlayerController playerController;

        public WeaponSwitcher weaponSwitcher;

        public GameObject hands;

        public GameObject cameraPos;

        


        private void Start()
        {
            
            if (!playerController)
            return;
            playerController.onSwapWeaponsPressed += SwitchWeapon;
            playerController.onShootButtonPressed += Shoot;

            if(!hands)
            return;

            //hands.SetActive(false);

            
        }

        public void Shoot()
        {
            if(!weaponSwitcher.GetCurrentWeapon() || !weaponSwitcher.GetCurrentWeapon().GetComponent<Weapon>())
            return;

            var currentWeapon = weaponSwitcher.GetCurrentWeapon();
            var currentWeaponComp = currentWeapon.GetComponent<Weapon>();
            
            if(currentWeaponComp.GetAmmo() <= 0 || !currentWeaponComp.CanShoot()) 
            return;

            currentWeaponComp.Shoot();
            weaponSwitcher.animator.SetTrigger("Fire");
        }

        public void EquipWeapon(GameObject weapon)
        {
            if (!weapon || !weapon.GetComponent<Weapon>() || weaponSwitcher.IsWaiting()) 
            return;

            var currentGun = weaponSwitcher.GetCurrentWeapon();
            var gunInBack = weaponSwitcher.GetWeaponInBack();

            if(!currentGun && !gunInBack)
            {
                hands.SetActive(true);
                weapon.GetComponent<Weapon>().ShootCenter = cameraPos;
                weaponSwitcher.TakeNewWeaponInHand(weapon);
                  
                return;
            }

            if(currentGun && IsSameWeapon(weapon, currentGun))
            {
                AddAmmo(currentGun); 
                return;
            }

            if(gunInBack && IsSameWeapon(weapon, gunInBack))
            {
                AddAmmo(gunInBack);
                return;
            }

            if(!gunInBack && currentGun)
            {
                weaponSwitcher.AddWeaponBack(weapon);
                weapon.GetComponent<Weapon>().ShootCenter = cameraPos;
                return;
            }

            else if(gunInBack && !currentGun)
            {
                weaponSwitcher.TakeNewWeaponInHand(weapon);
                weapon.GetComponent<Weapon>().ShootCenter = cameraPos; 
                return;
            }
             
            
        }

        public void SwitchWeapon()
        {
            var currentGun = weaponSwitcher.GetCurrentWeapon();
            var gunInBack = weaponSwitcher.GetWeaponInBack();
            if(!currentGun && !gunInBack || weaponSwitcher.IsWaiting())
                return;


            if(currentGun && !gunInBack)
            {
                weaponSwitcher.PutWeaponBack();

                return;
            }
                
            
            else if(!currentGun && gunInBack)
            {
                weaponSwitcher.TakeWeaponFromBack();
                return;
            }
                
            else
            {
                weaponSwitcher.ToggleWeapons();
            }
                

        }

        private bool IsSameWeapon(GameObject newWeapon, GameObject compareWithWeapon)
        {
            var newWeaponComponent = newWeapon.GetComponent<Weapon>();
            var comparedWeaponComponent = compareWithWeapon.GetComponent<Weapon>();
            if(newWeaponComponent.name != comparedWeaponComponent.name)
                return false;

            return true;
        }

        private void AddAmmo(GameObject weapon)
        {
            Debug.Log("Add weapon");
            Destroy(weapon);
        }


    }

}