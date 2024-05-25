using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public TMP_Text levelScoreText;
    public TMP_Text bestScoreText;
    public TMP_Text levelTimeText;
    public TMP_Text bestTimeText;
    public GameObject finishPopUp;

    public TMP_Text timerText;
    private float timer = 0f;
    private bool counting = false;

    private bool reachedFinish = false;

    private string levelKey;
    private float bestTime;
    private int bestScore;

    public AudioManager soundsController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isMoving = false;
        scoreText.text = "Score: 0";
        timerText.text = "Timer: 00:00";
        finishPopUp.SetActive(false);

        levelKey = "Level_" + SceneManager.GetActiveScene().name;
        bestTime = PlayerPrefs.GetFloat(levelKey + "_BestTime", float.MaxValue);
        bestScore = PlayerPrefs.GetInt(levelKey + "_BestScore", 0);
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
            Debug.Log("Tile");

            switch (tileName)
            {
                case "Coin":
                    IncreaseScore(10);
                    soundsController.PlayCollectItemSound("Coin");
                    break;

                case "SpeedPotion":
                    ChangeSpeed(2f, 5f);
                    soundsController.PlayCollectItemSound("SpeedPotion");
                    break;

                case "SlowPotion":
                    ChangeSpeed(0.5f, 5f);
                    soundsController.PlayCollectItemSound("SlowPotion");
                    break;

                default:
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


    private void SaveBestTime()
    {
        if (timer < bestTime)
        {
            bestTime = timer;
            PlayerPrefs.SetFloat(levelKey + "_BestTime", bestTime);
            PlayerPrefs.Save();
        }
    }

    private void SaveBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(levelKey + "_BestScore", bestScore);
            PlayerPrefs.Save();
        }
    }


    private void ShowFinishPopup()
    {
        finishPopUp.SetActive(true);
        levelScoreText.text = "Score: " + score.ToString();
        bestScoreText.text = "Best score: " + bestScore.ToString();

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        levelTimeText.text = "Time: " + timeString;

        minutes = Mathf.FloorToInt(bestTime / 60f);
        seconds = Mathf.FloorToInt(bestTime % 60f);
        timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        bestTimeText.text = "Best time: " + timeString;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == finishTrigger)
        {
            reachedFinish = true;
            isMoving = false;
            StopTimer();
            SaveBestTime();
            SaveBestScore();
            ShowFinishPopup();
            soundsController.PlayFinishSound();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == startTrigger)
        {
            StartTimer();
        }
    }
}
