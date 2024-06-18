using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///Manages the inventory grid for an inventory
/// </summary>
public class GridInv : MonoBehaviour
{
    [Header("Grid Size")]
    public int width;
    public int height;

    public static float TileSizeWidth;
    public static float TileSizeHeight;

    [Header("Tile Prefab")]
    [SerializeField] private GameObject tile;
    [SerializeField] private Vector3 scale;

    public Item[,] inventoryItemSlot;

    RectTransform rectTransform;

    public bool specialItemSlot = false;//When enabled no placement checks are performed

   

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        CreateGrid(width, height);

        //Tile size, take from the gridlayout group
        TileSizeWidth = GetComponent<GridLayoutGroup>().cellSize.x;
        TileSizeHeight = GetComponent<GridLayoutGroup>().cellSize.y;
    }

    private void Start()
    {
        inventoryItemSlot = new Item[height, width];//Initalies item array
        Vector2 size = new Vector2(height * (TileSizeHeight), width * (TileSizeWidth));//Sets the object size correctly
        this.gameObject.GetComponent<RectTransform>().sizeDelta = size;

        GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
        Destroy(gridLayoutGroup, 0.5f);//Destroys the gridlayoutgroup so items can be instantiated under this gameobject
    }

    /// <summary>
    /// Creates a visualised grid for the user interface
    /// </summary>
    /// <param name="width">Width of the grid</param>
    /// <param name="height">Height of the grid</param>
    public void CreateGrid(int width, int height)
    {

        this.width = width;
        this.height = height;
        for(int x = 0; x< width; x++)//For each x,y spawn a tile
        {
            for(int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tile);
                spawnedTile.transform.SetParent(this.transform);
                spawnedTile.GetComponent<RectTransform>().sizeDelta = new Vector2(TileSizeWidth, TileSizeHeight);
                spawnedTile.transform.localScale = scale;
                spawnedTile.name = $"{y},{x}";

            }
        }

    }

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPostion = new Vector2Int();
    /// <summary>
    /// Gets grid postion from mouse position
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPostion.x = (int)(positionOnTheGrid.x / (TileSizeWidth*1.8));//Multiplied by canvas size
        tileGridPostion.y = (int)(positionOnTheGrid.y / (TileSizeHeight*1.8));
        if (tileGridPostion.x > height - 1) tileGridPostion.x = height - 1;
        if (tileGridPostion.y > width - 1) tileGridPostion.y = width - 1;
        return tileGridPostion;
    }

    /// <summary>
    /// Adds item to inventory
    /// </summary>
    /// <param name="item">The item being placed</param>
    /// <param name="x">The x of the grid clicked</param>
    /// <param name="y">The y of the grid clicked</param>
    public void AddItem(Item item, int x, int y)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.gameObject.GetComponent<RectTransform>());


        y = Mathf.Abs(y);//Make sure Y is positive
        try
        {
            if (!specialItemSlot)
            {
                for (int w = 0; w < item.itemdata.width; w++)//For the size of the item add it to the grid
                {
                    for (int h = 0; h < item.itemdata.height; h++)
                    {
                        if (!item.GetComponent<Item>().rotated) inventoryItemSlot[x + h, y + w] = item;
                        else inventoryItemSlot[x + w, y + h] = item;
                    }
                }

                //The orginal x and y position stored for when the item is removed
                item.orginalPlacementPosX = x;
                item.orginalPlacementPosY = y;
            }
            else
            {
                inventoryItemSlot[x, y] = item;
            }
           
        }
        catch
        {
            Debug.Log("Failed to add " + item+ " at:" + x + "," + y);
        }

        //Places the item slightly off from the top left
        var tileSizeWidth = TileSizeWidth;
        var tileSizeHeight = TileSizeHeight;
        Vector2 position = new Vector2();
        position.x = ((x * tileSizeHeight) + item.itemdata.height * item.itemdata.height/2)- ((tileSizeHeight / 10) * item.itemdata.height / 2);
        position.y = (-((y * tileSizeWidth) + item.itemdata.width * item.itemdata.width/2)) + ((tileSizeWidth / 10) * item.itemdata.width / 2);
      

        rectTransform.localPosition = position;
    }

    /// <summary>
    /// This function returns a bool if the item can be placed or not
    /// </summary>
    /// <param name="item">The item</param>
    /// <param name="x">The x of the grid clicked</param>
    /// <param name="y">The y of the grid clicked</param>
    /// <returns></returns>
    public bool AcceptablePlacementCeck(Item item, int x, int y)
    {
        bool acceptable = true;
        if (specialItemSlot)//If it is a special inventory
        {
            if(inventoryItemSlot[x, y] == null) return acceptable;//return true if there is no item at x,y
            else //return false if there is
            {
                acceptable = false;
                return acceptable;
            }
        }
            

        for (int w = 0; w < item.itemdata.width; w++)//For each tile that it will be placed on check there is no existing item and that it it is not out of bounds
        {
            for (int h = 0; h < item.itemdata.height; h++)
            {
                try
                {
                    if (!item.GetComponent<Item>().rotated)//Swaps width and height if object is rotated
                    {
                        if (inventoryItemSlot[x + h, y + w] != false) acceptable = false;//checks if the the tile is null
                    }
                    else
                    {
                        if (inventoryItemSlot[x + w, y + h] != false) acceptable = false;
                    }
                
                }
                catch
                {
                    acceptable = false;//Out of bounds
                }
               

            }
        }
        return acceptable;
    }

    /// <summary>
    /// Changes the item to red if the place is invalid
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public IEnumerator OutputInvalidPlacement(Item item)
    {
        item.GetComponent<Image>().color = new Color32(100, 0, 0, 255);//Set the colour to red
        yield return new WaitForSeconds(0.05f);
        item.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

    }

    /// <summary>
    /// Removes Item from inventory, similar to the placement except the reverse
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public void RemoveItem(int x, int y)
    {
        try
        {
            y = Mathf.Abs(y);
            Item item = inventoryItemSlot[x, y];
            if (!specialItemSlot)
            {
                x = item.orginalPlacementPosX;//Gets the orginal placement pos
                y = item.orginalPlacementPosY;
                for (int w = 0; w < item.itemdata.width; w++)
                {
                    for (int h = 0; h < item.itemdata.height; h++)
                    {
                        if (!item.GetComponent<Item>().rotated) inventoryItemSlot[x + h, y + w] = null;//Sets the tile to null
                        else inventoryItemSlot[x + w, y + h] = null;
                    }
                }
            }
            else
            {
                inventoryItemSlot[x, y] = null;
            }
            
          
          
           
        }
        catch
        {
            Debug.Log("Failed to destroy item at:" + x + "," + y);

        }
    }

    /// <summary>
    /// Return the item at a position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Item GetItem(int x, int y)
    {
        Item item = inventoryItemSlot[x, y];
        return item;
    }

  


}
