using System;
using UnityEngine.Scripting;

namespace UnitySimpleContainer
{
    /// <summary>
    /// 注入する属性
    /// </summary>
#if UNITY_2018_4_OR_NEWER
    [JetBrains.Annotations.MeansImplicitUse(
        JetBrains.Annotations.ImplicitUseKindFlags.Access |
        JetBrains.Annotations.ImplicitUseKindFlags.Assign |
        JetBrains.Annotations.ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
#endif
    [AttributeUsage(AttributeTargets.Method)]
    public class InjectAttribute : PreserveAttribute { }
    
    /// <summary>
    /// Nullを許容する属性
    /// </summary>
#if UNITY_2018_4_OR_NEWER
    [JetBrains.Annotations.MeansImplicitUse(
        JetBrains.Annotations.ImplicitUseKindFlags.Access |
        JetBrains.Annotations.ImplicitUseKindFlags.Assign |
        JetBrains.Annotations.ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
#endif
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NullableAttribute : PreserveAttribute { }
    
    /// <summary>
    /// ランタイムではないときにNullを許容する属性
    /// </summary>
#if UNITY_2018_4_OR_NEWER
    [JetBrains.Annotations.MeansImplicitUse(
        JetBrains.Annotations.ImplicitUseKindFlags.Access |
        JetBrains.Annotations.ImplicitUseKindFlags.Assign |
        JetBrains.Annotations.ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
#endif
    [AttributeUsage(AttributeTargets.Parameter)]
    public class RuntimeOnlyAttribute : PreserveAttribute { }
}
