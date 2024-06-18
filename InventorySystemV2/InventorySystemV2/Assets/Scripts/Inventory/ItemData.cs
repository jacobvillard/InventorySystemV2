using UnityEngine;

[CreateAssetMenu(menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public string id = "";
    public string desc = "";
    public Sprite img;
    public int width = 1;
    public int height = 1;
    public string type = "";

   
}
