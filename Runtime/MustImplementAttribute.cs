using System;
using System.Linq;
using UnityEngine;

namespace Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class MustImplementAttribute : PropertyAttribute
    {
        public readonly Type[] baseTypes;

        public bool AllowSceneObjects { get; set; } = true;

        public MustImplementAttribute(Type baseType) : this(new [] {baseType})
        {
        }

        public MustImplementAttribute(params Type[] baseTypes)
        {
            this.baseTypes = baseTypes;
        }

        public bool IsTypeValid(Type type)
        {
            return baseTypes.Any(baseType => baseType.IsAssignableFrom(type));
        }
    }
}