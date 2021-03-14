using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        LoadingIcon.GetComponentInParent<Image>().enabled = true;
        FadeAnimator.SetTrigger("Fade Out");
        yield return new WaitForSeconds(0.8f);
        LoadingIcon.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneIndex,LoadSceneMode.Single);
    }

    IEnumerator OnLoaded()
    {
        int FromSave = PlayerPrefs.GetInt("FromSave");

        if (FromSave == 0)
        {
            FadeAnimator.SetTrigger("Fade In");
            LoadingIcon.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < OtherUI.Length; i++)
            {
                OtherUI[i].enabled = true;
            }
            yield return new WaitForSeconds(4);
            LoadingIcon.GetComponentInParent<Image>().enabled = false;
        }else
        {
            PlayerPrefs.DeleteKey("FromSave");

            PlayerMovement Pm = FindObjectOfType<PlayerMovement>();
            SavedData data = FindObjectOfType<SceneDataTransfer>().data;

            Vector3 Pos = new Vector3(data.Position[0], data.Position[1], data.Position[2]);
            Pm.GetComponent<Transform>().position = Pos;

         //   Pm.GetComponent<Inventory>().Items = data.Items;

            FadeAnimator.SetTrigger("Fade In");
            LoadingIcon.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < OtherUI.Length; i++)
            {
                OtherUI[i].enabled = true;
            }
            yield return new WaitForSeconds(4);
            LoadingIcon.GetComponentInParent<Image>().enabled = false;
        }
    }

    void Start()
    {
        StartCoroutine(OnLoaded());
    }
}
