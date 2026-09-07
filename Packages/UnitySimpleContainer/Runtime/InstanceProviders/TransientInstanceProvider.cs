using System;
using UnityEngine;

namespace UnitySimpleContainer
{
    /// <summary>
    /// Provide instances that are created each time
    /// If the type is a MonoBehaviour, it will be created as a GameObject
    /// </summary>
    public class TransientInstanceProvider : IInstanceProvider
    {
        public bool IsMonoBehaviour { get; } = false;

        private readonly Type _type;
        private Func<object> _factory;

        public TransientInstanceProvider(Type type)
        {
            _type = type;

            if (_type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                IsMonoBehaviour = true;
            }
        }

        public object GetInstance()
        {
            if (IsMonoBehaviour)
            {
                var go = new GameObject(_type.Name);
                return go.AddComponent(_type);
            }

            _factory ??= ReflectionCache.GetFactory(_type);
            return _factory();
        }

        public Component AddComponent(GameObject parent)
        {
            if (IsMonoBehaviour)
            {
                return parent.AddComponent(_type);
            }

            throw new InvalidOperationException("This type is not a MonoBehaviour");
        }
    }
}
