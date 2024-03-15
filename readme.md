# UnitySimpleContainer
A simple dependency injection container for Unity3D.

### Implemented Features
- Container that operates within a Scene
- Component can be registered and resolved as Interface, etc.
- OptionalInjection (Default value is entered if there is no target instance in the container by adding the `[Nullable]` attribute.)
- Method Injection
- Dependency injection into dynamically generated GameObjects. (IContainer.Instantiate())

## Installation
1. Open the Unity Package Manager
2. Click the + button
3. Select "Add package from git URL..."
4. Enter `https://github.com/witalosk/UnitySimpleContainer.git?path=Packages/com.witalosk.unity-simple-container`
   5. **[Caution]** The URL (above) is changed from v1.1.0. If you are using v1.0.0, please change the URL!!

## Usage
1. Create a `SceneContainer` in the scene.
2. Write a method with `[Inject]` attribute in the code of the injector.

```c#
    public class BarComponent : MonoBehaviour
    {
        [SerializeField]
        private BarComponent _prefab;

        private float _timer = 0f;
        private bool _isInstantiated = false;
        
        private IFooComponent _fooComponent;
        private IEnumerable<IParams> _paramsObjects;
        private IContainer _container;
        
        [Inject]
        public void Construct([Nullable] IFooComponent fooComponent, IEnumerable<IParams> paramsObjects, IContainer container)
        {
            _fooComponent = fooComponent;
            _paramsObjects = paramsObjects;
            _container = container;
        }

        private void Start()
        {
            if (_fooComponent != null)
            {
                Debug.Log(_fooComponent.Test());
            }
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (!_isInstantiated && _timer > 5f)
            {
                _container.Instantiate(_prefab);
                _isInstantiated = true;
            }
        }
    }
```
- If the `[Nullable]` attribute is given to the parameter, an exception will not be thrown if the target type is not registered in the container.
- if the `RuntimeOnly` attribute is given to the parameter, the parameter will be injected only at runtime. (No error in the editor)
