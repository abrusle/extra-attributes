using Runtime;
using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    public class SceneRadiusDrawer : SceneDrawer<SceneRadiusAttribute>
    {
        public SceneRadiusDrawer(SceneRadiusAttribute attribute, SerializedProperty prop, MonoBehaviour mono) : base(attribute, prop, mono)
        {
        }

        public override void OnSceneGui()
        {
            float radius = PropertyValue;
            var monoTransform = mono.transform;

            bool relativeSpace = attribute.Space == Space.Self;
            if (relativeSpace) radius *= monoTransform.localScale.magnitude;

            var pos = monoTransform.position;
            
            Handles.color = attribute.Color;
            Handles.DrawWireDisc(pos, relativeSpace ? monoTransform.forward : Vector3.forward, radius);
            Handles.DrawWireDisc(pos, relativeSpace ? monoTransform.up      : Vector3.up,      radius);
            Handles.DrawWireDisc(pos, relativeSpace ? monoTransform.right   : Vector3.right,   radius);
        }

        private float PropertyValue
        {
            get
            {
                switch (prop.propertyType)
                {
                    case SerializedPropertyType.Float:
                        return prop.floatValue;
                    case SerializedPropertyType.Integer:
                        return prop.intValue;
                    default: throw new System.Exception($"{prop.type} is not a valid type for {nameof(ScenePointAttribute)}");
                }
            }
        }
    }
}