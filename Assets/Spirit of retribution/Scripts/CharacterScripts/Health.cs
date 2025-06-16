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
            if (_isImmortal || _currentHealth <= 0)
                return;

            float reducedDamage = CalculateReducedDamage(
                value,
                _characterStats.GetArmor(),
                _characterStats.GetCurrentLevel(),
                _characterStats.Hierarchy == CharacterStats.CharacterHierarchy.Elite,
                _characterStats.Hierarchy == CharacterStats.CharacterHierarchy.Boss
            );

            _currentHealth -= reducedDamage;
            _currentHealth = Mathf.Max(_currentHealth, 0); // защита от отрицательных значений

            if (_currentHealth <= 0)
            {
                onDeath?.Invoke();
                _isDead = true;
            }

        }

        void AddHealth(float value)
        {
            _currentHealth += value;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _characterStats.GetMaxHealth());
        }

        private float CalculateReducedDamage(float baseDamage, float armor, int level, bool isElite, bool isBoss)
        {
            float baseReduction = armor / (armor + 50f + level * 5f);

            if (isElite)
                baseReduction += 0.2f;

            if (isBoss)
                baseReduction += 0.3f;

            baseReduction = Mathf.Clamp(baseReduction, 0f, 0.8f); // ограничение по максимуму

            float finalDamage = baseDamage * (1f - baseReduction);
            return finalDamage;
        }

        public bool isDead() => _isDead;

        public float GetCurrentHealth() => _currentHealth;

        public float GetMaxHealth() => _characterStats.GetMaxHealth();
        

    }
}
