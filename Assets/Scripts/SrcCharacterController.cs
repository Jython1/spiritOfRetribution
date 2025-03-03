using UnityEngine;
public class SrcCharacterController : MonoBehaviour
{
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _mouseSensitivity = 5.0f;
    [SerializeField] float _jumpHeight = 2.0f;
    [SerializeField] private float verticalRotation = 0f;
    [SerializeField] private Vector3 _playerVelocity;

    UnityEngine.CharacterController  _controller;
    Vector3 _playerMotion;
    
    private Transform cameraTransform;
    
   
    private void OnEnable() 
    {
        _controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }


    private void Update()
    {
	
        PlayerMotion();
        PlayerVelocity();
        PlayerLookRight(Input.GetAxis("Mouse X") * _mouseSensitivity);
        PlayerLookUp(Input.GetAxis("Mouse Y")* _mouseSensitivity * -1);

        
    }

    bool isSprinting()
    {
        if(Input.GetButton("Fire3") && Input.GetAxis("Horizontal") != 0  && _controller.isGrounded ||
        Input.GetButton("Fire3") && Input.GetAxis("Vertical") != 0  && _controller.isGrounded)
        {
            return true;
        }
        return false;


    }


    void PlayerMotion()
    {
        if(_controller.isGrounded && Input.GetAxis("Horizontal")!=0 ||
        _controller.isGrounded && Input.GetAxis("Vertical")!=0)
        {
        _playerMotion.x = Input.GetAxis("Horizontal") * Speed();
        _playerMotion.z = Input.GetAxis("Vertical") * Speed();
        _playerMotion = transform.TransformDirection(_playerMotion );
        }

        /*
        else if(Input.GetAxis("Horizontal")!=0 && Input.GetButtonDown("Jump") && _controller.isGrounded ||
        Input.GetAxis("Vertical")!=0 && Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
             _playerMotion = transform.TransformDirection(_playerMotion );
        }*/

        //_playerMotion = transform.TransformDirection(_playerMotion );
    }

    void PlayerVelocity()
    {

        
        if (_controller.isGrounded && _playerMotion.y < 0)
        {
            _playerMotion.y = 0f;

        }

        _playerMotion.y += _gravity * Time.deltaTime;

        

        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            _playerMotion.y += Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
            
        }
    
        _controller.Move(_playerMotion * Time.deltaTime);


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

        verticalRotation += val;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler( verticalRotation , 0 , 0 );

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




}
