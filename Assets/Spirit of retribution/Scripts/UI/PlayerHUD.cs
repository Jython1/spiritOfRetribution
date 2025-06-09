using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HealthScript;
using PlayerControllerScript;
using System;


namespace playerHudScript
{
    public class PlayerHUD : MonoBehaviour
    {
        public PlayerController playerController;
        public Health playerHealth;
        public Image healthImage;  
        public TextMeshProUGUI healthValue;



        
        private float _targetFill;
        private float _smoothSpeed = 5f;
        private float _currentHealth;
        private float _maxHealth;


        private void Start() 
        {

        }


        void Update()
        {
            UpdateHealth();

        }

        private void UpdateHealth()
        {
            if(!playerHealth)
            return; 
            _currentHealth = playerHealth.GetCurrentHealth();
            _maxHealth = playerHealth.GetMaxHealth();
            _targetFill = (_maxHealth > 0f) ? _currentHealth / _maxHealth : 0f;
            healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, _targetFill, Time.deltaTime * _smoothSpeed);
            healthValue.text = _currentHealth.ToString("F0");

        }

   
    }

}