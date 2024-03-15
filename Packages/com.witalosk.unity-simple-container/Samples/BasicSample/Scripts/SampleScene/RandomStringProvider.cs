using UnityEngine;

namespace UnitySimpleContainer.Sample
{
    public class RandomStringProvider : MonoBehaviour, IStringProvider
    {
        public string Identifier => gameObject.name;
        public string String => $"{_prefix}_{Random.Range(0, 100)}";
        
        [SerializeField]
        private string _prefix = "Goodbye, World.";
    }
}
