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
    public class FilePathAttributeDrawer : PropertyDrawerBase<FilePathAttribute>, IFileDialogueRecipient
    {
        public string FileDialogueResult { get; set; }
        
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

        protected override SerializedPropertyType[] SupportedTypes => new[] {SerializedPropertyType.String};

        public override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new ValidityScope(PathIsValid(property.stringValue), true))
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var fieldPosition = EditorGUI.PrefixLabel(position, label);
                
                var fieldRect = new Rect(fieldPosition)
                {
                    width = fieldPosition.width - (ButtonWidth + Spacing)
                };

                var buttonRect = new Rect(fieldPosition)
                {
                    width = ButtonWidth,
                    x = fieldRect.x + fieldRect.width + Spacing
                };
                DrawPrefix(position, fieldPosition);
                string newPath = EditorGUI.TextField(fieldRect, property.stringValue);
                if (GUI.Button(buttonRect, Icon, EditorStyles.miniButton))
                {
                    string modalPath = Utility.BasePathList[Attribute.FilePathType];
                    Debug.Log(Attribute.FilePathType);
                    Debug.Log(string.Join(", ", Attribute.Filters));
                    Debug.Log(modalPath);
                    Utility.OpenFileDialogue("Path to " + label.text, modalPath, Attribute.Filters, this);
                    return;
                }

                if (FileDialogueResult != null)
                {
                    newPath = Utility.ConvertAbsolutePath(FileDialogueResult, Attribute.FilePathType);;
                    FileDialogueResult = null;
                }

                if (!string.IsNullOrWhiteSpace(newPath) && newPath != property.stringValue)
                    OnPathChanged(property, newPath);
            }
        }

        private void OnPathChanged(SerializedProperty property, string newPath)
        {
            property.stringValue = newPath;
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
            }

            return File.Exists(path);
        }

        private void DrawPrefix(Rect totalPosition, Rect fieldPosition)
        {
            var prefixRect = new Rect(totalPosition)
            {
                width = totalPosition.width - fieldPosition.width - Spacing
            };
            var content = new GUIContent();
            switch (Attribute.FilePathType)
            {
                case FilePathType.ResourcesFolder:
                    content.text = "Resources/";
                    break;
                case FilePathType.AssetsFolder:
                    content.text = "Assets/";
                    break;
                default:
                    return;
            }
            
            var style = new GUIStyle(EditorStyles.label)
            {
                normal = new GUIStyleState {textColor = new Color(1f, 1f, 1f, 0.35f)},
                alignment = TextAnchor.MiddleRight
            };
            
            EditorGUI.LabelField(prefixRect, content, style);
        }
    }
}