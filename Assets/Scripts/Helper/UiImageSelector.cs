using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiImageSelector : MonoBehaviour
{
    [SerializeField] List<Sprite> _animalSprites = new List<Sprite>();

    private static Dictionary<CardType, Sprite> _cardSprites = new Dictionary<CardType, Sprite>();

    public void Initialize()
    {
        for (int i = 0; i < _animalSprites.Count; i++)
        {
            _cardSprites[(CardType)i] = _animalSprites[i];
        }
    }

    public static Sprite GetSlicedItemSprite(CardType cardType)
    {
        return _cardSprites[cardType];
    }

}
