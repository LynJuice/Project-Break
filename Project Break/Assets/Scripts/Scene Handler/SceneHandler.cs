using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] Animator FadeAnimator;
    [SerializeField] Canvas[] OtherUI;
    [SerializeField] GameObject LoadingIcon;
    public IEnumerator ChangeScene(int SceneIndex)
    {
        for (int i = 0; i < OtherUI.Length; i++)
        {
            OtherUI[i].enabled = false;
        }

        FadeAnimator.SetTrigger("Fade Out");
        yield return new WaitForSeconds(0.8f);
        LoadingIcon.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneIndex,LoadSceneMode.Single);
    }

    void OnLoaded()
    {
        FadeAnimator.SetTrigger("Fade In");
        LoadingIcon.SetActive(false);
    }

    void Start()
    {
        OnLoaded();
    }
}
