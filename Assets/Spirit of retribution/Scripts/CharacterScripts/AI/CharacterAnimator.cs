using UnityEngine;

namespace CharacterAnimatorScript
{
    public class CharacterAnimator : MonoBehaviour
    {
        private Animator _animator;

        private Vector3 _lastPosition;
        
        void Start()
        {

            _animator = gameObject.GetComponent<Animator>();
            _lastPosition = transform.position;
        }

        private void Update()
        {
            GetCharacterSpeed();
            
        }

        private void GetCharacterSpeed()
        {
            if (!_animator)
                return;

            Vector3 deltaPosition = transform.position - _lastPosition;
            deltaPosition.y = 0f;
            float speed = deltaPosition.magnitude / Time.deltaTime;

            _animator.SetFloat("Speed", speed);

            _lastPosition = transform.position;

        }

        public void Crouch()
        {
            _animator.SetBool("isCrouching", true);
        }

        public void UnCrouch()
        {
            _animator.SetBool("isCrouching", false);
        }
    }

}