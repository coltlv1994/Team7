using System;
using UnityEngine;
using static CS_EnemyScript;

public class CameraShake : MonoBehaviour //Created by Elliot 
{
    // m_cameraShake = FindAnyObjectByType<CameraShake>(); - Take this into Start, Awake or OnEnable of the script that will start camera shake
    // m_cameraShake.ActiveShake(); - Use this where you want to turn on and off the camera shaking

    [Header("Camera Settings")]
    [SerializeField] private float m_shakeAmount;
    private Vector3 m_ininitalPos;
    public bool m_isActiveShake;

    [Header("Chase Sequence")]
    [SerializeField] Transform m_bunnyOBJ;
    public bool wantInteractiveShaking;
    [SerializeField] double minValueA;
    [SerializeField] double minValueB;
    [SerializeField] double minValueC;
    [SerializeField] double minValueD;
    [SerializeField] double minValueE, maxValueE;


    void Awake()
    {
        m_ininitalPos = transform.localPosition;
    }

    public void ActiveShake()
    {
        m_isActiveShake = !m_isActiveShake;
    }

    public bool IsBetween(double testValue, double bound1, double bound2)
    {
        return (testValue >= Math.Min(bound1, bound2) && testValue <= Math.Max(bound1, bound2));
    }

    void Update()
    {
        if(wantInteractiveShaking)
        {
            float distance = Vector3.Distance(transform.position, m_bunnyOBJ.position);
            print(distance);
            if (distance > maxValueE) { m_isActiveShake = false; m_shakeAmount = 0f; }
            else m_isActiveShake = true;

            if (IsBetween(distance, minValueE, maxValueE))
            {
                m_shakeAmount++;
                if (m_shakeAmount >= 0.05) m_shakeAmount = 0.05f;
            }

            if (IsBetween(distance, minValueD, minValueE))
            {
                m_shakeAmount++;
                if (m_shakeAmount >= 0.1) m_shakeAmount = 0.1f;
            }

            if (IsBetween(distance, minValueC, minValueD))
            {
                m_shakeAmount++;
                if (m_shakeAmount >= 0.15) m_shakeAmount = 0.15f;
            }

            if (IsBetween(distance, minValueB, minValueC))
            {
                m_shakeAmount++;
                if (m_shakeAmount >= 0.25) m_shakeAmount = 0.25f;
            }

            if (IsBetween(distance, minValueA, minValueB))
            {
                m_shakeAmount++;
                if (m_shakeAmount >= 0.5) m_shakeAmount = 0.5f;
            }
        }
    
        if (m_isActiveShake)
        {
            transform.localPosition = m_ininitalPos + UnityEngine.Random.insideUnitSphere * m_shakeAmount;
        }
    }
}
