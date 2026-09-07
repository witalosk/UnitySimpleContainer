using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace UnitySimpleContainer
{
    /// <summary>
    /// リフレクションの結果やコンパイル済みデリゲートを型ごとにキャッシュするクラス
    /// </summary>
    public static class ReflectionCache
    {
        public sealed class TargetParameterInfo
        {
            public readonly Type ParameterType;
            public readonly bool HasNullableAttribute;
            public readonly bool HasRuntimeOnlyAttribute;

            public TargetParameterInfo(ParameterInfo parameterInfo)
            {
                ParameterType = parameterInfo.ParameterType;
                HasNullableAttribute = parameterInfo.IsDefined(typeof(NullableAttribute), true);
                HasRuntimeOnlyAttribute = parameterInfo.IsDefined(typeof(RuntimeOnlyAttribute), true);
            }
        }

        public sealed class TargetMethodInfo
        {
            public readonly MethodInfo MethodInfo;
            public readonly TargetParameterInfo[] ParameterInfos;

            private Action<object, object[]> _invoker;

            public TargetMethodInfo(MethodInfo methodInfo)
            {
                MethodInfo = methodInfo;
                var parameters = methodInfo.GetParameters();
                ParameterInfos = new TargetParameterInfo[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    ParameterInfos[i] = new TargetParameterInfo(parameters[i]);
                }
            }

            public void Invoke(object instance, object[] values)
            {
                _invoker ??= CreateInvoker(MethodInfo);
                _invoker(instance, values);
            }

            /// <summary>
            /// MethodInfo.Invoke()の代わりに使用する、コンパイル済みデリゲートを生成する
            /// </summary>
            private static Action<object, object[]> CreateInvoker(MethodInfo methodInfo)
            {
                try
                {
                    var instanceParam = Expression.Parameter(typeof(object), "instance");
                    var argsParam = Expression.Parameter(typeof(object[]), "args");

                    var parameters = methodInfo.GetParameters();
                    var arguments = new Expression[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        arguments[i] = Expression.Convert(
                            Expression.ArrayIndex(argsParam, Expression.Constant(i)),
                            parameters[i].ParameterType);
                    }

                    var call = Expression.Call(
                        Expression.Convert(instanceParam, methodInfo.DeclaringType!),
                        methodInfo,
                        arguments);

                    return Expression.Lambda<Action<object, object[]>>(call, instanceParam, argsParam).Compile();
                }
                catch (Exception)
                {
                    // Expressionが利用できない環境ではリフレクション呼び出しにフォールバックする
                    return (instance, values) => methodInfo.Invoke(instance, values);
                }
            }
        }

        private static readonly ConcurrentDictionary<Type, TargetMethodInfo[]> InjectMethodCache = new();
        private static readonly ConcurrentDictionary<Type, Func<object>> FactoryCache = new();

        /// <summary>
        /// [Inject]が付いたメソッドの情報を取得する(型ごとにキャッシュされる)
        /// </summary>
        public static TargetMethodInfo[] GetInjectMethods(Type type)
        {
            return InjectMethodCache.GetOrAdd(type, CollectInjectMethods);
        }

        /// <summary>
        /// 引数なしコンストラクタのコンパイル済みファクトリを取得する(型ごとにキャッシュされる)
        /// </summary>
        public static Func<object> GetFactory(Type type)
        {
            return FactoryCache.GetOrAdd(type, CreateFactory);
        }

        /// <summary>
        /// キャッシュをクリアする
        /// </summary>
        public static void Clear()
        {
            InjectMethodCache.Clear();
            FactoryCache.Clear();
        }

        private static TargetMethodInfo[] CollectInjectMethods(Type type)
        {
            var targets = new List<TargetMethodInfo>();
            foreach (var methodInfo in type.GetRuntimeMethods())
            {
                if (methodInfo.IsDefined(typeof(InjectAttribute), true))
                {
                    targets.Add(new TargetMethodInfo(methodInfo));
                }
            }

            return targets.ToArray();
        }

        /// <summary>
        /// Activator.CreateInstance()の代わりに使用する、コンパイル済みファクトリを生成する
        /// </summary>
        private static Func<object> CreateFactory(Type type)
        {
            try
            {
                var newExpression = Expression.New(type);
                var body = type.IsValueType
                    ? Expression.Convert(newExpression, typeof(object))
                    : (Expression)newExpression;
                return Expression.Lambda<Func<object>>(body).Compile();
            }
            catch (Exception)
            {
                // Expressionが利用できない環境ではActivatorにフォールバックする
                return () => Activator.CreateInstance(type);
            }
        }
    }
}
