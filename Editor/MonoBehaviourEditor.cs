using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Runtime;
using UnityEditor;
using UnityEngine;

namespace Abrusle.ExtraAtributes.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : UnityEditor.Editor
    {
        private MonoBehaviour TargetMono => target as MonoBehaviour;

        private ISceneDrawer[] _drawers;

        private void OnEnable()
        {
            _drawers = target.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Select(GetSceneDrawer)
                .Where(drawer => drawer != null)
                .ToArray();
        }

        private void OnSceneGUI()
        {
            foreach (var drawer in _drawers)
            {
                drawer.OnSceneGui();
            }
        }

        private ISceneDrawer GetSceneDrawer(FieldInfo field)
        {
            var attribute = field.GetCustomAttribute<SceneAttributeBase>(true);

            switch (attribute)
            {
                case ScenePointAttribute ptAttr:
                    return new ScenePointDrawer(ptAttr, FindProperty(), TargetMono);
                
                case SceneDirectionAttribute dirAttr:
                    return new SceneDirectionDrawer(dirAttr, FindProperty(), TargetMono);
                
                case SceneRadiusAttribute radAttr:
                    return new SceneRadiusDrawer(radAttr, FindProperty(), TargetMono);
                
                default: return null;
            }

            SerializedProperty FindProperty() => serializedObject.FindProperty(field.Name);
        }
    }
}