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
        RegisterInServiceLocator(_gameManagerConfig);
        RegisterInServiceLocator(_uiManagerConfig);
        RegisterInServiceLocator(_soundManagerConfig);
    }

    // protected virtual void OnDestroy()
    // {
    //     ServiceLocator.Instance.Clear();
    // }
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
    #endregion
}
