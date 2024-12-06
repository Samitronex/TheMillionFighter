using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("References")]
    public Animator animator;
    private Rigidbody2D rb;

    private bool isGrounded;

    private const string PLAYER_HITBOX_TAG = "PlayerHitbox"; // Tag de la hitbox del Player
    private float lastHurtTime = -Mathf.Infinity; // Último tiempo en que Player2 fue golpeado
    public float hurtCooldown = 0.5f; // Tiempo mínimo entre golpes consecutivos

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
    }

    private void HandleMovement()
    {
        float moveX = 0;

        // Movimiento con J y L
        if (Input.GetKey(KeyCode.L))
        {
            moveX = 1; // Movimiento a la derecha
            transform.localScale = new Vector3(1, 1, 1); // Asegurar orientación hacia la derecha
        }
        else if (Input.GetKey(KeyCode.J))
        {
            moveX = -1; // Movimiento a la izquierda
            transform.localScale = new Vector3(-1, 1, 1); // Voltear sprite hacia la izquierda
        }

        // Aplicar movimiento
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Actualizar animación
        animator.SetFloat("Speed", Mathf.Abs(moveX)); // Usamos Mathf.Abs para que la velocidad siempre sea positiva
    }

    private void HandleJump()
    {
        // Salto con I
        if (Input.GetKeyDown(KeyCode.I) && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
            animator.SetBool("Jump", true);
            isGrounded = false; // El jugador ya no está en el suelo
        }
    }

    private void HandleAttack()
    {
        // Ataque con K
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detectar si el jugador toca el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
        }
    }

    // Detectar colisión con la hitbox de Player usando OnTriggerStay2D
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_HITBOX_TAG))
        {
            // Verifica si puede recibir daño nuevamente
            if (Time.time >= lastHurtTime + hurtCooldown)
            {
                lastHurtTime = Time.time; // Actualiza el tiempo del último golpe

                // Activar la animación de daño
                animator.ResetTrigger("IsHurt"); // Reinicia el trigger por si está activo
                animator.SetTrigger("IsHurt");
                Debug.Log("Player2 está siendo golpeado por Player.");
            }
        }
    }
}
