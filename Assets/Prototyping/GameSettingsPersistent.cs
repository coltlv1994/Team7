using UnityEngine;

public class GameSettingsPersistent : MonoBehaviour
{
    public static GameSettingsPersistent settings;

    [SerializeField] public bool isLoadingSave = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); //I commented this out because the canvas kept overlaying every time player dies and resets
    }
}
