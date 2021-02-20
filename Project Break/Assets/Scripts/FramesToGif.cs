using UnityEngine;
using UnityEngine.UI;
public class FramesToGif : MonoBehaviour
{
    [SerializeField] Image MainTex;
    [SerializeField] Sprite[] Frames;
    [SerializeField] float FPS;

    void Update()
    {
        int index = Mathf.RoundToInt(Time.time * FPS);
        index = index % Frames.Length;
        MainTex.sprite = Frames[index];
    }
}
