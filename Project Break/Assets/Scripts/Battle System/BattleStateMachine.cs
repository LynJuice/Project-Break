using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
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
        EnemysInBattle.AddRange (GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        BI = FindObjectOfType<BattleInterface>();
    }

    void Update()
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

        switch (BattleStates)
        {
            case (PerformAction.Wait):
                if (PerformersList.Count >= Performinces)
                    BattleStates = PerformAction.TakeAction;
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

        /*
        for (int i = 0; i < HerosInBattle.Count; i++)
        {
            if (HerosInBattle[i].GetComponent<HeroStateMachine>().hero.CurHp <= 0)
            {
                BI.SetDead(HerosInBattle[i].GetComponent<HeroStateMachine>());
                HerosInBattle.Remove(HerosInBattle[i]);
            }
        }
        */
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
