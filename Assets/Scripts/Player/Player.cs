using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _point = 0;
    private int _score = 0;

    public void Initialize()
    {
        ResetPoint();
        ResetScore();
        UpdatePointText();
        UpdateScoreText();
    }

    protected void ResetPoint()
    {
        _point = 0;
    }
    protected void ResetScore()
    {
        _score = 0;
    }

    public void MakeMatch()
    {
        _point++;
        UpdatePointText();
    }

    protected void UpdatePointText()
    {
        _pointText.SetText(_point.ToString());
    }
    protected void UpdateScoreText()
    {
        _scoreText.SetText(_score.ToString());
    }

    public void WinRound()
    {
        _score++;
        UpdateScoreText();
    }

    public void NewRound()
    {
        _point = 0;
        UpdatePointText();
    }

    public int GetPoint()
    {
        return _point;
    }
    public int GetScore()
    {
        return _score;
    }

}
