using System;
using Runtime;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class SceneDirectionAttribute : SceneAttributeBase
{
    public float Length { get; set; } = 1;
}