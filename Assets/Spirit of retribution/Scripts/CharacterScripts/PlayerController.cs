using UnityEditor;
using UnityEngine;
using System;
using NoiseCauser;
using System.Collections;

namespace PlayerControllerScript
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float _mouseSensitivity = 5.0f;
        [SerializeField] float _jumpHeight = 2.0f;
        [SerializeField] private float _verticalRotation = 0f;
        [SerializeField] private Vector3 _playerMotion;
        private float _jumpDelay;
        private Rigidbody _rigidbody;
        private Transform _cameraTransform;
        private bool _canJump;
        private Noise _noiseMaker;

        public Animator animator;
        public event Action onSwapWeaponsPressed;
        public event Action onShootButtonPressed;

        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float stamina;
        [SerializeField] private float staminaDrainRate = 15f;  
        [SerializeField] private float staminaRecoveryRate = 10f;
        [SerializeField] private float minStaminaToSprint = 10f;

        [SerializeField] private float groundCheckDistance = 0.1f;
        [SerializeField] private LayerMask groundLayer;
private bool isGrounded;



        private void OnEnable()
        {
            _cameraTransform = Camera.main.transform;
            _jumpDelay = 0.7f;
            _canJump = false;
            _rigidbody = GetComponent<Rigidbody>();
            stamina = maxStamina;

            if (!gameObject.GetComponent<Animator>())
                return;

        }


        private void Update()
        {

            PlayerLookRight(Input.GetAxis("Mouse X") * _mouseSensitivity);
            PlayerLookUp(Input.GetAxis("Mouse Y") * _mouseSensitivity * -1);
            Shoot();
            Jump();

            if (Input.GetKeyDown(KeyCode.F))
            {
                //_noiseMaker.MakeNoise();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                onSwapWeaponsPressed?.Invoke();
            }
            HandleStamina();
            CheckIsGrounded();

        }

        private void FixedUpdate()
        {
            MovePlayer();

        }

        void CheckIsGrounded()
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        }

        


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

        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Vector3 velocity = _rigidbody.linearVelocity;
                velocity.y = 0;
                _rigidbody.linearVelocity = velocity;

                _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(2 * _jumpHeight * Physics.gravity.magnitude), ForceMode.VelocityChange);

            }

        }


        IEnumerator DelayJump()
        {
            yield return new WaitForSeconds(_jumpDelay);
            _canJump = true;
        }

        void PlayerLookRight(float val)
        {
            if (val != 0)
            {
                transform.eulerAngles += new Vector3(0, val, 0);
            }
        }

        void PlayerLookUp(float val)
        {
            _verticalRotation += val;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
            _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
        }


        float GetSpeed()
        {
            float speed = 10;
            if (Input.GetButton("Fire3") && stamina > minStaminaToSprint && GetMovementDirection() != Vector3.zero)
            {
                speed = 20;
            }
            return speed;
        }

        void Shoot()
        {
            if (Input.GetButton("Fire1"))
            {
                onShootButtonPressed?.Invoke();

            }
        }
        
        void HandleStamina()
        {
            if (Input.GetButton("Fire3") && stamina > minStaminaToSprint && GetMovementDirection() != Vector3.zero)
            {
                stamina -= staminaDrainRate * Time.deltaTime;
                if (stamina < 0) stamina = 0;
            }
            else
            {
                stamina += staminaRecoveryRate * Time.deltaTime;
                if (stamina > maxStamina) stamina = maxStamina;
            }
        }




    }
}
