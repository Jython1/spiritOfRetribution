using UnityEditor;
using UnityEngine;
using System.Collections;
using ShootingEvent;  

namespace PlayerController 
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float _mouseSensitivity = 5.0f;
        [SerializeField] float _jumpHeight = 2.0f;
        [SerializeField] private float _verticalRotation = 0f;
        [SerializeField] private Vector3 _playerMotion;
        private float _jumpDelay;
        private CharacterShoot _characterShoot;
        private Rigidbody  _rigidbody;
        private Transform _cameraTransform;
        private bool _canJump;
        
        private void OnEnable() 
        {
            _cameraTransform = Camera.main.transform;
            _jumpDelay = 0.7f;
            _canJump = false;
            _rigidbody = GetComponent<Rigidbody>();
        }


        private void Update()      
        {  
        
            PlayerLookRight(Input.GetAxis("Mouse X") * _mouseSensitivity);
            PlayerLookUp(Input.GetAxis("Mouse Y")* _mouseSensitivity * -1);
            Shoot();
            
        }

        private void FixedUpdate() {
            MovePlayer();
            
        }

    /*
        bool IsSprinting() 
        {
            if(Input.GetButton("Fire3") && Input.GetAxis("Horizontal") != 0  && _controller.isGrounded ||
            Input.GetButton("Fire3") && Input.GetAxis("Vertical") != 0  && _controller.isGrounded)
            {
                return true;
            }
            return false;


        }
    */

        Vector3 GetMovementDirection()
        {

            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");
            return new Vector3(moveX, 0f, moveZ).normalized;

        }

        void MovePlayer()
        {
            Vector3 inputDirection = GetMovementDirection();
            Vector3 worldDirection = transform.TransformDirection(inputDirection); 
            _rigidbody.MovePosition(_rigidbody.position + worldDirection * GetSpeed() * Time.fixedDeltaTime);
        }

        void PlayerVelocity()
        {

        }

        IEnumerator DelayJump()
            {
                yield return new WaitForSeconds(_jumpDelay);
                _canJump = true;
            }

        void PlayerLookRight(float val)
        {
            if(val != 0)
            {
                transform.eulerAngles += new Vector3( 0 , val , 0 );
            }
        }

        void PlayerLookUp(float val)
        {
            _verticalRotation += val;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
            _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation , 0 , 0);
        }


        float GetSpeed()
        {
            float speed = 10;
            if(Input.GetButton("Fire3"))
            {
                speed = 20;
            }
            return speed;
        }

        void Shoot()
        {
            if (Input.GetButton("Fire1") && _characterShoot != null)
            {
                _characterShoot.Shooting();
            }
        }




    }
}
