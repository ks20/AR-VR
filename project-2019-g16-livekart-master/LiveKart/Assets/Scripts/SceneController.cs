using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public GameObject ARCamera;
    public GameObject ARSessionOrigin;
    private float startTime;
    private ARPlaneManager arPlaneManager;
    private PlaceMultipleObjectsOnPlane planeScript;
    public AudioSource m_buttonAudioSource;
    public AudioSource m_clockAudio;
    public AudioSource m_countdownAudio;
    public AudioSource m_gameAudio;

    // public GameObject coins;
    // public GameObject powerUps;
    // public GameObject setup;
    public GameObject m_carFront;

    // Pictures
    public GameObject movePic;
    public GameObject tapPic;

    // Setup UI
    public GameObject m_setupPanel;
    public GameObject m_setupText;
    public GameObject m_setupCoins;
    public GameObject m_setupPowerUps;
    public GameObject m_instructionPanel;
    public GameObject m_instructionText;

    // Gameplay UI
    private int m_totalCoins;
    private float m_clock;
    public int m_countdown = 10;
    public GameObject m_playPanel;
    public GameObject m_playCoins;
    public GameObject m_timer;
    public GameObject m_countdownText;

    // Gameover
    public GameObject gameOverPanel;
    public GameObject gameOverTime;
    private int score;

    // GamePlay Scripts
    private CollisionDetection m_collisionDetectionScript;
    private MagnetPowerUp m_magnetPowerUpScript;
    private ArrowDirector m_arrowDirectorScript;
    private InkSplatter m_splatterScript;

    // Determines state of game
    public enum State { Setup1, Setup2, Setup3, Play, GameOver };
    public State currState;

    // Start is called before the first frame update
    void Start() {
        // Activate correct panels
        m_setupPanel.SetActive(true);
        m_instructionPanel.SetActive(true);
        m_playPanel.SetActive(false);
        m_countdownText.SetActive(false);
        gameOverPanel.SetActive(false);

        startTime = Time.time;
        arPlaneManager = ARSessionOrigin.GetComponent<ARPlaneManager>();
        planeScript = ARSessionOrigin.GetComponent<PlaceMultipleObjectsOnPlane>();
        m_setupText.GetComponent<Text>().text = "Setup: 0/3";
        m_countdownText.GetComponent<Text>().text = "10";
        currState = State.Setup1;

        // Play Gifs
        movePic.SetActive(false);
        tapPic.SetActive(false);
        float duration1 = 2, delay1 = 3;
        float duration2 = 2, delay2 = 8;
        StartCoroutine(ShowPic(movePic, duration1, delay1));
        StartCoroutine(ShowPic(tapPic, duration2, delay2));

        // Attach GamePlay scripts
        m_collisionDetectionScript = m_carFront.GetComponent<CollisionDetection>();
        m_magnetPowerUpScript = GetComponent<MagnetPowerUp>();
        m_arrowDirectorScript = GetComponent<ArrowDirector>();
        m_splatterScript = GetComponent<InkSplatter>();
    }

    public bool currStateIsSetup1() {
        return (currState == State.Setup1);
    }

    public bool currStateIsSetup2() {
        return (currState == State.Setup2);
    }

    public bool currStateIsSetup3() {
        return (currState == State.Setup3);
    }

    public bool currStateIsPlay() {
        return (currState == State.Play);
    }

    public bool currStateIsGameOver() {
        return (currState == State.GameOver);
    }

    // Update is called once per frame
    void Update() {
        if (planeScript.Coins.Count > 0) {
            StartCoroutine(RemovePics());
        }

        switch (currState) {
            case State.Setup1:
                UpdateSetup(1);
                UpdateCoinSetup();
                UpdateInstructions("Place Coins", Color.red);
                break;

            case State.Setup2:
                UpdatePowerUpSetup();
                UpdateInstructions("Place Power Ups", Color.yellow);
                break;

            case State.Setup3:
                UpdateInstructions("Place Phone\nOn RC Car", Color.green);
                break;

            case State.Play:
                UpdateCoinPlay();
                UpdateTimer();
                UpdateFinish();
                break;

            case State.GameOver:
                break;

            default:
                Debug.Log("STATE NOT FOUND");
                break;
        }
    }

    void UpdateFinish() {
        if (planeScript.Coins.Count == 0) {
            m_playPanel.SetActive(false);
            gameOverPanel.SetActive(true);
            gameOverTime.GetComponent<Text>().text = "Time: " + score.ToString() + "s";
            currState++;
        }
    }

    void UpdateSetup(int state) {
        if (state == -1) {
            m_setupPanel.SetActive(false);
            m_instructionPanel.SetActive(false);
        }
        m_setupText.GetComponent<Text>().text = "Setup: " + state.ToString() + "/3";
    }


    void UpdateCoinSetup() {
        m_setupCoins.GetComponent<Text>().text = "Coins: " + planeScript.Coins.Count.ToString();
        m_totalCoins = planeScript.Coins.Count;
    }

    void UpdatePowerUpSetup() {
        m_setupPowerUps.GetComponent<Text>().text = "PowerUps: " + planeScript.PowerUps.Count.ToString();
    }

    void UpdateInstructions(string msg, Color color) {
        m_instructionText.GetComponent<Text>().text = msg;
        m_instructionText.GetComponent<Text>().color = color;
    }

    void UpdateCoinPlay() {
        m_playCoins.GetComponent<Text>().text = "Coins: " + (m_totalCoins - planeScript.Coins.Count).ToString() + "/" + m_totalCoins.ToString();
    }

    void UpdateTimer() {
        if (m_countdown == 0) {
            m_clock += Time.deltaTime;
            int seconds = Mathf.RoundToInt(m_clock);
            m_timer.GetComponent<Text>().text = "Time: " + seconds.ToString() + "s";
            score = seconds;
        }
    }

    public void DecrementTime(float amount) {
        m_clockAudio.Play();
        Debug.Log("Decrementing clock by: " + amount);
        m_clock -= amount;
    }

    IEnumerator Countdown() {
        // m_countdownAudio.Play();
        while (m_countdown > 0) {
            m_countdown--;
            m_countdownText.GetComponent<Text>().text = m_countdown.ToString();
            if (m_countdown == 3) {
                m_countdownAudio.Play();
            }
            yield return new WaitForSeconds(1.0f);
        }

        m_countdownText.SetActive(false);

        // Turning on collision detection
        // TODO: Probably a better spot for this
        m_collisionDetectionScript.m_isCollisionActive = true;
        Debug.Log("Turning collision detection on");

        // Game sounds
        m_gameAudio.Play();
    }

    IEnumerator ShowPic(GameObject pic, float duration, float delay) {

        // Delay
        while (delay > 0) {
            delay -= Time.deltaTime;
            yield return null;
        }

        // Show pic
        pic.SetActive(true);
        while (duration > 0) {
            duration -= Time.deltaTime;
            yield return null;
        }

        // Remove pic
        pic.SetActive(false);
    }

    IEnumerator RemovePics() {
        movePic.SetActive(false);
        tapPic.SetActive(false);
        yield return null;
    }

    public void Continue() {
        // Play sound effect
        m_buttonAudioSource.Play();

        StartCoroutine(RemovePics());

        switch (currState) {

            case State.Setup1:
                if (planeScript.Coins.Count == 0) {
                    float duration = 3, delay = 0;
                    StartCoroutine(ShowPic(tapPic, duration, delay));
                } else {
                    UpdateSetup(2);
                    currState++;
                }

                break;

            case State.Setup2:
                currState++;
                UpdateSetup(3);
                break;

            case State.Setup3:
                currState++;
                UpdateSetup(-1);
                m_playPanel.SetActive(true);
                m_countdownText.SetActive(true);
                StartCoroutine(Countdown());
                break;

            case State.Play:
                break;

            case State.GameOver:
                break;

            default:
                Debug.Log("STATE NOT FOUND");
                break;
        }
    }

    public void Restart() {
        // Play sound effect
        m_buttonAudioSource.Play();
        SceneManager.LoadScene("GamePlay");
    }
}
