using UnityEngine;

public class MainMenuInstaller : MonoBehaviour
{
    [Header("Manager References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private UIManager _uiManager;

    private void Awake()
    {
        // Register all managers as services
        if (_gameManager != null)
            ServiceLocator.Instance.Register<GameManager>(_gameManager);
        else
            Debug.LogError("MainMenuInstaller: GameManager reference is missing!");

        if (_uiManager != null)
            ServiceLocator.Instance.Register<UIManager>(_uiManager);
        else
            Debug.LogError("MainMenuInstaller: UIManager reference is missing!");
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.Clear();
    }
}
