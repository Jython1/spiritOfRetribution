using UnityEngine;
using CharacterStateInterface;
using AIController; 

namespace CombatStateScript
{
    public class CombatState : ICharacterState
    {
        private CharacterAIController _aiController;
        private GameObject _enemy;
        private float _moveSpeed;
        private Vector3 _initialLocation;

        private Vector3 _attackOffset;
        private float _attackRange = 5f;


        public CombatState(CharacterAIController aiController, GameObject enemy)
        {
            _aiController = aiController;
            _enemy = enemy;
            _initialLocation = aiController.transform.position;

            Vector2 offset2D = Random.insideUnitCircle.normalized * UnityEngine.Random.Range(10f, 10f);
            _attackOffset = new Vector3(offset2D.x, 0f, offset2D.y);      
            
        }

        public void Execute()
        {
            if (!_enemy)
                return;

            var agent = _aiController.GetAgent();
            var enemyPosition = _enemy.transform.position;
            var currentPosition = _aiController.transform.position;

            Vector3 avoidanceOffset = EnemyManager.Instance.GetAvoidanceOffset(_aiController.transform, enemyPosition);
            Vector3 destination = enemyPosition + _attackOffset + avoidanceOffset;

            agent.speed = 3.5f;
            agent.acceleration = 8;
            agent.angularSpeed = 220f;
            agent.stoppingDistance = _aiController.GetCharacterAttack().GetStoppingDistance();
            agent.updateRotation = false;

            agent.SetDestination(enemyPosition);
            LookAtTarget(enemyPosition - currentPosition); 

        }


        private void LookAtTarget(Vector3 direction)
        {
            direction.y = 0f;

            if (direction != Vector3.zero) 
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _aiController.transform.rotation = Quaternion.Slerp(
                    _aiController.transform.rotation,
                    targetRotation,
                    Time.deltaTime * 5f);
            }

        }


    }

}