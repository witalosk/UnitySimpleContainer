using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnitySimpleContainer
{
    /// <summary>
    /// 登録したプレハブを生成するためのコンテナ
    /// プロジェクトで共通でSceneContainerの親になる
    /// PrefabはDontDestroyOnLoadになる
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    public class ProjectContainer : ObjectContainerBase
    {
        [SerializeField]
        private List<GameObject> _prefabs = new();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            InstantiateAndBindPrefab();
        }

        /// <summary>
        /// 登録されたPrefabをインスタンスにした↑で
        /// シーンの全てのオブジェクトに対してオブジェクトを注入する
        /// </summary>
        public void InstantiateAndBindPrefab()
        {
            // コンテナをリセットする
            Container.Clear();
            Container.Parent = null;
            
            BindManualBinders();
            
            List<Component> targetComponents = new();
            
            // コンテナに登録する
            Container.BindInstance<IContainer>(Container);
            foreach (var prefab in _prefabs)
            {
                var go = Instantiate(prefab, transform);
                go.name = prefab.name;

                var components = go.GetComponentsInChildren<Component>();
                BindComponents(components);
                targetComponents.AddRange(components);
            }
            
            // コンポーネントに対し注入
            foreach (var component in targetComponents)
            {
                Container.Inject(component);
            }
        }
    }
}
