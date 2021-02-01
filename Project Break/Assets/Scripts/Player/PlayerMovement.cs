using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] float CurrentMovementSpeed = 6;

    [Header("Movement")]
    CharacterController Controller;
    float TurnSmoothVelocity;
    [SerializeField] Transform Cam;
    [SerializeField] float TurnSmoothTime = 0.1f;
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, TurnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 MoveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            Controller.Move(MoveDir.normalized * CurrentMovementSpeed * Time.deltaTime);
            Controller.Move(new Vector3(0, -1, 0));
        }
    }
}