using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class TagAttribute : PropertyAttribute
{
    public bool ShowCopyButton { get; set; } = false;
}