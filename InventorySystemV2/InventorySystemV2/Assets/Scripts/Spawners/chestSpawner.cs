using UnityEngine;

public class chestSpawner : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] GameObject chestPrefab;
    [SerializeField] GameObject INVGRIDS;
    [SerializeField] GameObject ChestUI;
    [SerializeField] GameObject InvUI;
    [SerializeField] GameObject InvController;

    private GameObject chest;

    public void spawnChest()
    {
        Vector3 spawnPos = new Vector3(Camera.main.transform.position.x - 2.43f, -3.25f, 0.17f);
        if (chest != null)//Deletes previous chest
        {
            chest.GetComponent<chest>().destroyChest();
            Destroy(chest);
        }
        chest = Instantiate(chestPrefab, spawnPos, transform.rotation);//Instanties a chest and sets all the serialzed references stored here
        chest.transform.SetParent(parent.transform);
        chest.GetComponent<chest>().ChestUI = ChestUI;
        chest.GetComponent<chest>().INVGRIDS = INVGRIDS;
        chest.GetComponent<chest>().InvController = InvController;
        chest.GetComponent<chest>().InvUI = InvUI;
        chest.GetComponent<chest>().spawnChest();

    }
}
