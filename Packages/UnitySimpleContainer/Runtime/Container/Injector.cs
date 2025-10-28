using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UnitySimpleContainer
{
    public static class Injector
    {
        public sealed class TargetMethodInfo
        {
            public readonly MethodInfo MethodInfo;
            public readonly ParameterInfo[] ParameterInfos;

            public TargetMethodInfo(MethodInfo methodInfo)
            {
                MethodInfo = methodInfo;
                ParameterInfos = methodInfo.GetParameters();
            }
        }

        /// <summary>
        /// [Inject]が付いたメソッドを持つオブジェクトに大して、コンテナから適切な型を取得し、メソッドを実行する
        /// </summary>
        public static void Inject(object instance, IContainer container)
        {
            // オブジェクトの中の[Inject]が付いたメソッドを探す
            var targets = new List<TargetMethodInfo>();
            foreach (var methodInfo in instance.GetType().GetRuntimeMethods())
            {
                if (methodInfo.IsDefined(typeof(InjectAttribute), true))
                {
                    targets.Add(new TargetMethodInfo(methodInfo));
                }
            }

            if (targets is not { Count: > 0 }) return;

            // [Inject]が付いたメソッドの引数にオブジェクトを設定し、メソッドを実行する
            foreach (var methodInfo in targets)
            {
                var methodExecutable = true;

                // 引数情報とそれに対するインスタンスオブジェクトの配列
                var parameters = methodInfo.ParameterInfos;
                object[] values = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    var targetParameter = parameters[i];

#if UNITY_EDITOR
                    bool nullable = targetParameter.IsDefined(typeof(NullableAttribute), true)
                        || (!UnityEditor.EditorApplication.isPlaying && targetParameter.IsDefined(typeof(RuntimeOnlyAttribute), true));
#else
                    bool nullable = targetParameter.IsDefined(typeof(NullableAttribute), true);
#endif

                    // コンテナからインスタンスを取得する
                    try
                    {
                        values[i] = container.Resolve(targetParameter.ParameterType, nullable);
                    }
                    catch (ProviderNotRegisteredException e)
                    {
                        Debug.LogError($"[Injector] Injection failed. Provider not registered for type {targetParameter.ParameterType} in method {instance.GetType().FullName}.{methodInfo.MethodInfo.Name}.\n{e}");
                        methodExecutable = false;
                    }
                }

                // メソッドを実行する
                if (methodExecutable)
                {
                    methodInfo.MethodInfo.Invoke(instance, values);
                }
                else
                {
                    Debug.LogWarning($"[Injector] Method '{instance.GetType().FullName}.{methodInfo.MethodInfo.Name}()' was not executed due to unresolved dependencies.");
                }
            }
        }
    }
}
