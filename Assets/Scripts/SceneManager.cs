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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentTime = timelimit;
        _currentStartCounterTime = _startCounterTime;
        SetEchoCountText();
        SetActiveState(false);
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
        echoCountText.gameObject.SetActive(activeState);
        startCounterText.gameObject.SetActive(!activeState);
    }

    public void RespawnPlayer()
    {
        // Instantiate the echo prefab at the player's position
        var newEcho = Instantiate(prefabEcho, player.transform.position, Quaternion.identity);
        if (_echoQueue.Count >= maxEchoes)
        {
            // Destroy the oldest echo if we have reached the maximum number of echoes
            Transform oldEcho = _echoQueue.Dequeue();
            Destroy(oldEcho.gameObject);
        }
        _echoQueue.Enqueue(newEcho);
        SetEchoCountText();

        player.transform.position = spawner.transform.position;
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        // Reset the timer
        _currentTime = timelimit;
        _timerActive = true;

        _startCounterActive = true;
    }
    
    private void SetEchoCountText()
    {
        echoCountText.text = $"Echoes: {_echoQueue.Count}/{maxEchoes}";
    }
}
