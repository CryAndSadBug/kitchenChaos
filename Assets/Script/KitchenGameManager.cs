using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KitchenGameManager : MonoBehaviour
{

    public event EventHandler OnStateChange;

    public event EventHandler OnGamePused;
    public event EventHandler OnGameUnpused;

    public static KitchenGameManager Instance { get; private set; }

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;

    private float countDownToStartTimer = 3f;

    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 10f;

    private bool IsgamePaused = false;

    private void Awake()
    {

        Instance = this;

        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAcion += GameInput_OnInteractAcion;
    }

    private void GameInput_OnInteractAcion(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        ToggPauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                break;

            case State.CountdownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
                {
                    state = State.GameOver;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:
                
                break;
        }
        //Debug.Log(state);
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countDownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetPlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public void ToggPauseGame()
    {
        IsgamePaused = !IsgamePaused;
        if (IsgamePaused)
        {
            Time.timeScale = 0f;
            OnGamePused?.Invoke(this, EventArgs.Empty);
        } else
        {
            Time.timeScale = 1f;
            OnGameUnpused?.Invoke(this, EventArgs.Empty);
        }
    }

}
