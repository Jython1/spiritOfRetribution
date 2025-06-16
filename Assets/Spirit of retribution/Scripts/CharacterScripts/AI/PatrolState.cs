using UnityEngine;
using CharacterStateInterface;
using AIController;

namespace PatrolStateScript
{
    public class PatrolState : ICharacterState 
    {
        private CharacterAIController _aiController;
        private Transform[] _patrolPoints;
        private int _currentIndex;
        private float _waitTime;
        private float _waitTimer;
        private float _moveSpeed;
        private int _currentPointIndex;



    

        public PatrolState(CharacterAIController aiController, Transform[] patrolPoints, float waitTimeBetweenPoints = 3f)
        {
            _patrolPoints = patrolPoints;
            _waitTime = waitTimeBetweenPoints;
            _aiController = aiController;
            _currentPointIndex = -1;

        }



        public void Execute()
        {
            
            if (_patrolPoints.Length <= 0)
                return;

            var agent = _aiController.GetAgent();
            agent.radius = 0.45f;
            agent.speed = 1f;
            agent.acceleration = 4;
            agent.angularSpeed = 120f;
            agent.stoppingDistance = 0.02f; 


            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                _waitTimer += Time.deltaTime;

                if (_waitTimer >= _waitTime)
                {
                    MoveToNextPoint();
                    _waitTimer = 0f;
                    
                    
                }
            }
            else
            {
                _waitTimer = 0f;
                
            }
            
        }

        private void MoveToNextPoint()
        {
            if (_patrolPoints.Length == 0) return;

            var agent = _aiController.GetAgent();

            _currentPointIndex = (_currentPointIndex + 1) % _patrolPoints.Length;
            agent.SetDestination(_patrolPoints[_currentPointIndex].position);
        }

    }

}