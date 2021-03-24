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
        if(FindObjectOfType<SceneDataTransfer>() && FindObjectOfType<PlayerMovement>())
        {
            SavedData data = null;

            PlayerMovement Pm = FindObjectOfType<PlayerMovement>();
            if (FindObjectOfType<SceneDataTransfer>().TempData != null)
                data = FindObjectOfType<SceneDataTransfer>().data;
            else
                data = FindObjectOfType<SceneDataTransfer>().TempData;
           if(data != null)
                LoadData(data, Pm);
        }

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

    void Start()
    {
        StartCoroutine(OnLoaded());
    }

    void LoadData(SavedData data,PlayerMovement Player)
    {
        Inventory inv = Player.GetComponent<Inventory>();

        Vector3 PositionData = new Vector3(data.Position[0], data.Position[1], data.Position[2]);
        int CashData = data.Cash;

        Player.GetComponent<Transform>().position = PositionData;
        inv.Cash = CashData;

        Player.Day = data.Day;
        Player.Month = data.Month;

        for (int i = 0; i < data.Items.Length; i++)
        {
            Item ItemToAdd = null;
            switch(data.Items[i])
            {
                case 0 :
                    ItemToAdd = Resources.Load("Items/Charge Pot") as Item;
                    break;
                case 1:
                    ItemToAdd = Resources.Load("Items/Heal Pot") as Item;
                    break;
                case 2:
                    ItemToAdd = Resources.Load("Items/Key") as Item;
                    break;
            }

            if (ItemToAdd == null)
            {
                Debug.LogError("Unassinged Item ID");
            }

            inv.AddItem(ItemToAdd);
        }
    }
}
