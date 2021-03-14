using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInterface : MonoBehaviour
{
    [Header("Health,Icons,Charge")]
    [SerializeField] Image[] Icons = new Image[4];
    [SerializeField] Slider[] Healths = new Slider[4];
    [SerializeField] Slider[] Charge = new Slider[4];
    [SerializeField] Text[] HealthText = new Text[4];
    [SerializeField] Text[] ChargeText = new Text[4];
    BattleStateMachine BSM;

    [Header("Power Menu")]
    [SerializeField] Image PowerPanel;
    public Button[] Buttons = new Button[6];

    [Header("Round Switch")]
    [SerializeField] Camera Cam;
    [SerializeField] Transform OriginPos;
    [SerializeField] Animator Doors;
    bool Running;

    [Header("Inventory")]
    [SerializeField] GameObject InvPanel;

    void StartHIC()
    {
        BSM = FindObjectOfType<BattleStateMachine>();
        for (int i = 0; i < BSM.HerosInBattle.Count; i++)
        {
            if (BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().hero.Icon == null)
                return;

            if (Icons[i] == null)
                return;

            Icons[i].sprite = BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().hero.Icon;

            Healths[i].maxValue = BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().hero.BaseHp;
            Charge[i].maxValue = BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().hero.BaseMp;
        }
        for (int i = 0; i < 4; i++)
        {
            if (Icons[i].sprite == null)
                Icons[i].gameObject.SetActive(false);
        }
    }
    void UpdateHIC()
    {
        //Health Set
        for (int i = 0; i < BSM.HerosInBattle.Count; i++)
        {
            Healths[i].value = BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().hero.CurHp;
            HealthText[i].text = Mathf.RoundToInt(BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().hero.CurHp).ToString();
        }

        //Charge Set
        for (int i = 0; i < BSM.HerosInBattle.Count; i++)
        {
            Charge[i].value = BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().hero.CurMp;
            ChargeText[i].text = Mathf.RoundToInt(BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().hero.CurMp).ToString();
        }
    }
    void Start()
    {
        StartHIC();
        SetPowerButtons(BSM.HerosInBattle[0].GetComponent<HeroStateMachine>().hero);
    }
    void Update()
    {
        UpdateHIC();
    }
    public void SetDead(HeroStateMachine HSM)
    {
        for (int i = 0; i < 4; i++)
        {
            if (Icons[i].sprite == HSM.hero.Icon && Healths[i].value <= 0)
                Icons[i].sprite = HSM.hero.IconDown;
        }
    } //HIC
    void SetPowerButtons(BaseHero hero)
    {
        for (int i = 0; i < hero.Spirit.Powers.Count; i++)
        {
            if (hero.Spirit.Powers[i] == null)
                Buttons[i].gameObject.SetActive(false);
            else
            {
                Buttons[i].GetComponentInChildren<Text>().text = hero.Spirit.Powers[i].Name;

                if (hero.Spirit.Powers[i].ChargeCost > hero.CurMp)
                    Buttons[i].interactable = false;

                if (hero.Spirit.Powers[i].Name.Contains("Heal") && hero.CurHp == hero.BaseHp)
                    Buttons[i].interactable = false;
            }
        }
    }
    // buttons ui
    public void SelectStrike()
    {
        for (int i = 0; i < BSM.HerosInBattle.Count; i++)
        {
            if (BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn)
                BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().Strike = true;
        }
    }
    public void SelectEscape()
    {
        for (int i = 0; i < BSM.HerosInBattle.Count; i++)
        {
            if (BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn)
                BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().Escape = true;
        }
    }
    public void SelectBlock()
    {
        for (int i = 0; i < BSM.HerosInBattle.Count; i++)
        {
            if (BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn)
                BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().Guard = true;
        }
    }
    public void SelectSkillButton()
    {
        for (int i = 0; i < BSM.HerosInBattle.Count; i++)
        {
            if(BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn)
                SetPowerButtons(BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().hero);
        }
        PowerPanel.gameObject.SetActive(true);
    }

    public void OpenInventory()
    {
        InvPanel.SetActive(true);
    }


    public void SelectSkill(int Z)
    {
        StartCoroutine(UseSkill(Z));
    }
    IEnumerator UseSkill(int Z)
    {
        BaseHero hero = null;
        HeroStateMachine HeroState = null;
        for (int i = 0; i < BSM.HerosInBattle.Count; i++)
        {
            if (BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn)
            {
                hero = BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().hero;
                HeroState = BSM.HerosInBattle[i].GetComponent<HeroStateMachine>();
            }
        }

        if (hero == null)
            yield break;

        if (HeroState == null)
            yield break;

        if (hero.CurMp < hero.Spirit.Powers[Z].ChargeCost)
            yield break;

        if (hero.Spirit.Powers[Z].ElimentToApply != Power.ApplyEliment.Other)
        {
            if(!hero.Spirit.Powers[Z].Multi)
            {
                HeroState.Selecting = true;

                PowerPanel.gameObject.SetActive(false);

                yield return new WaitUntil(() => !HeroState.Selecting);

                for (int i = 0; i < BSM.EnemysInBattle.Count; i++)
                {
                    BSM.EnemysInBattle[i].GetComponent<EnemyStateMachine>().EnemySelected.SetActive(false);
                }

                BSM.EnemysInBattle[HeroState.SelectedEnemy].GetComponent<EnemyStateMachine>().ReciveDamage(Random.Range(hero.Spirit.Powers[Z].BaseDam, hero.Spirit.Powers[Z].MaxDam), hero, hero.Spirit.Powers[Z]);
                hero.CurMp -= hero.Spirit.Powers[Z].ChargeCost;
            }
            else
            {
                PowerPanel.gameObject.SetActive(false);

                for (int i = 0; i < BSM.EnemysInBattle.Count; i++)
                {
                    BSM.EnemysInBattle[i].GetComponent<EnemyStateMachine>().ReciveDamage(Random.Range(hero.Spirit.Powers[Z].BaseDam, hero.Spirit.Powers[Z].MaxDam), hero, hero.Spirit.Powers[Z]);
                    yield return new WaitForSeconds(.5f);
                }

                hero.CurMp -= hero.Spirit.Powers[Z].ChargeCost;
            }
        }
        else if(hero.Spirit.Powers[Z].ElimentToApply != Power.ApplyEliment.None)
        { // heal
            if(!hero.Spirit.Powers[Z].Multi)
            {
                hero.CurHp += Random.Range(hero.Spirit.Powers[Z].BaseHeal, hero.Spirit.Powers[Z].MaxHeal);

                if (hero.CurHp > hero.BaseHp)
                    hero.CurHp = hero.BaseHp;

                PowerPanel.gameObject.SetActive(false);
                hero.CurMp -= hero.Spirit.Powers[Z].ChargeCost;
            }
        }

        BSM.Turns++;
        HeroState.Done = true;
    }

    public IEnumerator UpdateCamera(bool OldSpot = false)
    {
        if (Running)
            yield return null;
        Running = true;
        HeroStateMachine hero = null;
        for (int i = 0; i < BSM.HerosInBattle.Count; i++)
        {
            if (BSM.HerosInBattle[i].GetComponent<HeroStateMachine>().MyTurn)
                hero = BSM.HerosInBattle[i].GetComponent<HeroStateMachine>();
        }
        if (hero != null && Cam.transform.parent != hero.hero.CameraPos && !OldSpot)
        {
            Doors.SetTrigger("Open");
            yield return new WaitForSeconds(.2f);
            Cam.transform.parent = hero.hero.CameraPos;
            Cam.transform.localPosition = Vector3.zero;
            Cam.transform.localRotation = Quaternion.Euler(0,0,0);
        }

        if(OldSpot)
        {
            Doors.SetTrigger("Open");
            yield return new WaitForSeconds(.2f);
            OriginPos.transform.parent = OriginPos;
            OriginPos.transform.localPosition = Vector3.zero;
            OriginPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}