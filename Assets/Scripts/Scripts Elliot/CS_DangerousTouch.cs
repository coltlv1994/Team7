using System.Collections;
using UnityEngine;

public class CS_DangerousTouch : MonoBehaviour // Created by Elliot
{
    public GameObject m_playerOBJ;
    public CS_PlayerHealthbar m_playerHealthbar;
    public int m_damageDealPerHalfSecond;
    bool m_isOutSideDangerous;

    private void Start()
    {
        m_playerOBJ = FindAnyObjectByType<FPSController>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == m_playerOBJ)
        {
            m_isOutSideDangerous = false;
            StartCoroutine(TakingDangerousTouchDamage());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == m_playerOBJ)
        {
            m_isOutSideDangerous = true;
            StopCoroutine(TakingDangerousTouchDamage());
        }
    }

    private IEnumerator TakingDangerousTouchDamage()
    {
        if(m_playerHealthbar.currentHealth <= 0) StopCoroutine(TakingDangerousTouchDamage());
        m_playerHealthbar.TakeDamage(m_damageDealPerHalfSecond, this.transform.position, false);
        yield return new WaitForSeconds(0.5f);
        if(!m_isOutSideDangerous) StartCoroutine(TakingDangerousTouchDamage());
    }
}
