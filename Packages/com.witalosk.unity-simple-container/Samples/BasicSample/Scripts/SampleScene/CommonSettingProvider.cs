using UnityEngine;

namespace UnitySimpleContainer.Sample
{
    public class CommonSettingProvider : MonoBehaviour, ICommonSettingProvider
    {
        public string SettingJson => "{\"name\":\"UnitySimpleContainer\", \"version\":\"1.0.0\"}";
    }
}
