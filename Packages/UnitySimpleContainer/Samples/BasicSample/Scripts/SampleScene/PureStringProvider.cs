namespace UnitySimpleContainer.Sample
{
    public class PureStringProvider : IStringProvider
    {
        public string Identifier => "PureStringProvider";
        public string String => "Text from Pure C# Class";
        
        public override string ToString()
        {
            return $"{nameof(Identifier)}: {Identifier}, {nameof(String)}: {String}";
        }
    }
}
