using System;
using UnityEngine;

namespace Runtime
{
    public abstract class SceneAttributeBase : Attribute
    {
        public readonly Space space;
        
        public Color Color { get; set; } = Color.white;

        protected SceneAttributeBase(Space space = Space.World)
        {
            this.space = space;
        }
    }
}