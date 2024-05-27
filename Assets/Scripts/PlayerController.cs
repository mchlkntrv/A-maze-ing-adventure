using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 400;
    private int score = 0;
    private Vector2 input;
    private Animator animator;
    private bool isMoving;

    [SerializeField] private Tilemap collectibles;
    [SerializeField] private GameObject startTrigger;
    [SerializeField] private GameObject finishTrigger;

    [SerializeField] private GameObject finishPopUp;
    [SerializeField] private TMP_Text levelScoreText;
    [SerializeField] private TMP_Text bestScoreText;
    [SerializeField] private TMP_Text levelTimeText;
    [SerializeField] private TMP_Text bestTimeText;
    private string levelKey;
    private float bestTime;
    private int bestScore;

    [SerializeField] private GameObject quitConfirmationPanel;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    private float timer = 0f;
    private bool counting = false;
    private bool reachedFinish = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isMoving = false;
        scoreText.text = "Score: 0";
        timerText.text = "Timer: 00:00";
        finishPopUp.SetActive(false);

        quitConfirmationPanel.SetActive(false);
        yesButton.onClick.AddListener(OnYesButton);
        noButton.onClick.AddListener(OnNoButton);

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
                rb.velocity = Vector2.zero;
                isMoving = false;
            }
            animator.SetBool("IsMoving", isMoving);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowQuitConfirmationPanel();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == startTrigger)
        {
            StartTimer();
        }
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
            AudioManager.instance.PlayFinishSound();
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
                    IncreaseScore(10);
                    AudioManager.instance.PlayCollectItemSound("Coin");
                    break;
                case "DoubleCoin":
                    IncreaseScore(25);
                    AudioManager.instance.PlayCollectItemSound("Coin");
                    break;
                case "SpeedPotion":
                    ChangeSpeed(2f, 5f);
                    AudioManager.instance.PlayCollectItemSound("SpeedPotion");
                    break;

                case "SlowPotion":
                    ChangeSpeed(0.5f, 5f);
                    AudioManager.instance.PlayCollectItemSound("SlowPotion");
                    break;

                default:
                    break;
            }
            collectibles.SetTile(cellPosition, null);
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
    
    private void ShowQuitConfirmationPanel()
    {
        quitConfirmationPanel.SetActive(true);
    }

    private void HideQuitConfirmationPanel()
    {
        quitConfirmationPanel.SetActive(false);
    }
    private void OnYesButton()
    {
        HideQuitConfirmationPanel();
        Application.Quit();
    }
    private void OnNoButton()
    {
        HideQuitConfirmationPanel();
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
