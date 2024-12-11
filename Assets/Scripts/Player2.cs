using UnityEngine;

public class Player2Controller : MonoBehaviour
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
    public AudioClip attackSound1; // Sonido del primer ataque
    public AudioClip attackSound2; // Sonido del segundo ataque
    public AudioClip attackSound3; // Sonido del tercer ataque
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
        float moveInput = 0;

        if (Input.GetKey(KeyCode.L)) // Mover a la derecha
        {
            moveInput = 1;
            transform.localScale = new Vector3(1, 1, 1); // Mirar a la derecha
        }
        else if (Input.GetKey(KeyCode.J)) // Mover a la izquierda
        {
            moveInput = -1;
            transform.localScale = new Vector3(-1, 1, 1); // Mirar a la izquierda
        }

        transform.Translate(Vector2.right * moveInput * moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
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
        if (Input.GetKeyDown(KeyCode.K)) // Primer ataque
        {
            animator.SetTrigger("Attack");
            if (audioSource != null && attackSound1 != null)
            {
                audioSource.PlayOneShot(attackSound1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.O)) // Segundo ataque
        {
            animator.SetTrigger("Attack2");
            if (audioSource != null && attackSound2 != null)
            {
                audioSource.PlayOneShot(attackSound2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.P)) // Tercer ataque
        {
            animator.SetTrigger("Attack3");
            if (audioSource != null && attackSound3 != null)
            {
                audioSource.PlayOneShot(attackSound3);
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
        if (collision.gameObject.CompareTag("Player") && hitbox.activeSelf) // Detecta colisión con Player
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
    }
}
