using UnityEngine;

public class Health : MonoBehaviour
{
    public int curHealth = 0;
    public int maxHealth = 100;
    public HealthBar healthBar;

    void Start()
    {
        curHealth = maxHealth;
        if (healthBar == null)
        {
            Debug.LogError("Health: El componente 'HealthBar' no está asignado en el inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DamagePlayer(10);
        }
    }

    public void DamagePlayer(int damage)
    {
        curHealth -= damage;
        if (healthBar != null)
        {
            healthBar.SetHealth(curHealth);
            Debug.Log($"Health: Salud actualizada a {curHealth}.");
        }
        else
        {
            Debug.LogError("Health: No se puede actualizar la barra de salud porque no está asignada.");
        }
    }
}
