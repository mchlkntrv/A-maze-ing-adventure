using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 input;
    private Animator animator;
    private bool isMoving;
    private Rigidbody2D rb;
    public Tilemap collectibles;
    public TMP_Text scoreText;
    private int score = 0;

    public GameObject startTrigger;
    public GameObject finishTrigger;

    public TMP_Text finishScoreText;
    public GameObject finishPopUp;

    public TMP_Text timerText;
    private float timer = 0f;
    private bool counting = false;

    private bool reachedFinish = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isMoving = false;
        scoreText.text = "Score:  0";
        finishPopUp.SetActive(false);
    }

    void Update()
    {
        if (!reachedFinish)
        {
            if (counting)
            {
                timer += Time.deltaTime;
                UpdateTimerUI();
            }

            if (!isMoving)
            {
                input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                if (input != Vector2.zero)
                {
                    animator.SetFloat("MoveX", input.x);
                    animator.SetFloat("MoveY", input.y);

                    Vector2 movement = speed * Time.deltaTime * input.normalized;

                    Vector2 newPosition = rb.position + movement;
                    rb.MovePosition(newPosition);

                    CollectItem(newPosition);

                    isMoving = true;
                }
            }
            else
            {
                isMoving = false;
            }

            animator.SetBool("IsMoving", isMoving);
        }
        
    }

    void CollectItem(Vector2 position)
    {
        Vector3Int cellPosition = collectibles.WorldToCell(position);
        TileBase tile = collectibles.GetTile(cellPosition);

        if (tile != null)
        {
            string tileName = tile.name;

            switch (tileName)
            {
                case "Coin":
                    Debug.Log("Coin Collected!");
                    IncreaseScore(10);
                    break;

                case "SpeedPotion":
                    Debug.Log("Speed Potion Collected!");
                    ChangeSpeed(2f, 5f);
                    break;

                case "SlowPotion":
                    Debug.Log("Slow Potion Collected!");
                    ChangeSpeed(0.5f, 5f);
                    break;

                default:
                    Debug.Log("Unknown Collectible!");
                    break;
            }

            collectibles.SetTile(cellPosition, null);
        }
    }

    public void StartTimer()
    {
        counting = true;
    }

    public void StopTimer()
    {
        counting = false;
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.text = "Timer: " + timeString;
        }
    }


    void IncreaseScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
            Debug.Log("Score: " + score.ToString());
        }
    }

    public void ChangeSpeed(float multiplier, float duration)
    {
        StartCoroutine(ChangeSpeedCoroutine(multiplier, duration));
    }

    private IEnumerator ChangeSpeedCoroutine(float multiplier, float duration)
    {
        speed *= multiplier;
        yield return new WaitForSeconds(duration);
        speed /= multiplier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == finishTrigger)
        {
            finishPopUp.SetActive(true);
            finishScoreText.text = "Final score: " + score.ToString();
            reachedFinish = true;
            isMoving = false;
            StopTimer();
            Debug.Log("Finish reached!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == startTrigger)
        {
            StartTimer();
            Debug.Log("Start exited!");
        }
    }
}
