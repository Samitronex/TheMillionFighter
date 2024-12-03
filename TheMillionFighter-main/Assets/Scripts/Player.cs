using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 10;
    public HealthBar healthBar;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        // Movimiento con WASD
        float moveX = 0;

        if (Input.GetKey(KeyCode.A)) moveX = -1; // Izquierda
        if (Input.GetKey(KeyCode.D)) moveX = 1;  // Derecha

        transform.Translate(Vector2.right * moveX * moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", Mathf.Abs(moveX));
        transform.localScale = new Vector3(Mathf.Sign(moveX == 0 ? transform.localScale.x : moveX), 1, 1);

        // Salto con W
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
            animator.SetBool("Jump", true);
            isGrounded = false;
        }

        // Ataque con clic izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");

            // Detectar al enemigo y aplicar daño
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f);
            if (hit.collider != null && hit.collider.CompareTag("Player2"))
            {
                hit.collider.GetComponent<Player2Controller>().TakeDamage(damage);
            }
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false; // Desactivar el script
    }
}