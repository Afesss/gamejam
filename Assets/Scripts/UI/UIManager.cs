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
    [SerializeField] AnimationClip menuOn;
    [SerializeField] AnimationClip menuOff;

    private UIEventMethods uIValues;
    private Animation _animation;
    private bool gameOverState;
    private bool gameStart;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

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
    private void Victory()
    {
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
        StartCoroutine(WaitSecond());
    }
    IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(4);
        gameOver.SetActive(false);
        Pause();
    }
    internal void NewGame()
    {
        gameStart = true;
        gameOverState = false;
        GameManager.Instance.StartGame();
        GameManager.Instance.UpdateGameState(GameManager.GameState.RUNNING);

        EventBroker.OnFloodingComplete += Victory;
        EventBroker.UpdateChocolateAmount += uIValues.UpdateChocolateAmount;
        EventBroker.UpdatePriceAmoun += uIValues.UpdatePriceAmount;
        EventBroker.GameOver += GameOver;
        MenuOff();
        gUI.SetActive(true);
        victory.SetActive(false);

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
<<<<<<< Updated upstream
            if (!gameOverState)
            {
                EventBroker.OnFloodingComplete += Victory;
                EventBroker.UpdateChocolateAmount += uIValues.UpdateChocolateAmount;
                EventBroker.UpdatePriceAmoun += uIValues.UpdatePriceAmount;
                EventBroker.GameOver += GameOver;
                GameManager.Instance.UpdateGameState(GameManager.GameState.RUNNING);
                MenuOff();
                gUI.SetActive(true);
            }
=======
            EventBroker.OnFloodingComplete += Victory;
            EventBroker.UpdateChocolateAmount += uIValues.UpdateChocolateAmount;
            EventBroker.UpdatePriceAmoun += uIValues.UpdatePriceAmount;
            EventBroker.GameOver += GameOver;
            GameManager.Instance.UpdateGameState(GameManager.GameState.RUNNING);
            MenuOff();
            gUI.SetActive(true);

            OnUpdateGameState?.Invoke();
>>>>>>> Stashed changes
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
