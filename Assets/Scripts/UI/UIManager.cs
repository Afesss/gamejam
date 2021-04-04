using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class UIManager : Singleton<UIManager>
{
    internal event Action OnUpdateGameState;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gUI;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject victory;
    [SerializeField] GameObject instruction;
    [SerializeField] AnimationClip menuOn;
    [SerializeField] AnimationClip menuOff;

    private UIEventMethods uIValues;
    private Animation _animation;
    private bool gameOverState;
    private bool gameStart;
    private bool vin;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        instruction.SetActive(true);
        _animation = GetComponent<Animation>();
        uIValues = GetComponent<UIEventMethods>();
        gUI.SetActive(false);
        gameOver.SetActive(false);
        victory.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }
    public void CloseInstruction()
    {
        instruction.SetActive(false);
    }
    public void Instruction()
    {
        instruction.SetActive(true);
    }
    private void Victory()
    {
        vin = true;
        victory.SetActive(true);
        EventBroker.UpdateChocolateAmount -= uIValues.UpdateChocolateAmount;
        EventBroker.UpdatePriceAmoun -= uIValues.UpdatePriceAmount;
        EventBroker.GameOver -= GameOver;
        EventBroker.OnFloodingComplete -= Victory;
    }
    private void GameOver()
    {
        gameStart = false;
        gameOverState = true;
        EventBroker.OnFloodingComplete -= Victory;
        EventBroker.UpdateChocolateAmount -= uIValues.UpdateChocolateAmount;
        EventBroker.UpdatePriceAmoun -= uIValues.UpdatePriceAmount;
        EventBroker.GameOver -= GameOver;
        gameOver.SetActive(true);
    }
    internal void NewGame()
    {
        gameStart = true;
        gameOverState = false;
        vin = true;
        GameManager.Instance.StartGame();
        GameManager.Instance.UpdateGameState(GameManager.GameState.RUNNING);

        EventBroker.OnFloodingComplete += Victory;
        EventBroker.UpdateChocolateAmount += uIValues.UpdateChocolateAmount;
        EventBroker.UpdatePriceAmoun += uIValues.UpdatePriceAmount;
        EventBroker.GameOver += GameOver;
        MenuOff();
        gUI.SetActive(true);
        victory.SetActive(false);
        gameOver.SetActive(false);

        OnUpdateGameState?.Invoke();
    }
    internal void Pause()
    {
        gUI.SetActive(false);
        victory.SetActive(false);
        EventBroker.UpdateChocolateAmount -= uIValues.UpdateChocolateAmount;
        EventBroker.UpdatePriceAmoun -= uIValues.UpdatePriceAmount;
        EventBroker.GameOver -= GameOver;
        EventBroker.OnFloodingComplete -= Victory;
        GameManager.Instance.UpdateGameState(GameManager.GameState.PAUSE);
        MenuOn();

        OnUpdateGameState?.Invoke();
    }
    internal void Ñontinue()
    {
        if (gameStart)
        {
            if (!gameOverState && !vin)
            {
                EventBroker.OnFloodingComplete += Victory;
                EventBroker.UpdateChocolateAmount += uIValues.UpdateChocolateAmount;
                EventBroker.UpdatePriceAmoun += uIValues.UpdatePriceAmount;
                EventBroker.GameOver += GameOver;
                GameManager.Instance.UpdateGameState(GameManager.GameState.RUNNING);
                MenuOff();
                gUI.SetActive(true);

                OnUpdateGameState?.Invoke();
            }
        }
    }
    internal void Exit()
    {
        EventBroker.UpdateChocolateAmount -= uIValues.UpdateChocolateAmount;
        EventBroker.UpdatePriceAmoun -= uIValues.UpdatePriceAmount;
        EventBroker.GameOver -= GameOver;
        Application.Quit();
    }
    private void MenuOn()
    {
        _animation.Stop();
        _animation.clip = menuOn;
        _animation.Play();
    }
    private void MenuOff()
    {
        _animation.Stop();
        _animation.clip = menuOff;
        _animation.Play();
    }
    public void Developers()
    {
        Application.OpenURL("https://vk.com/whitecubegames");
    }
}
