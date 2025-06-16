using UnityEngine;
using WeaponControllerScript;
using NoiseCauser;

namespace WeaponScript
{
    public abstract class Weapon : MonoBehaviour
    {
        public string name;
        public float damage;
        public int maxAmmo;
        public int ammoOnStart;

        public float maxSpreadAngle = 5f;

        [Range(0f, 1f)]
        public float accuracy = 0.8f; 

        public float defaultNoiseRadius;
        public float fireDistance;
        private Rigidbody _rb;
        private bool _isEquiped;
        private int _currentAmmo;
        protected bool _canShoot;

        public bool isInfinityAmmo;

        private GameObject _shootCenter;
        private Noise _noiseEmitter;
        

        

        public enum WeaponType
        {
            Pistol = 0,
            Rifle = 1,
        }

        public WeaponType weaponType;
        public abstract void Shoot();


        private void Start()
        {
            if (!gameObject.GetComponent<Rigidbody>())
                return;

            _rb = gameObject.GetComponent<Rigidbody>();
            _canShoot = true;
            _currentAmmo = Mathf.Clamp(ammoOnStart, 0, maxAmmo);

            if (!gameObject.GetComponent<Noise>())
                return;

            _noiseEmitter = gameObject.GetComponent<Noise>();

        }

        protected Vector3 CalculateSpread()
        {
            float currentSpread = maxSpreadAngle * (1f - accuracy);

            Vector3 spreadDirection = Quaternion.Euler(
                Random.Range(-currentSpread, currentSpread),
                Random.Range(-currentSpread, currentSpread),
                0) * ShootCenter.transform.forward;

            return spreadDirection;
        }


        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                if (!col.gameObject.GetComponent<WeaponController>())
                    return;

                var weaponController = col.gameObject.GetComponent<WeaponController>();
                weaponController.EquipWeapon(gameObject);
                SetEquiped();
            }
        }

        public void SetEquiped()
        {
            foreach (Collider c in GetComponents<Collider>())
                c.enabled = false;

            _rb.isKinematic = true;
            _isEquiped = true;
        }

        public void SetUnequiped()
        {
            foreach (Collider c in GetComponents<Collider>())
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

        public void MakeNoise()
        {
            _noiseEmitter.MakeNoise(defaultNoiseRadius);

        }

    }

}