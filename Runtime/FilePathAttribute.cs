using System;
using UnityEngine;
// ReSharper disable CheckNamespace

public enum FilePathType
{
    Absolute,
    ResourcesFolder,
    AssetsFolder
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FilePathAttribute : PropertyAttribute
{
    public FilePathType FilePathType { get; set; } = FilePathType.ResourcesFolder;

    public string[] Filters { get; set; } =
    {
        "All Files",
#if UNITY_EDITOR_OSX
        ""
#else
        "*"
#endif
    };
    
    // TODO : Allow specifying base directory path to make the value relative to.
}