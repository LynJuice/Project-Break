using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    bool Waiting;
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
    HandleTurn HeroChoice;

    BattleInterface BI;

    void Awake()
    {
        BattleStates = PerformAction.Wait;
        EnemysInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        BI = FindObjectOfType<BattleInterface>();
    }
    IEnumerator HandlePlayersTurn()
    {
        for (int i = 0; i < HerosInBattle.Count; i++)
        {
            HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn = true;
            yield return new WaitUntil(() => HerosInBattle[i].GetComponent<HeroStateMachine>().Done);
            HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn = false;
            HerosInBattle[i].GetComponent<HeroStateMachine>().Done = false;
            yield return new WaitForSeconds(1);
            Debug.Log("Turn End");
        }

        for (int i = 0; i < PerformersList.Count; i++)
        {
            BattleStates = PerformAction.TakeAction;
            yield return new WaitUntil(() => Waiting);
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
        if (BattleStates == PerformAction.Wait)
            Waiting = true;
        else
            Waiting = false;

        NumberOfPerformers = (HerosInBattle.Count + EnemysInBattle.Count);

        switch (BattleStates)
        {
            case (PerformAction.Wait):
                StartCoroutine(HandlePlayersTurn());
                break;
            case (PerformAction.TakeAction):
                GameObject Performer = PerformersList[0].AttackerGameObject;
                if (PerformersList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = Performer.GetComponent<EnemyStateMachine>();
                    ESM.HeroToAttack = PerformersList[0].AttackersTarget.transform;
                    ESM.CurrentState = EnemyStateMachine.TurnState.Action;
                }

                if (PerformersList[0].Type == "Hero")
                {
                    HeroStateMachine HSM = Performer.GetComponent<HeroStateMachine>();
                    HSM.EnemyToAttack = PerformersList[0].AttackersTarget.transform;
                    HSM.CurrentState = HeroStateMachine.TurnState.Action;
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
}
