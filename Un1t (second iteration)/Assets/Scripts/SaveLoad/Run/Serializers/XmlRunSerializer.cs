using System.IO;
using System.Xml.Serialization;

public class XmlRunSerializer : RunSerializer
{
    private static XmlSerializer serializer = new XmlSerializer(typeof(GameRunState));
    public override string FileExtension => "xml";
    public override void Save(GameRunState gameRunState, string path)
    {
        TextWriter writer = new StreamWriter(path);
        serializer.Serialize(writer, gameRunState);
        writer.Close();
    }

    public override bool TryLoad(out GameRunState gameRunState, string path)
    {
        gameRunState = null;
        if (!File.Exists(path)) return false;
        TextReader reader = new StreamReader(path);
        gameRunState = (GameRunState)serializer.Deserialize(reader);
        reader.Close();
        return true;
    }
}