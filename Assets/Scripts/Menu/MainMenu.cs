using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        Debug.Log("Start Game");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
        
        // If running in the editor, stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
