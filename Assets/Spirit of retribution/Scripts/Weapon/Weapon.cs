using UnityEngine;
using WeaponControllerScript;

namespace WeaponScript
{
    public abstract class Weapon : MonoBehaviour
    {
        public string name;
        public float damage;
        public int maxAmmo;
        public int ammoOnStart;
        public float fireRate;
        private Rigidbody _rb;
        private bool _isEquiped;
        private int _currentAmmo;
        protected bool _canShoot;

        public bool isInfinityAmmo;

        private GameObject _shootCenter;

        public enum WeaponType
        {
            Pistol = 0,
            Rifle = 1,
        }

        public WeaponType weaponType;
        public abstract void Shoot();


        private void Start() 
        {
            if(!gameObject.GetComponent<Rigidbody>())
            return;

            _rb = gameObject.GetComponent<Rigidbody>();
            _canShoot = true;
            _currentAmmo = Mathf.Clamp(ammoOnStart, 0, maxAmmo);

        }


        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                if(!col.gameObject.GetComponent<WeaponController>())
                return;

                var weaponController = col.gameObject.GetComponent<WeaponController>();
                weaponController.EquipWeapon(gameObject);
                SetEquiped(); 
            }
        }

        public void SetEquiped()
        {
            foreach(Collider c in GetComponents<Collider>()) 
                c.enabled = false;

            _rb.isKinematic = true;
        _isEquiped = true;
        }

        public void SetUnequiped()
        {
            foreach(Collider c in GetComponents<Collider>())
                c.enabled = true;

            _rb.isKinematic = false;
        _isEquiped = false;
        }

        public int GetAmmo()
        {
            return _currentAmmo;
        }

        public void AddAmmo(int val)
        {
            Debug.Log("Add ammo");
        }

        public bool CanShoot()
        {
            return _canShoot;
        }

        public void ReduceAmmo(int val)
        {
            _currentAmmo -= val;
        }

        public GameObject ShootCenter
        {
            get => _shootCenter;
            set => _shootCenter = value;
        }

    }

}