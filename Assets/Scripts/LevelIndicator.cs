using System;
using UnityEngine;

public class LevelIndicator : MonoBehaviour
{
    public GameObject leftLocation;
    public GameObject rightLocation;
    public GameObject upLocation;
    public GameObject downLocation;

    public string levelSceneName;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private int sceneNumber;

    public int SceneNumber => sceneNumber;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
    
    public bool HasLeftLocation()
    {
        return leftLocation != null;
    }

    public bool HasRightLocation()
    {
        return rightLocation != null;
    }

    public bool HasDownLocation()
    {
        return downLocation != null;
    }

    public bool HasUpLocation()
    {
        return upLocation != null;
    }
}