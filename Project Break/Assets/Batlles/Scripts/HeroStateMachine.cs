using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{
    public BaseHero hero;
    BattleStateMachine BSM;

    [Header("Selecting What To Do")]
    public bool Strike;
    public bool Item;
    public bool Change;
    public bool Guard;
    public bool Escape;

    [Header("Selecting Enemy")]
    public int SelectedEnemy;
    public bool Selecting;


    [SerializeField] Animator Anim;


    public enum SelectingEnemy
    {
        Selecting,
        EnemySelected,
        NotSelecting
    }
    public SelectingEnemy CurrentSelection;


    public enum TurnState
    { 
        Processing,
        AddToList,
        Waiting,
        Selecting,
        Action,
        Dead
    }

    public TurnState CurrentState;

    [Header("Progress")]
    public Image ProgressBar;
    float CurCoolDown;
    float MaxCoolDown = 1;
    private bool ActionStarted;

    public Transform EnemyToAttack;
    public Vector3 StartPos;
    public bool MyTurn;
    public float AnimSpeed = 17;

    public bool Done;

    void Update()
    {
        if (Guard)
            TryToBlock();

        if (Strike)
            Selecting = true;

        if (Escape)
            EscapeTest();

        if (Selecting)
            SelectEnemy();

            switch (CurrentState)
            {
                case (TurnState.Processing):
                    UpgradeProgressBar();
                    break;

                case (TurnState.AddToList):
                    CurrentSelection = SelectingEnemy.Selecting;
                    CurrentState = TurnState.Waiting;
                    break;

                case (TurnState.Waiting):
                    // Idle
                    break;

                case (TurnState.Selecting):

                    break;


                case (TurnState.Action):
                    CurrentSelection = SelectingEnemy.NotSelecting;
                if(Strike)
                    StartCoroutine(Melle());
                    break;

                case (TurnState.Dead):

                    break;
            }
        if (IsDead())
            CurrentState = TurnState.Dead;
    }
    private void Start()
    {
        CurrentState = TurnState.Processing;
        BSM = FindObjectOfType<BattleStateMachine>();
        StartPos = transform.position;
    }
    void UpgradeProgressBar()
    {
        CurCoolDown += Time.deltaTime;
        float CaclCoolDown = CurCoolDown / MaxCoolDown;
   //     ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(CaclCoolDown,0,1),ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if(CurCoolDown >= MaxCoolDown)
        {
            CurrentState = TurnState.AddToList;
        }
    }  // WHY
    void SelectEnemy()
    {
        if (Selecting)
        {
            int t = BSM.EnemysInBattle.Count;
            t--;
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                SelectedEnemy--;
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                SelectedEnemy++;
            if (SelectedEnemy > t)
                SelectedEnemy = 0;
            if (SelectedEnemy < 0)
                SelectedEnemy = t;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < BSM.EnemysInBattle.Count; i++)
                {
                    BSM.EnemysInBattle[i].GetComponent<EnemyStateMachine>().EnemySelected.SetActive(false);
                }

                EnemyToAttack = BSM.EnemysInBattle[SelectedEnemy].transform;

                if (Strike)
                {
                    Done = true;
                    BSM.Turns++;
                    StartCoroutine(Melle());
                }

                Selecting = false;
            }

            for (int i = 0; i < BSM.EnemysInBattle.Count; i++)
            {
                if (i == SelectedEnemy)
                    BSM.EnemysInBattle[i].GetComponent<EnemyStateMachine>().EnemySelected.SetActive(true);
                else
                    BSM.EnemysInBattle[i].GetComponent<EnemyStateMachine>().EnemySelected.SetActive(false);
            }
            if (!Selecting)
                for (int i = 0; i < BSM.EnemysInBattle.Count; i++)
                {
                    BSM.EnemysInBattle[i].GetComponent<EnemyStateMachine>().EnemySelected.SetActive(false);
                }
        }
    }
    IEnumerator Melle() // Rewrite
    {
        if (ActionStarted)
            yield break;

        ActionStarted = true;
        Anim.SetBool("Walk", true);

        Vector3 HeroPos = new Vector3(EnemyToAttack.transform.position.x, EnemyToAttack.position.y, EnemyToAttack.position.z + 1.5f);
        while (MoveTowardsEnemy(HeroPos)) { yield return null; }

        EnemyToAttack.gameObject.GetComponent<EnemyStateMachine>().ReciveDamage(hero.CutAtk,hero);

        yield return new WaitForSeconds(0.5f);

        while (MoveTowardsEnemy(StartPos)) { yield return null; }
        Anim.SetBool("Walk", false);
    }
    bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, AnimSpeed * Time.deltaTime));
    }
    public void EscapeTest()
    {
        if (Random.Range(0, 100) > 0)
        {
            Debug.Log("Escaped");
            StartCoroutine(BSM.SH.ChangeScene(0));
        }
        else
        {
            Debug.Log("Failed");
        }
        BSM.Turns++;
        Done = true;
        Escape = false;
    }
    public void TryToBlock()
    {
        hero.CurDef = hero.CurDef * 2;
        Debug.Log("Block increased: " + hero.CurDef + " For : " + hero.Name);
        Done = true;
        BSM.Turns++;
        Guard = false;
    }

    public bool IsDead()
    {
        if (hero.CurHp <= 0)
        {
            BSM.BI.SetDead(this);
            BSM.HerosInBattle.Remove(gameObject);
        }
        if (hero.CurHp <= 0)
            return true;
        else return false;
    }
}
