using Runtime;
using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    internal class SceneDirectionDrawer : SceneDrawer<SceneDirectionAttribute>
    {
        public SceneDirectionDrawer(SceneDirectionAttribute dirAttr, SerializedProperty prop, MonoBehaviour mono) : base(dirAttr, prop, mono)
        {
        }

        public override void OnSceneGui()
        {
            var dir = PropertyValue.normalized;
            var monoTransform = mono.transform;
            
            if (attribute.Space == Space.Self)
                dir = monoTransform.TransformDirection(dir);
            
            var position = monoTransform.position;
            
            Handles.color = attribute.Color;
            Handles.DrawLine(position, position + dir * attribute.Length);
        }
        
        private Vector3 PropertyValue
        {
            get
            {
                switch (prop.propertyType)
                {
                    case SerializedPropertyType.Vector2:
                        return prop.vector2Value;
                    case SerializedPropertyType.Vector3:
                        return prop.vector3Value;
                    default: throw new System.Exception($"{prop.type} is not a valid type for {nameof(SceneDirectionAttribute)}");
                }
            }
        }
    }
}