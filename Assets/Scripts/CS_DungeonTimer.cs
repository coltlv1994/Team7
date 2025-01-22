using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CS_DungeonTimer : MonoBehaviour
{
    public float m_dungeonTimer, m_maxdungeonTimer;
    private float flickerTimer;

    public GameObject m_warningForTimerOBJ, m_outOfTimeOBJ;
    bool m_activeFlicker;
    int flickerCount = 0;
    public void Update()
    {
        m_dungeonTimer += Time.deltaTime;
        CountingTimer();
        if (m_activeFlicker)
        {
            flickerTimer += Time.deltaTime;
            FlickerWarning();
        }
    }

    private void OnEnable()
    {
        m_activeFlicker = false;
        flickerTimer = 0;
        m_dungeonTimer = 0;
        flickerCount = 0;
        m_warningForTimerOBJ.SetActive(false); m_outOfTimeOBJ.SetActive(false);
    }
    private void CountingTimer()
    {
        if(m_dungeonTimer >= m_maxdungeonTimer / 2)
        {
            if(flickerCount == 0)
            {
                m_activeFlicker = true;
            }
        }

        if(m_dungeonTimer >= m_maxdungeonTimer)
        {
            m_outOfTimeOBJ.SetActive(true);
            m_dungeonTimer = m_maxdungeonTimer;
        }
    }

    private void FlickerWarning()
    {
            if (flickerTimer <= 0.5f)
            {              
              m_warningForTimerOBJ.SetActive(true);
            }
            else
            {
                m_warningForTimerOBJ.SetActive(false);
                if (flickerTimer >= 1f)
                {
                    flickerCount++;
                    flickerTimer = 0;
                }
            }
            if (flickerCount >= 5) 
            {
                m_warningForTimerOBJ.SetActive(false);
                m_activeFlicker = false;
            }
    }
}
