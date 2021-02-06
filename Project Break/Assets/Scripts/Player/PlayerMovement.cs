using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Enemys")]
    bool EnemysInRange;
    [SerializeField] LayerMask EnemyLayer;
    float ClosestEnemy;
    [SerializeField] SeachEnemy[] AllEnemys = null;

    [Header("Health")]
    [SerializeField] float Health;
    [SerializeField] float Defense;

    [Header("Speeds")]
    [SerializeField] float CurrentMovementSpeed = 6;
    [SerializeField] float RunningSpeed = 12;
    [SerializeField] float WalkingSpeed = 6;

    [Header("Movement")]
    CharacterController Controller;
    float TurnSmoothVelocity;
    [SerializeField] Transform Cam;
    [SerializeField] float TurnSmoothTime = 0.1f;

    [Header("Attack")]
    [SerializeField] GameObject AttackButton;
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
         if(AllEnemys != null)
            FindClosestEnemy();
        if (ClosestEnemy < 3)
        { 
            EnemysInRange = true;
        } 
        else 
        {
            EnemysInRange = false;
        }


        if (EnemysInRange)
            AttackInRange();


        Running();
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
        }
        Controller.Move(new Vector3(0, -1, 0));
    }
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
    }
    public void RecieveDamage(float damage)
    {
        Health -= damage;
    }
    public bool IsDead()
    {
        if(Health > 0)
            return false;

        if (Health <= 0)
            return true;


        Debug.LogError("Couldn't determine Death");
        return false;
    }
    void AttackInRange()
    {
        if(AllEnemys != null)
        AttackButton.SetActive(true);

        if (Input.GetKeyDown(KeyCode.G))
        {
            JumpOnEnemy(FindClosestEnemy());
        }
    }
    SeachEnemy FindClosestEnemy()
    {
        float DistanceBetweenEnemys = float.MaxValue;
        SeachEnemy Closet = null;
        for (int i = 0; i < AllEnemys.Length; i++)
        {
            if(AllEnemys[i] != null)
                if (Vector3.Distance(transform.position, AllEnemys[i].transform.position) <= DistanceBetweenEnemys)
                {
                    Closet = AllEnemys[i];
                    DistanceBetweenEnemys = Vector3.Distance(transform.position, AllEnemys[i].transform.position);
                }
        }
        return Closet;
    }
    void JumpOnEnemy(SeachEnemy Enemy)
    {
        if (Enemy == null)
        {
            Debug.Log("No Enemy Near By");
            return;
        }
        Debug.Log("Jumping on " + Enemy.name);
        Enemy.SummonReinforcements(Random.Range(15,25));
    }
}