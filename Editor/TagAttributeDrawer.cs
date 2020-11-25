using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagAttributeDrawer : PropertyDrawerBase<TagAttribute>
    {
        private const float CopyButtonWidth = 40;
        private const float FieldButtonSpacing = 5;

        private static GUIContent CopyIcon
        {
            get
            {
                var gc = EditorGUIUtility.IconContent(IconIDs.darkTheme.clipboardIcon);
                gc.tooltip = "Copy to clipboard";
                return gc;
            }
        }
        
        protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                position = EditorGUI.PrefixLabel(position, label);

                var fieldRect = new Rect(position);

                if (Attribute.ShowCopyButton)
                {
                    fieldRect.width = position.width - (CopyButtonWidth + FieldButtonSpacing);
                }
                    
                var buttonRect = new Rect(position)
                {
                    x = position.x + position.width - CopyButtonWidth,
                    width = CopyButtonWidth
                };
                property.stringValue = EditorGUI.TagField(fieldRect, property.stringValue);
                if (Attribute.ShowCopyButton && GUI.Button(buttonRect, CopyIcon, EditorStyles.miniButton))
                {
                    EditorGUIUtility.systemCopyBuffer = property.stringValue;
                    Debug.Log($"Tag \"{property.stringValue}\" copied to clipboard.");
                }
            }
        }
    }
}