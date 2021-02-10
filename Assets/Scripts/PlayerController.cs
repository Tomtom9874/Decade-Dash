using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 9.0f;
    [SerializeField] private float playerGravity;
    [SerializeField] private PlayerAudio playerAudio;
    
    public GameObject camera;
    private bool canJump;
    

    //keep player from going too far left

    private float leftBound;
    private readonly float lowerBound = -10;
    private bool onClimbable;
    private bool onPipe;
    private GameObject pipeCurrentlyOn;
    private float pipeDistanceTravelled;
    private Rigidbody2D playerRb;
    private bool travellingThroughPipe;

    private void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody2D>();
        playerRb.gravityScale = playerGravity;
        canJump = true;
        travellingThroughPipe = false;
        onClimbable = false;
        pipeDistanceTravelled = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
            onPipe = false;
            playerAudio.PlayJumpSound();
        }
    }

    private void FixedUpdate()
    {
        leftBound = camera.transform.position.x - 8.5f;

        if (!travellingThroughPipe) MoveHorizontally();

        if (travellingThroughPipe)
        {
            transform.position += new Vector3(0, -0.05f, 0);
            pipeDistanceTravelled += 0.05f;
            if (pipeDistanceTravelled >= 0.1) StartCoroutine(GoDownPipe());
        }

        var verticalInput = Input.GetAxis("Vertical");
        if (onPipe && verticalInput < 0 && !travellingThroughPipe &&
            pipeCurrentlyOn.GetComponent<PipeTransporter>().hasDestination)
        {
            travellingThroughPipe = true;
            CenterOnPipe();
            //turn off my box collider and playerGravity
            playerRb.gravityScale = 0;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (onClimbable && verticalInput > 0) transform.position += new Vector3(0, 0.1f, 0);

        if (transform.position.y < lowerBound) Die();
    }

    // Start is called before the first frame update

    private void OnEnable()
    {
        EnemyMovement.EnemyCollides += HitEnemy;
        PipeEnemyController.EnemyCollides += HitEnemy;
    }

    private void OnDisable()
    {
        EnemyMovement.EnemyCollides -= HitEnemy;
        PipeEnemyController.EnemyCollides -= HitEnemy;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            onPipe = false;
        }

        if (collision.gameObject.CompareTag("Ground") && onClimbable && collision.gameObject.TryGetComponent<BoxCollider2D>(out _)) StartCoroutine(GoThroughCloud(collision));

        if (collision.gameObject.CompareTag("Pipe"))
        {
            onPipe = true;
            pipeCurrentlyOn = collision.gameObject;
            canJump = true;
        }

        if (collision.gameObject.CompareTag("Block")) playerRb.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable"))
        {
            onClimbable = true;
            playerRb.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable"))
        {
            onClimbable = false;
            playerRb.gravityScale =  playerGravity;
        }
    }

    private void MoveHorizontally()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var newPosition = transform.position + Vector3.right * Time.deltaTime * horizontalInput * speed;
        if (newPosition.x < leftBound)
        {
            newPosition.x = leftBound;
        }
        transform.position = newPosition;
        if (canJump) playerAudio.PlayWalkingSound();
        if (horizontalInput == 0) playerAudio.StopSound();
    }

    private void CenterOnPipe()
    {
        var pipeXPosition = pipeCurrentlyOn.transform.position.x;
        var position = transform.position;
        position = new Vector3(pipeXPosition, position.y, position.z);
        transform.position = position;
    }

    private IEnumerator GoDownPipe()
    {
        yield return new WaitForSeconds(1);
        var destionationPipe = pipeCurrentlyOn.GetComponent<PipeTransporter>().destionationPipe;
        transform.position = destionationPipe.transform.position + Vector3.up;
        travellingThroughPipe = false;
        playerRb.gravityScale = playerGravity;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        pipeDistanceTravelled = 0;
    }

    private void HitEnemy(EnemyMovement enemy)
    {
        if (transform.position.y > enemy.transform.position.y)
            enemy.GetStomped();
        else
            Die();
    }

    private void HitEnemy(PipeEnemyController enemy)
    {
        Die();
    }

    private static void Die()
    {
        GameManager.Instance.KillPlayer();
    }

    private IEnumerator GoThroughCloud(Collision2D collision)
    {
        var collider = collision.gameObject.GetComponent<BoxCollider2D>();
        collider.enabled = false;
        yield return new WaitForSeconds(1);
        collider.enabled = true;
    }
}
