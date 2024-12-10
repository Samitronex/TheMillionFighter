using UnityEngine;

public class HitboxDamage : MonoBehaviour
{
    public int damage = 10; // Daño que inflige el ataque

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detectar si el objeto golpeado tiene un componente Health
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage); // Aplicar daño
            Debug.Log($"Se infligieron {damage} puntos de daño a {other.gameObject.name}");
        }
    }
}