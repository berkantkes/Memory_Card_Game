using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreenPanelsManager : MonoBehaviour
{
    [SerializeField] private Transform _roundChangePanel;
    [SerializeField] private TextMeshProUGUI _roundChangePanelText;
    [SerializeField] private Transform _roundWinPanel;
    [SerializeField] private TextMeshProUGUI _roundwinPanelText;
    [SerializeField] private Transform _gameEndPanelCanvas;
    [SerializeField] private TextMeshProUGUI _gameEndPanelPlayer1Score;
    [SerializeField] private TextMeshProUGUI _gameEndPanelPlayer2Score;
    [SerializeField] private TextMeshProUGUI _gameEndPanelWhoWin;
    [SerializeField] private Button _mainMenuButton;

    private TurnManager _turnManager;
    private Player _mainPlayer;
    private Player _aiPlayer;

    public void Initialize(TurnManager turnManager, Player mainPlayer, Player aiPlayer)
    {
        _turnManager = turnManager;
        _mainPlayer = mainPlayer;
        _aiPlayer = aiPlayer;
    }

    private void OnEnable()
    {
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void OnDisable()
    {
        _mainMenuButton.onClick.RemoveListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        _gameEndPanelCanvas.gameObject.SetActive(false);
        _turnManager.GoToMainMenu(); 
    }

    public async UniTask WhoWin()
    {
        DetectAndWriteWhoWin();

        _roundWinPanel.gameObject.SetActive(true);

        await UniTask.WaitForSeconds(2);

        _roundWinPanel.gameObject.SetActive(false);
    }

    private void DetectAndWriteWhoWin()
    {
        int mainPlayerPoint = _mainPlayer.GetPoint();
        int aiPlayerPoint = _aiPlayer.GetPoint();

        if (mainPlayerPoint > aiPlayerPoint)
        {
            _roundwinPanelText.SetText("PLAYER 1 WINS");
            _mainPlayer.WinRound();
        }
        else if (mainPlayerPoint < aiPlayerPoint)
        {
            _roundwinPanelText.SetText("PLAYER 2 WINS");
            _aiPlayer.WinRound();
        }
        else
        {
            _roundwinPanelText.SetText("PLAYERS DRAW");
        }
    }

    public async UniTask SetRoundPanel()
    {
        _turnManager.SetBackground(true);
        _roundChangePanel.gameObject.SetActive(true);
        _roundChangePanelText.SetText("ROUND " + _turnManager.GetRoundCount());

        await UniTask.WaitForSeconds(2);

        _roundChangePanel.gameObject.SetActive(false);
    }

    public void SetEndGamePanel()
    {
        int mainPlayerScore = _mainPlayer.GetScore();
        int aiPlayerScore = _aiPlayer.GetScore();

        _gameEndPanelCanvas.gameObject.SetActive(true);
        _gameEndPanelPlayer1Score.SetText(mainPlayerScore.ToString());
        _gameEndPanelPlayer2Score.SetText(aiPlayerScore.ToString());

        if (mainPlayerScore > aiPlayerScore)
        {
            _gameEndPanelWhoWin.SetText("PLAYER 1 WINS");
        }
        else if (mainPlayerScore < aiPlayerScore)
        {
            _gameEndPanelWhoWin.SetText("PLAYER 2 WINS");
        }
        else
        {
            _gameEndPanelWhoWin.SetText("PLAYERS DRAW");
        }
    }

}
