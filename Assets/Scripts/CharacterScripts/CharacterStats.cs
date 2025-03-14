using UnityEngine;

namespace CharStats
{
    public class CharacterStats : MonoBehaviour
    {

        [SerializeField] string name;
        [SerializeField] int level = 1;
        [SerializeField] float maxHealth;
        [SerializeField] float armor;
        [SerializeField] float endurance;



        private enum Side
        {
            Citizen,
            Spetsnaz,
            ZaulGardin,
            Ivi
        }

        [SerializeField] Side side;
        


        public float GetMaxHealth()
        {
            return maxHealth;
        }

        public float GetArmor()
        {
            return armor;
        }

        public float GetEndurance()
        {
            return endurance;
        }

        public int GetLevel()
        {
            return level;
        }

        public void IncreaseLevel()
        {
            level++;
        }

        public void SetName(string value)
        {
            name = value;
        }

        public string GetName()
        {
            return name;
        }

    }

}
