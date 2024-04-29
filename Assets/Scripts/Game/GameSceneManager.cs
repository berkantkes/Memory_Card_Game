using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private CardsManager _cardsManager;

    public void Initialize(int roundCount, int roundTime, BoardSize boardSize)
    {
        _cardsManager.Initialize(boardSize);
        _turnManager.Initialize(roundCount, roundTime, _cardsManager);
    }
}
