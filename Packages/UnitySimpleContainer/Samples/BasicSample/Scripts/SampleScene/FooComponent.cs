using System.Collections.Generic;
using UnityEngine;

namespace UnitySimpleContainer.Sample
{
    public class FooComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject _barComponentPrefab;
        
        private INumberProvider _numberProvider;
        private IEnumerable<IStringProvider> _stringProviders;
        private ITextureProvider _textureProvider;
        private IContainer _container;
        
        [Inject]
        public void Construct(INumberProvider numberProvider, IEnumerable<IStringProvider> stringProviders,
            [Nullable] ITextureProvider textureProvider, IContainer container)
        {
            _numberProvider = numberProvider;
            _stringProviders = stringProviders;
            _textureProvider = textureProvider;
            _container = container;
        }
    
        private void Awake()
        {
            // Number Provider
            Debug.Log($"{_numberProvider.Identifier}: {_numberProvider.Number}");
            
            // String Providers
            // IEnumerable<T> is used to get all the registered T type objects.
            foreach (var stringProvider in _stringProviders)
            {
                Debug.Log($"{stringProvider.Identifier}: {stringProvider.String}");
            }
            
            // Texture Provider
            // Nullable attribute is given in Construct() so that there is no warning even if the object is not registered to the container.
            Debug.Log(_textureProvider != null ? $"{_textureProvider.Identifier}: {_textureProvider.Texture}" : "Texture provider is not registered.");
            
            // Add a new string provider via container
            // The new string provider is registered to the container and injected.
            var newProvider = _container.Instantiate(_barComponentPrefab, transform);
            newProvider.name = "Added String Provider";
        }
    }
}

