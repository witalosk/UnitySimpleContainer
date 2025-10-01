using UnityEngine;

namespace UnitySimpleContainer.Sample
{
    public class SimpleLogger : ILogger
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }
    }
}