using UnityEngine;
namespace UnitySimpleContainer.Sample
{
    /// <summary>
    /// To register an instance of a pure C# class that is not a MonoBehaviour,
    /// create a ManualBinder and attach it to a SceneContainer or similar.
    /// </summary>
    public class SampleManualBinder : ManualBinderBase
    {
        public override void BindObjects(IBindOnlyContainer bindOnlyContainer)
        {
            bindOnlyContainer.BindInstance<IStringProvider>(new PureStringProvider());
            bindOnlyContainer.BindAsTransient<ISomeComponent, SomeComponent>();
            bindOnlyContainer.BindAsTransient<ILogger, SimpleLogger>();
        }
    }
}
