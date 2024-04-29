using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSizeManager : CountManager
{
    private BoardSize _boardSize;
    private int boardSizeDefaultValue = 1;
    private int _defaultMinValue = 1;
    private int _defaultMaxValue = 2;

    public void Initilaize()
    {
        base.Initilaize(_defaultMinValue, _defaultMaxValue);
        Value = boardSizeDefaultValue;
        UpdateValueText();
    }

    public override void UpdateValueText()
    {
        string boardSizeText = "";

        if (Value == 1)
        {
            _boardSize = BoardSize.Small;
            boardSizeText = "4x4";
        }
        else if (Value == 2)
        {
            _boardSize = BoardSize.Big;
            boardSizeText = "6x6";
        }

        _valueText.SetText(boardSizeText);
    }

    public BoardSize GetBoardSize()
    {
        return _boardSize;
    }

}

public enum BoardSize
{
    Small,
    Big
}
