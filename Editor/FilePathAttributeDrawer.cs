#define EXTRA_ATTRIBUTES_DEBUG
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Utility = Abrusle.ExtraAtributes.Editor.ExtraAttributesUtility;

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
            using (new ValidityScope(PathIsValid(property.stringValue), true))
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
                    string modalPath = Utility.GetBasePathFromType(Attribute.FilePathType);
                    Debug.Log(Attribute.FilePathType);
                    Debug.Log(string.Join(", ", Attribute.Filters));
                    Debug.Log(modalPath);
                    newPath = EditorUtility.OpenFilePanelWithFilters(
                        "Path to " + label.text, 
                        modalPath,
                        Attribute.Filters);

                    Debug.Log("Selected Asset: " + newPath);
                    newPath = Utility.ConvertAbsolutePath(newPath, Attribute.FilePathType);
                }

                if (!string.IsNullOrWhiteSpace(newPath) && newPath != property.stringValue)
                    OnPathChanged(property, newPath);
            }
        }

        private void OnPathChanged(SerializedProperty property, string newPath)
        {
            property.stringValue = newPath;
            // _referencedObject = GetReferenceObject(newPath);
        }

        private bool PathIsValid(string path)
        {
            switch (Attribute.FilePathType)
            {
                case FilePathType.ResourcesFolder:
                    path = Application.dataPath + "/Resources/" + path;
                    break;
                case FilePathType.AssetsFolder:
                    path = Application.dataPath + "/" + path;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return File.Exists(path);
        }
    }
}