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
    int Selected;
    bool SpacePressed;
    bool AlreadySelected;
    bool Select;
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
        if (Escape)
            ChooseAction(0,true);

        if (Guard)
            TryToBlock();

        SpacePressed = Input.GetKey(KeyCode.Space);

        if (CurrentSelection == SelectingEnemy.Selecting)
        {
            SwitchEnemy();

           StartCoroutine(ChooseEnemy());
        }

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

        if (hero.CurHp <= 0)
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
    void ChooseAction(int NNN, bool nulled = false)
    {
        if (!nulled)
        {
            HandleTurn myAttack = new HandleTurn();
            myAttack.Attacker = hero.Name;
            myAttack.Type = "Hero";
            myAttack.AttackerGameObject = gameObject;
            myAttack.AttackersTarget = BSM.EnemysInBattle[NNN];
            BSM.CollectActions(myAttack);
        }
        else 
        {
            HandleTurn myAttack = new HandleTurn();
            myAttack.Attacker = hero.Name;
            myAttack.Type = "Nulled";
            myAttack.AttackerGameObject = gameObject;
            myAttack.AttackersTarget = null;
            BSM.CollectActions(myAttack);
            Done = true;
        }
    }
    void SwitchEnemy()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Selected--;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            Selected++;

        if (Select)
            for (int i = 0; i < BSM.EnemysInBattle.Count; i++)
            {
                if (i == Selected)
                    BSM.EnemysInBattle[i].GetComponent<EnemyStateMachine>().EnemySelected.SetActive(true);
                else
                    BSM.EnemysInBattle[i].GetComponent<EnemyStateMachine>().EnemySelected.SetActive(false);
            }
        if (!Select)
            for (int i = 0; i < BSM.EnemysInBattle.Count; i++)
            {
                BSM.EnemysInBattle[i].GetComponent<EnemyStateMachine>().EnemySelected.SetActive(false);
            }
    }
    IEnumerator ChooseEnemy()
    {
        if (Strike)
            if (MyTurn && !Done)
            {
                EnemyStateMachine[] Enemys = FindObjectsOfType<EnemyStateMachine>();
                Select = true;
                yield return new WaitUntil(() => SpacePressed);
                if (CurrentSelection == SelectingEnemy.Selecting)
                    ChooseAction(Selected);
                Select = false;
                Done = true;
                Strike = false;
                EnemyToAttack = BSM.EnemysInBattle[Selected].transform;
                CurrentSelection = SelectingEnemy.EnemySelected;
            }

    }
    IEnumerator Melle()
    {
        if (ActionStarted)
            yield break;

        ActionStarted = true;

        Vector3 HeroPos = new Vector3(EnemyToAttack.transform.position.x, EnemyToAttack.position.y, EnemyToAttack.position.z + 1.5f);
        while (MoveTowardsEnemy(HeroPos)) { yield return null; }

        if (EnemyToAttack.GetComponent<EnemyStateMachine>().Enemy.curDef < Random.Range(0, 100))
            EnemyToAttack.GetComponent<EnemyStateMachine>().Enemy.CurHp -= hero.CutAtk;
        else
            Debug.Log("Blocked By: " + EnemyToAttack.GetComponent<EnemyStateMachine>().Enemy.Name);

        yield return new WaitForSeconds(0.5f);

        while (MoveTowardsEnemy(StartPos)) { yield return null; }

        // after it completes

        BSM.PerformersList.RemoveAt(0);
        BSM.BattleStates = BattleStateMachine.PerformAction.Wait;

        ActionStarted = false;
        CurCoolDown = 0;
        CurrentState = TurnState.Processing;
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
        BSM.PerformersList.RemoveAt(0);
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
}
