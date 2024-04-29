using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTimeManager : CountManager
{
    private int _roundTimeDefaultValue = 60;
    private int _defaultMinValue = 40;
    private int _defaultMaxValue = 80;

    public void Initilaize()
    {
        base.Initilaize(_defaultMinValue, _defaultMaxValue);
        Value = _roundTimeDefaultValue;
        UpdateValueText();
    }
}