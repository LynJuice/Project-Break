using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    public SceneHandler SH;
    public BattleInterface BI;
    [SerializeField] GameObject Temp;
    public int CurrentRound = 1;
    [HideInInspector] public int Turns;
    int Turn;
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

    void Awake()
    {
        BattleStates = PerformAction.Wait;
        EnemysInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        SH = FindObjectOfType<SceneHandler>();
        BI = FindObjectOfType<BattleInterface>();
    }

    void Start()
    {
        StartCoroutine(HandlePlayersTurn());
    }

    IEnumerator HandlePlayersTurn()
    {
        for (int i = 0; i < HerosInBattle.Count; i++)
        {
            if (!HerosInBattle[i].GetComponent<HeroStateMachine>().Done && !HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn)
            {
                HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn = true;
                StartCoroutine(BI.UpdateCamera());
                yield return new WaitUntil(() => HerosInBattle[i].GetComponent<HeroStateMachine>().Done);
                HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn = false;
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
        if(Turn < Turns)
        {
            Turn = Turns;
            StartCoroutine(HandlePlayersTurn());
        }

        NumberOfPerformers = (HerosInBattle.Count + EnemysInBattle.Count);
        int NumberOfPerformersminus = NumberOfPerformers - Turns;
        if (Turns == NumberOfPerformers)
        {
            Turns = 0;
            StartNewTurn(); // round
        }

        switch (BattleStates)
        {
            case (PerformAction.Wait):
                int e = 0;
                for (int i = 0; i < HerosInBattle.Count; i++)
                {
                    if (HerosInBattle[i].GetComponent<HeroStateMachine>().Done)
                        e++;
                }

                if (e == HerosInBattle.Count  && PerformersList.Count > 0)
                    BattleStates = PerformAction.TakeAction;

                break;
            case (PerformAction.TakeAction):
                GameObject Performer = PerformersList[0].AttackerGameObject;
                if (PerformersList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = Performer.GetComponent<EnemyStateMachine>();

                    if (HerosInBattle.Contains(PerformersList[0].AttackersTarget))
                    {
                        ESM.HeroToAttack = PerformersList[0].AttackersTarget.transform;
                        Turns++;
                        ESM.CurrentState = EnemyStateMachine.TurnState.Action;
                    }
                    else
                    {
                        PerformersList.RemoveAt(0);
                    }
                }
                BattleStates = PerformAction.PerfomAction;
                break;
            case (PerformAction.PerfomAction):
                break;
        }
    }

    void Update()
    {
        Battlestating();
        CheckWinLose();
    }

    public void CollectActions(HandleTurn input)
    {
        for (int i = 0; i < PerformersList.Count; i++)
        {
            if (PerformersList[i].Attacker == input.Attacker)
                return;
        }

        PerformersList.Add(input);
        CheckForDupes();
    }

    void StartNewTurn()
    {
        for (int i = 0; i < HerosInBattle.Count; i++)
        {
            HerosInBattle[i].GetComponent<HeroStateMachine>().Done = false;
            HerosInBattle[i].GetComponent<HeroStateMachine>().hero.CurDef = HerosInBattle[i].GetComponent<HeroStateMachine>().hero.BaseDef;
        }

        for (int i = 0; i < EnemysInBattle.Count; i++)
        {
            EnemysInBattle[i].GetComponent<EnemyStateMachine>().Done = false;
        }
        CurrentRound++;
        BattleStates = PerformAction.Wait;
        Turn = 0;
        Turns = 0;
        StartCoroutine(HandlePlayersTurn());
        Debug.Log("New Round Started");
    }

    void CheckWinLose()
    {
        if (HerosInBattle.Count == 0)
        {
            Debug.Log("Lost");
            StartCoroutine( SH.ChangeScene(0));
        }

        if (EnemysInBattle.Count == 0)
            Debug.Log("Won");
    }

    void AddPower(BaseHero hero, Power power)
    {
        if (hero.Spirit.Powers.Count >= 6)
            return;
        else
            hero.Spirit.Powers.Add(power);
    }
}