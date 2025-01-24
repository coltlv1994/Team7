using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPrefab; //Bisma Ayesh


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newState = currentGameState == GameState.Play
                ? GameState.Pause
                : GameState.Play;

            GameStateManager.Instance.SetState(newState);

            if (newState == GameState.Pause)
            {
                ShowPauseMenu();

            }

            else
            {

                HidePauseMenu();

            }
        }


     }

    public void ShowPauseMenu()
    {
        if (pauseMenuPrefab != null)
        {
            pauseMenuPrefab.SetActive(true);
        }

    }

    public void HidePauseMenu()
    {
        if (pauseMenuPrefab != null)
        {
            pauseMenuPrefab.SetActive(false);
        }

    }



}
