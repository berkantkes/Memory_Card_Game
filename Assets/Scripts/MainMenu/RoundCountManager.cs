using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundCountManager : CountManager
{
    private int _roundCountDefaultValue = 4;
    private int _defaultMinValue = 1;
    private int _defaultMaxValue = 6;

    public void Initilaize()
    {
        base.Initilaize(_defaultMinValue, _defaultMaxValue);
        Value = _roundCountDefaultValue;
        UpdateValueText();
    }
}