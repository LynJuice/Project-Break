using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class TutorialBase : MonoBehaviour
{ 
    [SerializeField] ScriptableTutorial SampleTutorial;
    [SerializeField] Text NameField;
    [SerializeField] Text TutorialText;
    [SerializeField] GameObject Button;

    bool Next;

    private void Start()
    {
        StartCoroutine(StartTutorial(SampleTutorial));
    }

    void Update()
    {
        Next = Input.GetKey(KeyCode.H);
    }

    public IEnumerator StartTutorial(ScriptableTutorial Tutorial)
    {
        GetComponent<Canvas>().enabled = true;
        NameField.text = Tutorial.Name;

        Time.timeScale = 0;

        for (int i = 0; i < Tutorial.Lines.Length; i++)
        {
            TutorialText.text = Tutorial.Lines[i];
            yield return new WaitForSecondsRealtime(1);
            Button.SetActive(true);
            yield return new WaitUntil(() => Next);
            Button.SetActive(false);
        }

        GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;
    }
}

