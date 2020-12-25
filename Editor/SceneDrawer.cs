using System;
using Runtime;
using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    public abstract class SceneDrawer<TAttribute> : ISceneDrawer where TAttribute : SceneAttributeBase
    {
        protected readonly SerializedProperty prop;
        protected readonly MonoBehaviour mono;
        protected readonly TAttribute attribute;

        protected SceneDrawer(TAttribute attribute, SerializedProperty prop, MonoBehaviour mono)
        {
            this.attribute = attribute;
            this.prop = prop;
            this.mono = mono;
        }

        /// <inheritdoc />
        public abstract void OnSceneGui();
    }
}