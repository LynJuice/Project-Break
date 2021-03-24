using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public struct ScriptableLines
{
    public ScriptableTalker Talker;
    public string line;
}

[CreateAssetMenu(fileName = "Conversation", menuName = "Dialouge Tutorial System/New Conversation")]
public class ScriptableConversation : ScriptableObject
{
    public ScriptableLines[] Conversation;
}

public class DialougeBase : MonoBehaviour
{
    [Header("Not Player Character Speak")]
    [SerializeField] Text NameSpace;
    [SerializeField] Text TextSpace;
    [SerializeField] Image TalkerIcon;
    [SerializeField] Image TalkerFace;
    [SerializeField] GameObject DialougeBox;

    bool Open;

    [SerializeField] ScriptableConversation TestConversation;

    bool AnyKeyIsPressed;
    bool LastLineCompleted;

    private void Update()
    {
        AnyKeyIsPressed = Input.anyKey;
    }

    private void Start()
    {
        StartCoroutine(StartConversation(TestConversation));
    }

    public IEnumerator StartConversation(ScriptableConversation SC)
    {
        if (!Open)
        {
            DialougeBox.SetActive(true);
            Open = true;
        }

        for (int i = 0; i < SC.Conversation.Length; i++)
        {
           StartCoroutine(Talk(SC.Conversation[i].Talker, SC.Conversation[i].line));

           yield return new WaitUntil(() => LastLineCompleted);
           yield return new WaitUntil(() => AnyKeyIsPressed);
        }

        EndConversation();
    }

    void EndConversation()
    {
        if (Open)
        {
            DialougeBox.SetActive(false);
            Open = false;
        }
    }

    public IEnumerator Talk(ScriptableTalker Talker, string Dialouge)
    {
        LastLineCompleted = false;
        NameSpace.text = Talker.Name;
        TextSpace.text = "";
        for (int i = 0; i < Dialouge.Length; i++)
        {
            TextSpace.text += Dialouge[i];
            yield return new WaitForSeconds(0.05f);
        }
        LastLineCompleted = true;
    }
}
