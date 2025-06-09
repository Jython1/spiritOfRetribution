using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SkillScript;
using SkillSystemScript;
using SkillButtonScript;

namespace SkillTreeUIScript
{

    public class SkillTreeUI : MonoBehaviour
    {
        public GameObject skillButtonPrefab;
        public GameObject skillLinePrefab;

        public SkillSystem skillSystem;


        private void Awake()
        {
            
            if (!skillButtonPrefab || !skillLinePrefab || !skillSystem)
                return;

            
            skillSystem.OnLevelUp += AddNewSkills;
            
        }


        public void AddNewSkills()
        {
            Transform skillsContainer = gameObject.transform;
            Skill[] skills = skillSystem.assaultSkills;
            var currentLevel = skillSystem.playerStats.GetCurrentLevel();

            foreach (Transform child in skillsContainer)
                Destroy(child.gameObject);


            for (int i = 1; i <= currentLevel; i++)
            {
                GameObject skillLine = Instantiate(skillLinePrefab, skillsContainer);
                foreach (var skill in skills)
                {
                    if (skill.availableLevel != i)
                        continue;


                    GameObject buttonPrefab = Instantiate(skillButtonPrefab, skillLine.transform);
                    if (!buttonPrefab.GetComponent<SkillButton>())
                    return;

                    var skillButtonComp = buttonPrefab.GetComponent<SkillButton>();
                    skillButtonComp.SetInfluencedSkill(skill);
                }
                
            }


        }

    }

}