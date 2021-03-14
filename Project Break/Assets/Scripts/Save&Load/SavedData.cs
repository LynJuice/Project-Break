using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SavedData
{
    public int Scene;
    public float[] Position;
  //  public List<Item> Items;

    public SavedData(PlayerMovement PM,Inventory INV)
    {
        Scene = SceneManager.GetActiveScene().buildIndex;

        Position = new float[3];
        Position[0] = PM.GetComponent<Transform>().position.x;
        Position[1] = PM.GetComponent<Transform>().position.y;
        Position[2] = PM.GetComponent<Transform>().position.z;

     //   Items = INV.Items;
    }
}
