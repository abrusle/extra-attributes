using Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "new Stats", menuName = "Data/! My Stats !", order = 0)]
public class MyStats : ScriptableObject, ISomething
{
    [PredefinedValues("abc", "def", "this is a sentence", "this is a paragraph\nthis is another paragraph", AllowOtherValues = true)]
    public string factionName;

    [PredefinedValues(new []{2, 4, 8, 16, 32}, new [] {"Half", "Quarter", "Eighth", "Sixteenth", "Thirty-second"})]
    public int damageReduction;
    [PredefinedValues("this", "is", "an", "array")]
    public string[] friendList;
    [ToggleLeft]
    public bool isHappy;
    public float[] things;

    [Space]
    [FilePath(FilePathType = FilePathType.ResourcesFolder)]
    public string myResourceFile;
    
    [FilePath(FilePathType = FilePathType.AssetsFolder)]
    public string myAssetFile;

    [FilePath(FilePathType = FilePathType.Absolute)]
    public string myAbsoluteFile;

    [Space]
    [MustImplement(typeof(ISomething), typeof(ILogHandler))]
    public Object mySettings;
}

public interface ISomething
{
    
}