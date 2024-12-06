using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private bool isGrounded;

    // Constante para el tag de la hitbox de Player2
    private const string PLAYER2_HITBOX_TAG = "Player2Hitbox";

    // Cooldown para recibir daño
    private float lastHurtTime = -Mathf.Infinity; // Última vez que el Player recibió daño
    public float hurtCooldown = 0.5f; // Tiempo mínimo entre golpes consecutivos

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // Movimiento horizontal
        if (moveInput != 0)
        {
            transform.Translate(Vector2.right * moveInput * moveSpeed * Time.deltaTime);
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1); // Cambiar dirección
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
            animator.SetBool("Jump", true);

            isGrounded = false;
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
        }
    }

    // Detectar colisión con la hitbox de Player2 usando OnTriggerStay2D
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER2_HITBOX_TAG))
        {
            // Verificar si puede recibir daño nuevamente
            if (Time.time >= lastHurtTime + hurtCooldown)
            {
                lastHurtTime = Time.time; // Actualizar tiempo del último daño

                // Activar la animación de daño
                animator.ResetTrigger("IsHurt"); // Reinicia el trigger por si está activo
                animator.SetTrigger("IsHurt");
                Debug.Log("Player está siendo golpeado por Player2.");
            }
        }
    }
}
