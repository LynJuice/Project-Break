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

    private void Start()
    {
        BattleStates = PerformAction.Wait;
        EnemysInBattle.AddRange (GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
    }

    void Update()
    {
        switch (BattleStates)
        {
            case (PerformAction.Wait):

                break;
            case (PerformAction.TakeAction):

                break;
            case (PerformAction.PerfomAction):

                break;
        }
    }

    public void CollectActions(HandleTurn input)
    {
        PerformersList.Add(input);
    }
}
