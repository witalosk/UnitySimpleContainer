using System;

namespace UnitySimpleContainer
{
    public interface IBindOnlyContainer
    {
        /// <summary>
        /// Register a type as transient (new instance every time)
        /// </summary>
        public void BindAsTransient<TRegister, TConcrete>();
        
        /// <summary>
        /// Register a type as transient (new instance every time)
        /// </summary>
        public void BindAsTransient(Type registerType, Type concreteType);
        
        /// <summary>
        /// Register an instance that already exists
        /// </summary>
        public void BindInstance<T>(object obj);

        /// <summary>
        /// Register an instance that already exists
        /// </summary>
        public void BindInstance(Type registerType, object obj);

        /// <summary>
        /// Register InstanceProvider
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
        /// プロバイダを解決する
        /// </summary>
        public IInstanceProvider ResolveProvider(Type type);

        /// <summary>
        /// オブジェクトにこのコンテナに登録されているインスタンスを注入する
        /// </summary>
        public void Inject(object instance);
    }
}
