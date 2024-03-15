using UnityEngine;

namespace UnitySimpleContainer.Sample
{
    public interface IIdentifier
    {
        string Identifier { get; }
    }

    public interface INumberProvider : IIdentifier
    {
        int Number { get; }
    }
    
    public interface IStringProvider : IIdentifier
    {
        string String { get; }
    }
    
    public interface ITextureProvider : IIdentifier
    {
        Texture Texture { get; }
    }

    public interface ICommonSettingProvider
    {
        string SettingJson { get; }
    }
}
