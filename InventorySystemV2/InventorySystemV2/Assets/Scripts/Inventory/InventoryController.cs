using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Acts as a central controller for all inventories
/// </summary>
public class InventoryController : Singleton<InventoryController>
{
    [Header("Shows selected grid, DOES NOT require assignment")]
    public GridInv selectedItemGrid;

    [Header("Hotbar Inventory")]
    public GridInv primary;
    public GridInv hotbar;

    [Header("Player Reference")]
    [SerializeField] GameObject player;


    [Header("Inventory Reference for enabling/disabiling the entire UI and item spawning")]
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject chestUI;


    [Header("Item datas")]
    public List<ItemData> items;

    [Header("Item Prefab")]
    public GameObject itemPrefab;
    [SerializeField] GameObject itemToolTipPrefab;



    [Header("Keybinds")]
    [SerializeField] KeyCode generateItem = KeyCode.Q;  
    [SerializeField] KeyCode rotate = KeyCode.R;
    [SerializeField] KeyCode OpenCloseInventory = KeyCode.E;

    Item selectedItem;
    Item weaponItem;
    RectTransform rectTransform;

    private Item currentitem = null;
    private GameObject instaniatedToolTip = null;
    private void Update()
    {
        if (Input.GetKeyDown(OpenCloseInventory))
        {
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
                chestUI.SetActive(false);
            }
            else
            {
                inventory.SetActive(true);
            }
          
        }



        SetPlayerWeapon();

