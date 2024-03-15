namespace UnitySimpleContainer
{
    /// <summary>
    /// 既に存在するインスタンスを提供する
    /// </summary>
    public class ExistingInstanceProvider : IInstanceProvider
    {
        private readonly object _instance;
        
        public ExistingInstanceProvider(object instance)
        {
            _instance = instance;
        }
        
        public object GetInstance()
        {
            return _instance;
        }
    }
}