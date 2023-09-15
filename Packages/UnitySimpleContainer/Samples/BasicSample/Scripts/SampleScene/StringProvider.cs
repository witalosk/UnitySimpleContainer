using UnityEngine;

namespace UnitySimpleContainer.Sample
{
    public class StringProvider : MonoBehaviour, IStringProvider
    {
        public string Identifier => gameObject.name;
        public string String => _string;
        
        [SerializeField]
        private string _string = "Hello, World!";
    }
}
