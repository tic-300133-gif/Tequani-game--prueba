using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorEnemigo : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5.0f;
    public float speed = 2.0f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // <-- importante
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = new Vector2(direction.x, 0);

            animator.SetBool("isMoving", true);

            // ?? Voltea sin cambiar tamaño
            // Si el sprite ORIGINAL mira hacia la izquierda:
            if (movement.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // mirar a derecha
            }
            else if (movement.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // mirar a izquierda
            }

        }
        else
        {
            movement = Vector2.zero;
            animator.SetBool("isMoving", false);
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    public void Morir()
    {
        isDead = true;
        animator.SetTrigger("Muerte");
        animator.SetBool("isMoving", false);
        rb.velocity = Vector2.zero;
    }
}
