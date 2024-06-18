using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SlimeSpawner : MonoBehaviour
{

    [SerializeField] GameObject slimePrefab;
    [SerializeField] GameObject healthInputField;
    [SerializeField] GameObject damageInputField;
    [SerializeField] Transform parentObject;

    private Vector3 spawnPos;
    private string healthString;
    private string damageString;

    private int healthInt;
    private int damageInt;


    public void spawnSlime()
    {
        spawnPos = new Vector3(Camera.main.transform.position.x + 7.5f, 1, 0);
        GameObject enemy = Instantiate(slimePrefab, spawnPos, transform.rotation);
        enemy.transform.SetParent(parentObject);
        healthString = healthInputField.GetComponent<TMP_InputField>().text;
        damageString = damageInputField.GetComponent<TMP_InputField>().text;

        int.TryParse(healthString, out healthInt);
        int.TryParse(damageString, out damageInt);
        enemy.GetComponent<Slime>().health = healthInt;
        enemy.GetComponent<Slime>().damage = damageInt;
    }
}
