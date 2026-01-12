using System.IO;
using System.Xml.Serialization;

public class XMLGameSerializer : GameSerializer
{
    private static XmlSerializer serializer = new XmlSerializer(typeof(GameState));
    private string path;

    public XMLGameSerializer(string filePath)
    {
        path = filePath;
    }

    public override void Save(GameState gameState)
    {
        TextWriter writer = new StreamWriter(path);
        serializer.Serialize(writer, gameState);
        writer.Close();
    }

    public override GameState Load()
    {
        var file = File.OpenRead(path);
        var gameState = serializer.Deserialize(file);
        file.Close();
        return gameState as GameState;
    }
}