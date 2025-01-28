//Bisma
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    //States of the main menu

    private MainMenuState currentState;

    public GameObject mainMenu;
    public GameObject newGame;
    public GameObject loadGame;
    public GameObject options;

    public Button newGameButton;
    public Button loadGameButton;
    public Button optionsButton;
    public Button exitButton;

    public Button yesNewGameButon;
    public Button noNewGameButton;

    public Button yesLoadGameButton;
    public Button noLoadGameButton;

    void Start()

    {
        SetState(MainMenuState.MainMenu);


        newGameButton.onClick.AddListener(() => SetState(MainMenuState.NewGame));
        loadGameButton.onClick.AddListener(() => SetState(MainMenuState.LoadGame));
        optionsButton.onClick.AddListener(() => SetState(MainMenuState.Options));
        exitButton.onClick.AddListener(() => SetState(MainMenuState.Exit));


        yesNewGameButon.onClick.AddListener(StartNewGame);
        noNewGameButton.onClick.AddListener(BackMainMenuFromNewGame);


        yesLoadGameButton.onClick.AddListener(LoadGame);
        noNewGameButton.onClick.AddListener(BackMainMenuFromLoadGame);

    }

    void Update()
    {

        switch (currentState)

        {
            case MainMenuState.MainMenu:
                HandleMainMenu();
                break;

            case MainMenuState.NewGame:
                HandleNewGame();
                break;

            case MainMenuState.LoadGame:
                HandleLoadGame();
                break;

            case MainMenuState.Options:
                HandleOptions();
                break;

            case MainMenuState.Exit:
                HandleExit();
                break;

        }

    }

    private void SetState(MainMenuState newState)
    {
        currentState = newState;

        mainMenu.SetActive(false);
        newGame.SetActive(false);
        loadGame.SetActive(false);
        options.SetActive(false);

        switch (newState)

        {
            case MainMenuState.MainMenu:
                mainMenu.SetActive(true);
                break;

            case MainMenuState.NewGame:
                newGame.SetActive(true);
                break;

            case MainMenuState.LoadGame:
                loadGame.SetActive(true);
                break;

            case MainMenuState.Options:
                options.SetActive(true);
                break;

            case MainMenuState.Exit:
                Exitgame();
                break;

        }

    }

    private void HandleMainMenu()
    {

    }

    private void HandleNewGame()
    {

    }

    private void HandleLoadGame()
    {

    }

    private void HandleOptions()
    {

    }

    private void HandleExit()
    {

    }

    private void StartNewGame()
    {
        SceneManager.LoadScene("");
    }

    private void BackMainMenuFromNewGame()
    {
        SetState(MainMenuState.MainMenu);
    }

    private void LoadGame()

    {

    }

    private void BackMainMenuFromLoadGame()
    {
        SetState(MainMenuState.MainMenu);
    }

    private void Exitgame()

    {
        Application.Quit();
    }



}
