using System;
using System.Linq;
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

    public (string fileType, string extension)[] Filters { get; set; } = FilterPresets.AllFiles;

    public string[] FileModalFilters => Filters
        .SelectMany(filter => new[] {filter.fileType, filter.extension})
        .ToArray();

    
    public struct FilterPresets
    {
        public static readonly (string fileType, string extension)[] AllFiles = {("All Files", "*")};
        public static readonly (string fileType, string extension)[] Assets = {("Assets", "asset")};
        public static readonly (string fileType, string extension)[] Images = {("Images", "jpg,png,jpeg")};
    }
}