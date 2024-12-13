using UnityEngine;

public class MovementController : MonoBehaviour
{
    private readonly float _gravity = Physics.gravity.y;

    [SerializeField]
    private CharacterController _controller;

    [SerializeField]
    private float _speed = 5;

    [SerializeField]
    private float _lookSpeed = 5;

    [SerializeField]
    private float _jumpPower = 1f;

    private Vector3 _movement;
    private float _yaw;
    private float _pitch;

    private void Update()
    {
        if (_controller.isGrounded)
        {
            _movement = transform.TransformDirection(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _movement.y = 0;
            _movement *= _speed;
            if (Input.GetButtonDown("Jump"))
            {
                _movement.y += Mathf.Sqrt(_jumpPower * -3.0f * _gravity);
            }
        }

        _movement.y += _gravity * Time.deltaTime;
        _controller.Move(_movement * Time.deltaTime);

        _yaw = (_yaw + _lookSpeed * Input.GetAxis("Mouse X")) % 360f;
        _pitch = Mathf.Clamp((_pitch - _lookSpeed * Input.GetAxis("Mouse Y")), -20, 65);
        transform.rotation = Quaternion.AngleAxis(_yaw, Vector3.up) * Quaternion.AngleAxis(_pitch, Vector3.right);
    }
}