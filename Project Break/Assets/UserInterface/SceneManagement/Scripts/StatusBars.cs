using UnityEngine.UI;
using UnityEngine;

public class StatusBars : MonoBehaviour
{
    [SerializeField] PlayerMovement Player;
    [SerializeField] Slider Health;

    void Update()
    {
        Health.value = Player.Health;
    }
}
