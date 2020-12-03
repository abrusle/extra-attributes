using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    public static class ExtraAttributesUtility
    {
        internal static readonly IReadOnlyDictionary<FilePathType, string> BasePathList = new Dictionary<FilePathType, string>
        {
            {FilePathType.ResourcesFolder, Application.dataPath + "/Resources/"},
            {FilePathType.AssetsFolder   , Application.dataPath + '/'},
            {FilePathType.Absolute       , string.Empty},
        };
        
        public static string ConvertAbsolutePath(string path, FilePathType desiredFilePathType)
        {
            if (!Uri.TryCreate(path, UriKind.Absolute, out var uri))
            {
                if (string.IsNullOrWhiteSpace(path)) return string.Empty;
                else throw new ArgumentException($"File path not supported ({path})");
            }

            if (!uri.IsFile || !uri.IsAbsoluteUri)
                throw new ArgumentException($"File path not supported ({path})");
            if (desiredFilePathType == FilePathType.Absolute) return path;

            string basePath = BasePathList[desiredFilePathType];
            var baseUri = new Uri(basePath);
            if (!baseUri.IsBaseOf(uri))
            {
                Debug.LogError($"File outside of scope. Path={path}, Scope={desiredFilePathType}");
                return path;
            }

            return path.Substring(basePath.Length);
        }

        internal static void OpenFileDialogue(string title, string path, string[] filters, IFileDialogueRecipient recipient)
        {
            Async();
            async void Async()
            {
                await Task.Yield();
                recipient.FileDialogueResult = EditorUtility.OpenFilePanelWithFilters(title, path, filters);
            }
        }

        public static Type GetPropertyType(this SerializedProperty prop)
        {
            try
            {
                return prop.serializedObject.targetObject.GetType()
                    .GetField(prop.name, 
                        BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.Instance)?.FieldType;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}