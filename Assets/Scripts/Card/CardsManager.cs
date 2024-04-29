using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private List<CardController> _allCards = new List<CardController>();
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    private BoardSize _boardSize;

    private List<CardType> choosenCardType = new List<CardType>();
    private List<CardController> _cardsToUse = new List<CardController>();

    public void Initialize(BoardSize boardSize)
    {
        _boardSize = boardSize;
        _cardsToUse.Clear();
        _cardsToUse.AddRange(_allCards);
        SetBoardSize();
        SetCards();
    }

    private void SetBoardSize()
    {
        if (_boardSize == BoardSize.Small)
        {
            if (_cardsToUse.Count > 16)
            {
                int excessCount = _cardsToUse.Count - 16;
                for (int i = 0; i < excessCount; i++)
                {
                    CardController objToRemove = _cardsToUse[_cardsToUse.Count - 1];
                    objToRemove.gameObject.SetActive(false);
                    _cardsToUse.Remove(objToRemove);
                }
            }
            _gridLayoutGroup.constraintCount = 4;
        }
        else if(_boardSize == BoardSize.Big)
        {
            foreach (CardController card in _cardsToUse)
            {
                card.gameObject.SetActive(true);
            }
            transform.localScale = new Vector3(.7f, .7f, .7f);
            _gridLayoutGroup.constraintCount = 6;
        }
    }

    private void SetCards()
    {
        List<CardType> cardTypesList = new List<CardType>((CardType[])Enum.GetValues(typeof(CardType)));
        List<CardController> copyCards = new List<CardController>();

        copyCards.AddRange(_cardsToUse);

        int cardTypesCount = _cardsToUse.Count / 2;

        for (int i = 0; i < cardTypesCount; i++)
        {
            int randomCardTypeIndex = GetRandomNumber(cardTypesList.Count);

            CardType chooseCardType = cardTypesList[randomCardTypeIndex];
            choosenCardType.Add(chooseCardType);

            for (int k = 0; k < 2; k++)
            {
                int randomCardControllerIndex = GetRandomNumber(copyCards.Count);
                CardController chooseCardController = copyCards[randomCardControllerIndex];
                chooseCardController.Initialize(chooseCardType, this);
                copyCards.Remove(chooseCardController);
            }
            cardTypesList.Remove(chooseCardType);
        }
    }
    public void ResetCards()
    {
        List<CardType> cardTypesList = new List<CardType>();
        List<CardController> copyCards = new List<CardController>();

        copyCards.AddRange(_cardsToUse);
        cardTypesList.AddRange(choosenCardType);

        int cardTypesCount = _cardsToUse.Count / 2;

        for (int i = 0; i < cardTypesCount; i++)
        {
            int randomCardTypeIndex = GetRandomNumber(cardTypesList.Count);

            CardType chooseCardType = cardTypesList[randomCardTypeIndex];

            for (int k = 0; k < 2; k++)
            {
                int randomCardControllerIndex = GetRandomNumber(copyCards.Count);
                CardController chooseCardController = copyCards[randomCardControllerIndex];
                chooseCardController.Initialize(chooseCardType, this);
                copyCards.Remove(chooseCardController);
            }
            cardTypesList.Remove(chooseCardType);
        }
    }

    private int GetRandomNumber(int i)
    {
        return UnityEngine.Random.Range(0, i);
    }

    public void AllowInput(bool status)
    {
        foreach (CardController cardController in _cardsToUse)
        {
            cardController.CanProvideInput(status);
        }
    }

    public List<CardController> GetCards()
    {
        return _cardsToUse;
    }

}
