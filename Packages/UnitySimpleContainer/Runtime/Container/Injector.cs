using UnityEngine;

namespace UnitySimpleContainer
{
    public static class Injector
    {
        /// <summary>
        /// [Inject]が付いたメソッドを持つオブジェクトに大して、コンテナから適切な型を取得し、メソッドを実行する
        /// </summary>
        public static void Inject(object instance, IContainer container)
        {
            // オブジェクトの中の[Inject]が付いたメソッドを探す(型ごとにキャッシュされる)
            var targets = ReflectionCache.GetInjectMethods(instance.GetType());

            if (targets.Length == 0) return;

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
                    bool nullable = targetParameter.HasNullableAttribute
                        || (!UnityEditor.EditorApplication.isPlaying && targetParameter.HasRuntimeOnlyAttribute);
#else
                    bool nullable = targetParameter.HasNullableAttribute;
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
                    methodInfo.Invoke(instance, values);
                }
                else
                {
                    Debug.LogWarning($"[Injector] Method '{instance.GetType().FullName}.{methodInfo.MethodInfo.Name}()' was not executed due to unresolved dependencies.");
                }
            }
        }
    }
}
