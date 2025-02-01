using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_PlayerHealthbar : MonoBehaviour
{
    [SerializeField] Slider m_Slider;
    public int maxHealth;
    public int currentHealth;
    public GameObject playerOBJ;
    public CharacterController characterController;
    public Transform m_respawnLocation;

    public GameObject m_takingDamageOBJ;
    private RawImage m_colorChanger;
    private float m_transparency = 0.15f;
    public float m_speedOfFading = 30f;

    [SerializeField] CS_DamageIndicatorUI m_damageIndicator;
    public CS_RespawnCheck CS_RespawnCheck;

    private void Start()
    {
        currentHealth = maxHealth;
        m_colorChanger = m_takingDamageOBJ.GetComponent<RawImage>();
    }

    public void TakeDamage(int amount, Vector3 damagePositon, bool needIndicator)
    {
        currentHealth -= amount;

        if(needIndicator)
        {
            m_damageIndicator.DamageLocation = damagePositon;
            GameObject damageIndicatorOBJ = Instantiate(m_damageIndicator.gameObject, m_damageIndicator.transform.position, m_damageIndicator.transform.rotation, m_damageIndicator.transform.parent);
            damageIndicatorOBJ.SetActive(true);
        }

        m_takingDamageOBJ.SetActive(true);
        m_Slider.value = currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            characterController.enabled = false;
            CS_RespawnCheck.Respawn();
            characterController.enabled = true;
            currentHealth = maxHealth;
            print("Player Has Died");
        }
    }

    public void UpdateFixedHealtBar(int currentValue, int maxValue)
    {
        m_Slider.value = currentValue / maxValue;
    }

    void Update()
    {
        TakingDamageEffect();
        m_Slider.value = currentHealth;
    }
    private void TakingDamageEffect()
    {
        if (m_takingDamageOBJ.activeSelf)
        {
            m_transparency -= Time.fixedDeltaTime / m_speedOfFading;
            m_colorChanger.color = new Color(1f, 0f, 0f, m_transparency);
            if (m_transparency <= 0f)
            {
                m_takingDamageOBJ.SetActive(false);
                m_transparency = 0.15f;
            }
        }
    }
}
