// Player2Controller Script
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float knockbackForce = 10f;

    [Header("References")]
    public Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Hitbox Settings")]
    public GameObject hitbox;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip damageSound;
    public AudioClip deathSound;

    private bool isDead = false; // Para comprobar si el jugador está muerto

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource no asignado en Player2Controller.");
        }

        if (damageSound == null)
        {
            Debug.LogWarning("Clip de sonido de daño no asignado.");
        }

        if (deathSound == null)
        {
            Debug.LogWarning("Clip de sonido de muerte no asignado.");
        }
    }

    void Update()
    {
        if (isDead) return; // Si está muerto, no puede hacer nada

        HandleMovement();
        HandleJump();
        HandleAttack();
    }

    private void HandleMovement()
    {
        float moveX = 0;

        if (Input.GetKey(KeyCode.L)) // Mover a la derecha
        {
            moveX = 1;
            if (transform.localScale.x < 0) // Si el sprite está mirando a la izquierda, corrige la escala
            {
                transform.localScale = new Vector3(1, Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
            }
        }
        else if (Input.GetKey(KeyCode.J)) // Mover a la izquierda
        {
            moveX = -1;
            if (transform.localScale.x > 0) // Si el sprite está mirando a la derecha, corrige la escala
            {
                transform.localScale = new Vector3(-1, Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
            }
        }

        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(moveX));
    }



    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.I) && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
            animator.SetBool("Jump", true);
            isGrounded = false;
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Attack");
            if (audioSource != null && attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
    }

    public void ActivateHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(true);
            Debug.Log("Hitbox de Player2 activada.");
        }
    }

    public void DeactivateHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(false);
            Debug.Log("Hitbox de Player2 desactivada.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitbox.activeSelf && collision.gameObject.CompareTag("Player"))
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (otherRb != null)
            {
                otherRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                if (audioSource != null && damageSound != null)
                {
                    audioSource.PlayOneShot(damageSound);
                }
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Pausar la música usando el MusicManager
        MusicManager.Instance?.PauseBackgroundMusic();
    }
}
