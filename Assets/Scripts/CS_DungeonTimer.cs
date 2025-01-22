using System.Collections;
using UnityEngine;

public class CS_DungeonTimer : MonoBehaviour
{
    public float m_dungeonTimer, m_maxdungeonTimer;
    private float flickerTimer;

    public GameObject m_warningForTimerOBJ, m_outOfTimeOBJ;
    public bool m_activeFlicker;
    int flickerCount = 0;
    public void Update()
    {
        m_dungeonTimer += Time.deltaTime;
        CountingTimer();
        FlickerWarning();
        if (m_activeFlicker)
        {
            flickerTimer += Time.deltaTime;
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
        print("Start Counting");
        if(m_dungeonTimer >= m_maxdungeonTimer / 2)
        {
            print("HalfWay");
            m_activeFlicker = true;
        }

        if(m_dungeonTimer >= m_maxdungeonTimer)
        {
            print("TimeIsUp");
            m_outOfTimeOBJ.SetActive(true);
            m_dungeonTimer = m_maxdungeonTimer;
        }
    }

    private void FlickerWarning()
    {
        if(m_activeFlicker)
        {
            print("Flickering");
            if (flickerTimer >= 0.5f)
            {
                print("FirstFlicker");
                m_warningForTimerOBJ.SetActive(false);
                flickerCount++;
                flickerTimer = 0;
            }
            else if(flickerTimer <= 0.5f)
            {
                m_warningForTimerOBJ.SetActive(true);
            }

        }
        if (flickerCount >= 10) print("EndOfFlicker"); m_warningForTimerOBJ.SetActive(false); return;
    }
}
