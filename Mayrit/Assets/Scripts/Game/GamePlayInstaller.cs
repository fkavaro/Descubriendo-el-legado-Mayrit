using UnityEngine;

/// <summary>
/// Installer/Bootstrapper that registers all game services at startup.
/// </summary>
public class GamePlayInstaller : MonoBehaviour
{
    [Header("Manager References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private ProgressManager _progressManager;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private TourManager _tourManager;
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private TownManager _townManager;
    [SerializeField] private NPCPoolManager _npcPoolManager;

    private void Awake()
    {
        // Register all managers as services
        if (_gameManager != null)
            ServiceLocator.Instance.Register<GameManager>(_gameManager);
        else
            Debug.LogError("GamePlayInstaller: GameManager reference is missing!");

        if (_uiManager != null)
            ServiceLocator.Instance.Register<UIManager>(_uiManager);
        else
            Debug.LogError("GamePlayInstaller: UIManager reference is missing!");

        if (_progressManager != null)
            ServiceLocator.Instance.Register<ProgressManager>(_progressManager);
        else
            Debug.LogError("GamePlayInstaller: ProgressManager reference is missing!");

        if (_cameraManager != null)
            ServiceLocator.Instance.Register<CameraManager>(_cameraManager);
        else
            Debug.LogError("GamePlayInstaller: CameraManager reference is missing!");

        if (_tourManager != null)
            ServiceLocator.Instance.Register<TourManager>(_tourManager);
        else
            Debug.LogError("GamePlayInstaller: TourManager reference is missing!");

        if (_timeManager != null)
            ServiceLocator.Instance.Register<TimeManager>(_timeManager);
        else
            Debug.LogError("GamePlayInstaller: TimeManager reference is missing!");

        if (_townManager != null)
            ServiceLocator.Instance.Register<TownManager>(_townManager);
        else
            Debug.LogError("GamePlayInstaller: TownManager reference is missing!");

        if (_npcPoolManager != null)
            ServiceLocator.Instance.Register<NPCPoolManager>(_npcPoolManager);
        else
            Debug.LogError("GamePlayInstaller: NPCPoolManager reference is missing!");
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.Clear();
    }
}
