using Puzzles;
using UnityEngine;

namespace Puzzles
{
    public class CS_TestReceiver : CS_Receiver
    {

        protected override void Activate()
        {
            State = ReceiverState.Active;
            Debug.LogWarning("ACTIVE RECEIVER");
        }
        protected override void Deactivate()
        {
            State = ReceiverState.Inactive;
            Debug.LogWarning("DEACTIVE RECEIVER");
        }
    }
}

