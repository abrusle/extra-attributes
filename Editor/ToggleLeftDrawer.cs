using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    [CustomPropertyDrawer(typeof(ToggleLeftAttribute))]
    public class ToggleLeftDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                property.boolValue = EditorGUI.ToggleLeft(position, label, property.boolValue);
            }
        }
    }
}