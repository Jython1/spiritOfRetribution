using UnityEngine;
using CharacterStateInterface;
using AIController;

namespace MoveStateScript
{
    public class MoveState : ICharacterState
    {
        private CharacterAIController _aiController;
        private Vector3 _moveToPosition;
        public MoveState(CharacterAIController aiController, Vector3 moveToPosition)
        {
            _aiController = aiController;
            _moveToPosition = moveToPosition;
        }


        public void Execute()
        {
            //Debug.Log(_moveToPosition);
            var agent = _aiController.GetAgent();
            agent.SetDestination(_moveToPosition);
            agent.radius = 2f;
            agent.stoppingDistance = 0f;
            agent.speed = 3.5f;
            agent.acceleration = 8;
            agent.angularSpeed = 220f;
            agent.updateRotation = true;
        }
        
    }

}