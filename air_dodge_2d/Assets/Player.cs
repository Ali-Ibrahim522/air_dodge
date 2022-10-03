using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer square;
    public float speed = 5f;
    public float airSpeed = 2.5f;
    public float gravityScale = 3f;
    public bool grounded = true;
    public float dodgeDuration = .1f;
    private float dodgeTime = 0;
    private Rigidbody2D rb2d;
    public float jumpPower;
    public float cooldownTime = 2;
    private float nextFireTime = 0;
    private bool cooldown = false;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private LayerMask platformsLayerMask;

    void Start()
    {
        rb2d = rb2d = GetComponent<Rigidbody2D> ();
        rb2d.gravityScale = gravityScale;
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        square = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.time > nextFireTime) {
            if (cooldown) 
            {
                cooldown = false;
                square.color = Color.cyan;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && !IsGrounded())
            {
                cooldown = true;
                airSpeed = 30f;
                nextFireTime = Time.time + cooldownTime;
                square.color = Color.blue;
            }
        }

        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.D))
        {
            pos.x += IsGrounded() ? speed * Time.deltaTime : airSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= IsGrounded() ? speed * Time.deltaTime : airSpeed * Time.deltaTime;
        }

        transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            rb2d.velocity = Vector2.up * jumpPower;
        }

        if (airSpeed == 30f)
        {
            dodgeTime += Time.deltaTime;
            if (dodgeTime >= dodgeDuration) 
            {
                airSpeed = 2.5f;
                dodgeTime = 0;
            }
        }
    }


    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2D.collider != null;
    }
}
