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
        switch (BattleStates)
        {
            case (PerformAction.Wait):
                if (PerformersList.Count > 0)
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
                    
                }

                BattleStates = PerformAction.PerfomAction;
                break;
            case (PerformAction.PerfomAction):

                break;
        }

        for (int i = 0; i < HerosInBattle.Count; i++)
        {
            if (HerosInBattle[i].GetComponent<HeroStateMachine>().hero.CurHp <= 0)
            {
                BI.SetDead(HerosInBattle[i].GetComponent<HeroStateMachine>());
                HerosInBattle.Remove(HerosInBattle[i]);
            }
        }
    }

    public void CollectActions(HandleTurn input)
    {
        PerformersList.Add(input);
    }
}
