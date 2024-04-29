using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPlayer : Player
{
    private List<CardController> _cardControllers = new List<CardController>();

    private Dictionary<CardType, List<CardController>> _matchingPairsDictionary = new Dictionary<CardType, List<CardController>>();
    private List<List<CardController>> _nonMatchingPairsList = new List<List<CardController>>(); 
    List<List<CardController>> _matchingPairsList = new List<List<CardController>>();

    private int correctMatchPecentage = 40;
    private bool _isTrueMatch = false;
    private TurnManager _turnManager;

    public void Initialize(List<CardController> cardControllers ,TurnManager turnManager)
    {
        _cardControllers = cardControllers;
        _turnManager = turnManager;
        ResetScore();
        ResetAi();
    }

    public void ResetAi()
    {
        ResetPoint();

        CreateMatchLists();
        UpdatePointText();
        UpdateScoreText();
    }

    private void CreateMatchLists()
    {
        _matchingPairsDictionary.Clear();
        _nonMatchingPairsList.Clear();
        _matchingPairsList.Clear();

        FindMatchingPairs();
        FindNonMatchingPairs();
    }

    private void FindMatchingPairs()
    {
        foreach (CardController cardController in _cardControllers)
        {
            if (!_matchingPairsDictionary.ContainsKey(cardController.GetCardType()))
            {
                _matchingPairsDictionary[cardController.GetCardType()] = new List<CardController>();
            }

            _matchingPairsDictionary[cardController.GetCardType()].Add(cardController);
        }

        foreach (var pair in _matchingPairsDictionary)
        {
            List<CardController> cards = pair.Value;
            if (cards.Count >= 2)
            {
                _matchingPairsList.Add(cards);
            }
        }
    }

    private void FindNonMatchingPairs()
    {
        List<List<CardController>> allPossiblePairs = new List<List<CardController>>();

        for (int i = 0; i < _cardControllers.Count - 1; i++)
        {
            for (int j = i + 1; j < _cardControllers.Count; j++)
            {
                List<CardController> pair = new List<CardController> { _cardControllers[i], _cardControllers[j] };
                allPossiblePairs.Add(pair);
            }
        }

        List<List<CardController>> sameTypePairs = new List<List<CardController>>();

        foreach (var pair in allPossiblePairs)
        {
            if (pair[0].GetCardType() != pair[1].GetCardType())
            {
                _nonMatchingPairsList.Add(pair);
            }
        }
    }

    public bool RemovePairsBelongingToSelectedController(CardController selectedController)
    {
        List<List<CardController>> pairsToRemove = new List<List<CardController>>();

        foreach (var pair in _nonMatchingPairsList)
        {
            if (pair.Contains(selectedController))
            {
                pairsToRemove.Add(pair);
            }
        }

        foreach (var pair in pairsToRemove)
        {
            _nonMatchingPairsList.Remove(pair);
        }

        pairsToRemove.Clear();

        foreach (var pair in _matchingPairsList)
        {
            if (pair.Contains(selectedController))
            {
                pairsToRemove.Add(pair);
            }
        }

        foreach (var pair in pairsToRemove)
        {
            _matchingPairsList.Remove(pair);
        }

        return _matchingPairsList.Count == 0;
    }

    public async void SelectCards()
    {
        do
        {
            await UniTask.WaitForSeconds(1);
            List<CardController> selectedCards = GetSelectedCards();

            for (int i = 0; i < selectedCards.Count; i++)
            {
                await UniTask.WaitForSeconds(1);

                if (_turnManager.GetCurrentPlayer() == this && _turnManager.GetRoundTime() > 2)
                {
                    selectedCards[i].ClickedCard();
                }
                else
                {
                    return;
                }
            }

        } while (_isTrueMatch && _matchingPairsList.Count > 0 && _turnManager.GetCurrentPlayer() == this);
        
    }

    private List<CardController> GetSelectedCards()
    {
        float randomValue = UnityEngine.Random.value * 100;

        if (randomValue <= correctMatchPecentage)
        {
            if (_matchingPairsList.Count > 0)
            {
                return GetMatcingPairs();
            }
        }
        else
        {
            if (_nonMatchingPairsList.Count > 0)
            {
                return GetNonMatcingPairs();
            }
            else
            {
                return GetMatcingPairs();
            }
        }

        return null;
    }

    private List<CardController> GetMatcingPairs()
    {
        int randomIndex = UnityEngine.Random.Range(0, _matchingPairsList.Count);
        List<CardController> selectedPair = _matchingPairsList[randomIndex];
        _isTrueMatch = true;

        foreach (CardController pair in selectedPair)
        {
            RemovePairsBelongingToSelectedController(pair);
        }

        return selectedPair;
    }
    private List<CardController> GetNonMatcingPairs()
    {
        int randomIndex = UnityEngine.Random.Range(0, _nonMatchingPairsList.Count);
        List<CardController> selectedPair = _nonMatchingPairsList[randomIndex];


        _isTrueMatch = false;
        return selectedPair;
    }
}

