using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] AnimationClip menuOn;
    [SerializeField] AnimationClip menuOff;
    
    private Animation _animation;
    
    protected override void Awake()
    {
        base.Awake();
        _animation = GetComponent<Animation>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }
    internal void NewGame()
    {
        GameManager.Instance.StartGame();
        GameManager.Instance.UpdateGameState(GameManager.GameState.RUNNING);
        MenuOff();
    }
    internal void Pause()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.PAUSE);
        MenuOn();
    }
    internal void Ñontinue()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.RUNNING);
        MenuOff();
    }
    internal void Exit()
    {
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
