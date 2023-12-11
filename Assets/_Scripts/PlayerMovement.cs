using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpThrust;
    private bool isGrounded = true;
    private Vector2 dir;
    private Rigidbody2D rb;
    private Transform spawnPoint;
    private bool finished;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spawnPoint == null) spawnPoint = GameObject.Find("SpawnPoint").transform;
        GameManager.instance.AddPlayer(gameObject);
        NewRound();
    }

    private void FixedUpdate()
    {
        if (!finished && !GameManager.instance.isDrawing && GameManager.instance.startGame)
        {
            Move();
            Jump();
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) => dir = ctx.ReadValue<Vector2>();

    public void NewRound()
    {
        rb.velocity = Vector2.zero;
        GetComponent<SpriteRenderer>().enabled = true;
        finished = false;
        transform.position = spawnPoint.transform.position;
    }

    public void Move()
    {
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    public void Jump()
    {
        if (dir.y <= 0 || !isGrounded) return;
        isGrounded = false;
        rb.AddForce(Vector2.up * jumpThrust);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Ground")) isGrounded = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Finish") && !finished)
        {
            finished = true;
            GameManager.instance.PlayerFinished(gameObject);
        }

        if (other.transform.CompareTag("Death") && !finished)
        {
            finished = true;
            GetComponent<SpriteRenderer>().enabled = false;
            GameManager.instance.PlayerDied(gameObject);
        }
    }
}
