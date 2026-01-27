using UnityEngine;

public class GamePlayServicesInstaller : BaseGameServicesInstaller
{
    #region EDITOR PROPERTIES
    [SerializeField] private ServiceConfig<ProgressManager> _progressManagerConfig;
    [SerializeField] private ServiceConfig<CameraManager> _cameraManagerConfig;
    [SerializeField] private ServiceConfig<TourManager> _tourManagerConfig;
    [SerializeField] private ServiceConfig<TimeManager> _timeManagerConfig;
    [SerializeField] private ServiceConfig<TownManager> _townManagerConfig;
    [SerializeField] private ServiceConfig<NPCPoolManager> _npcPoolManagerConfig;
    #endregion

    #region LIFE CYCLE
    protected override void Awake()
    {
        base.Awake();

        RegisterInServiceLocator(_progressManagerConfig);
        RegisterInServiceLocator(_cameraManagerConfig);
        RegisterInServiceLocator(_tourManagerConfig);
        RegisterInServiceLocator(_timeManagerConfig);
        RegisterInServiceLocator(_townManagerConfig);
        RegisterInServiceLocator(_npcPoolManagerConfig);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        // Unregister services that are not marked as DontDestroyOnLoad
        if (!_progressManagerConfig.dontDestroyOnLoad)
            ServiceLocator.Instance.Unregister<ProgressManager>();
        if (!_cameraManagerConfig.dontDestroyOnLoad)
            ServiceLocator.Instance.Unregister<CameraManager>();
        if (!_tourManagerConfig.dontDestroyOnLoad)
            ServiceLocator.Instance.Unregister<TourManager>();
        if (!_timeManagerConfig.dontDestroyOnLoad)
            ServiceLocator.Instance.Unregister<TimeManager>();
        if (!_townManagerConfig.dontDestroyOnLoad)
            ServiceLocator.Instance.Unregister<TownManager>();
        if (!_npcPoolManagerConfig.dontDestroyOnLoad)
            ServiceLocator.Instance.Unregister<NPCPoolManager>();
    }
    #endregion

    protected override void ReassignDuplicatedService(object obj)
    {
        base.ReassignDuplicatedService(obj);

        if (obj is ProgressManager)
            _progressManagerConfig.service = obj as ProgressManager;
        else if (obj is CameraManager)
            _cameraManagerConfig.service = obj as CameraManager;
        else if (obj is TourManager)
            _tourManagerConfig.service = obj as TourManager;
        else if (obj is TimeManager)
            _timeManagerConfig.service = obj as TimeManager;
        else if (obj is TownManager)
            _townManagerConfig.service = obj as TownManager;
        else if (obj is NPCPoolManager)
            _npcPoolManagerConfig.service = obj as NPCPoolManager;
    }
}
