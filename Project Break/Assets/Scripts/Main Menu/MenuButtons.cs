using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] Animator Menus;
    [SerializeField] SceneHandler SH;
    public void NewGame()
    {
        StartCoroutine(SH.ChangeScene(2));
    }

    public void LoadGame()
    {
        Menus.SetTrigger("To Load");
    }

    public void Config()
    {
        Menus.SetTrigger("To Config");
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void Back()
    {
        Menus.SetTrigger("To Menu");
    }

    public void Audio()
    { 
    
    }

    public void Graphics()
    { 
        
    }

    public void Keyboard()
    { 
        
    }
}
