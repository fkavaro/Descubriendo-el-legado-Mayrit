using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BehaviourController))]
public abstract class ABehaviourControllable : MonoBehaviour, IBehaviourControllable
{
    #region PROPERTIES
    public string Name => gameObject.name;
    public BehaviourController BehaviourController
    {
        get => _behaviourController;
        set => _behaviourController = value;
    }
    BehaviourController _behaviourController;
    #endregion

    #region MONOBEHAVIOUR
    protected virtual void Awake()
    {
        _behaviourController = GetComponent<BehaviourController>();
    }

    void IBehaviourControllable.StartCoroutine(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }
    #endregion

}
