using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    public class ScenePointDrawer : SceneDrawer<ScenePointAttribute>
    {

        public ScenePointDrawer(
            ScenePointAttribute attribute, 
            SerializedProperty prop, 
            MonoBehaviour mono) : base(attribute, prop, mono)
        {
        }

        public override void OnSceneGui()
        {
            Vector3 pt = PropertyValue;

            if (attribute.Space == Space.Self)
            {
                pt = mono.transform.TransformPoint(pt);
            }
            
            Handles.color = attribute.Color;

            float offset = attribute.Size * 0.5f;

            {
                var lineStartPos = new Vector3(pt.x - offset, pt.y, pt.z);
                var lineEndPos   = new Vector3(pt.x + offset, pt.y, pt.z);
                Handles.DrawLine(lineStartPos, lineEndPos);
            }
            
            {
                var lineStartPos = new Vector3(pt.x, pt.y - offset, pt.z);
                var lineEndPos   = new Vector3(pt.x, pt.y + offset, pt.z);
                Handles.DrawLine(lineStartPos, lineEndPos);
            }
            
            {
                var lineStartPos = new Vector3(pt.x, pt.y, pt.z - offset);
                var lineEndPos   = new Vector3(pt.x, pt.y, pt.z + offset);
                Handles.DrawLine(lineStartPos, lineEndPos);
            }
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
                    default: throw new System.Exception($"{prop.type} is not a valid type for {nameof(ScenePointAttribute)}");
                }
            }
        }
    }
}