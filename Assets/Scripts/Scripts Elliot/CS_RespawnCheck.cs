using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CS_EnemyScript;

public class CS_RespawnCheck : MonoBehaviour //Created by Elliot
{
    public float m_raycastToGround;
    public List<Vector3> m_positionHistory = new List<Vector3>();
    [SerializeField] private float m_positionHistoryUpdateDelay;
    [SerializeField] private int m_maxHistoryEntries;
    private bool m_onGround;
    private float m_checkingOnGround;

    public bool m_trailRespawnCheckpoint;

    public PlayerCombatState state = 0;

    public enum PlayerCombatState
    {
        CombatState = 0,
        OutsideCombatState = 1,
    }

    private void Update()
    {
        if(state == PlayerCombatState.OutsideCombatState) m_trailRespawnCheckpoint = true;
        else if(state == PlayerCombatState.CombatState) m_trailRespawnCheckpoint = false;

        if (m_trailRespawnCheckpoint)
        {
            GroundCheck();
            m_checkingOnGround += Time.deltaTime;
            if (m_onGround && m_checkingOnGround >= m_positionHistoryUpdateDelay)
            {
                StartCoroutine(UpdatePositionHistory());
                m_checkingOnGround = 0;
            }
        }
        else { StopCoroutine(UpdatePositionHistory()); m_checkingOnGround = 0; }
    }

    public void Respawn()
    {
        transform.position = new Vector3(m_positionHistory[0].x, m_positionHistory[0].y, m_positionHistory[0].z);
        m_positionHistory.RemoveRange(1, m_positionHistory.Count - 1);
    }

    private void GroundCheck()
    {
        Vector3 down = transform.TransformDirection(Vector3.down) * m_raycastToGround;
        Debug.DrawRay(transform.position, down, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, down, out hit, m_raycastToGround)) m_onGround = true;
        else { m_onGround = false; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var point in m_positionHistory)
        {
            Gizmos.DrawSphere(new Vector3(point.x, point.y, point.z), 0.5f);
        }
    }

    private IEnumerator UpdatePositionHistory()
    {
       if (m_positionHistory.Count > m_maxHistoryEntries) m_positionHistory.RemoveAt(0);

       m_positionHistory.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        yield return null;
    }
}
