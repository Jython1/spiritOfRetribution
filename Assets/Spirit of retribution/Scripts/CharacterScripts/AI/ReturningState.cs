using UnityEngine;
using CharacterStateInterface;
using AIController;

namespace ReturnStateScript
{
    public class ReturningState: ICharacterState 
    {
        private CharacterAIController _aiController;
        private Transform _returningPosition;

        public ReturningState(CharacterAIController aiController, Transform returningPosition)
        {
            _aiController = aiController;
            _returningPosition = returningPosition;

        }



        public void Execute()
        {

            var agent = _aiController.GetAgent();

            agent.speed = 3f;
            agent.acceleration = 4;
            agent.angularSpeed = 120f;
            agent.stoppingDistance = 0f; 
            agent.updateRotation = true; 
            agent.SetDestination(_returningPosition.position);

            float distanceToTarget = Vector3.Distance(agent.transform.position, _returningPosition.position);


            if (!agent.pathPending &&
                distanceToTarget <= 0.5f &&
                agent.velocity.sqrMagnitude < 0.01f)
            {
                _aiController.SetPatrol();
                _aiController.RemoveTargets();
            }

            
        }


    }


        
    

}