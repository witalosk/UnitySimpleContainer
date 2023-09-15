using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySimpleContainer
{
    [Flags]
    public enum BindType
    {
        Interfaces = 1 << 0,
        BaseTypes = 1 << 1,
        Self = 1 << 2
    }
    
    [DefaultExecutionOrder(-9999)]
    public class ObjectContainerBase : MonoBehaviour
    {
        public IContainer Container { get; } = new Container();

        [SerializeField]
        protected BindType _bindType = BindType.Interfaces;

        [SerializeField]
        protected List<ManualBinderBase> _manualBinders;

        /// <summary>
        /// ManualBinderを実行してコンテナに登録する
        /// </summary>
        protected void BindManualBinders()
        {
            foreach (var manualBinder in _manualBinders)
            {
                manualBinder.BindObjects(Container);
            }
        }
        
        /// <summary>
        /// コンポーネントを指定されたBindTYpeに基づいてコンテナに登録
        /// </summary>
        protected void BindComponents(IEnumerable<Component> components)
        {
            foreach (var component in components)
            {
                if (component is IEnumerable) continue;
                    
                List<Type> targetTypes = new();
                if (_bindType.HasFlag(BindType.Interfaces))
                {
                    targetTypes.AddRange(component.GetType().GetInterfaces());
                }
                if (_bindType.HasFlag(BindType.BaseTypes))
                {
                    targetTypes.AddRange(GetBaseTypes(component.GetType()));
                }
                if (_bindType.HasFlag(BindType.Self))
                {
                    targetTypes.Add(component.GetType());
                }
                
                foreach (var type in targetTypes)
                {
                    Container.BindInstance(type, component);
                }
            }
        }
        
        /// <summary>
        /// すべてのベースクラスの型を取得する
        /// </summary>
        protected static IEnumerable<Type> GetBaseTypes(Type self)
        {
            for (var baseType = self.BaseType; baseType != null; baseType = baseType.BaseType)
            {
                yield return baseType;
            }
        } 
    }
}
