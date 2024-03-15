using System;

namespace UnitySimpleContainer
{
    public interface IBindOnlyContainer
    {
        /// <summary>
        /// 既に存在するインスタンスを登録する
        /// </summary>
        public void BindInstance<T>(object obj);

        /// <summary>
        /// 既に存在するインスタンスを登録する
        /// </summary>
        public void BindInstance(Type registerType, object obj);

        /// <summary>
        /// InstanceProviderを登録する
        /// </summary>
        public void BindProvider(Type type, IInstanceProvider provider);
    }

    public interface IContainer : IBindOnlyContainer
    {
        /// <summary>
        /// 親コンテナ
        /// </summary>
        IContainer Parent { get; set; }

        /// <summary>
        /// コンテナをリセットする
        /// </summary>
        public void Clear();

        /// <summary>
        /// コンテナから対応する型のインスタンスを取得する
        /// </summary>
        public T Resolve<T>(bool nullable = false);

        /// <summary>
        /// コンテナから対応する型のインスタンスを取得する
        /// </summary>
        public object Resolve(Type type, bool nullable = false);

        /// <summary>
        /// オブジェクトにこのコンテナに登録されているインスタンスを注入する
        /// </summary>
        public void Inject(object instance);
    }
}
