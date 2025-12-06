using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSerializer : MonoBehaviour
{
    [SerializeField] private PlayerModelMB player;
    private XmlSerializer serializer;

    private void Start()
    {
        serializer = new XmlSerializer(typeof(PlayerSaveData));
    }

    public void SerializeAndPrint(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        var playerSaveData = player.PlayerModel.ToSaveData();
        Debug.Log("Serializing...");
        StringWriter writer = new();
        serializer.Serialize(writer, playerSaveData);
        Debug.Log(writer.ToString());
        writer.Close();
    }
}