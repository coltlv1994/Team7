// Created by Linus Jernström
using UnityEngine;

namespace Interactables
{
    public class CS_TestInteractions : CS_InteractableObject
    {
        private GameObject _displayCube;
        public override void OnInteract()
        {
            Debug.Log("INTERACT!");
        }
        public override void OnActivate()
        {
            _displayCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _displayCube.transform.position = transform.position;
            _displayCube.transform.position += new Vector3 (0, transform.localScale.y, 0);
            _displayCube.transform.rotation = transform.rotation;
            _displayCube.transform.localScale = transform.localScale / 4;
            Debug.Log("ACTIVATE");
        }
        public override void OnDeactivate()
        {
            Destroy(_displayCube);
            Debug.Log("DEACTIVATE");
        }
    }
}
