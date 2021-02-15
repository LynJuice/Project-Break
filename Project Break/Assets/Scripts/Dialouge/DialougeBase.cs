using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialougeBase : MonoBehaviour
{
    [SerializeField] Text TextField;
    [SerializeField] Text NameField;

    public IEnumerator WriteText(ScriptableTalker Speaker,string Text)
    {
       NameField.text = Speaker.Name;

        for (int i = 0; i < Text.Length; i++)
        {
            TextField.text += Text[i];
            yield return new WaitForSeconds(0.05f);
        }
    }
}
