using System.Linq;
using Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Abrusle.ExtraAtributes.Editor
{
    [CustomPropertyDrawer(typeof(MustImplementAttribute))]
    public class MustImplementAttributeDrawer : PropertyDrawerBase<MustImplementAttribute>
    {
        protected override SerializedPropertyType[] SupportedTypes => new[] {SerializedPropertyType.ObjectReference};

        protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.tooltip = Tooltip;
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var fieldObject = EditorGUI.ObjectField(position, label, property.objectReferenceValue, property.GetPropertyType() ?? typeof(Object), Attribute.AllowSceneObjects);
                if (fieldObject != property.objectReferenceValue)
                    OnValueChange(fieldObject, property);
            }
        }

        private void OnValueChange(Object fieldObject, SerializedProperty property)
        {
            var objectType = fieldObject ? fieldObject.GetType() : typeof(Object);
            if (!fieldObject || Attribute.IsTypeValid(objectType))
            {
                property.objectReferenceValue = fieldObject;
            }
            else
            {
                Debug.LogErrorFormat("{0} ({1}) is not valid type for {2}. Valid types are: {3}.",
                    fieldObject.name,
                    objectType.FullName,
                    property.propertyPath,
                    string.Join(", ", GetBaseTypeNames()));
            }
        }

        private string[] GetBaseTypeNames()
        {
            return Attribute.baseTypes.Select(type => type.FullName).ToArray();
        }

        private string Tooltip
        {
            get
            {
                return Attribute.baseTypes
                    .Aggregate("Must implement:", (current, baseType) =>
                        current + "\n- " + baseType.FullName);
            }
        }
    }
}