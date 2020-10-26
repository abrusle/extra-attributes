using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    public static class ExtraAttributesUtility
    {
        public static readonly IReadOnlyDictionary<FilePathType, string> ReferenceDirectoryPaths = new Dictionary<FilePathType, string>
        {
            {FilePathType.AssetsFolder, Application.dataPath},
            {FilePathType.ResourcesFolder, Application.dataPath + "/Resources/"},
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

            string basePath = GetBasePathFromType(desiredFilePathType);
            var baseUri = new Uri(basePath);
            if (!baseUri.IsBaseOf(uri))
            {
                Debug.LogError($"File outside of scope. Path={path}, Scope={desiredFilePathType}");
                return path;
            }

            return path.Substring(basePath.Length);
        }

        internal static string GetBasePathFromType(FilePathType pathType)
        {
            switch (pathType)
            {
                case FilePathType.ResourcesFolder:
                    return Application.dataPath + "/Resources/";
                case FilePathType.AssetsFolder:
                    return Application.dataPath;
                default:
                    return string.Empty;
            }
        }
    }
}