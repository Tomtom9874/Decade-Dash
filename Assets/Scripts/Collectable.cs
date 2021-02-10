using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public delegate void CollectableCollision(int v);

    [SerializeField] private int value = 1;

    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _audioSource.Play();
            CollectableCollides?.Invoke(value);
            _spriteRenderer.enabled = false;
            Destroy(gameObject, .3f);
        }
    }

    public static event CollectableCollision CollectableCollides;
}