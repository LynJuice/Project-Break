using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] Animator FadeAnimator;
    public IEnumerator ChangeScene(int SceneIndex)
    {
        FadeAnimator.SetTrigger("Fade Out");
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(SceneIndex);
    }

    void OnLoaded()
    {
        FadeAnimator.SetTrigger("Fade In");
    }

    void Start()
    {
        OnLoaded();
    }
}
