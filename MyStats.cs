using UnityEngine;

[CreateAssetMenu(fileName = "new Stats", menuName = "Data/! My Stats !", order = 0)]
public class MyStats : ScriptableObject
{
        [PredefinedValues("abc", "def", "this is a sentence", "this is a paragraph\nthis is another paragraph", AllowOtherValues = true)]
        public string factionName;

        [PredefinedValues(2, 4, 8, 16, 32, AllowOtherValues = true)]
        public int damageReduction;
        public string[] friendList;
        [ToggleLeft]
        public bool isHappy;
        public float[] things;
}