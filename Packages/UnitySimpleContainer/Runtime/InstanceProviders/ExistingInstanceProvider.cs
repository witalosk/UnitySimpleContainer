namespace UnitySimpleContainer
{
    /// <summary>
    /// Provide instances that already exist
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