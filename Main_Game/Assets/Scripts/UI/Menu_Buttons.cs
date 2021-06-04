using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu_Buttons : MonoBehaviour
{
    public void StartGame(string nameOfScene) 
    {
        SceneManager.LoadScene(nameOfScene);
    }
    public void GameOver() 
    {
        //This is when an action button occurs for gameover to start again or not.
    }
    public void QuitScene() 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit;
#endif
    }
}
