using UnityEngine;
using UnityEngine.UI;

public class CS_DamageIndicatorUI : MonoBehaviour
{
    public Vector3 DamageLocation;
    public Transform playerOBJ;
    public Transform DamageImagePivot;

    public CanvasGroup DamageImageCanvas;
    public float FadeStartTime = 1.5f, FadeTime = 1.5f;
    float maxFadeTime;
    void Start()
    { 
        if(DamageImagePivot != null || DamageImageCanvas != null)
        {
            DamageImagePivot = GetComponent<RectTransform>();
            DamageImageCanvas = GetComponent<CanvasGroup>();
        }
        maxFadeTime = FadeTime;
    }

    void Update()
    {
        if(FadeStartTime > 0) { FadeStartTime -= Time.deltaTime;}
        else 
        { 
            FadeTime -= Time.deltaTime; 
            DamageImageCanvas.alpha = FadeTime / maxFadeTime;
            if(FadeTime <= 0) Destroy(this.gameObject);  
        }

        DamageLocation.y = playerOBJ.position.y;
        Vector3 Direction = (DamageLocation - playerOBJ.position).normalized;
        float angel = (Vector3.SignedAngle(Direction, playerOBJ.forward, Vector3.up));
        DamageImagePivot.transform.localEulerAngles = new Vector3 (0, 0, angel);    
    }
}
