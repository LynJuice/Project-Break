using UnityEngine;

public class OpenNewScene : MonoBehaviour
{
    [SerializeField] SceneHandler SH;
    [SerializeField] int SceneIndex;

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(SH.ChangeScene(SceneIndex));
    }
}
