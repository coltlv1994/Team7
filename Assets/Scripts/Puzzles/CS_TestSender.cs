using Interactables;
using UnityEngine;

namespace Puzzles
{
    public class CS_TestSender : CS_Sender, IInteractable
    {
        public bool Focused { get; set; }
        public bool IsActive { get; set; }

        [SerializeField] private Material _outline;
        private Renderer _renderer;
        
        public Material Outline => _outline;
        public Renderer Renderer => _renderer ??= GetComponent<Renderer>();

        private GameObject _displayCube;
        public void OnInteract()
        {
            Debug.Log("INTERACT!");
        }
        public void OnActivate()
        {
            _displayCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _displayCube.transform.position = transform.position;
            _displayCube.transform.position += new Vector3 (0, transform.localScale.y, 0);
            _displayCube.transform.rotation = transform.rotation;
            _displayCube.transform.localScale = transform.localScale / 4;
            SendInput();
        }
        public void OnDeactivate()
        {
            Destroy(_displayCube);
            RevokeInput();
        }
    }
}
