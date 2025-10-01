using UnityEngine;

namespace UnitySimpleContainer.Sample
{
    public class BarComponent : MonoBehaviour
    {
        private INumberProvider _numberProvider;
        
        [Inject]
        public void Construct(INumberProvider numberProvider)
        {
            _numberProvider = numberProvider;
        }

        private void Start()
        {
            Debug.Log($"Number Provider by Dynamically added component: {_numberProvider.Number}");
        }
    }
}
