using System;
using System.Collections.Generic;
using System.IO;
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
        
        public static string ConvertPath(string path, FilePathType desiredFilePathType)
        {
            var uri = new Uri(path);

            if (!uri.IsFile || !uri.IsAbsoluteUri)
                throw new ArgumentException($"File path not supported ({path})");
            if (desiredFilePathType == FilePathType.Absolute) return path;

            return MakeRelative(path, ReferenceDirectoryPaths[desiredFilePathType]);
        }

        public static string MakeRelative(string filePath, string referencePath)
        {
            var fileUri = new Uri(filePath);
            var referenceUri = new Uri(referencePath);
            return Uri
                .UnescapeDataString(referenceUri.MakeRelativeUri(fileUri).ToString())
                .Replace('/', Path.DirectorySeparatorChar);
        }
    }
}