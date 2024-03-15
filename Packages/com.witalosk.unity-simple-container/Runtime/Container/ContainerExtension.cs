using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnitySimpleContainer
{
    public static class ContainerExtension
    {
        /// <summary>
        /// Resolve and add component to GameObject
        /// Works only if registered as MonoBehaviour with TransientInstanceProvider.
        /// </summary>
        public static Component ResolveAndAddComponent<T>(this IContainer container, GameObject gameObject)
        {
            var provider = container.ResolveProvider(typeof(T));
            
            if (provider is TransientInstanceProvider { IsMonoBehaviour: true } transientProvider)
            {
                return transientProvider.AddComponent(gameObject);
            }
            
            throw new InvalidOperationException("This type is not a MonoBehaviour");
        }

        /// <summary>
        /// Instantiate Prefab, register to container, Inject
        /// </summary>
        /// <param name="container">Container</param>
        /// <param name="prefab">Target prefab to instantiate</param>
        public static T Instantiate<T>(this IContainer container, T prefab)
            where T : Object
        {
            var instance = Object.Instantiate(prefab);
            
            if (instance is GameObject gameObject)
            {
                container.InjectToGameObject(gameObject);
            }
            else
            {
                container.Inject(instance);                
            }
            
            return instance;
        }
        
        /// <summary>
        /// Instantiate Prefab, register to container, Inject
        /// </summary>
        /// <param name="container">Container</param>
        /// <param name="prefab">Target prefab to instantiate</param>
        /// <param name="parent">Parent Transform</param>
        /// <param name="worldPositionStays">When assigning a parentObject, true if the new Object is placed directly in world space; passing false sets the Object's position relative to its new parent.</param>
        public static T Instantiate<T>(this IContainer container, T prefab, Transform parent, bool worldPositionStays = false)
            where T : Object
        {
            var instance = Object.Instantiate(prefab, parent, worldPositionStays);

            if (instance is GameObject gameObject)
            {
                container.InjectToGameObject(gameObject);
            }
            else
            {
                container.Inject(instance);                
            }
            
            return instance;
        }
        
        /// <summary>
        /// If the target GameObject has a component, inject it.
        /// (executed recursively)
        /// </summary>
        public static void InjectToGameObject(this IContainer container, GameObject gameObject)
        {
            InjectGameObjectRecursive(container, gameObject);
        }
        
        private static void InjectGameObjectRecursive(IContainer container, GameObject current)
        {
            if (current == null) return;
            
            var components =  current.GetComponents<Component>();
            foreach (var component in components)
            {
                container.Inject(component);
            }

            var transform = current.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                InjectGameObjectRecursive(container, child.gameObject);
            }
        }
        
    }
}
