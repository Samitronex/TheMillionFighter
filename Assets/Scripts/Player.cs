// Player Script
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float knockbackForce = 10f;
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
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource no asignado en Player.");
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
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput != 0)
        {
            transform.Translate(Vector2.right * moveInput * moveSpeed * Time.deltaTime);
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
        if (Input.GetKeyDown(KeyCode.E))
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
            Debug.Log("Hitbox de Player activada.");
        }
    }

    public void DeactivateHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(false);
            Debug.Log("Hitbox de Player desactivada.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitbox.activeSelf && collision.gameObject.CompareTag("Player2"))
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