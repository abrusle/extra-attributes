﻿using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    public abstract class PropertyDrawerBase<T> : PropertyDrawer where T : PropertyAttribute
    {
        protected static class IconIDs
        {
            public static readonly Theme darkTheme = new Theme {folderIcon = "d_FolderOpened Icon"};
            public static readonly Theme lightTheme = new Theme {folderIcon = "FolderOpened Icon"};
            
            public class Theme
            {
                public string folderIcon;
            }
        }
        
        protected T Attribute => _attr ?? (_attr = (T) attribute);
        private T _attr;
        
        public abstract override void OnGUI(Rect position, SerializedProperty property, GUIContent label);

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
            
            public ValidityScope(bool valid, bool tintBackgound = false)
                : base(valid ? GUI.contentColor : ContentColor, 
                    tintBackgound ? (valid ? GUI.backgroundColor : BackgroundColor) : (Color?) null)
            {
            }
        }
    }
}