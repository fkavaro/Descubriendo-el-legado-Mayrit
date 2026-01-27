using System;
using UnityEngine;

public class BaseGameServicesInstaller : MonoBehaviour
{
    #region EDITOR PROPERTIES
    [SerializeField] private ServiceConfig<GameManager> _gameManagerConfig;
    [SerializeField] private ServiceConfig<UIManager> _uiManagerConfig;
    [SerializeField] private ServiceConfig<SoundManager> _soundManagerConfig;
    #endregion

    #region LIFE CYCLE
    protected virtual void Awake()
    {
        ServiceLocator.Instance.OnDuplicatedServiceEvent += ReassignDuplicatedService;

        RegisterInServiceLocator(_gameManagerConfig);
        RegisterInServiceLocator(_uiManagerConfig);
        RegisterInServiceLocator(_soundManagerConfig);
    }

    protected virtual void OnDestroy()
    {
        ServiceLocator.Instance.OnDuplicatedServiceEvent -= ReassignDuplicatedService;

        // Unregister services that are not marked as DontDestroyOnLoad
        if (!_gameManagerConfig.dontDestroyOnLoad)
            ServiceLocator.Instance.Unregister<GameManager>();
        if (!_uiManagerConfig.dontDestroyOnLoad)
            ServiceLocator.Instance.Unregister<UIManager>();
        if (!_soundManagerConfig.dontDestroyOnLoad)
            ServiceLocator.Instance.Unregister<SoundManager>();
    }
    #endregion

    #region HELPERS
    protected void RegisterInServiceLocator<T>(ServiceConfig<T> serviceConfig) where T : MonoBehaviour
    {
        if (serviceConfig.service == null)
        {
            Debug.LogError($"{typeof(T)} reference is missing!");
            return;
        }

        ServiceLocator.Instance.Register(serviceConfig);
    }

    protected virtual void ReassignDuplicatedService(object obj)
    {
        if (obj is GameManager)
            _gameManagerConfig.service = obj as GameManager;
        else if (obj is UIManager)
            _uiManagerConfig.service = obj as UIManager;
        else if (obj is SoundManager)
            _soundManagerConfig.service = obj as SoundManager;
    }
    #endregion
}
