using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : Singleton<chest>
{
    bool chestGenerated = false;
    private GameObject chestInv;
    Item item;

    public GameObject INVGRIDS;
    public GameObject ChestUI;
    public GameObject InvUI;
    [SerializeField] GameObject GridInvPrefab;
    public GameObject InvController;
    public int numberOfItemsToSpawn = 10;
    public void spawnChest()
    {
        ChestUI.SetActive(true);//enables gameobject for parenting
        Vector3 spawnPos = new Vector3(Camera.main.transform.position.x - 6f, 1, 0);

        if (!chestGenerated)//Only instiate if an inventory hasnt been generated
        {
            //isntanstaies a chest inventory
            chestInv = Instantiate(GridInvPrefab, spawnPos, transform.rotation);
            chestInv.transform.SetParent(ChestUI.transform);
            chestInv.transform.localScale = new Vector3(1, 1, 1);
            chestInv.transform.position = new Vector3(1405.4473876953125f, 971.8651123046875f, 19.33611297607422f);

        }
        ChestUI.SetActive(false);
    }
    public void clickChest()//Sets ui elements active
    {
        InvUI.SetActive(true);
        ChestUI.SetActive(true);
        chestInv.SetActive(true);
        if (!chestGenerated)Invoke("addItems", 0.5f);//invokes function as items require parenting to an object that needs to be activated
    }
    private void addItems()
    {
        
        int fails = 0;
        for (int n = 0; n < numberOfItemsToSpawn; n++)//Will loop for number of items to spawm
        {
            item = InvController.GetComponent<InventoryController>().CreateRandomItem();//generates a random item 
            item.transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);
            for (int x = 0; x < GridInvPrefab.GetComponent<GridInv>().width; x++)//loops for the size of the inventory
            {
                for (int y = 0; y < GridInvPrefab.GetComponent<GridInv>().height; y++)
                {
                    if (chestInv.GetComponent<GridInv>().AcceptablePlacementCeck(item, y, x))//if the item can be placed place it
                    {
                        chestInv.GetComponent<GridInv>().AddItem(item, y, x);
                        x = GridInvPrefab.GetComponent<GridInv>().width;//exits the for loop so the item isnt placed on every tile
                        y = GridInvPrefab.GetComponent<GridInv>().height;
                    }
                    else 
                    {
                        if (x == GridInvPrefab.GetComponent<GridInv>().width && y == GridInvPrefab.GetComponent<GridInv>().height)//if iem cant be placed on the the last tile log a fail
                        {
                            fails++;
                        }
                        if (fails == 30)//if 30 fails exit the placement loop
                        {
                            n = numberOfItemsToSpawn;
                        }
                    }

                }
            }

        }
        chestGenerated = true;


    }

    public void destroyChest()
    {
        Destroy(chestInv);//destroy the invetory space
    }

}
