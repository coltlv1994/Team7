using UnityEngine;
using UnityEngine.UI;

public class CS_PlayerHealthbar : MonoBehaviour
{
    [SerializeField] Slider m_Slider;
    public int maxHealth;
    public int currentHealth;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        m_Slider.value = currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
           m_Slider.enabled = false;
            print("Player Has Died");
        }
    }

    public void UpdateFixedHealtBar(int currentValue, int maxValue)
    {
        m_Slider.value = currentValue / maxValue;
    }

    void Update()
    {
        m_Slider.value = currentHealth;
    }
}
