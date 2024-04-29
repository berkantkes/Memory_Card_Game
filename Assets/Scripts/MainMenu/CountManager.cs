using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountManager : MonoBehaviour
{
    [SerializeField] private Button _valueIncreaseButton;
    [SerializeField] private Button _valueDecreaseButton;
    [SerializeField] protected TextMeshProUGUI _valueText;

    protected int Value = 10;

    private int _minValue;
    private int _maxValue;

    public void Start()
    {
        UpdateValueText();
    }

    public void Initilaize(int minValue, int maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
    }

    private void OnEnable()
    {
        _valueIncreaseButton.onClick.AddListener(IncreaseValue);
        _valueDecreaseButton.onClick.AddListener(DecreaseValue);
    }
    private void OnDisable()
    {
        _valueIncreaseButton.onClick.RemoveListener(IncreaseValue);
        _valueDecreaseButton.onClick.RemoveListener(DecreaseValue);
    }

    public virtual void IncreaseValue()
    {
        Value++;
        Value = Mathf.Clamp(Value, _minValue, _maxValue);
        UpdateValueText();
    }
    public virtual void DecreaseValue()
    {
        Value--;
        Value = Mathf.Clamp(Value, _minValue, _maxValue);
        UpdateValueText();
    }

    public virtual void UpdateValueText()
    {
        _valueText.SetText(Value.ToString());
    }

    public int GetValue()
    {
        return Value;
    }
}
