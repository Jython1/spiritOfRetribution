using UnityEngine;
using CharacterStateInterface;
using AIController;
using CoverScript;

namespace CoverStateScript
{
    public class CoverState : ICharacterState
    {
        private readonly CharacterAIController _aiController;
        private readonly GameObject _cover;
        private readonly GameObject _enemy;
        private bool _hasReachedCover = false;

        public CoverState(CharacterAIController aiController, GameObject cover, GameObject enemy)
        {
            _aiController = aiController;
            _cover = cover;
            _enemy = enemy;
            
            Debug.Log($"[CoverState] Created - Target: {cover.name} at {cover.transform.position}");
        }

        public void Execute()
        {
            if (!_enemy || !_cover)
            {
                return;
            }

            var agent = _aiController.GetAgent();
            var currentPosition = _aiController.transform.position;
            var enemyPosition = _enemy.transform.position;
            var coverPosition = _cover.transform.position;

            // Debug agent status
            if (!agent.enabled)
            {
                return;
            }

            if (!agent.isOnNavMesh)
            {
                return;
            }

            ConfigureAgent(agent);
            
            agent.SetDestination(coverPosition);
            

            LookAt(enemyPosition - currentPosition);

            if (!_hasReachedCover && IsAtDestination(agent))
            {
                _hasReachedCover = true;
                
                var coverComponent = _cover.GetComponent<Cover>();
                if (coverComponent != null)
                {
                    coverComponent.TakeCover();
                    if (coverComponent.shouldCrouch)
                    {
                        _aiController.Crouch();
                    }
                }
            }
        }

        private bool IsAtDestination(UnityEngine.AI.NavMeshAgent agent)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.1f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ConfigureAgent(UnityEngine.AI.NavMeshAgent agent)
        {

            agent.speed = 3f;
            agent.radius = 1f;
            agent.acceleration = 8f;
            agent.angularSpeed = 220f;
            agent.stoppingDistance = 0.5f; 
            agent.updateRotation = false;
        }

        private void LookAt(Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.01f) return; // Use sqrMagnitude for better performance

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _aiController.transform.rotation = Quaternion.Slerp(
                _aiController.transform.rotation,
                targetRotation,
                Time.deltaTime * 5f);
        }
    }
}