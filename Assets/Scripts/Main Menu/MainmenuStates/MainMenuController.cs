//Bisma, Bogdan
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject newGameConfirmation;
    public GameObject loadGameConfirmation;
    public GameObject options;

    public string mainMenu = "Main";
    public string gameStart;
    public string newGameStart;
    public string gameLoad;
    [SerializeField] private float loadingDelay = 5f;

    public Button newGameButton;
    public Button loadGameButton;
    public Button optionsButton;
    public Button exitButton;

    public Button yesNewGameButton;
    public Button noNewGameButton;

    public Button yesLoadGameButton;
    public Button noLoadGameButton;

    public Button backButton;

    public GameObject settings;
    [SerializeField] private CameraMovement cameraController;

    [SerializeField] private GameObject startScreen;

    //public GameSettingsPersistent gsp;

    private void Start()
    {

        SetState(MainMenuState.MainMenu);

        newGameButton.onClick.AddListener(() => SetState(MainMenuState.NewGame));
        loadGameButton.onClick.AddListener(() => SetState(MainMenuState.LoadGame));
        optionsButton.onClick.AddListener(() => SetState(MainMenuState.Options));
        exitButton.onClick.AddListener(() => SetState(MainMenuState.Exit));

        yesNewGameButton.onClick.AddListener(StartNewGame);
        noNewGameButton.onClick.AddListener(BackToMainMenu);

        yesLoadGameButton.onClick.AddListener(LoadGame);
        noLoadGameButton.onClick.AddListener(BackToMainMenu);

        backButton.onClick.AddListener(BackToMainMenu);

    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Destroy(startScreen);
        }
    }

    private void SetState(MainMenuState newState)
    {
        startMenu.SetActive(newState == MainMenuState.MainMenu);
        newGameConfirmation.SetActive(newState == MainMenuState.NewGame);
        loadGameConfirmation.SetActive(newState == MainMenuState.LoadGame);
        options.SetActive(newState == MainMenuState.Options);

        if (newState == MainMenuState.Exit)
        {
            ExitGame();
        }
    }

    public void Startgame()
    {
        SceneManager.LoadScene(1);
    }

    private void StartNewGame()
    {
        //Reset save file?
        SceneManager.LoadScene(1);
    }

    private void LoadGame()
    {
        //player prefas to load saved scene data
        settings.GetComponent<GameSettingsPersistent>().isLoadingSave = true;
        SceneManager.LoadScene(newGameStart);


        // start new game as normal
        // in PrototypeTimer.cs, it will check settings.isLoadingSave,
        // if we found saving file and this is true, the game will load from save
        // otherwise a new game will start
    }

    private void BackToMainMenu()
    {
        SetState(MainMenuState.MainMenu);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

}
