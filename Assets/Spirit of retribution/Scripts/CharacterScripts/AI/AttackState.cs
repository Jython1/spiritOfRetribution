using UnityEngine;
using CharacterStateInterface;
using AIController;

namespace AttackStateScript
{
    public class AttackState : ICharacterState 
    {
        private CharacterAIController _aiController;
        private GameObject _enemy;
        private float _moveSpeed;
        private Vector3 _initialLocation;

        public AttackState(CharacterAIController aiController, GameObject enemy)
        {
            _aiController = aiController;
            _enemy = enemy;
            _initialLocation = aiController.transform.position;
        }

        public void Execute()
        {


            var agent = _aiController.GetAgent();
            var enemyPosition = _enemy.transform.position;
            var currentPosition = _aiController.transform.position;
            float distanceToEnemy = Vector3.Distance(currentPosition, enemyPosition);
            LootAtTarget(enemyPosition - currentPosition);

            agent.speed = 3.5f;
            agent.acceleration = 8;
            agent.angularSpeed = 220f;
            agent.stoppingDistance = 5f; 

            agent.SetDestination(enemyPosition);

            if(distanceToEnemy <= 10)
            {
                Attack();
            }

        }

        private void Attack()
        {
            //Debug.Log("Attacking");
        }

        private void LootAtTarget(Vector3 direction)
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