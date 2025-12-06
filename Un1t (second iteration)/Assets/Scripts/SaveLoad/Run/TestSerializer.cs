using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class TestSerializer : MonoBehaviour
{
    private XmlSerializer serializer;
    private GameRunState runState;

    private void Start()
    {
        var player = FindFirstObjectByType<PlayerModelMB>();
        var runState = new GameRunState(player.PlayerModel);
        serializer = new XmlSerializer(typeof(GameRunState));
    }

    public void SerializeAndPrint()
    {
        Debug.Log("Serializing...");
        StringWriter writer = new();
        serializer.Serialize(writer, runState);
        Debug.Log(writer.ToString());
        writer.Close();
    }
}