using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CollectiblesTracker : MonoBehaviour
{
    #region EDITOR PROPERTIES
    [SerializeField] List<Collectible> _collectibles = new();
    [SerializeField] Collectible _nextCollectible = null;
    #endregion

    #region INTERNAL PROPERTIES
    public event Action<Collectible> OnCollectibleFoundEvent;
    #endregion

    #region LIFE CYCLE
    void Awake()
    {
        _collectibles = new List<Collectible>(GetComponentsInChildren<Collectible>());

        foreach (Collectible collectible in _collectibles)
        {
            collectible.OnFoundEvent += OnCollectibleFound;
        }

        ServiceLocator.Instance.Register(this);
    }

    void Start()
    {
        GetNextCollectible();
    }

    void OnDisable()
    {
        ServiceLocator.Instance.Unregister(this);
    }
    #endregion

    #region PUBLIC METHODS
    public Collectible NextCollectible => _nextCollectible;

    public void GetNextCollectible()
    {
        _nextCollectible = null;

        foreach (Collectible collectible in _collectibles)
        {
            if (!collectible.IsFound)
            {
                _nextCollectible = collectible;
                break;
            }
        }
    }
    #endregion

    #region PRIVATE METHODS

    #endregion

    #region CALLBACK METHODS
    void OnCollectibleFound(Collectible collectible)
    {
        GetNextCollectible();
        OnCollectibleFoundEvent?.Invoke(collectible);
    }
    #endregion
}