        if (inventory.activeInHierarchy)//Only run when the inventory is open
        {
            ItemIconDrag();
            

            //if (Input.GetKeyDown(generateItem)) CreateRandomItem();
            if (Input.GetKeyDown(rotate)) Rotate();


            if (selectedItemGrid == null) return;//Prevents the following code being run if there is no grid selected, reduces the need for validation

            if (Input.GetMouseButtonDown(0)) ClickItem();
            else if (Input.GetMouseButtonDown(1)) RightClickItem(selectedItemGrid.GetTileGridPosition(Input.mousePosition), selectedItemGrid);
            else ToolTip();


        }
        else DestroyToolTip();
    }

   
    /// <summary>
    /// Sets the weapon in the player hand sprite and scale
    /// </summary>
    private void SetPlayerWeapon()
    {
        Transform hand = player.transform.GetChild(0);
        SpriteRenderer weaponSprite = hand.GetComponent<SpriteRenderer>();

        try
        {
            if (!player.GetComponent<Controller>().removeItem)
            {
                if (player.GetComponent<Controller>().hotbar == 1) weaponItem = primary.GetItem(0, 0);//Hotbar Keybinds
                else if (player.GetComponent<Controller>().hotbar == 2) weaponItem = hotbar.GetItem(0, 0);
                else if (player.GetComponent<Controller>().hotbar == 3) weaponItem = hotbar.GetItem(1, 0);
                else if (player.GetComponent<Controller>().hotbar == 4) weaponItem = hotbar.GetItem(2, 0);
                else if (player.GetComponent<Controller>().hotbar == 5) weaponItem = hotbar.GetItem(3, 0);
                else if (player.GetComponent<Controller>().hotbar == 6) weaponItem = hotbar.GetItem(4, 0);
                weaponSprite.sprite = weaponItem.itemdata.img;//Set sprite and size
                hand.transform.localScale = new Vector3(0.1f * weaponItem.itemdata.width, 0.1f * weaponItem.itemdata.width, 0.1f);
                hand.transform.GetComponentInParent<Controller>().item = weaponItem;
            }
            else
            {
                if (player.GetComponent<Controller>().hotbar == 1) RightClickItem(new Vector2Int(0,0),primary);
                else if (player.GetComponent<Controller>().hotbar == 2) RightClickItem(new Vector2Int(0, 0), hotbar);
                else if (player.GetComponent<Controller>().hotbar == 3) RightClickItem(new Vector2Int(1, 0), hotbar);
                else if (player.GetComponent<Controller>().hotbar == 4) RightClickItem(new Vector2Int(2, 0), hotbar);
                else if (player.GetComponent<Controller>().hotbar == 5) RightClickItem(new Vector2Int(3, 0), hotbar);
                else if (player.GetComponent<Controller>().hotbar == 6) RightClickItem(new Vector2Int(4, 0), hotbar);

                weaponSprite.sprite = null; ;//Set sprite
                hand.transform.GetComponentInParent<Controller>().item = null;
                player.GetComponent<Controller>().removeItem = false;
            }
           
           

           
        }
        catch
        {
            weaponSprite.sprite = null;
            hand.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            hand.transform.GetComponentInParent<Controller>().item = null;
        }
       
    }



    /// <summary>
    /// Creates a tool tip using the itemdata
    /// </summary>
    private void ToolTip()
    {
        Vector2Int tileGridPos = selectedItemGrid.GetTileGridPosition(Input.mousePosition);
        try
        {
            var tooltip = selectedItemGrid.GetItem(tileGridPos.x, tileGridPos.y);
            if (tooltip != null)
            {
                if (tooltip == currentitem) return;// Only instantiate once
                else Destroy(instaniatedToolTip);//Delete when item is changed or nulled

                currentitem = tooltip;

                //Instantiate the tool tip
                instaniatedToolTip = Instantiate(itemToolTipPrefab, Input.mousePosition, Quaternion.identity);
                instaniatedToolTip.transform.SetParent(inventory.transform);

                //Set the text of the tool tip
                var text = instaniatedToolTip.transform.GetChild(0).GetComponent<Text>();
                text.text = tooltip.itemdata.id;
                text = instaniatedToolTip.transform.GetChild(1).GetComponent<Text>();
                text.text = tooltip.itemdata.desc;

            }
            else//Destroy tool tip when item is null
            {
                DestroyToolTip();
            }
        }
        catch
        {
            Debug.Log("tool tip failed");
        }
        


    }

    /// <summary>
    /// Destroy tooltip
    /// </summary>
    private void DestroyToolTip()
    {
        Destroy(instaniatedToolTip);
        currentitem = null;
    }

    /// <summary>
    /// Create a random item for testing purposes, should be deleted
    /// </summary>
    public Item CreateRandomItem()
    {
        Item item = Instantiate(itemPrefab).GetComponent<Item>();
        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        item.Set(items[selectedItemID]);
        return item;

    }


    /// <summary>
    /// Click Item allowing you to pick it up
    /// </summary>
    private void ClickItem()
    {
        Vector2Int tileGridPos = selectedItemGrid.GetTileGridPosition(Input.mousePosition);
        Debug.Log(tileGridPos);
        if (selectedItem == null)
        {
            selectedItem = selectedItemGrid.GetItem(tileGridPos.x, tileGridPos.y);
            selectedItemGrid.RemoveItem(tileGridPos.x, tileGridPos.y);
            if (selectedItem != null)
            {
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetParent(inventory.transform);

            }

        }
        else
        {
            if(selectedItemGrid.AcceptablePlacementCeck(selectedItem, tileGridPos.x, tileGridPos.y))
            {
                rectTransform = selectedItem.GetComponent<RectTransform>();
                selectedItemGrid.AddItem(selectedItem, tileGridPos.x, tileGridPos.y);
                selectedItem = null;
            }
            else
            {
                StartCoroutine(selectedItemGrid.OutputInvalidPlacement(selectedItem));
            }
            
        }
    }


    /// <summary>
    /// removes the item from the invenotry
    /// </summary>
    private void RightClickItem(Vector2Int tileGridPos, GridInv grid)
    {
        if (selectedItem == null)
        {
            selectedItem = grid.GetItem(tileGridPos.x, tileGridPos.y);
            if (selectedItem == null) return;//if there is no item
            grid.RemoveItem(tileGridPos.x, tileGridPos.y);
            eatitem();
        }
        else return;
       
    }

    /// <summary>
    /// This function exists for the button component on the player viewport it allows the player to "feed" the player
    /// </summary>
    public void eatitem()
    {
        if (selectedItem != null)
        {
            player.GetComponent<Controller>().interaction(selectedItem);//Run the interaction
            Destroy(selectedItem.gameObject);
            selectedItem = null;
        }
    }


    //Drag Icon to the Mouse
    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }


    //Rotate the item
    private void Rotate()
    {
        if (selectedItem != null)
        {
            if (selectedItem.itemdata.width == selectedItem.itemdata.height) return;//Dont rotate if item is a square
            rectTransform = selectedItem.GetComponent<RectTransform>();
            
            if (selectedItem.GetComponent<Item>().rotated == true)
            {
                selectedItem.GetComponent<Item>().rotated = false;
                rectTransform.pivot = new Vector2(0f, 1f);
                rectTransform.transform.eulerAngles = new Vector3(selectedItem.transform.eulerAngles.x, selectedItem.transform.eulerAngles.x, 0f);

            }
            else
            {
                selectedItem.GetComponent<Item>().rotated = true;
                rectTransform.pivot = new Vector2(1f, 1f);
                rectTransform.transform.eulerAngles = new Vector3(selectedItem.transform.eulerAngles.x, selectedItem.transform.eulerAngles.x, 90f);

            }

        }
    }
}
