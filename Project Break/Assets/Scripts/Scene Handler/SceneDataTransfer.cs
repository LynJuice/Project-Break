using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDataTransfer : MonoBehaviour
{
    public SavedData data;
    void Start()
    {   
        DontDestroyOnLoad(this);
    }
}
