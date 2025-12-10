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
    #endregion
}
