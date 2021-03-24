using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDataTransfer : MonoBehaviour
{
    [Header("Saved Data")]
    public SavedData data;
    public int LastScene;
    void Start()
    {   
        DontDestroyOnLoad(this);
    }

    [Header("Temp Data")]
    public SavedData TempData;
}
