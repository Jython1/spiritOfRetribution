using UnityEngine;
using AIController;

namespace CharacterAttackScript
{
    public abstract class CharacterAttack : MonoBehaviour
    {
        protected GameObject _enemy;
        protected float _stoppingDistance;

        public virtual void Initialize(GameObject enemy)
        {
            _enemy = enemy;
        }
        public abstract void Execute();
        public float GetStoppingDistance() => _stoppingDistance;

    }

}