using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInterface : MonoBehaviour
{
    [SerializeField] Image[] Icons = new Image[4];
    [SerializeField] Slider[] Healths = new Slider[4];
    [SerializeField] Slider[] Charge = new Slider[4];
    [SerializeField] Text[] HealthText = new Text[4];
    [SerializeField] Text[] ChargeText = new Text[4];
    BattleStateMachine BSM;

    void Start()
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

    void Update()
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

    public void SetDead(HeroStateMachine HSM)
    {
        for (int i = 0; i < 4; i++)
        {
            if (Icons[i].sprite == HSM.hero.Icon && Healths[i].value <= 0)
                Icons[i].sprite = HSM.hero.IconDown;
        }
    }
}
