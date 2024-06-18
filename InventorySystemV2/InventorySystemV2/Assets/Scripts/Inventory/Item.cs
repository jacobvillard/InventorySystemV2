using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData itemdata;

    //Item Variables
    public bool rotated = false;
    public int orginalPlacementPosX;
    public int orginalPlacementPosY;

    internal void Set(ItemData itemdata)
    {
        this.itemdata = itemdata;
        GetComponent<Image>().sprite = itemdata.img;

        Vector2 size = new Vector2();
        size.x = itemdata.height * GridInv.TileSizeHeight;
        size.y = itemdata.width * GridInv.TileSizeWidth;
        GetComponent<RectTransform>().sizeDelta = size;
        this.transform.localScale = new Vector3(1f, 1f, 1f);

    }

}
