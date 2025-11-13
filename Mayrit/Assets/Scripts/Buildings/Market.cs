using System;
using System.Collections.Generic;
using UnityEngine;

public class Market : Building
{
    #region EDITOR PROPERTIES
    [SerializeField] List<Stall> _stalls = new();
    #endregion
}
