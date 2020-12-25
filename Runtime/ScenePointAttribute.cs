using System;
using Runtime;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ScenePointAttribute : SceneAttributeBase
{
    public float Size { get; set; } = 1;

    public ScenePointAttribute(Space space = Space.World) : base(space)
    {
    }
}