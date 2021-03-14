using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Interactables")]
    public bool CanMove = true;
    [SerializeField] GameObject Camera;

    [Header("Scenes")]
    [SerializeField] SceneHandler scenemanager;

    [Header("Speeds")]
    [SerializeField] float CurrentMovementSpeed = 0;
    [SerializeField] float RunningSpeed = 12;
    [SerializeField] float WalkingSpeed = 6;

    [Header("Movement")]
    [SerializeField] Animator Anim;
    CharacterController Controller;
    float TurnSmoothVelocity;
    [SerializeField] Transform Cam;
    [SerializeField] float TurnSmoothTime = 0.1f;

    [Header("Health")]
    public int Health;

    void Start()
    {
        Controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (Time.timeScale == 1)
        {
            Movement();
            Running();
        }
    }
    void Movement()
    {
        if (CanMove)
        {
            Camera.SetActive(true);
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

            if (dir.magnitude >= 0.1f)
            {
                Anim.SetBool("Walk", true);
                float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, TurnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                Vector3 MoveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                Controller.Move(MoveDir.normalized * CurrentMovementSpeed * Time.deltaTime);
            }
            else
                Anim.SetBool("Walk", false);
            Controller.Move(new Vector3(0, -1, 0));
        }else
        {
            Anim.SetBool("Walk",false);
            Camera.SetActive(false);
        }
    }                         // Applys Movement
    void Running()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            CurrentMovementSpeed = RunningSpeed;
        }
        else
        {
            CurrentMovementSpeed = WalkingSpeed;
        }
    }                          // Allows Player To run
    public void startBattle(int advantage)
    {
        PlayerPrefs.SetInt("Battle",advantage);
        StartCoroutine(scenemanager.ChangeScene(3));
    }
}
