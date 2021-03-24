﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public HandleTurn HT;

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
                if (HT.PowerUsed == null)
                    StartCoroutine(Melle());
                else
                    StartCoroutine(UsePower(HT.PowerUsed));
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
        if (Enemy.Powers.Count > 0)
        {
            Power PowerToUse = Enemy.Powers[Random.Range(0, Enemy.Powers.Count -1)];

            if (PowerToUse == null)
                Debug.Log("Broken");

            if (Enemy.CurMp > PowerToUse.ChargeCost && Enemy.CurHp > PowerToUse.HealthCost)
                SelectPowerMove(PowerToUse);
            else
                SelectMelee();
        }
        else
            SelectMelee();
    }

    void SelectMelee()
    {
        if (BSM.HerosInBattle.Count == 0)
            return;

        GameObject Herotoattack = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];

        if (Herotoattack.GetComponent<HeroStateMachine>().IsDead())
            return;

        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = Enemy.Name;
        myAttack.Type = "Enemy";
        myAttack.AttackerGameObject = gameObject;
        myAttack.AttackersTarget = Herotoattack; // To be replaced
        Done = true;
        BSM.CollectActions(myAttack);
    }
    void SelectPowerMove(Power Used)
    {
        if (BSM.HerosInBattle.Count == 0)
            return;

        GameObject hero = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];

        if (hero.GetComponent<HeroStateMachine>().IsDead())
             return;

        HandleTurn attack = new HandleTurn();
        attack.Attacker = Enemy.Name;
        attack.Type = "Enemy";
        attack.AttackerGameObject = gameObject;
        attack.AttackersTarget = hero;
        attack.PowerUsed = Used;
        Enemy.CurMp -= Used.ChargeCost;
        BSM.CollectActions(attack);
    }

    IEnumerator UsePower(Power Used)
    {
        if (ActionStarted)
            yield break;

        ActionStarted = true;

        Anim.SetTrigger("Attack");

        if (Used == null)
            Debug.Log("What ???");

        if (!HeroToAttack.GetComponent<HeroStateMachine>())
            Debug.Log("The flying fuck?");

        HeroToAttack.GetComponent<HeroStateMachine>().hero.CurHp -= Random.Range(Used.BaseDam,Used.MaxDam);

        yield return new WaitForSeconds(1);
        Done = true;

        BSM.PerformersList.RemoveAt(0);
        BSM.BattleStates = BattleStateMachine.PerformAction.Wait;

        ActionStarted = false;
        CurCoolDown = 0;
        CurrentState = TurnState.Processing;
    }

    IEnumerator Melle()
    {
        if (ActionStarted)
            yield break;

        ActionStarted = true;

        Vector3 HeroPos = new Vector3(HeroToAttack.transform.position.x, HeroToAttack.position.y, HeroToAttack.position.z + 1.5f);
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

    public void ReciveDamage(int Damage, BaseHero hero, Power Eliment = null)
    {
        int TotalDamage = 0;

        if (Eliment != null)
        {
            switch (Enemy.EnemyType)
            {
                case BaseEnemy.Type.Wind:
                    if (Eliment.ElimentToApply == Power.ApplyEliment.Steel || Eliment.ElimentToApply == Power.ApplyEliment.Cursed || Eliment.ElimentToApply == Power.ApplyEliment.Ice)
                        TotalDamage = Damage + Random.Range(0, Damage);
                    else
                        TotalDamage = Damage;
                    break;
                case BaseEnemy.Type.Fire:
                    if (Eliment.ElimentToApply == Power.ApplyEliment.Wind || Eliment.ElimentToApply == Power.ApplyEliment.Blessed || Eliment.ElimentToApply == Power.ApplyEliment.Ice)
                        TotalDamage = Damage + Random.Range(0, Damage);
                    else
                        TotalDamage = Damage;
                    break;
                case BaseEnemy.Type.Electric:
                    if (Eliment.ElimentToApply == Power.ApplyEliment.Blessed)
                        TotalDamage = Damage + Random.Range(0, Damage);
                    else
                        TotalDamage = Damage;
                    break;
                case BaseEnemy.Type.Ice:
                    if (Eliment.ElimentToApply == Power.ApplyEliment.Steel || Eliment.ElimentToApply == Power.ApplyEliment.Cursed)
                        TotalDamage = Damage + Random.Range(0, Damage);
                    else
                        TotalDamage = Damage;
                    break;
                case BaseEnemy.Type.Steel:
                    if (Eliment.ElimentToApply == Power.ApplyEliment.Fire || Eliment.ElimentToApply == Power.ApplyEliment.Cursed || Eliment.ElimentToApply == Power.ApplyEliment.Blessed || Eliment.ElimentToApply == Power.ApplyEliment.Electric)
                        TotalDamage = Damage + Random.Range(0, Damage);
                    else
                        TotalDamage = Damage;
                    break;
            }
        }
        else
            TotalDamage = Damage;

        if(Enemy.curDef > Random.Range(0,100))
        {
            Debug.Log(Enemy.Name + " Blocked");
            Anim.SetTrigger("Block");
        }
        else
        {
            Enemy.CurHp -= TotalDamage;
            Anim.SetTrigger("Hit");
            Debug.Log("Player attacked: " + Enemy.Name + " And Delt: " + TotalDamage + " Current health after attack: " + Enemy.CurHp);
        }
    }
}