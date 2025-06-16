using UnityEngine;
using CharacterStateInterface;
using AIController;

namespace ReturnStateScript
{
    public class ReturningState : ICharacterState
    {
        private readonly CharacterAIController _aiController;
        private readonly Vector3 _returningPosition;

        private const float ArrivalThreshold = 0.5f;

        public ReturningState(CharacterAIController aiController, Transform returningTransform)
        {
            _aiController = aiController;
            _returningPosition = returningTransform.position;
        }

        public void Execute()
        {
            var agent = _aiController.GetAgent();

            ConfigureAgent(agent);
            agent.SetDestination(_returningPosition);

            if (HasArrived(agent))
            {
                _aiController.SetPatrolState();
                _aiController.RemoveTargets();
            }
        }

        private void ConfigureAgent(UnityEngine.AI.NavMeshAgent agent)
        {
            agent.speed = 3f;
            agent.acceleration = 4f;
            agent.angularSpeed = 120f;
            agent.stoppingDistance = 0f;
            agent.updateRotation = true;
        }

        private bool HasArrived(UnityEngine.AI.NavMeshAgent agent)
        {
            float distance = Vector3.Distance(agent.transform.position, _returningPosition);
            return !agent.pathPending && distance <= ArrivalThreshold && agent.velocity.sqrMagnitude < 0.01f;
        }
    }
}
