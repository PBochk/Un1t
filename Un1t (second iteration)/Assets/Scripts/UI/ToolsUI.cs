using TMPro;
using UnityEngine;

public class ToolsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText; 
    public TMP_Text AmmoText => ammoText; // make string public instead?
}
