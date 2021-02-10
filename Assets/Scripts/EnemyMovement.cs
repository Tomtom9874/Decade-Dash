using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public delegate void EnemyCollision(EnemyMovement e);

    [SerializeField] private int walkDistance;
    [SerializeField] private Vector3 speed;
    [SerializeField] private float jumpForce = 11.0f;

    private int _direction = 1;
    private int _distanceRemaining;
    private Rigidbody2D _rb;
    private bool _dead;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _distanceRemaining = walkDistance;
    }

    private void FixedUpdate()
    {
        _distanceRemaining--;
        if (_distanceRemaining < 1)
        {
            _direction *= -1;
            transform.Rotate(0, 180, 0);
            _distanceRemaining = walkDistance;
        }

        var distance = speed * _direction;
        if (!_dead) _rb.MovePosition(transform.position + distance);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EnemyCollides?.Invoke(this);
        }
    }

    public static event EnemyCollision EnemyCollides;

    public void GetStomped()
    {
        _dead = true;
        Destroy(gameObject, 2);
        Destroy(GetComponent<BoxCollider2D>());
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}