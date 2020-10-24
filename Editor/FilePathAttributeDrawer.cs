using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Abrusle.ExtraAtributes.Editor
{
    [CustomPropertyDrawer(typeof(FilePathAttribute))]
    public class FilePathAttributeDrawer : PropertyDrawerBase<FilePathAttribute>
    {
        private static GUIContent Icon
        {
            get
            {
                var c = EditorGUIUtility.IconContent(IconIDs.darkTheme.folderIcon);
                c.text = string.Empty;
                c.tooltip = "Browse...";
                return c;
            }
        }

        private const float ButtonWidth = 50;
        private const float Spacing = 4f;

        private Object _referencedObject = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new ValidityScope(File.Exists(property.stringValue), true))
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                position = EditorGUI.PrefixLabel(position, label);
                
                var fieldRect = new Rect(position)
                {
                    width = position.width - (ButtonWidth + Spacing)
                };
                
                string newPath = EditorGUI.TextField(fieldRect, property.stringValue);
                
                var buttonRect = new Rect(position)
                {
                    width = ButtonWidth,
                    x = fieldRect.x + fieldRect.width + Spacing
                };
                if (GUI.Button(buttonRect, Icon, EditorStyles.miniButton))
                {
                    newPath = EditorUtility.OpenFilePanelWithFilters(
                        "Path to " + label.text, 
                        Application.dataPath,
                        Attribute.FileModalFilters);

                    newPath = ExtraAttributesUtility.ConvertPath(newPath, Attribute.FilePathType);
                }

                if (newPath != property.stringValue)
                    OnPathChanged(property, newPath);
            }

            // if (!string.IsNullOrEmpty(property.stringValue) && _referencedObject == null)
            //     _referencedObject = GetReferenceObject(property.stringValue);
            //
            // EditorGUI.indentLevel++;
            // using (new EditorGUI.DisabledScope(true))
            //     EditorGUILayout.ObjectField(_referencedObject, typeof(Object), false);
            // EditorGUI.indentLevel--;
        }

        private void OnPathChanged(SerializedProperty property, string newPath)
        {
            property.stringValue = newPath;
            // _referencedObject = GetReferenceObject(newPath);
        }

        private Object GetReferenceObject(string path)
        {
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out _)) return null;
            switch (Attribute.FilePathType)
            {
                case FilePathType.Absolute:
                    path = ExtraAttributesUtility.ConvertPath(path, FilePathType.AssetsFolder);
                    goto case FilePathType.AssetsFolder;
                case FilePathType.ResourcesFolder:
                    return Resources.Load(path);
                case FilePathType.AssetsFolder:
                    return AssetDatabase.LoadMainAssetAtPath(path);
                default:
                    return null;
            }
        }
    }
}