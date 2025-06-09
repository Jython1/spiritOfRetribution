using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SkillScript;

namespace SkillButtonScript
{
    public class SkillButton : MonoBehaviour
    {
        public TMP_Text skillUpgradeText;

        private int _currentUpgradePoints;

        private Button _button;

        private Skill _influencedSkill;

        void Start()
        {
            if (!gameObject.GetComponent<Button>())
                return;

            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);

            skillUpgradeText.text = $"{_currentUpgradePoints}/{_influencedSkill.maxUpgadePoints}";

        }
        void OnButtonClick()
        {
            if (_currentUpgradePoints == _influencedSkill.maxUpgadePoints)
            return;

            _currentUpgradePoints++;
            skillUpgradeText.text = $"{_currentUpgradePoints}/{_influencedSkill.maxUpgadePoints}";
            Debug.Log("Кнопка нажата!");
            // Тут твоя логика
        }

        public void SetInfluencedSkill(Skill skill)
        {
            _influencedSkill = skill;
        }


    }

}