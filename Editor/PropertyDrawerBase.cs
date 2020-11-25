using System;
using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    public abstract class PropertyDrawerBase<T> : PropertyDrawer where T : PropertyAttribute
    {
        protected static class IconIDs
        {
            public static readonly Theme darkTheme = new Theme
            {
                folderIcon = "d_FolderOpened Icon",
                clipboardIcon = "Clipboard"
            };
            public static readonly Theme lightTheme = new Theme
            {
                folderIcon = "FolderOpened Icon",
                clipboardIcon = "Clipboard"
            };
            
            public class Theme
            {
                public string folderIcon;
                public string clipboardIcon;
            }
        }
        
        protected T Attribute => _attr ?? (_attr = (T) attribute);
        private T _attr;

        protected virtual SerializedPropertyType[] SupportedTypes { get; } = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (IsSupportedProperty(property, out string customErrorMessage))
            {
                DrawGUI(position, property, label);
            }
            else
            {
                DrawInvalidAttributeUsage(position, label, customErrorMessage);
            }
        }

        protected abstract void DrawGUI(Rect position, SerializedProperty property, GUIContent label);

        protected virtual bool IsSupportedProperty(SerializedProperty property, out string customErrorMessage)
        {
            customErrorMessage = null;
            return SupportedTypes == null || Array.IndexOf(SupportedTypes, property.propertyType) != -1;
        }

        protected class ColorScope : GUI.Scope
        { 
            private readonly Color _initialContentColor;
            private readonly Color _initialBgColor;

            public ColorScope(Color contentColor, Color? backgroundColor)
            {
                _initialContentColor = GUI.contentColor;
                _initialBgColor = GUI.backgroundColor;
                
                GUI.contentColor = contentColor;
                GUI.backgroundColor  = backgroundColor ?? GUI.backgroundColor;
            }
            
            /// <inheritdoc />
            protected override void CloseScope()
            {
                GUI.contentColor = _initialContentColor;
                GUI.backgroundColor = _initialBgColor;
            }
        }

        protected sealed class ValidityScope : ColorScope
        {
            private static Color ContentColor => new Color(1f, 0.27f, 0.19f);
            private static Color BackgroundColor => new Color(0.64f, 0.47f, 0.42f);
            
            public ValidityScope(bool valid, bool tintBackground = false)
                : base(valid ? GUI.contentColor : ContentColor, 
                    tintBackground ? (valid ? GUI.backgroundColor : BackgroundColor) : (Color?) null)
            {
            }
        }

        protected void DrawInvalidAttributeUsage(Rect position, GUIContent label, string customErrorMessage = null)
        {
            var message = new GUIContent($"Invalid use of {typeof(T)}");
            if (customErrorMessage != null)
                message.text = customErrorMessage;
            if (SupportedTypes != null && SupportedTypes.Length > 0)
                message.tooltip = "Valid on:\t\n- " + string.Join("\n- ", SupportedTypes);
            
            using (new ValidityScope(false))
                EditorGUI.LabelField(position, label, message);
        }
    }
}