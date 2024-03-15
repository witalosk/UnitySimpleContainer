using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnitySimpleContainer
{
    /// <summary>
    /// オブジェクトの型とそのInstanceProviderの辞書をもつオブジェクト
    /// </summary>
    public class Container : IContainer
    {
        public IContainer Parent { get; set; } = null;
        private readonly ConcurrentDictionary<Type, IInstanceProvider> _containerData = new();

        /// <summary>
        /// コンテナをリセットする
        /// </summary>
        public void Clear()
        {
            _containerData.Clear();
        }

        /// <summary>
        /// コンテナから対応する型のインスタンスを取得する
        /// </summary>
        public T Resolve<T>(bool nullable = false)
        {
            return (T)Resolve(typeof(T), nullable);
        }

        /// <summary>
        /// コンテナから対応する型のインスタンスを取得する
        /// </summary>
        public object Resolve(Type type, bool nullable = false)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                object obj = null;
                if (_containerData.TryGetValue(type, out IInstanceProvider provider))
                {
                    obj = provider.GetInstance();
                }

                // 親に登録されていない場合, コンテナに登録されていない型のIEnumerableを要求された場合は、空の配列を返す
                if (Parent?.Resolve(type) is not IList { Count: > 0 } parentArr)
                    return obj ?? Array.CreateInstance(type.GenericTypeArguments[0], 0);

                // 親にのみ登録されている場合
                if (obj is not IList { Count: > 0 } arr) return parentArr;

                // 親と子、両方に登録されている場合、合体して返す
                Array result = Array.CreateInstance(type.GenericTypeArguments[0], arr.Count + parentArr.Count);
                arr.CopyTo(result, 0);
                parentArr.CopyTo(result, arr.Count);
                return result;
            }

            if (_containerData.TryGetValue(type, out IInstanceProvider value))
            {
                return value.GetInstance();
            }

            if (Parent is not null)
            {
                return Parent.Resolve(type, nullable);
            }

            if (!nullable)
            {
                // [Nullable]属性が付いていない場合は警告を出力する
                Debug.LogAssertion($"\"{type}\" が要求されましたが、コンテナに対応する型の登録がありません。");
            }

            return GetDefault(type);
        }

        /// <summary>
        /// オブジェクトにこのコンテナに登録されているインスタンスを注入する
        /// </summary>
        /// <param name="instance"></param>
        public void Inject(object instance)
        {
            Injector.Inject(instance, this);
        }

        /// <summary>
        /// 既に存在するインスタンスを登録する
        /// </summary>
        public void BindInstance<T>(object obj)
        {
            BindProvider(typeof(T), new ExistingInstanceProvider(obj));
        }

        /// <summary>
        /// 既に存在するインスタンスを登録する
        /// </summary>
        public void BindInstance(Type registerType, object obj)
        {
            BindProvider(registerType, new ExistingInstanceProvider(obj));
        }

        /// <summary>
        /// InstanceProviderを登録する
        /// </summary>
        public void BindProvider(Type type, IInstanceProvider provider)
        {
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                Debug.LogAssertion($"リスト型のオブジェクトは登録できません。 適当なクラスでラップしてから登録してください。 (Type: {type}) {provider.GetInstance()}");
            }
            
            Type collectionType = typeof(IEnumerable<>).MakeGenericType(type);
            if (_containerData.ContainsKey(type))
            {
                // そのタイプ自体を上書き
                _containerData[type] = provider;

                // そのタイプのリストに追加
                ((CollectionInstanceProvider)_containerData[collectionType]).AddProvider(provider);
            }
            else
            {
                // そのタイプ自体を登録
                _containerData.TryAdd(type, provider);

                // そのタイプのリストを登録
                _containerData.TryAdd(collectionType, new CollectionInstanceProvider(type, provider));
            }
        }

        private static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
