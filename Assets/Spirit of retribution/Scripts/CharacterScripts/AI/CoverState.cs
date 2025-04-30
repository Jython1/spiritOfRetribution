using UnityEngine;
using CharacterStateInterface;
using AIController;
using AttackStateScript;
using CoverScript;

namespace CoverStateScript
{
    public class CoverState : ICharacterState
    {
        private CharacterAIController _aiController;
        private GameObject _cover;
        private GameObject _enemy;
        private float _moveSpeed;
        private Vector3 _initialLocation;
        
        
        public CoverState(CharacterAIController aiController, GameObject cover, GameObject enemy)
        {
            _aiController = aiController;
            _cover= cover;
            _enemy = enemy;
            _initialLocation = aiController.transform.position;
        }

        public void Execute()
        {


            var agent = _aiController.GetAgent();
            var enemyPosition = _enemy.transform.position;
            var coverPosition = _cover.transform.position;
            var currentPosition = _aiController.transform.position;
            float distanceFromStartToEnemy = Vector3.Distance(_initialLocation, enemyPosition);
            float distanceToEnemy = Vector3.Distance(currentPosition, enemyPosition);
            LootAtTarget(enemyPosition - currentPosition);

            agent.speed = 3f;
            agent.acceleration = 8;
            agent.angularSpeed = 220f;
            agent.stoppingDistance = 0.02f; 

            agent.SetDestination(coverPosition);



            if(distanceToEnemy <= 10f)
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