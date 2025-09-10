using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemType itemType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SimpleInventory inventory = other.GetComponent<SimpleInventory>();

            inventory.AddItem(itemType);
            Destroy(gameObject);
        }
    }
}
