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
    public AudioClip attackSound1;
    public AudioClip attackSound2;
    public AudioClip attackSound3;
    public AudioClip damageSound;
    public AudioClip deathSound;

    private bool isDead = false;
    public float attackCooldown = 1f;
    private bool canAttack = true;

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
    }

    void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleJump();
        HandleAttack();
    }

    private void HandleMovement()
    {
        float moveInput = 0;

        if (Input.GetKey(KeyCode.L))
        {
            moveInput = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.J))
        {
            moveInput = -1;
            transform.localScale = new Vector3(-1, 1, 1);
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
            PlaySound(jumpSound);
        }
    }

    private void HandleAttack()
    {
        if (!canAttack) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            PerformAttack("Attack", attackSound1);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            PerformAttack("Attack2", attackSound2);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            PerformAttack("Attack3", attackSound3);
        }
    }

    private void PerformAttack(string attackTrigger, AudioClip attackSound)
    {
        animator.SetTrigger(attackTrigger);
        PlaySound(attackSound);
        StartCoroutine(AttackCooldown());
    }

    private System.Collections.IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void ActivateHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(true);
        }
    }

    public void DeactivateHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitbox != null && hitbox.activeSelf && collision.CompareTag("Player"))
        {
            Rigidbody2D targetRb = collision.GetComponent<Rigidbody2D>();
            if (targetRb != null)
            {
                ApplyKnockback(targetRb);
            }
        }
    }

    private void ApplyKnockback(Rigidbody2D targetRb)
    {
        Vector2 knockbackDirection = (targetRb.transform.position - transform.position).normalized;
        targetRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        rb.velocity = Vector2.zero;
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }
    }

    public void PlayDamageSound()
    {
        PlaySound(damageSound);
    }

    public void PlayDeathSound()
    {
        PlaySound(deathSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
