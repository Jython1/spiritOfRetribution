using UnityEngine;
using UnityEngine.UI;
using HealthScript;
using CharStats;
using TMPro;

namespace EnemyUIScript
{
    public class EnemyUI : MonoBehaviour
    {
        [Header("UI References")]
        public Image healthFill;
        public TextMeshProUGUI levelText;
        public GameObject bossIcon;
        public GameObject higherLevelIcon;

        [Header("External References")]
        public GameObject playerCamera;
        public CharacterStats playerStats;

        [Header("Colors")]
        public Color strongCharacterColor = new Color(1f, 0.2f, 0.2f);
        public Color normalCharacterColor = new Color(0.2f, 1f, 0.2f);
        public Color weakCharacterColor = new Color(0.7f, 0.7f, 0.7f);

        private Health _healthComp;
        private CharacterStats _ownerStats;
        private Transform _root;

        private void Start()
        {
            InitializeComponents();
            SetupInitialUI();
        }

        private void InitializeComponents()
        {
            _root = gameObject.transform.root;

            // Fixed: Remove duplicate check
            _healthComp = _root.GetComponent<Health>();
            if (_healthComp == null)
            {
                Debug.LogError($"Health component not found on {_root.name}");
                return;
            }

            _ownerStats = _root.GetComponent<CharacterStats>();
            if (_ownerStats == null)
            {
                Debug.LogError($"CharacterStats component not found on {_root.name}");
                return;
            }
        }

        private void SetupInitialUI()
        {
            if (_ownerStats == null) return;

            if (_ownerStats.Hierarchy == CharacterStats.CharacterHierarchy.Boss)
            {
                SetupBossUI();
            }
            else
            {
                SetupNormalUI();
            }
        }
        private void SetupBossUI()
        {
            levelText.gameObject.SetActive(false);
            higherLevelIcon.gameObject.SetActive(false);
            bossIcon.gameObject.SetActive(true);
            healthFill.color = strongCharacterColor;
        }

        private void SetupNormalUI()
        {
            bossIcon.gameObject.SetActive(false);
            UpdateLevelBasedUI();
        }

        private void Update()
        {
            
            UpdateHealthBar(_healthComp.GetCurrentHealth(), _ownerStats.GetMaxHealth());
            if (_ownerStats.Hierarchy != CharacterStats.CharacterHierarchy.Boss)
            {
                UpdateLevelBasedUI();
            }
        }


        private void UpdateHealthBar(float currentHealth, float maxHealth)
        {

            healthFill.fillAmount = currentHealth / maxHealth;

            if (currentHealth <= 0)
                Destroy(gameObject);
        }

        private void UpdateLevelBasedUI()
        {
            if (!playerStats)
                return;

            int levelDiff = _ownerStats.GetCurrentLevel() - playerStats.GetCurrentLevel();

            if (levelDiff >= 3)
            {
                SetStrongEnemyUI();
            }
            else if (levelDiff <= -3)
            {
                SetWeakEnemyUI();
            }
            else
            {
                SetNormalEnemyUI();
            }
        }

        private void SetStrongEnemyUI()
        {
            levelText.gameObject.SetActive(false);
            higherLevelIcon.gameObject.SetActive(true);
            healthFill.color = strongCharacterColor;
        }

        private void SetWeakEnemyUI()
        {
            levelText.gameObject.SetActive(true);
            higherLevelIcon.gameObject.SetActive(false);
            levelText.text = _ownerStats.GetCurrentLevel().ToString();
            healthFill.color = weakCharacterColor;
        }

        private void SetNormalEnemyUI()
        {
            levelText.gameObject.SetActive(true);
            higherLevelIcon.gameObject.SetActive(false);
            levelText.text = _ownerStats.GetCurrentLevel().ToString();
            healthFill.color = normalCharacterColor;
        }

        private void LateUpdate()
        {
            if (playerCamera != null)
            {
                // Billboard effect - face the camera
                Vector3 directionToCamera = transform.position - playerCamera.transform.position;
                transform.rotation = Quaternion.LookRotation(directionToCamera);
            }
        }

    }
}