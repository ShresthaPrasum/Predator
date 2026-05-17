using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool InputEnabled { get; private set; } = true;

    [SerializeField] private Canvas canvas;

    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private TextMeshProUGUI gameTimerText;

    [SerializeField] private GameObject countdownPanel;

    [SerializeField] private GameObject winningPanel;

    [SerializeField] private TextMeshProUGUI winnerText;
    
    [SerializeField] private Animator bombExplosionAnimator;

    [SerializeField] private string bombTag = "Bomb";

    [SerializeField] private string playerOneTag = "Player1";

    [SerializeField] private string playerTwoTag = "Player2";

    [SerializeField] private string explosionEndedParam = "hasEnded";

    [SerializeField] private float preStartSeconds = 3f;

    [SerializeField] private float gameMinutes = 2f;

    [SerializeField] private float loadDelay = 1f;

    private Coroutine countdownRoutine;

    private void Start()
    {
        if (countdownText == null && canvas != null)
        {
            countdownText = canvas.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (gameTimerText != null)
        {
            gameTimerText.text = string.Empty;
            gameTimerText.gameObject.SetActive(false);
        }

        if (countdownPanel != null)
        {
            countdownPanel.SetActive(true);
        }

        if (winningPanel != null)
        {
            winningPanel.SetActive(false);
        }

        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
        }

        countdownRoutine = StartCoroutine(RunCountdowns());
    }

    private IEnumerator RunCountdowns()
    {
        InputEnabled = false;
        yield return RunPreStartCountdown();

        InputEnabled = true;
        float gameStartTime = Time.time;
        yield return RunGameCountdown(gameStartTime);

        InputEnabled = false;
        yield return RunGameOverSequence();
    }

    private IEnumerator RunPreStartCountdown()
    {
        int seconds = Mathf.CeilToInt(preStartSeconds);
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        for (int i = seconds; i > 0; i--)
        {
            if (countdownText != null)
            {
                countdownText.text = i.ToString();
            }

            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    private IEnumerator RunGameCountdown(float startTime)
    {
        int totalSeconds = Mathf.Max(0, Mathf.CeilToInt(gameMinutes * 60f));
        if (gameTimerText != null)
        {
            gameTimerText.gameObject.SetActive(true);
        }

        while (true)
        {
            int elapsedSeconds = Mathf.FloorToInt(Time.time - startTime);
            int remainingSeconds = Mathf.Max(0, totalSeconds - elapsedSeconds);
            int minutes = remainingSeconds / 60;
            int seconds = remainingSeconds % 60;
            if (gameTimerText != null)
            {
                gameTimerText.text = string.Format("{0}:{1:00}", minutes, seconds);
            }

            if (remainingSeconds <= 0)
            {
                break;
            }

            yield return null;
        }
    }

    private IEnumerator RunGameOverSequence()
    {
        if (bombExplosionAnimator != null)
        {
            bombExplosionAnimator.SetBool(explosionEndedParam, true);
        }

        if (loadDelay > 0f)
        {
            yield return new WaitForSeconds(loadDelay);
        }

        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false);
        }

        if (winningPanel != null)
        {
            winningPanel.SetActive(true);
        }

        if (winnerText != null)
        {
            string winnerTag = DetermineWinnerTag();
            if (!string.IsNullOrEmpty(winnerTag))
            {
                winnerText.text = winnerTag;
                winnerText.color = winnerTag == playerTwoTag ? Color.blue : Color.red;
            }
        }
    }

    private string DetermineWinnerTag()
    {
        GameObject bomb = GameObject.FindGameObjectWithTag(bombTag);
        if (bomb == null)
        {
            return string.Empty;
        }

        Transform holderRoot = bomb.transform.parent;
        if (holderRoot != null)
        {
            holderRoot = holderRoot.parent;
        }

        if (holderRoot == null)
        {
            return string.Empty;
        }

        string holderTag = holderRoot.tag;
        if (holderTag == playerOneTag)
        {
            return playerTwoTag;
        }

        if (holderTag == playerTwoTag)
        {
            return playerOneTag;
        }

        return string.Empty;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Home");
    }
    public void LoadCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(sceneName);
    }
}
