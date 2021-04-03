using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gUI;
    [SerializeField] GameObject gameOver;
    [SerializeField] AnimationClip menuOn;
    [SerializeField] AnimationClip menuOff;

    private UIEventMethods uIValues;
    private Animation _animation;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        _animation = GetComponent<Animation>();
        uIValues = GetComponent<UIEventMethods>();
        gUI.SetActive(false);
        gameOver.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }
    private void GameOver()
    {
        EventBroker.UpdateChocolateAmount -= uIValues.UpdateChocolateAmount;
        EventBroker.UpdatePriceAmoun -= uIValues.UpdatePriceAmount;
        EventBroker.GameOver -= GameOver;
        gameOver.SetActive(true);
        StartCoroutine(WaitSecond());
    }
    IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(1);
        gameOver.SetActive(false);
    }
    internal void NewGame()
    {

        GameManager.Instance.StartGame();
        GameManager.Instance.UpdateGameState(GameManager.GameState.RUNNING);

        EventBroker.UpdateChocolateAmount += uIValues.UpdateChocolateAmount;
        EventBroker.UpdatePriceAmoun += uIValues.UpdatePriceAmount;
        EventBroker.GameOver += GameOver;
        MenuOff();
        gUI.SetActive(true);
    }
    internal void Pause()
    {
        EventBroker.UpdateChocolateAmount -= uIValues.UpdateChocolateAmount;
        EventBroker.UpdatePriceAmoun -= uIValues.UpdatePriceAmount;
        EventBroker.GameOver -= GameOver;

        GameManager.Instance.UpdateGameState(GameManager.GameState.PAUSE);
        MenuOn();
        gUI.SetActive(false);
    }
    internal void Ñontinue()
    {
        EventBroker.UpdateChocolateAmount += uIValues.UpdateChocolateAmount;
        EventBroker.UpdatePriceAmoun += uIValues.UpdatePriceAmount;
        EventBroker.GameOver += GameOver;
        GameManager.Instance.UpdateGameState(GameManager.GameState.RUNNING);
        MenuOff();
        gUI.SetActive(true);
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
    
}
