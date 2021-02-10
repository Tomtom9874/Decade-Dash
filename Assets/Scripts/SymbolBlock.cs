using UnityEngine;

public class SymbolBlock : MonoBehaviour
{
    [SerializeField] private float bounciness;
    [SerializeField] private float snappiness;
    [SerializeField] private int bounceTime;
    private int _bouncing;
    private Rigidbody2D _rb;
    private Vector3 _startPosition;


    private void Start()
    {
        _startPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _bouncing--;
        if (_bouncing == 0) _rb.velocity = Vector2.zero;
        if (transform.position != _startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPosition, snappiness);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        _rb.AddForce(Vector2.up * bounciness, ForceMode2D.Impulse);
        _bouncing = bounceTime;
    }
}