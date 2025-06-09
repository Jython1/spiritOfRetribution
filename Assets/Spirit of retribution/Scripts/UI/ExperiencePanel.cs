using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SkillSystemScript;

namespace ExperiencePannelScript
{

    public class ExperiencePanel : MonoBehaviour
    {
        public TextMeshProUGUI experienceText;
        public TextMeshProUGUI currentLevelText;
        public Image experienceLine;
        public SkillSystem skillSystem;

        private float _smoothSpeed = 5f;
        private float _targetFill;


        private void Update()
        {
            if (!experienceText || !currentLevelText || !experienceLine || !skillSystem)
                return;
            _targetFill = skillSystem.GetValueForLine();
            experienceLine.fillAmount = Mathf.Lerp(experienceLine.fillAmount, _targetFill, Time.deltaTime * _smoothSpeed);
            experienceText.text = skillSystem.GetExperience().ToString();
            currentLevelText.text = skillSystem.playerStats.GetCurrentLevel().ToString();
        }  
    }

}