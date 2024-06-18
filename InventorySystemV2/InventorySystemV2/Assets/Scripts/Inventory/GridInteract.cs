using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This script assigns the active grid that the player is currently hovered on
/// </summary>
[RequireComponent(typeof(GridInv))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject invManager;
    InventoryController inventoryController;
    GridInv itemGrid;
    private void Awake()
    {
        if(invManager == null) invManager = GameObject.Find("InventoryManager");
        inventoryController = invManager.GetComponent<InventoryController>();
        itemGrid = GetComponent<GridInv>();
    }

    //Select this grid
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.selectedItemGrid = itemGrid;
    }

    //Delesect this grid
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.selectedItemGrid = null;
    }

}
