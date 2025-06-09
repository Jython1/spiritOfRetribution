using UnityEngine;
using CharStats;
using System;

namespace HealthScript
{
    public class Health : MonoBehaviour
    {
        CharacterStats _characterStats;
        private float _currentHealth;
        [SerializeField] bool _isImmortal;
        [SerializeField] bool _isDead;
        public event Action onDeath;


        void Awake()
        {
            if(!GetComponent<CharacterStats>())
            return;
            _characterStats = GetComponent<CharacterStats>();
            _currentHealth = _characterStats.GetMaxHealth();
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.I))
            {
                TakeDamage(40);
            }
        }

        public void TakeDamage(float value)
        {
            if(_isImmortal || _currentHealth <= 0)
            return;

            _currentHealth -= value;


            if(_currentHealth <= 0)
            {
                onDeath?.Invoke();
                _isDead = true;
            }

           /* {

                
                float damage = value - ReducedDamageValue();
                damage = Mathf.Clamp(damage, 2, value);
                currentHealth = currentHealth - damage;
                currentHealth = Mathf.Clamp(currentHealth, 0, _characterStats.GetMaxHealth());
            }*/

        }

        void AddHealth(float value)
        {
            _currentHealth += value;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _characterStats.GetMaxHealth());
        }

        public float ReducedDamageValue()
        {
            float maxHealth = _characterStats.GetMaxHealth();
            float armor = _characterStats.GetArmor();
            int level = _characterStats.GetCurrentLevel();
            float reducedDamageVal = maxHealth/(armor+level);
            return reducedDamageVal;
        }

        public bool isDead() => _isDead;

        public float GetCurrentHealth() => _currentHealth;

        public float GetMaxHealth() => _characterStats.GetMaxHealth();
        

    }
}
