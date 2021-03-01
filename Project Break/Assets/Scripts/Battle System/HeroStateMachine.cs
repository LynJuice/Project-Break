using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{
    public BaseHero hero;
    BattleStateMachine BSM;

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

    void Update()
    {
        switch (CurrentState)
        {
            case (TurnState.Processing):
                UpgradeProgressBar();
                break;

            case (TurnState.AddToList):

                break;

            case (TurnState.Waiting):

                break;

            case (TurnState.Selecting):

                break;


            case (TurnState.Action):

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
    }
}
