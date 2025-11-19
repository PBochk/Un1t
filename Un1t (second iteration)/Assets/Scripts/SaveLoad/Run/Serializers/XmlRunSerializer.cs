public class XmlRunSerializer : RunSerializer
{
    public override string FileExtension => "xml";
    public override void Save(GameRunState gameRunState, string path)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryLoad(out GameRunState gameRunState, string path)
    {
        throw new System.NotImplementedException();
    }
}