using UnityEngine;
using UnityEngine.Audio;
public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource AS;
    [SerializeField] PlayerMovement Player;
    bool Playing;
    void Update()
    {
        if (Player.Fighting)
        {
            AS.Play();
        }
    }
}
