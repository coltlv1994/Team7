// Created by Linus Jernström

using System;
using System.Collections;
using UnityEngine;

public class CS_DoorCamera : MonoBehaviour
{
    private GameObject _player;
    private Camera _camera;
    private FPSController _playerController;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<FPSController>();
        _camera = _player.GetComponentInChildren<Camera>();
    }

    public void MoveCameraToPos(Transform targetTransform, float lookTime = 2f)
    {
        _playerController.canMove = false;
        
        Vector3 oldPosition = _camera.transform.position;
        Vector3 oldRotation = _camera.transform.localEulerAngles;

        _camera.transform.position = targetTransform.position;
        _camera.transform.rotation = targetTransform.rotation;

        IEnumerator coroutine = MoveCameraToPlayer(lookTime, oldPosition, oldRotation);
        StartCoroutine(coroutine);
    }

    private IEnumerator MoveCameraToPlayer(float waitTime, Vector3 position, Vector3 rotation)
    {
        yield return new WaitForSeconds(waitTime);

        _playerController.canMove = true;

        _camera.transform.position = position;
        _camera.transform.localEulerAngles = rotation;

    }
}
