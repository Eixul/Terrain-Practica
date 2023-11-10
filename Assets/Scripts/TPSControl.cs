using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSControl : MonoBehaviour
{
    CharacterController _controller;
    Animator _animator;
    Transform _camera;

    float _horizontal;
    float _vertical;
    public GameObject _cameraNormal;
    [SerializeField] float _playerSpeed = 5;
    float _turnSmoothVelocity;
    [SerializeField] float _turnSmoothTime = 0.1f;

    [SerializeField] float _jumpHeigh = 1;
    float _gravity = -2f;

    Vector3 _playerGravity;
    //variables sensor
    [SerializeField] Transform _sensorPosition;
    [SerializeField] float _sensorRadius = 0.2f;
    [SerializeField] LayerMask _groundLayer;

    bool _isGrounded;
    
    private GameManager gamingManager;

    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main.transform;
        gamingManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if(Input.GetButton("Fire2") && gamingManager.dead == false)
        {
            AimMovement();
        }
        else if (gamingManager.dead == false)
        {
            Movement();            
        }

        if(gamingManager.dead == false)
        {
            Jump();
        }
        _animator.SetBool("IsJumping", !_isGrounded);

        Muerte();


        
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        _animator.SetFloat("VelX", 0);
        _animator.SetFloat("VelZ", direction.magnitude);

        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            _controller.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
        }
    }

    void AimMovement()
        {
            Vector3 direction = new Vector3(_horizontal, 0, _vertical);

            _animator.SetFloat("VelX", _horizontal);
            _animator.SetFloat("VelZ", _vertical);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +  _camera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _camera.eulerAngles.y, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            
            if(direction != Vector3.zero)
            {

                Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                _controller.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
            }
            
        }


    void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);

        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = -2;
        }
        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeigh * -2 * _gravity);
            _animator.SetBool("IsJumping", true);
        }
        _playerGravity.y += _gravity * Time.deltaTime;
        
        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void Muerte()
    {
        if(Input.GetKey(KeyCode.J) && gamingManager.dead == false)
        {
            gamingManager.GameOver();
            _animator.SetTrigger("IsDead");
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.layer == 7 && gamingManager.dead == false)
        {
            gamingManager.GameOver();
            _animator.SetTrigger("IsDead");
        }
    }
}
