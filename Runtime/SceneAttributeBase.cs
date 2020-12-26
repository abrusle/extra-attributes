using System;
using UnityEngine;

namespace Runtime
{
    public abstract class SceneAttributeBase : Attribute
    {
        public Space Space { get; set; } = Space.World;
        public Color Color => ColorUtility.TryParseHtmlString(ColorCode, out var c) ? c : Color.white;

        public string ColorCode { get; set; }
    }
}