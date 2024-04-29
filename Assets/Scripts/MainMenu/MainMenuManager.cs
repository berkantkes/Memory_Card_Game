using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private RoundTimeManager _roundTimeManager;
    [SerializeField] private RoundCountManager _roundCountManager;
    [SerializeField] private BoardSizeManager _boardSizeManager;

    [SerializeField] private Button _playButton;

    private GameManager _gameManager;

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
        _roundTimeManager.Initilaize();
        _roundCountManager.Initilaize();
        _boardSizeManager.Initilaize();
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(PlayGame);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(PlayGame);
    }

    private void PlayGame()
    {
        _gameManager.PlayGame();
    }

    public int GetRoundTime()
    {
        return _roundTimeManager.GetValue();
    }
    public int GetRoundCount()
    {
        return _roundCountManager.GetValue();
    }
    public BoardSize GetBoardSize()
    {
        return _boardSizeManager.GetBoardSize();
    }
}
