using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEnemyController : MonoBehaviour
{
    public delegate void EnemyCollision(PipeEnemyController e);
    //should come up and down constantly
    //don't come up if a player is on the pipe
    //kill player if he runs into me
    private Vector3 movement = new Vector3(0, -1f, 0);
    private float initialY;
    private float distanceToTravel = 2.5f;
    private bool movingDown;
    private bool paused;
    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.position.y;
        movingDown = true;
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused && movingDown && transform.position.y > initialY - distanceToTravel)
        {
            transform.position += movement * Time.deltaTime;
        }
        else if (!paused && movingDown)
        {
            movingDown = false;
        }
        if (!paused && !movingDown && transform.position.y < initialY)
        {
            transform.position -= movement * Time.deltaTime;
        }
        else if (!paused && !movingDown)
        {
            movingDown = true;
            StartCoroutine(PauseAtTop());
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EnemyCollides?.Invoke(this);
        }
    }

    public static event EnemyCollision EnemyCollides;
    IEnumerator PauseAtTop()
    {
        paused = true;
        yield return new WaitForSeconds(1);
        paused = false;
    }
}
