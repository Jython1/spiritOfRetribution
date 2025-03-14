using UnityEngine;
using CharStats;

namespace HealthScript
{
    public class Health : MonoBehaviour
    {
        CharacterStats _characterStats;
        float currentHealth;
        [SerializeField] bool _isImmortal;
        


        void Awake()
        {
            if(!GetComponent<CharacterStats>())
            return;
            _characterStats = GetComponent<CharacterStats>();
            currentHealth = _characterStats.GetMaxHealth();
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.I))
            {
                GetDamage(600);
                Debug.Log(currentHealth);
            }
        }

        void GetDamage(float value)
        {
            if(_isImmortal != true)
            {
                float damage = value - ReducedDamageValue();
                damage = Mathf.Clamp(damage, 2, value);
                currentHealth = currentHealth - damage;
                currentHealth = Mathf.Clamp(currentHealth, 0, _characterStats.GetMaxHealth());
            }

        }

        void AddHealth(float value)
        {
            currentHealth += value;
            currentHealth = Mathf.Clamp(currentHealth, 0, _characterStats.GetMaxHealth());
        }

        public float ReducedDamageValue()
        {
            float maxHealth = _characterStats.GetMaxHealth();
            float armor = _characterStats.GetArmor();
            int level = _characterStats.GetLevel();
            float reducedDamageVal = maxHealth/(armor+level);
            return reducedDamageVal;
        }
        

    }
}
