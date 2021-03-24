using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int Cash;
    public List<Item> Items;

    [Header("Social Qualitys")]
    public int UnderstandingEXP;
    public int KnowledgeEXP;
    public int CourageEXP;
    public int ExpressionEXP;
    public int DiligenceEXP;

    public int UnderstandingLVL;
    public int KnowledgeLVL;
    public int CourageLVL;
    public int ExpressionLVL;
    public int DiligenceLVL;

    public enum Quality
    {
        Understanding,
        Knowledge,
        Courage,
        Expression,
        Diligence
    };

    public bool CheckQuality(Quality quality,int Level)
    {
        bool Pass = false;
        switch(quality)
        {
            case Quality.Understanding:
                if (UnderstandingLVL >= Level)
                    Pass = true;
                break;
            case Quality.Knowledge:
                if (KnowledgeLVL >= Level)
                    Pass = true;
                break;
            case Quality.Courage:
                if (CourageLVL >= Level)
                    Pass = true;
                break;
            case Quality.Expression:
                if (ExpressionLVL >= Level)
                    Pass = true;
                break;
            case Quality.Diligence:
                if (DiligenceLVL >= Level)
                    Pass = true;
                break;
        }

        return Pass;
    }

    public void GainQuality(Quality quality, int XP)
    { 
        switch(quality)
        {
            case Quality.Understanding:
                UnderstandingEXP += XP;
                UnderstandingLVL = UnderstandingEXP / 25;
                break;

            case Quality.Knowledge:
                KnowledgeEXP += XP;
                KnowledgeLVL = KnowledgeEXP / 25;
                break;

            case Quality.Courage:
                CourageEXP += XP;
                CourageLVL = CourageEXP / 25;
                break;

            case Quality.Expression:
                ExpressionEXP += XP;
                ExpressionLVL = ExpressionEXP / 25;
                break;

            case Quality.Diligence:
                DiligenceEXP += XP;
                DiligenceLVL = DiligenceEXP / 25;
                break;
        }
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
    }
}
