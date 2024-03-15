using UnityEngine;

namespace UnitySimpleContainer
{
    public static class ContainerExtension
    {
        /// <summary>
        /// Prefabをインスタンス化してコンテナに登録、Injectする
        /// </summary>
        /// <param name="container">コンテナ</param>
        /// <param name="prefab">インスタンス化するPrefab</param>
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
        /// prefabをインスタンス化してコンテナに登録、Injectする
        /// </summary>
        /// <param name="container">コンテナ</param>
        /// <param name="prefab">インスタンス化するPrefab</param>
        /// <param name="parent">親Transform</param>
        /// <param name="worldPositionStays">親Objectを割り当てるとき、新しいObjectをワールド空間に直接配置するときはtrue。falseを渡すと、Objectの位置が新しい親から相対的に設定される。</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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
        /// 対象のGameObjectにコンポーネントがあればInjectする
        /// (再帰的に実行される)
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
