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

    public string[] Filters { get; set; } = FilterPresets.AllFiles;

    private struct FilterPresets
    {
        public static readonly string[] AllFiles = {"All Files", "*"};
        public static readonly string[] Assets = {"Assets", "asset"};
        public static readonly string[] Images = {"Images", "jpg,png,jpeg"};
    }
}