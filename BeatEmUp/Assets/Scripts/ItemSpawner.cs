using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] Item[] itemPrefabs;
    [SerializeField] Transform Orientation;
    bool SpawnAgain = true;

    public void SpawnRandomItem() 
    {
        //Function Call To Spawn Items
       
       Item RandomItem = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)], Orientation.forward * 5, Quaternion.identity);
    }     

    private void Update() { 
        if (Input.GetKey(KeyCode.F) && SpawnAgain) {
            SpawnAgain = false;

            SpawnRandomItem();

            Invoke(nameof(ResetSpawn), .25f);
        }    
    }

    private void ResetSpawn() {
        SpawnAgain = true;
    }
    

}
