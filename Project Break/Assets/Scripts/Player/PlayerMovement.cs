using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Debug Mode")]
    [SerializeField] bool ShowAttackSize;
    [SerializeField] bool InfHelth;
    [SerializeField] bool InfCharge;
    [SerializeField] bool InfDefence;
    [SerializeField] bool InfBullets;
    [SerializeField] bool InfChance;
    [SerializeField] GameObject DemonAttackShow;

    [Header("Guns")]
    [SerializeField] float MaxDamage;
    [SerializeField] float MinDamage;
    [SerializeField] int BulletsLeft;

    [Header("Demons")]
    public ScriptableDemon CurrentDemon;
    [SerializeField] Inventory inv;

    [Header("States")]
    public bool Hidden;
    public bool Fighting;

    [Header("Enemys")]
    bool EnemysInRange;
    float ClosestEnemy;
    [SerializeField] SeachEnemy[] AllEnemys = null;

    [Header("Health")]
    [SerializeField] float Health;
    [SerializeField] float Defense;

    [Header("Speeds")]
    [SerializeField] float CurrentMovementSpeed = 0;
    [SerializeField] float RunningSpeed = 12;
    [SerializeField] float WalkingSpeed = 6;

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
        InputsAndOther();
        EnemyRange();
        Movement();
        Running();
        DebugMode();
    }
    public void SetPlayerState(bool fight)
    {
        Fighting = !fight;
        Hidden = fight;
    }  // Sets Current Status Of Player [Hidden,Fighting]
    void DebugMode()
    {
        if (ShowAttackSize)
            DemonAttackShow.transform.localScale = new Vector3(GetComponent<UseDemon>().Reach, GetComponent<UseDemon>().Reach, GetComponent<UseDemon>().Reach);

        if (InfHelth)
            Health = Mathf.Infinity;

        if (InfCharge)
            CurrentDemon.Charge = Mathf.Infinity;

        if (InfDefence)
            Defense = Mathf.Infinity;

        if (InfBullets)
            BulletsLeft = Mathf.RoundToInt(Mathf.Infinity);

        if(InfChance)
            for (int i = 0; i < FindObjectsOfType<AttackPlayerEnemy>().Length; i++)
            {
                FindObjectsOfType<AttackPlayerEnemy>()[i].Chance = 0;
            }

    }                        // Debug mode Duh!
    void InputsAndOther()
    {
        if (Input.GetKeyDown(KeyCode.T) && !Fighting)
            GetComponent<UseDemon>().Strike(CurrentDemon);
        
        if (FindObjectsOfType<AttackPlayerEnemy>().Length == 0)
            SetPlayerState(false);

        if (Input.GetKeyDown(KeyCode.E))
            inv.ChangeDemon(true);

        if (Input.GetKeyDown(KeyCode.Q))
            inv.ChangeDemon(false);

        if (Input.GetKeyDown(KeyCode.R))
        {
            bool AlrUsed = false;

            if (CurrentDemon.Charge == 100)
                return;

            if (inv.Items.Count == 0)
                return;

            for (int i = 0; i < inv.Items.Count; i++)
            {
                if (!AlrUsed)
                {
                    if (inv.Items[i].Name == "Charge Pot")
                    {
                        UseChargePot(inv.Items[i].ChargeBy);
                        inv.Items.Remove(inv.Items[i]);
                        AlrUsed = true;
                    }
                }
            }

            if (CurrentDemon.Charge > 100)
                CurrentDemon.Charge = 100;
        } // Charge Pots
         
        if (Input.GetKeyDown(KeyCode.F))
        {
            bool AlrUsed = false;

            if (Health == 100)
                return;

            if (inv.Items.Count == 0)
                return;

            for (int i = 0; i < inv.Items.Count; i++)
            {
                if (!AlrUsed)
                {
                    if (inv.Items[i].Name == "Health Pot")
                    {
                        UseHealthPot(inv.Items[i].HealBy);
                        inv.Items.Remove(inv.Items[i]);
                        AlrUsed = true;
                    }
                }
            }

            if (Health > 100)
                Health = 100;
        } // Health Pots

        if (BulletsLeft > 0 && Input.GetMouseButtonDown(0))
        {
            Shoot();
        } // Gun
    }                   // Gets All Inputs 
    void Movement()
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
        }
        Controller.Move(new Vector3(0, -1, 0));
    }                         // Applys Movement
    void EnemyRange()
    {
        FindClosestEnemy();
        if (ClosestEnemy < 10)
        {
            EnemysInRange = true;
        }
        else
        {
            EnemysInRange = false;
        }


        if (EnemysInRange)
            AttackInRange();
    }                       // Finds Enemys In Range Used By Demons
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
    public void RecieveDamage(float damage)
    {
        if (Random.Range(0, 100) < Defense)
        {
            Debug.Log("Blocked");
            return;
        }

        Health -= damage;
    } // Removes Health From Player
    public bool IsDead()
    {
        if(Health > 0)
            return false;

        if (Health <= 0)
            return true;


        Debug.LogError("Couldn't determine Death");
        return false;
    }                    // Checks if Player Is dead
    void AttackInRange()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            JumpOnEnemy(FindClosestEnemy());
        }
    }                    // Attacks All Enemys In range
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
        ClosestEnemy = DistanceBetweenEnemys;
        return Closet;
    }           // Finds Closest Guard
    void JumpOnEnemy(SeachEnemy Enemy)
    {
        if (Enemy == null)
        {
            Debug.Log("No Enemy Near By");
            return;
        }
        Debug.Log("Jumping on " + Enemy.name);
        Enemy.SummonReinforcements(Random.Range(15,25),true);
        SetPlayerState(true);
    }      // Attacks Guard
    void CheckEnemys()
    {
        if (FindObjectsOfType<AttackPlayerEnemy>().Length == 0)
            SetPlayerState(false);
    }                      // Checks If Any Enemys Near By If Not Sets Status As Hidden
    public void OnKill()
    {
        CheckEnemys();
    }                    // Runs On Kill
    public void Jump(Transform Where)
    {
        Controller.enabled = false;
        transform.position = Where.position;
        Controller.enabled = true;
    }       // Jumps on Objects
    void UseChargePot(int ChargeBy)
    {
        CurrentDemon.Charge += ChargeBy;
        Debug.Log("Player Used Charge Pot, Charged By " + ChargeBy);
    }         // Allows Player To Use Charge Pots
    void UseHealthPot(int AddHealthBy)
    {
        Health += AddHealthBy;
        Debug.Log("Player Used Health Pot, Healed By " + AddHealthBy);
    }      // Allows Player To Use Health Pots
    GameObject FindCloseTagedEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }        // Finds Closest object with tag Enemy
    void Shoot()
    {
        AttackPlayerEnemy Enemy;
        if (FindCloseTagedEnemy() != null)
            Enemy = FindCloseTagedEnemy().GetComponent<AttackPlayerEnemy>();
        else
            return;

        Vector3 targetPostition = new Vector3(Enemy.transform.position.x, transform.position.y, Enemy.transform.position.z);
        transform.LookAt(targetPostition);
        Enemy.health -= Mathf.RoundToInt(Random.Range(MinDamage,MaxDamage));
        BulletsLeft--;
    }                            // Shoots Bullets
}