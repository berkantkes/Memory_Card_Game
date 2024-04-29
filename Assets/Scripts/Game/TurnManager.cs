using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private Player _mainPlayer;
    [SerializeField] private AiPlayer _aiPlayer;
    [SerializeField] private TurnVariablesManager _turnVariablesManager;
    [SerializeField] private GameScreenPanelsManager _gameScreenPanelsManager;
    [SerializeField] private Image _background;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameScreen;

    private Player _currentPlayer; 
    private Player _firstPlayer; 
    private CardsManager _cardsManager;
    private int _roundCount;
    private int _maxRound = 1;
    private float _roundTime;
    private float _defaultRoundTime;

    private bool _isGameContunie = false;
    private List<CardController> _clickedCards = new List<CardController>();
    private bool _isTimer = true;
    private bool _isFinishSelectableCard = true;

    public void Initialize(int roundCount, int roundTime, CardsManager cardsManager)
    {
        gameObject.SetActive(true);
        _maxRound = roundCount;
        _roundCount = 1;
        _defaultRoundTime = roundTime;
        _cardsManager = cardsManager;
        _roundTime = _defaultRoundTime;

        _mainPlayer.Initialize();
        _aiPlayer.Initialize(_cardsManager.GetCards(), this);
        _gameScreenPanelsManager.Initialize(this, _mainPlayer, _aiPlayer);

        StartTurn();
        UpdateTimerText();
        UpdateRoundCountText();
    }

    private void Update()
    {
        if (_isGameContunie && _isTimer)
        {
            _roundTime -= Time.deltaTime;
            UpdateTimerText();


            if (_roundTime < 0)
            {
                _isGameContunie = false;
                EndTurn();
            }
        }
    }

    private void OnEnable()
    {
        EventManager<CardController>.Subscribe(GameEvents.OnClickCard, ClickCard);
    }

    private void OnDisable()
    {
        EventManager<CardController>.Unsubscribe(GameEvents.OnClickCard, ClickCard);
    }

    private void ClickCard(CardController controller)
    {
        _clickedCards.Add(controller);

        CheckMatch();

        if (_clickedCards.Count == 2)
        {
            AllowTimer(false);
            _cardsManager.AllowInput(false);
        }
    }

    public void GoToMainMenu()
    {
        _mainMenu.SetActive(true);
        _gameScreen.SetActive(false);
    } 

    private async void CheckMatch()
    {
        if (_clickedCards.Count == 2)
        {
            _cardsManager.AllowInput(false);
            AllowTimer(false);

            if (_clickedCards[0].GetCardType() == _clickedCards[1].GetCardType())
            {
                _currentPlayer.MakeMatch();

                foreach (CardController controller in _clickedCards)
                {
                    controller.SetMatchedCard();
                    _isFinishSelectableCard =  _aiPlayer.RemovePairsBelongingToSelectedController(controller);

                    await UniTask.WhenAll(_clickedCards.Select(clickCard => UniTask.WaitUntil(() => clickCard._isCardOpen)));

                }

                if (_isFinishSelectableCard)
                {
                    EndTurn();
                }
                else
                {
                    AllowTimer(true);
                }

                if (_currentPlayer == _mainPlayer)
                {
                    _cardsManager.AllowInput(true);
                }

            }
            else
            {
                await UniTask.WhenAll(_clickedCards.Select(clickCard => UniTask.WaitUntil(() => clickCard._isCardOpen)));
                await UniTask.WhenAll(_clickedCards.Select(clickCard => clickCard.CloseCard()));

                ChangeTurn();
            }

            _clickedCards.Clear();
        }
    }

    private async void StartTurn()
    {
        await _gameScreenPanelsManager.SetRoundPanel();

        _firstPlayer = _mainPlayer;
        ResetGameSettings();
    }

    private void ChangeTurn()
    {
        _currentPlayer = (_currentPlayer == _mainPlayer) ? _aiPlayer : _mainPlayer;

        SetBackground();

        if (_currentPlayer == _mainPlayer)
        {
            _cardsManager.AllowInput(true);

        }
        else
        {
            _aiPlayer.SelectCards();
            _cardsManager.AllowInput(false);
        }

        AllowTimer(true);
    }

    private async void EndTurn()
    {
        CloseIfOpenCardExist();

        AllowTimer(false);
        _cardsManager.AllowInput(false);

        _roundCount++;

        if ((_roundCount - 1) == _maxRound)
        {
            EndGame();
            return;
        }

        UpdateRoundCountText();

        foreach (var card in _cardsManager.GetCards())
        {
            if (card._isCardOpen)
            {
                card.CloseCard();
            }
        }

        _cardsManager.ResetCards();

        await _gameScreenPanelsManager.WhoWin();
        await _gameScreenPanelsManager.SetRoundPanel();

        _aiPlayer.ResetAi();
        _mainPlayer.NewRound();
        _aiPlayer.NewRound();

        ResetGameSettings();
    }
    private void CloseIfOpenCardExist()
    {
        _clickedCards.Clear();
    }

    private async void EndGame()
    {
        await _gameScreenPanelsManager.WhoWin();

        _gameScreenPanelsManager.SetEndGamePanel();
    }

    private void ResetGameSettings()
    {
        _currentPlayer = _firstPlayer;
        _isGameContunie = true;
        _roundTime = _defaultRoundTime;
        SetBackground();
        _cardsManager.AllowInput(true);
        AllowTimer(true);
        UpdateRoundCountText();
        UpdateTimerText();

    }

    private void UpdateRoundCountText()
    {
        _turnVariablesManager.UpdateRoundCountText(_maxRound - _roundCount);
    }
    private void UpdateTimerText()
    {
        _turnVariablesManager.UpdateTimerText(_roundTime);
    }

    public void SetBackground(bool isTurnChange = false)
    {
        if (isTurnChange)
        {
            _background.color = Color.gray;
            return;
        }

        if(_currentPlayer == _mainPlayer)
        {
            _background.color = Color.green;
        }
        else
        {
            _background.color = Color.red;
        }
    }

    private void AllowTimer(bool status)
    {
        _isTimer = status;
    }

    public Player GetCurrentPlayer()
    {
        return _currentPlayer;
    }

    public int GetRoundCount()
    {
        return _roundCount;
    }
    public float GetRoundTime()
    {
        return _roundTime;
    }

}
