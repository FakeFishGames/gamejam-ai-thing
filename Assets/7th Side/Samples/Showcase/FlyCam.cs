using UnityEngine;

public class FlyCam : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 5;

    [SerializeField]
    private float _lookSpeed = 5;

    private float _yaw;
    private float _pitch;

    private void Update()
    {
        var forward = Input.GetAxisRaw("Vertical");
        var right = Input.GetAxisRaw("Horizontal");
        var up = ((Input.GetKey(KeyCode.Q) ? 1f : 0f) - (Input.GetKey(KeyCode.E) ? 1f : 0f));
        transform.position += (transform.forward * forward + transform.right * right + Vector3.up * up) * (_movementSpeed * Time.deltaTime);

        _yaw = (_yaw + _lookSpeed * Input.GetAxis("Mouse X")) % 360f;
        _pitch = (_pitch - _lookSpeed * Input.GetAxis("Mouse Y")) % 360f;
        transform.rotation = Quaternion.AngleAxis(_yaw, Vector3.up) * Quaternion.AngleAxis(_pitch, Vector3.right);
    }
}