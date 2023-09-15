using UnityEngine;

namespace UnitySimpleContainer
{
    /// <summary>
    /// シーンにあるオブジェクト以外をコンテナにバインドしたいときに使う
    /// </summary>
    public abstract class ManualBinderBase : MonoBehaviour
    {
        /// <summary>
        /// オブジェクトをバインド
        /// </summary>
        public abstract void BindObjects(IBindOnlyContainer bindOnlyContainer);
    }
}
