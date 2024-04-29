using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnVariablesManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnTimer;
    [SerializeField] private TextMeshProUGUI _remainingRoundCount;

    public void UpdateRoundCountText(int remainingRoundCount)
    {
        _remainingRoundCount.SetText((remainingRoundCount+1).ToString());
    }

    public void UpdateTimerText(float time)
    {
        _turnTimer.SetText(time.ToString("F1"));
    }
}
