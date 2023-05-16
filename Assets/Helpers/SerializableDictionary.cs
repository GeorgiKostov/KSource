using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    abstract public class SerializableDictionary<K, V> : ISerializationCallbackReceiver {
        [SerializeField]
        private K[] keys;
        [SerializeField]
        private V[] values;
 
        public Dictionary<K, V> dictionary;
 
        static public T New<T>() where T : SerializableDictionary<K, V>, new() {
            var result = new T();
            result.dictionary = new Dictionary<K, V>();
            return result;
        }
 
        public void OnAfterDeserialize() {
            var c = this.keys.Length;
            this.dictionary = new Dictionary<K, V>(c);
            for (int i = 0; i < c; i++) {
                this.dictionary[this.keys[i]] = this.values[i];
            }
            this.keys = null;
            this.values = null;
        }
 
        public void OnBeforeSerialize() {
            var c = this.dictionary.Count;
            this.keys = new K[c];
            this.values = new V[c];
            int i = 0;
            using (var e = this.dictionary.GetEnumerator())
                while (e.MoveNext()) {
                    var kvp = e.Current;
                    this.keys[i] = kvp.Key;
                    this.values[i] = kvp.Value;
                    i++;
                }
        }
    }
}
