using UnityEngine;

namespace CharStats
{
    public class CharacterStats : MonoBehaviour
    {

        [SerializeField] string _name;
        [SerializeField] int _currentLevel;
        [SerializeField] float _maxHealth;
        [SerializeField] float _armor;
        [SerializeField] float _endurance;
        [SerializeField] CharacterSide _characterSide;
        public enum CharacterHierarchy
        {
            [InspectorName("Босс")]
            Boss,

            [InspectorName("Элита")]
            Elite,

            [InspectorName("Обычный персонаж")]
            Normal
        }
        [SerializeField] private CharacterHierarchy _hierarchy = CharacterHierarchy.Normal;


        public CharacterHierarchy Hierarchy
        {
            get => _hierarchy;
            set => _hierarchy = value;
        }

        public float GetMaxHealth()
        {
            return _maxHealth;
        }

        public float GetArmor()
        {
            return _armor;
        }

        public float GetEndurance()
        {
            return _endurance;
        }

        public int GetCurrentLevel()
        {
            return _currentLevel;
        }

        public void IncreaseLevel()
        {
            _currentLevel++;
        }

        public void SetName(string value)
        {
            _name = value;
        }

        public string GetName()
        {
            return _name;
        }

        public CharacterSide GetCharacterSide()
        {
            return _characterSide;
        }


    }

}
