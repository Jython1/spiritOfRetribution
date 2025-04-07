using UnityEngine;
using System.Collections;
using ShootingEvent;  

namespace PlayerController 
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float _gravity = -9.81f;
        [SerializeField] float _mouseSensitivity = 5.0f;
        [SerializeField] float _jumpHeight = 2.0f;
        [SerializeField] private float _verticalRotation = 0f;
        [SerializeField] private Vector3 _playerMotion;
        private float _jumpDelay;
        private CharacterShoot _characterShoot;
        private CharacterController  _controller;
        private Transform _cameraTransform;
        private bool _canJump;
        
        private void OnEnable() 
        {
            _controller = GetComponent<CharacterController>();
            _cameraTransform = Camera.main.transform;
            _jumpDelay = 0.7f;
            _canJump = false;
            _characterShoot = GetComponent<CharacterShoot>();
        }


        private void Update()      
        {  
        
            PlayerMotion();
            PlayerVelocity();
            PlayerLookRight(Input.GetAxis("Mouse X") * _mouseSensitivity);
            PlayerLookUp(Input.GetAxis("Mouse Y")* _mouseSensitivity * -1);
            Shoot();
            
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

        void PlayerMotion()
        {
            if(_controller.isGrounded && Input.GetAxis("Horizontal")!=0 ||
            _controller.isGrounded && Input.GetAxis("Vertical")!=0)
            {
            _playerMotion.x = Input.GetAxis("Horizontal") * Speed();
            _playerMotion.z = Input.GetAxis("Vertical") * Speed();
            _playerMotion = transform.TransformDirection(_playerMotion );
            }

        }

        void PlayerVelocity()
        {
            if (_controller.isGrounded && _playerMotion.y < 0)
            {
                _playerMotion.y = -2f;
                StartCoroutine(DelayJump());
            }
            _playerMotion.y += _gravity * Time.deltaTime;

            if (Input.GetButtonDown("Jump") && _controller.isGrounded && _canJump)
            {
                _playerMotion.y += Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
            }
            _controller.Move(_playerMotion * Time.deltaTime);
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


        float Speed()
        {
            float speed = 10;
            if(Input.GetButton("Fire3") && _controller.isGrounded)
            {
                speed = 20;
            }
            return speed;
        }

        void Shoot()
        {
            if (Input.GetButton("Fire1"))
            {
                _characterShoot.Shooting();
            }
        }




    }
}
