using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    [CustomPropertyDrawer(typeof(PredefinedValuesAttribute))]
    public class PredefinedValuesAttributeDrawer : PropertyDrawerBase<PredefinedValuesAttribute>
    {
        private const string OtherValueKey = "Other...";
        private const float DualFieldPopUpWidth = 80;
        
        private IDrawer _drawer;

        protected override SerializedPropertyType[] SupportedTypes => new[]
        {
            SerializedPropertyType.Float,
            SerializedPropertyType.Integer,
            SerializedPropertyType.String
        };

        /// <inheritdoc />
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        /// <inheritdoc />
        public override void DrawGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, prop))
            {
                switch (prop.propertyType)
                {
                    case SerializedPropertyType.String:
                        if (Attribute.StorageType != typeof(string)) goto default;
                        if (_drawer == null)
                            _drawer = new DrawerString(GetAttributeData<string>(), Attribute.AllowOtherValues, prop);
                        _drawer.DrawProperty(position, label);
                        break;
                    case SerializedPropertyType.Integer:
                        if (Attribute.StorageType != typeof(int)) goto default;
                        if (_drawer == null)
                            _drawer = new DrawerInt(GetAttributeData<int>(), Attribute.AllowOtherValues, prop);
                        _drawer.DrawProperty(position, label);
                        break;
                    case SerializedPropertyType.Float:
                        if (Attribute.StorageType != typeof(float)) goto default;
                        if (_drawer == null)
                            _drawer = new DrawerFloat(GetAttributeData<float>(), Attribute.AllowOtherValues, prop);
                        _drawer.DrawProperty(position, label);
                        break;
                    default:
                        DrawInvalidAttributeUsage(position, prop, label);
                        return;
                }
            }
        }

        private Dictionary<T, string> GetAttributeData<T>()
        {
            return Attribute.storage.Values.ToDictionary(kvp => (T) kvp.Key, kvp => kvp.Value);
        }

        public static void DrawInvalidAttributeUsage(Rect position, SerializedProperty prop, GUIContent label)
        {
            label.text = "\u26A0 " + label.text;
            label.tooltip += "\nInappropriate usage of PredefinedValuesAttribute.";
            EditorGUI.PropertyField(position, prop, label, true);
        }

        private interface IDrawer
        {
            void DrawProperty(Rect position, GUIContent label);
        }

        private abstract class Drawer<T> : IDrawer
        {
            protected readonly Dictionary<T, string> Data;
            protected readonly bool AllowOtherValues;
            protected readonly SerializedProperty Property;
            protected abstract T DefaultValue { get; }

            protected abstract DrawFieldDelegate DoDrawField { get; }

            protected abstract T PropertyValue { get; set; }

            protected delegate T DrawFieldDelegate(Rect position, T value);

            protected Drawer(Dictionary<T, string> data, bool allowOtherValues, SerializedProperty property)
            {
                Data = data;
                AllowOtherValues = allowOtherValues;
                Property = property;
            }

            public void DrawProperty(Rect position, GUIContent label)
            {
                position = EditorGUI.PrefixLabel(position, label);
                var labels = Data.Values.ToArray();
                if (AllowOtherValues) labels = labels.Concat(new[] {OtherValueKey}).ToArray();
                int index = SelectedIndex;

                if (index == -1)
                {
                    if (AllowOtherValues)
                    {
                        var fieldRect = new Rect(position)
                        {
                            width = position.width - DualFieldPopUpWidth
                        };

                        PropertyValue = DoDrawField(fieldRect, PropertyValue);

                        position.x += fieldRect.width + 5;
                        position.width = DualFieldPopUpWidth - 5;
                        index = labels.Length - 1;
                    }
                    else PropertyValue = Data.Keys.First();
                }

                int newIndex = EditorGUI.Popup(position, index, labels);
                if (index != newIndex) // Pop up value changed
                {
                    if (AllowOtherValues && newIndex == labels.Length - 1)
                        PropertyValue = DefaultValue;
                    else OnPopUpValueChanged(newIndex);
                }
            }

            protected int SelectedIndex => Array.IndexOf(Data.Keys.ToArray(), PropertyValue);


            private void OnPopUpValueChanged(int newIndex)
            {
                var values = Data.Keys.ToArray();
                if (newIndex < 0 || newIndex >= values.Length) return;
                PropertyValue = values[newIndex];
            }
        }

        private sealed class DrawerString : Drawer<string>
        {
            public DrawerString(Dictionary<string, string> data, bool allowOtherValues, SerializedProperty property)
                : base(data, allowOtherValues, property)
            {
                DefaultValue = string.Empty;
                DoDrawField = EditorGUI.TextField;
            }

            /// <inheritdoc />
            protected override string DefaultValue { get; }

            /// <inheritdoc />
            protected override DrawFieldDelegate DoDrawField { get; }

            /// <inheritdoc />
            protected override string PropertyValue
            {
                get => Property.stringValue;
                set => Property.stringValue = value;
            }
        }

        private sealed class DrawerInt : Drawer<int>
        {
            /// <inheritdoc />
            public DrawerInt(Dictionary<int, string> data, bool allowOtherValues, SerializedProperty property)
                : base(data, allowOtherValues, property)
            {
                DefaultValue = default;
                DoDrawField = EditorGUI.IntField;
            }

            /// <inheritdoc />
            protected override int DefaultValue { get; }

            /// <inheritdoc />
            protected override DrawFieldDelegate DoDrawField { get; }

            /// <inheritdoc />
            protected override int PropertyValue
            {
                get => Property.intValue;
                set => Property.intValue = value;
            }
        }

        private sealed class DrawerFloat : Drawer<float>
        {
            /// <inheritdoc />
            public DrawerFloat(Dictionary<float, string> data, bool allowOtherValues, SerializedProperty property)
                : base(data, allowOtherValues, property)
            {
                DefaultValue = default;
                DoDrawField = EditorGUI.FloatField;
            }

            /// <inheritdoc />
            protected override float DefaultValue { get; }

            /// <inheritdoc />
            protected override DrawFieldDelegate DoDrawField { get; }

            /// <inheritdoc />
            protected override float PropertyValue
            {
                get => Property.floatValue;
                set => Property.floatValue = value;
            }
        }
    }
}