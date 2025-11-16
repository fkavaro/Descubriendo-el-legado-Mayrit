using System;
using UnityEngine;

/// <summary>
/// Marks a serialized field as read-only in the inspector. Requires the matching
/// property drawer in the Editor assembly to render as disabled.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class ReadOnlyAttribute : PropertyAttribute { }
