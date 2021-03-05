using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    public SceneHandler SH;
    [SerializeField] GameObject Temp;
    public int CurrentRound = 1;
    [HideInInspector] public int Turns;
    public bool PlayerCanChooseTimer;
    public bool PlayerWaited;
    public enum PerformAction
    {
        Wait,
        TakeAction,
        PerfomAction
    }

    public PerformAction BattleStates;

    public List<HandleTurn> PerformersList = new List<HandleTurn>();

    public List<GameObject> HerosInBattle = new List<GameObject>();
    public List<GameObject> EnemysInBattle = new List<GameObject>();

    int NumberOfPerformers;

    public enum HeroGUI
    {
        Activate,
        Waiting,
        Input,
        SelectEnemy,
        Done
    }

    public HeroGUI HeroInput;

    public List<GameObject> HerosToManage = new List<GameObject>();

    void Awake()
    {
        BattleStates = PerformAction.Wait;
        EnemysInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        SH = FindObjectOfType<SceneHandler>();
    }

    IEnumerator HandlePlayersTurn()
    {
        PlayerWaited = true;
        for (int i = 0; i < HerosInBattle.Count; i++)
        {
            if (!HerosInBattle[i].GetComponent<HeroStateMachine>().Done)
            {
                HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn = true;
                Debug.Log(HerosInBattle[i].GetComponent<HeroStateMachine>().hero.Name + "'s turn");
                yield return new WaitUntil(() => HerosInBattle[i].GetComponent<HeroStateMachine>().Done);
                HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn = false;
                PlayerCanChooseTimer = false;
                Debug.Log(HerosInBattle[i].GetComponent<HeroStateMachine>().hero.Name + "'s turn is done");
            }
        }
    }
    void CheckForDupes()
    {
        int Performinces = EnemysInBattle.Count + HerosInBattle.Count;
        if (PerformersList.Count > 0)
        {
            if (PerformersList[0].Type == "Enemy")
            {
                if (PerformersList[0].AttackersTarget.GetComponent<HeroStateMachine>().hero.CurHp <= 0)
                    PerformersList.Remove(PerformersList[0]);
            }

            if (PerformersList[0].Type == "Hero")
            {
                if (PerformersList[0].AttackersTarget.GetComponent<EnemyStateMachine>().Enemy.CurHp <= 0)
                    PerformersList.Remove(PerformersList[0]);
            }
        }
    }
    void Battlestating()
    {
        NumberOfPerformers = (HerosInBattle.Count + EnemysInBattle.Count);
        int NumberOfPerformersminus = NumberOfPerformers - Turns;
        if(Turns == NumberOfPerformers)
        {
            Turns = 0;
            StartNewTurn(); // round
        }

        switch (BattleStates)
        {
            case (PerformAction.Wait):

                //     if(!PlayerWaited)
                StartCoroutine(HandlePlayersTurn());

                if (PerformersList.Count > 0)
                    if (NumberOfPerformersminus <= PerformersList.Count || PerformersList[0].Type == "Hero")
                        BattleStates = PerformAction.TakeAction;
                break;
            case (PerformAction.TakeAction):
                PlayerWaited = false;
                GameObject Performer = PerformersList[0].AttackerGameObject;
                if (PerformersList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = Performer.GetComponent<EnemyStateMachine>();
                    ESM.HeroToAttack = PerformersList[0].AttackersTarget.transform;
                    Turns++;
                    ESM.CurrentState = EnemyStateMachine.TurnState.Action;
                }

                if (PerformersList[0].Type == "Hero")
                {
                    HeroStateMachine HSM = Performer.GetComponent<HeroStateMachine>();
                    HSM.EnemyToAttack = PerformersList[0].AttackersTarget.transform;
                    Turns++;
                    HSM.CurrentState = HeroStateMachine.TurnState.Action;
                }

                if (PerformersList[0].Type == "Elimental - Hero")
                {
                    HeroStateMachine HSM = Performer.GetComponent<HeroStateMachine>();

                }

                if (PerformersList[0].Type == "Nulled")
                {
                    HeroStateMachine HSM = Performer.GetComponent<HeroStateMachine>();

                    if (HSM.Guard)
                        HSM.TryToBlock();
                    if(HSM.Escape)
                        HSM.EscapeTest();

                    Turns++;
                    HSM.CurrentState = HeroStateMachine.TurnState.Action;
                    HSM.hero.CurDef = HSM.hero.BaseDef;
                }


                BattleStates = PerformAction.PerfomAction;
                break;
            case (PerformAction.PerfomAction):
                break;
        }
    }

    void Update()
    {
        CheckForDupes();
        Battlestating();
    }

    public void CollectActions(HandleTurn input)
    {
        for (int i = 0; i < PerformersList.Count; i++)
        {
            if (PerformersList[i].Attacker == input.Attacker)
                return;
        }


        PerformersList.Add(input);
    }

    void StartNewTurn()
    {
        for (int i = 0; i < HerosInBattle.Count; i++)
        {
            HerosInBattle[i].GetComponent<HeroStateMachine>().Done = false;
            HerosInBattle[i].GetComponent<HeroStateMachine>().hero.CurDef = HerosInBattle[i].GetComponent<HeroStateMachine>().hero.BaseDef;
        }
        CurrentRound++;
        BattleStates = PerformAction.Wait;
        Debug.Log("New Round Started");
    }

    ///////////////////////////////////////////////////////////////////////////////////    PLAYER BUTTONS    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    

    public void SelectStrike()
    {
        for (int i = 0; i < HerosInBattle.Count; i++)
        {
            if(HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn)
                HerosInBattle[i].GetComponent<HeroStateMachine>().Strike = true;
        }
    }

    public void SelectEscape()
    {
        for (int i = 0; i < HerosInBattle.Count; i++)
        {
            if (HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn)
                HerosInBattle[i].GetComponent<HeroStateMachine>().Escape = true;
        }
    }

    public void SelectBlock()
    {
        for (int i = 0; i < HerosInBattle.Count; i++)
        {
            if (HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn)
                HerosInBattle[i].GetComponent<HeroStateMachine>().Guard = true;
        }
    }
}
