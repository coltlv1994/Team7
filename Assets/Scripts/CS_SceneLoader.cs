//Created by Linus Jernström
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_SceneLoader : MonoBehaviour
{
    private GameSettingsPersistent _settings;

    void Start()
    {
        _settings = GameObject.FindGameObjectWithTag("GlobalSettings")?.GetComponent<GameSettingsPersistent>();
        if (_settings == null)
        {
            _settings = gameObject.AddComponent<GameSettingsPersistent>();
            _settings.tag = "GlobalSettings"; // NOTE: this line is for debugging purpose and should be removed before final build.
        }
        
        this.gameObject.SetActive(false);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadGame()
    {
        //Reset save file?
        _settings.isLoadingSave = true;
        SceneManager.LoadScene("Main");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartDay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
