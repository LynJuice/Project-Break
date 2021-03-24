using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SavedData
{
    public int Scene;
    public float[] Position;
    public int[] Items;
    public int Cash;
    public int Day;
    public int Month;

    // Qualitys
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

    public SavedData(PlayerMovement PM)
    {
        Inventory inv = PM.GetComponent<Inventory>();

        Scene = SceneManager.GetActiveScene().buildIndex;

        Cash = PM.GetComponent<Inventory>().Cash;

        Position = new float[3];
        Position[0] = PM.GetComponent<Transform>().position.x;
        Position[1] = PM.GetComponent<Transform>().position.y;
        Position[2] = PM.GetComponent<Transform>().position.z;

        Items = new int[inv.Items.Count];
        for (int i = 0; i < inv.Items.Count; i++)
        {
            Items[i] = inv.Items[i].ID;
        }

        Day = PM.Day;
        Month = PM.Month;

        // Save Qualitys
        UnderstandingEXP = inv.UnderstandingEXP;
        UnderstandingLVL = inv.UnderstandingLVL;

        KnowledgeEXP = inv.KnowledgeEXP;
        KnowledgeLVL = inv.KnowledgeLVL;

        CourageEXP = inv.CourageEXP;
        CourageLVL = inv.CourageLVL;

        ExpressionEXP = inv.ExpressionEXP;
        ExpressionLVL = inv.ExpressionLVL;

        DiligenceEXP = inv.DiligenceEXP;
        DiligenceLVL = inv.DiligenceLVL;
    }
}
