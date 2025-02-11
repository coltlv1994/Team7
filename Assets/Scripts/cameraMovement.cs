using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


public class CameraMovement : MonoBehaviour
{
    [System.Serializable]
    [Tooltip("Preset values for the camera's rotation, position, and scale")]
    public class TransformPresets
    {
        public Vector3[] rotations;
        public Vector3[] transforms;
        public Vector3[] scales;
        public Button[] buttons;
    }

    [Header("Camera Settings")]

    [InspectorName("Move Speed")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private float smoothRotationSpeed = 5f;
    [SerializeField] private float smoothTranslationSpeed = 5f;

    [Header("Transform Presets")]
    [SerializeField] private TransformPresets presets;

    private Quaternion currentRotation;
    private Quaternion targetRotation;private Vector3 currentPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        currentPosition = transform.position;
        targetPosition = currentPosition;
        currentRotation = transform.rotation;
        targetRotation = currentRotation;

        SetupPresetButtons();
    }

    private void Update()
    {
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * smoothTranslationSpeed);
        transform.position = currentPosition;
        
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * smoothRotationSpeed);
        transform.rotation = currentRotation;

        
    }

    #region Transform Methods

    // Rotate to a specific preset
    public void RotateTo(int presetIndex)
    {
        if (presets != null && presetIndex < presets.rotations.Length)
        {
            targetRotation = Quaternion.Euler(presets.rotations[presetIndex]);
        }
    }

    // Move to a specific preset
    public void TransformTo(int presetIndex)
    {
        if (presets != null && presetIndex < presets.transforms.Length)
        {
            targetPosition = presets.transforms[presetIndex];
        }
    }

    // Scale to a specific preset
    public void ScaleTo(int presetIndex)
    {
        if (presets != null && presetIndex < presets.scales.Length)
        {
            transform.localScale = presets.scales[presetIndex];
        }
    }
    #endregion

    private void SetupPresetButtons()
    {
        for (int i = 0; i < presets.buttons.Length; i++)
        {
            int index = i; // Capture the index for the lambda
            presets.buttons[i].onClick.AddListener(() => {
                RotateTo(index);
                TransformTo(index);
                ScaleTo(index);
            });
        }
    }

    private void OnDestroy()
    {
        // Clean up button listeners
        foreach (Button button in presets.buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}