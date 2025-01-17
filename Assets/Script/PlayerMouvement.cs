using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpStrength = 8f;
    [SerializeField] private float upGravity = 1f, downGravity = 5f;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Transform feet;
    [SerializeField] private float acceleration = 2f; 
    [SerializeField] private float deceleration = 4f; 

    private Rigidbody2D _rigidbody;
    private float horizontalInput = 0f;
    private float horizontalVelocity = 0f; 
    private bool jump = false, isGrounded = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }
        else
        {
            //fix
            horizontalInput = 0f;
        }


        CheckGround();

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        float targetVelocity = horizontalInput * speed;
        if (horizontalInput != 0)
        {
            horizontalVelocity = Mathf.MoveTowards(horizontalVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            horizontalVelocity = Mathf.MoveTowards(horizontalVelocity, 0, deceleration * Time.fixedDeltaTime);
        }

        _rigidbody.velocity = new Vector2(horizontalVelocity, _rigidbody.velocity.y);

        if (jump)
        {
            _rigidbody.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
            _rigidbody.gravityScale = upGravity;
            jump = false;
        }

        if (!isGrounded && _rigidbody.velocity.y < 0)
        {
            _rigidbody.gravityScale = downGravity;
        }
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(feet.position, Vector2.down, 0.1f, groundLayers);
        isGrounded = hit.collider != null;
    }
}
