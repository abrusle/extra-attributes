using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    [CustomPropertyDrawer(typeof(SingleLayerAttribute))]
    public class SingleLayerAttributeDrawer : PropertyDrawerBase<SingleLayerAttribute>
    {
        protected override SerializedPropertyType[] SupportedTypes => new[] {SerializedPropertyType.Integer};

        protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                position = EditorGUI.PrefixLabel(position, label);
                property.intValue = EditorGUI.LayerField(position, property.intValue);
            }
        }
    }
}