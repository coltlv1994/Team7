using UnityEngine;

public class AnimationReceiver : MonoBehaviour
{
    [SerializeField] PrototypeTimer timer;
    public void SwitchDay()
    {
        timer.NewDay();
    }
}
