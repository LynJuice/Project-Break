using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            SaveButton(1);

        if (Input.GetKeyDown(KeyCode.Y))
            LoadButton(1);
    }

    public void SaveButton(int Slot)
    {
        SaveSystem.SaveData(Slot,FindObjectOfType<PlayerMovement>(),FindObjectOfType<Inventory>());
    }

    public void LoadButton(int Slot)
    {
        Debug.Log("Load");
        SavedData data = SaveSystem.LoadData(Slot);

        SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
        PlayerPrefs.SetFloat("FromSave",1);
        FindObjectOfType<SceneDataTransfer>().data = data;


        
        StartCoroutine(sceneHandler.ChangeScene(data.Scene));
    }
}
