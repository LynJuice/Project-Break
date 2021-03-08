﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public bool Done;

    public BaseEnemy Enemy;
    BattleStateMachine BSM;
    Animator Anim;

    [Header("Used In PlayerStateMachine")]
    public GameObject EnemySelected;

    public enum TurnState
    {
        Processing,
        ChoosingAction,
        Waiting,
        Action,
        Dead
    }

    public TurnState CurrentState;

    [Header("Progress")]
    float CurCoolDown;
    float MaxCoolDown = 1;

    [Header("Movement")]
    Vector3 StartPos;
    [Header("Action")]
    bool ActionStarted;
    public Transform HeroToAttack;
    float AnimSpeed = 17f;


    void Update()
    {
        switch (CurrentState)
        {
            case (TurnState.Processing):
                UpgradeProgressBar();
                break;

            case (TurnState.ChoosingAction):
                if (!Done)
                {
                    ChooseAction();
                    CurrentState = TurnState.Waiting;
                }
               break;

            case (TurnState.Waiting):
                // Idle
                break;

            case (TurnState.Action):
                StartCoroutine(Melle());
                break;

            case (TurnState.Dead):

                break;
        }

        if (IsDead())
        {
            CurrentState = TurnState.Dead;
            BSM.EnemysInBattle.Remove(gameObject);

            for (int i = 0; i < BSM.PerformersList.Count; i++)
            {
                if (BSM.PerformersList[i].AttackerGameObject == gameObject)
                    BSM.PerformersList.RemoveAt(i);
            }

            Destroy(gameObject);
        }
    }

    void Start()
    {
        CurrentState = TurnState.Processing;
        BSM = FindObjectOfType<BattleStateMachine>();
        StartPos = transform.position;
        Anim = GetComponent<Animator>();
    }

    void UpgradeProgressBar()
    {
        CurCoolDown += Time.deltaTime;
        float CaclCoolDown = CurCoolDown / MaxCoolDown;
        if (CurCoolDown >= MaxCoolDown)
        {
            CurrentState = TurnState.ChoosingAction;
        }
    }

    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn();

        if (BSM.HerosInBattle.Count == 0)
            return;

        GameObject Herotoattack = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];

        if (Herotoattack.GetComponent<HeroStateMachine>().IsDead())
            return;

        myAttack.Attacker = Enemy.Name;
        myAttack.Type = "Enemy";
        myAttack.AttackerGameObject = gameObject;
        myAttack.AttackersTarget = Herotoattack; // To be replaced
        Done = true;
        BSM.CollectActions(myAttack);
    }

    IEnumerator Melle()
    {
        if (ActionStarted)
            yield break;

        ActionStarted = true;

        Vector3 HeroPos = new Vector3(HeroToAttack.transform.position.x,HeroToAttack.position.y, HeroToAttack.position.z + 1.5f);
        while (MoveTowardsEnemy(HeroPos)) { yield return null; }

        if (HeroToAttack.GetComponent<HeroStateMachine>().hero.CurDef < Random.Range(0, 100))
            HeroToAttack.GetComponent<HeroStateMachine>().hero.CurHp -= Enemy.curATK;
        else
            Debug.Log("Blocked By: " + HeroToAttack.GetComponent<HeroStateMachine>().hero.Name);

        Anim.SetTrigger("Attack");

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

    bool IsDead()
    {
        if (Enemy.CurHp <= 0)
            return true;
        else
            return false;
    }
}