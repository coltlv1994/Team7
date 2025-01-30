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

    private void Start()
    {
        currentHealth = maxHealth;
        m_colorChanger = m_takingDamageOBJ.GetComponent<RawImage>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        m_takingDamageOBJ.SetActive(true);
        m_Slider.value = currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            characterController.enabled = false;
            playerOBJ.transform.position = m_respawnLocation.position;
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
        TakingDamgeEffect();
        m_Slider.value = currentHealth;
    }
    private void TakingDamgeEffect()
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
