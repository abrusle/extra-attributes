using System;
using Runtime;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class SceneDirectionAttribute : SceneAttributeBase
{
    public readonly float length;

    public SceneDirectionAttribute(Space space = Space.World, float length = 1) : base(space)
    {
        this.length = length;
    }
}