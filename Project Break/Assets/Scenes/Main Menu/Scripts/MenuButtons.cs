using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] Animator Menus;
    [SerializeField] SceneHandler SH;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void NewGame()
    {
        StartCoroutine(SH.ChangeScene(1));
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
