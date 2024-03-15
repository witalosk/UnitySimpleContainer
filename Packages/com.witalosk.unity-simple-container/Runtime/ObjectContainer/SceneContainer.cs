using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnitySimpleContainer
{
    /// <summary>
    /// シーン上にある全てのオブジェクトをコンテナに登録して
    /// 全てのオブジェクトにオブジェクトをを注入するコンテナ
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    public class SceneContainer : ObjectContainerBase
    {
        [SerializeField]
        private string _projectContainerPrefabName = "ProjectContainer";
        
        [SerializeField]
        private bool _isIncludeInactive = true;

        private void Awake()
        {
            // 親コンテナが存在しない場合はProjectContainerを親にする
            if (Container.Parent == null)
            {
                var projectContainer = GameObject.Find(_projectContainerPrefabName)?.GetComponent<ProjectContainer>();
                if (projectContainer is null)
                {
                    // 存在しない場合はResourcesから探す
                    var prefab = Resources.Load<GameObject>(_projectContainerPrefabName);
                    if (prefab is null)
                    {
                        throw new NullReferenceException("ProjectContainerが見つかりませんでした. ProjectContainerのPrefabをResources以下に作成してください。");
                    }
                    
                    var pcObj = Instantiate(prefab);
                    pcObj.name = _projectContainerPrefabName;
                    projectContainer = pcObj.GetComponent<ProjectContainer>();
                }
                Container.Parent = projectContainer.Container;
            }

            BindAndInjectToSceneObjects();
        }

        /// <summary>
        /// シーンの全てのオブジェクトをコンテナに登録して
        /// シーンの全てのオブジェクトに対してオブジェクトを注入する
        /// </summary>
        public void BindAndInjectToSceneObjects()
        {
            // コンテナをリセットする
            Container.Clear();
            
            BindManualBinders();
            
            // シーンにあるComponentを全て取得
            string scenePath = gameObject.scene.path;
            List<Component> targetComponents = FindObjectsOfType<Component>(_isIncludeInactive).Where(c => c.gameObject.scene.path == scenePath).ToList();

            // コンテナに登録する
            Container.BindInstance<IContainer>(Container);
            BindComponents(targetComponents);

            // 登録されたComponentに対してInjectを行う
            foreach (var component in targetComponents)
            {
                Container.Inject(component);
            }
        }
    }
    
    #if UNITY_EDITOR
    
    /// <summary>
    /// delayCall時にSceneContainerを探してBindAndInjectToSceneObjectsを呼ぶ
    /// </summary>
    [InitializeOnLoad]
    public static class EditorContainerUtility
    {
        static EditorContainerUtility()
        {
            EditorApplication.delayCall += () =>
            {
                foreach (var sceneContainer in UnityEngine.Object.FindObjectsOfType<SceneContainer>())
                {
                    sceneContainer.BindAndInjectToSceneObjects();
                }
            };
        }

        public static void BindAndInjectToSceneObjects()
        {
            foreach (var sceneContainer in UnityEngine.Object.FindObjectsOfType<SceneContainer>())
            {
                sceneContainer.BindAndInjectToSceneObjects();
            }
        }
    }

    [CustomEditor(typeof(SceneContainer)), CanEditMultipleObjects]
    public class SceneContainerEditor : Editor
    {
        private SceneContainer _sceneContainer;
        
        public void OnEnable()
        {
            _sceneContainer = target as SceneContainer;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10f);
            
            if (GUILayout.Button("Bind and Inject Manually"))
            {
                _sceneContainer.BindAndInjectToSceneObjects();
                Debug.Log("Binded and Injected Manually");
            }
        }
    }
    #endif
}
