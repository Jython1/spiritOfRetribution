using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WeaponScript;
using WeaponControllerScript;

namespace WeaponPannelScript
{
    public class WeaponPannel : MonoBehaviour
    {
        public WeaponController weaponController;
        private Weapon _weaponComponent;
        private GameObject _currentWeapon;
        public Image GunIcon;
        public TextMeshProUGUI ammoCounter;
        public TextMeshProUGUI weaponName;

        private void Start() 
        {
            if(!weaponController)
            return;
            UpdateCurrentWeapon();
            weaponController.weaponSwitcher.onWeaponChanged += UpdateCurrentWeapon;
            
        }
        void Update()
        {
            UpdateCurrentAmmo();
        }
        
        private void UpdateCurrentAmmo()
        {

            if(!_currentWeapon || !_weaponComponent)
            {
                ammoCounter.text = "0";
                return;
            }

            ammoCounter.text = _weaponComponent.GetAmmo().ToString();

        }

        private void UpdateCurrentWeapon()
        {
            if(!weaponController || !weaponController.weaponSwitcher.GetCurrentWeapon()
            || !weaponController.weaponSwitcher.GetCurrentWeapon().GetComponent<Weapon>())
            {
                _currentWeapon = null;
                _weaponComponent = null;
                weaponName.text = "Руки";
                return;
            }
            

            _currentWeapon = weaponController.weaponSwitcher.GetCurrentWeapon();
            _weaponComponent = _currentWeapon.GetComponent<Weapon>();
            weaponName.text = _weaponComponent.name;

        }
    }

}