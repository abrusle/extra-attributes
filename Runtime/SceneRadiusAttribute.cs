using System;
using Runtime;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class SceneRadiusAttribute : SceneAttributeBase
{
    public SceneRadiusAttribute(Space space = Space.World) : base(space)
    {
    }
}