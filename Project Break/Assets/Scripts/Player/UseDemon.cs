using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UseDemon : MonoBehaviour
{
    [SerializeField] Transform SummoningDemonSpot;
    public int Reach;
    [SerializeField] int RequeredCharge = 75;
    public void Strike(ScriptableDemon SD)
    {
        AttackPlayerEnemy[] ape = FindObjectsOfType<AttackPlayerEnemy>();

        if (SD.Charge >= RequeredCharge)
        {
            if (FindAllCloseEnemys(Reach, ape).Length == 0)
                Debug.LogError("Failed To find Close Enemy");

            StartCoroutine(SummonDemon(SD.Model, SummoningDemonSpot, 1.5f));

            for (int i = 0; i < FindAllCloseEnemys(Reach, ape).Length; i++)
            {
                FindAllCloseEnemys(Reach, ape)[i].health -= Random.Range(50, 75);
            }

            SD.Charge -= RequeredCharge; 
        }
        else 
        {
            Debug.Log("Not inuff Charge");
        }
    }  // Strikes All Near By Enemys [50,75]
    AttackPlayerEnemy[] FindAllCloseEnemys(int Distance,AttackPlayerEnemy[] APE)
    {
        List<AttackPlayerEnemy> CloseEnemys = new List<AttackPlayerEnemy>();

        if(APE.Length == 0)
            return null;

        for (int i = 0; i < APE.Length; i++)
        {
            if (Vector3.Distance(transform.position, APE[i].transform.position) < Distance)
            {
                CloseEnemys.Add(APE[i]);
            }
        };

        return CloseEnemys.ToArray();
    } // Finds All Near By Enemys

    IEnumerator SummonDemon(GameObject Demon,Transform Where,float Stay)
    {
        GameObject SummonedDemon = Instantiate(Demon,Where);

        yield return new WaitForSeconds(Stay);
        Destroy(SummonedDemon);
    }
}
