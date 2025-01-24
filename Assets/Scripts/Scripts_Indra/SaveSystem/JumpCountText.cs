//using UnityEngine;
//using TMPro;
//public class JumpCountText : MonoBehaviour, IData
//{
//    private int jumpCount = 0;

//    private TextMeshProUGUI jumpCountText;

//    private void Awake()
//    {
//        jumpCountText = this.GetComponent<TextMeshProUGUI>();
//    }

//    public void LoadData(GameData data)
//    {
//        this.jumpCount = data.jumpCount;
//    }

//    public void SaveData(ref GameData data)
//    {
//        data.jumpCount = this.jumpCount;
//    }

//    private void Start()
//    {
//        EventsManager.instance.onPlayerJump += OnPlayerJump;
//    }

//    private void OnDestroy()
//    {
//        EventsManager.instance.onPlayerJump -= OnPlayerJump;
//    }

//    private void OnPlayerJump()
//    {
//        jumpCount++;
//    }

//    private void Update()
//    {
//        jumpCountText.text = "" + jumpCount;
//    }
//}