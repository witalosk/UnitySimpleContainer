using UnityEngine;

namespace UnitySimpleContainer.Sample
{
    public class RandomNumberProvider : MonoBehaviour, INumberProvider
    {
        public string Identifier => gameObject.name;
        public int Number => Random.Range(_range.x, _range.y);
        
        [SerializeField]
        private Vector2Int _range = new(0, 100);
    }
}
