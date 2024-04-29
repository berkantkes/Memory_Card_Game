using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MainMenuManager _mainMenuManager;
    [SerializeField] private GameSceneManager _gameSceneManager;
    [SerializeField] private UiImageSelector _uiImageSelect;

    private void Start()
    {
        _mainMenuManager.Initialize(this);
        _uiImageSelect.Initialize();
    }
    public void PlayGame()
    {
        _mainMenuManager.gameObject.SetActive(false);
        _gameSceneManager.gameObject.SetActive(true);
        _gameSceneManager.Initialize(_mainMenuManager.GetRoundCount(), _mainMenuManager.GetRoundTime(), _mainMenuManager.GetBoardSize());
    }
}
