using UnityEngine;

public class Health : MonoBehaviour
{
    public int curHealth = 100; // Salud actual
    public int maxHealth = 100; // Salud máxima
    public HealthBar healthBar; // Referencia al script de la barra de salud
    public GameObject gameOverScreen; // Referencia a la pantalla de Game Over

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        curHealth = maxHealth;

        // Configurar barra de salud al inicio
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        else
        {
            Debug.LogError("Health: El componente 'HealthBar' no está asignado.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Health: No se encontró un componente Animator en el GameObject.");
        }

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false); // Asegurarse de que está desactivada al iniciar
        }
        else
        {
            Debug.LogError("Health: La referencia a 'GameOverScreen' no está asignada.");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // No hacer nada si el personaje ya está muerto

        curHealth -= damage;
        if (healthBar != null)
        {
            healthBar.SetHealth(curHealth); // Actualizar la barra de salud
        }

        Debug.Log($"{gameObject.name} recibió {damage} de daño. Salud actual: {curHealth}.");

        // Animación de daño
        if (animator != null)
        {
            animator.SetTrigger("IsHurt");
        }

        // Si la salud llega a 0, activar la muerte
        if (curHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
{
    isDead = true;
    Debug.Log($"{gameObject.name} ha muerto.");

    // Activar animación de muerte
    if (animator != null)
    {
        animator.SetTrigger("IsDeath");
    }

    // Detener el movimiento del Rigidbody2D
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.velocity = Vector2.zero; // Detener cualquier movimiento
        rb.bodyType = RigidbodyType2D.Static; // Congelar la posición
    }

    // Deshabilitar los controles del personaje
    if (TryGetComponent(out Player player))
    {
        player.enabled = false;
    }
    else if (TryGetComponent(out Player2Controller player2))
    {
        player2.enabled = false;
    }

    // Opcional: Desactivar colisionadores para evitar más interacciones
    Collider2D collider = GetComponent<Collider2D>();
    if (collider != null)
    {
        collider.enabled = false;
    }

    // Mostrar la pantalla de Game Over (si aplica)
    if (gameOverScreen != null)
    {
        gameOverScreen.SetActive(true); // Activar la pantalla de Game Over
    }
}
}