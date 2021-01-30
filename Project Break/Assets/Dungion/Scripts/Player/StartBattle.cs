using UnityEngine;
using UnityEngine.SceneManagement;
public class StartBattle : MonoBehaviour
{
    [SerializeField] string BattleScene;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadBattleScene(true);
        }
    }

    void LoadBattleScene(bool Advantige)
    {
        if (Advantige)
        {
            PlayerPrefs.SetInt("Adventige", 1);
        }
        else 
        {
            PlayerPrefs.SetInt("Adventige",0);
        }

        SceneManager.LoadScene(BattleScene);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            LoadBattleScene(true);
        }
    }
}
