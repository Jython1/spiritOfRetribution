using UnityEngine;
using CharacterStateInterface;
using AIController;

namespace CombatStateScript
{
    public class CombatState : ICharacterState
    {
        private readonly CharacterAIController _aiController;
        private readonly GameObject _enemy;

        public CombatState(CharacterAIController aiController, GameObject enemy)
        {
            _aiController = aiController;
            _enemy = enemy;
        }

        public void Execute()
        {
            if (_enemy == null)
                return;

            var agent = _aiController.GetAgent();
            var currentPosition = _aiController.transform.position;
            var enemyPosition = _enemy.transform.position;

            ConfigureAgent(agent);
            agent.SetDestination(enemyPosition);

            LookAt(enemyPosition - currentPosition);

            /*
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                _aiController.GetCharacterAttack().Attack(_enemy); // Предположительно
            }*/
        }

        private void ConfigureAgent(UnityEngine.AI.NavMeshAgent agent)
        {
            agent.speed = 3.5f;
            agent.radius = 2f;
            agent.acceleration = 8f;
            agent.angularSpeed = 220f;
            //agent.stoppingDistance = _aiController.GetCharacterAttack().GetStoppingDistance();
            agent.stoppingDistance = 10f;
            agent.updateRotation = false;
        }

        private void LookAt(Vector3 direction)
        {
            direction.y = 0f;

            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _aiController.transform.rotation = Quaternion.Slerp(
                _aiController.transform.rotation,
                targetRotation,
                Time.deltaTime * 5f);
        }
    }
}
