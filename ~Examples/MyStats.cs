using System;
using Runtime;
using UnityEngine;

// [CreateAssetMenu(fileName = "new Stats", menuName = "Data/! My Stats !", order = 0)]
public class MyStats : ScriptableObject, ISomething
{
    [Header("Predefined Values")]
    [PredefinedValues("abc", "def", "this is a sentence", "this is a paragraph\nthis is another paragraph", AllowOtherValues = true)]
    public string factionName;

    [PredefinedValues(new []{2, 4, 8, 16, 32}, new [] {"Half", "Quarter", "Eighth", "Sixteenth", "Thirty-second"})]
    public int damageReduction;
    [PredefinedValues("this", "is", "an", "array")]
    public string[] friendList;
    [PredefinedValues(new float[]{3, 6, 9, 12})]
    public float[] things;

    [ToggleLeft, Header("Toggle Left")]
    public bool isHappy;

    [Space, Header("File Paths")]
    [FilePath(FilePathType = FilePathType.ResourcesFolder)]
    public string myResourceFile;
    
    [FilePath(FilePathType = FilePathType.AssetsFolder)]
    public string myAssetFile;

    [FilePath(FilePathType = FilePathType.Absolute)]
    public string myAbsoluteFile;

    [Space, Header("Must Implement")]
    [MustImplement(typeof(ISomething))]
    public ScriptableObject mySettings;

    [Space, Tag(ShowCopyButton = true), Header("Tags")]
    public string myTagName;
    [Tag] public string mySimpleTagName;

    [Header("Layers"), SingleLayer]
    public int myLayer;

    [Header("Read Only"), ReadOnly]
    public string taunt = "you can't change this message :p";
    [ReadOnly]
    public MyData data;
    
    [Serializable]
    public struct MyData
    {
        public int id;
        public string subject;
        public string comment;
    }
}

public interface ISomething
{
    
}