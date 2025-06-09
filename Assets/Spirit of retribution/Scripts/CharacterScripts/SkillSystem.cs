using UnityEngine;
using System;
using System.Collections.Generic;
using SkillScript;
using CharStats;

namespace SkillSystemScript
{
    public class SkillSystem : MonoBehaviour
    {
        public CharacterStats playerStats;

        public event Action OnLevelUp;

        public Skill[] assaultSkills;

        private int _experience;

        public enum SkillsBranch
        {
            NotChosen,
            Stealth,
            Assault
        }

        SkillsBranch skillsBranch;

        private int[] _xpRequeredForLevel = { 100, 150, 200, 300, 400, 550, 700, 900, 1150, 1500 };

        public float GetValueForLine()
        {
            var currentPlayerLevel = playerStats.GetCurrentLevel();

            if (currentPlayerLevel >= _xpRequeredForLevel.Length)
                return 1f;

            return (float)_experience / _xpRequeredForLevel[currentPlayerLevel];
        }

        public int GetExperience()
        {
            return _experience;
        }
        private void Start()
        {
            skillsBranch = SkillsBranch.NotChosen;


            //NewLevel();
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                increaseExperience(40);
            }
            
        }

        public void increaseExperience(int val)
        {
            var currentPlayerLevel = playerStats.GetCurrentLevel();



            if (currentPlayerLevel == 10 ||
            _experience == _xpRequeredForLevel[currentPlayerLevel-1])
                return;

            _experience += val;

            if (_experience >= _xpRequeredForLevel[currentPlayerLevel-1])
                NewLevel();

        }

        public void NewLevel()
        {
            playerStats.IncreaseLevel();
            _experience = _xpRequeredForLevel[playerStats.GetCurrentLevel() - 1] - _experience;
            OnLevelUp?.Invoke();
            

        }
        
        
        
    }

}