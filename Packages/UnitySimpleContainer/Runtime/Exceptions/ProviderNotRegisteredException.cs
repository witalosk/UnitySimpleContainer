using System;

namespace UnitySimpleContainer
{
    /// <summary>
    /// コンテナにプロバイダが登録されていない場合の例外
    /// </summary>
    [Serializable]
    public class ProviderNotRegisteredException : System.Exception
    {
        public ProviderNotRegisteredException()
        {
        }

        public ProviderNotRegisteredException(string message) : base(message)
        {
        }

        public ProviderNotRegisteredException(string message, System.Exception inner) : base(message, inner)
        {
        }

        protected ProviderNotRegisteredException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

    }
}
