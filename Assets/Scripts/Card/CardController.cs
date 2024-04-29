using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [SerializeField] private Transform _cardBack;
    [SerializeField] private Transform _cardFront;
    [SerializeField] private Image _cardImage;
    [SerializeField] private Button _cardButton;

    private CardType _cardType;
    private CardsManager _cardsManager;

    public bool _isCardOpen = false;

    public void Initialize(CardType cardType, CardsManager cardsManager)
    {
        _cardType = cardType;
        _cardsManager = cardsManager;
        SetCardImage();
        DefaultCardSettings();
    }
    private void OnEnable()
    {
        _cardButton.onClick.AddListener(ClickedCard);
    }
    private void OnDisable()
    {
        _cardButton.onClick.RemoveListener(ClickedCard);
    }

    private void SetCardImage()
    {
        Sprite spriteToUse = UiImageSelector.GetSlicedItemSprite(_cardType);

        _cardImage.sprite = spriteToUse;

        float spriteWidth = spriteToUse.rect.width;
        float spriteHeight = spriteToUse.rect.height;

        float newWidth = spriteWidth * 2f;
        float newHeight = spriteHeight * 2f;

        RectTransform rectTransform = _cardImage.rectTransform;
        rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
    }

    private void DefaultCardSettings()
    {
        _cardFront.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
        _cardBack.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        CanProvideInput(false);
        _isCardOpen = false;
    }

    public void ClickedCard()
    {
        _cardButton.enabled = false;
        EventManager<CardController>.Execute(GameEvents.OnClickCard, this);
        StartCoroutine(OpenCardCoroutine());
    }

    private IEnumerator OpenCardCoroutine()
    {
        yield return _cardBack.transform.DORotate(new Vector3(0, -90, 0), .35f).SetEase(Ease.Linear).WaitForCompletion();

        yield return _cardFront.transform.DORotate(new Vector3(0, 0, 0), .35f).SetEase(Ease.Linear).WaitForCompletion();

        _isCardOpen = true;

        EventManager<CardController>.Execute(GameEvents.OnOpenCard, this);
    }

    public async UniTask CloseCard()
    {
        _isCardOpen = false;

        await _cardFront.transform.DORotate(new Vector3(0, -90, 0), .3f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        await _cardBack.transform.DORotate(new Vector3(0, 0, 0), .3f).SetEase(Ease.Linear).AsyncWaitForCompletion();
    }

    public void SetMatchedCard()
    {
        _cardButton.enabled = false;
    }

    public CardType GetCardType()
    {
        return _cardType;
    }

    public void CanProvideInput(bool status)
    {
        _cardButton.enabled = status;
    }

}

