using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PredefinedValuesAttribute : PropertyAttribute
{
    public Type StorageType => storage.Type;
    
    public bool AllowOtherValues { get; set; }
    
    public readonly IStorage storage;

    // String
    public PredefinedValuesAttribute(char separator, string values) 
        : this(values.Split(separator)) {}
    public PredefinedValuesAttribute(char separator, string values, string labels) 
        : this(values.Split(separator), labels.Split(separator)) {}
    public PredefinedValuesAttribute(params string[] values) 
        : this(values, values) { }
    private PredefinedValuesAttribute(string[] values, string[] labels)
    {
        storage = new Storage<string>(Combine(values, labels));
    }
    
    // Int
    public PredefinedValuesAttribute(params int[] values) : this(values, ConvertToStrings(values)) { }
    private PredefinedValuesAttribute(int[] values, string[] labels)
    {
        storage = new Storage<int>(Combine(values, labels));
    }
    
    // Float
    public PredefinedValuesAttribute(params float[] values) : this(values, ConvertToStrings(values)) { }
    private PredefinedValuesAttribute(float[] values, string[] labels)
    {
        storage = new Storage<float>(Combine(values, labels));
    }

    private static Dictionary<T, string> Combine<T>(IEnumerable<T> values, IEnumerable<string> labels)
    {
        return values
            .Zip(labels, (v, l) => (v, l))
            .ToDictionary(t => t.v, t => t.l);
    }

    private static string[] ConvertToStrings<T>(IEnumerable<T> values)
    {
        return values.Select(v => v.ToString()).ToArray();
    }

    public interface IStorage
    {
        Type Type { get; }
        
        Dictionary<object, string> Values { get; }
    }

    public class Storage<T> : IStorage
    {
        public Type Type { get; }
        public Dictionary<object, string> Values { get; }

        public Storage(Dictionary<T, string> values)
        {
            Type = typeof(T);
            Values = values.ToDictionary(kvp => (object) kvp.Key, kvp => kvp.Value);
        }
    }
}