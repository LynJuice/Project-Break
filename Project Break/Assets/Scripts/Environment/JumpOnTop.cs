using UnityEngine;

public class JumpOnTop : MonoBehaviour
{
    public Transform JumpSpot;
    [SerializeField] PlayerMovement Player;
    [SerializeField] GameObject Show;
    [SerializeField] float MinDistance;

    void Update()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) < MinDistance)
        {
            Show.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
                Player.Jump(JumpSpot);
        }
        else
        {
            Show.SetActive(false);
        }
    }
}
