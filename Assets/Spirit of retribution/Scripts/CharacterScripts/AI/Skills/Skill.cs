using UnityEngine;
using CharStats;

namespace SkillScript
{
    [CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
    public class Skill : ScriptableObject 
        
    {
        public string skillName;
        public string description;
        public Sprite icon; 
        public int availableLevel;

        public int maxUpgadePoints;
        

        public void Apply()
        {
            Debug.Log($"Применён скилл: {skillName}");
        }
    }
    

}