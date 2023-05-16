using System;
using UnityEngine;

namespace Assets.Scripts.Helpers.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Knowledge", menuName = "Data/InfoData")]
    public class InfoData : ScriptableObject
    {
        [SerializeField]
        private string key;
    }
}