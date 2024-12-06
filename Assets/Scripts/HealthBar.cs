using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar; // Referencia al componente Slider
    public Health playerHealth; // Referencia al script Health del jugador

    private void Start()
    {
        // Intentar encontrar el script Health del jugador
        playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Health>();
        if (playerHealth == null)
        {
            Debug.LogError("El componente 'Health' no fue encontrado en el objeto etiquetado como 'Player'.");
            return;
        }

        // Intentar encontrar el Slider
        healthBar = GetComponentInChildren<Slider>();
        if (healthBar == null)
        {
            Debug.LogError("El componente 'Slider' no está asignado o no existe en el objeto actual.");
            return;
        }

        // Inicializar los valores del Slider
        healthBar.maxValue = playerHealth.maxHealth;
        healthBar.value = playerHealth.maxHealth;
    }

    // Método para actualizar la barra de salud
    public void SetHealth(int hp)
    {
        if (healthBar != null)
        {
            healthBar.value = hp;
        }
        else
        {
            Debug.LogError("El Slider no está asignado.");
        }
    }
}
