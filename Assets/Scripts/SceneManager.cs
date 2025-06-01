using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private bool _timerActive = true;
    private float _currentTime;
    public TMP_Text timerText;
    public TMP_Text echoCountText;
    public float timelimit = 5f;
    public int maxEchoes = 3;
    private Queue<Transform> _echoQueue = new Queue<Transform>();

    public TMP_Text startCounterText;
    private float _currentStartCounterTime;
    private float _startCounterTime = 4f;
    private bool _startCounterActive = true;

    public Transform prefabEcho;
    public GameObject player;
    public GameObject spawner;

    public GameObject winText;
    public TMP_Text echoesUsedCountText;
    public TMP_Text totalDeadCountText;
    private int _echoesUsedCount = 0;
    private int _totalDeadCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentTime = timelimit;
        _currentStartCounterTime = _startCounterTime;
        SetEchoCountText();
        SetActiveState(false);
        winText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_startCounterActive)
        {
            SetActiveState(false);
            _currentStartCounterTime -= Time.deltaTime;

            TimeSpan timeStart = TimeSpan.FromSeconds(_currentStartCounterTime);
            startCounterText.text = string.Format("{0:D1}", timeStart.Seconds);

            if (_currentStartCounterTime <= 0)
            {
                _startCounterActive = false;
                _currentStartCounterTime = _startCounterTime;
                SetActiveState(true);
            }
            return;
        }


        if (_timerActive)
        {
            _currentTime -= Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(_currentTime);
        timerText.text = string.Format("{0:D2}", time.Seconds);

        if (_currentTime <= 0)
        {
            _timerActive = false;
            _currentTime = 0;

            RespawnPlayer();
        }
    }

    private void SetActiveState(bool activeState)
    {
        timerText.gameObject.SetActive(activeState);
        player.SetActive(activeState);
        startCounterText.gameObject.SetActive(!activeState);
    }

    public void RespawnPlayer(bool isDeath = false)
    {
        if (isDeath)
        {
            _totalDeadCount++;
            totalDeadCountText.text = $"Total deaths: {_totalDeadCount}";
        }
        else
        {
            // Instantiate the echo prefab at the player's position
            _echoesUsedCount++;
            var newEcho = Instantiate(prefabEcho, player.transform.position, Quaternion.identity);
            if (_echoQueue.Count >= maxEchoes)
            {
                // Destroy the oldest echo if we have reached the maximum number of echoes
                Transform oldEcho = _echoQueue.Dequeue();
                oldEcho.GetComponent<BoxCollider>().transform.position = new Vector3(-1000, -1000, -1000);
                Destroy(oldEcho.gameObject, 0.1f);
            }
            _echoQueue.Enqueue(newEcho);
            SetEchoCountText();
        }

        player.transform.position = spawner.transform.position;
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        // Reset the timer
        _currentTime = timelimit;
        _timerActive = true;

        _startCounterActive = true;
    }

    public void RestartLevel()
    {
        // Reset the timer
        _currentTime = timelimit;
        _timerActive = true;

        // Clear the echoes
        while (_echoQueue.Count > 0)
        {
            Transform echo = _echoQueue.Dequeue();
            echo.GetComponent<BoxCollider>().transform.position = new Vector3(-1000, -1000, -1000);
            Destroy(echo.gameObject, 0.1f);
        }
        SetEchoCountText();

        // Reset player position
        player.transform.position = spawner.transform.position;
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        _startCounterActive = true;
    }

    public void WinGame()
    {
        _timerActive = false;
        winText.SetActive(true);
        player.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    private void SetEchoCountText()
    {
        echoCountText.text = $"Echoes: {_echoQueue.Count}/{maxEchoes}";
        echoesUsedCountText.text = $"Echoes used: {_echoesUsedCount}";
    }
}
