using UnityEngine;

public enum ObjectType
{
    Obstacle,
    NormalBox,
    GoldenBox,
    BoobyTrap,
    BonusBox
}

public class InteractableObject : MonoBehaviour
{
    public ObjectType type;
    public float dropChance = 50.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SimpleInventory inventory = other.GetComponent<SimpleInventory>();

            switch (type)
            {
                case ObjectType.Obstacle:
                    if (Random.Range(0f, 100f) < dropChance)
                    {
                        inventory.AddItem(ItemType.HpPotion);
                    }
                    break;

                case ObjectType.NormalBox:
                    if (Random.Range(0, 2) == 0)
                    {
                        inventory.AddItem(ItemType.HpPotion);
                    }
                    else
                    {
                        inventory.AddItem(ItemType.MpPotion);
                    }
                    Destroy(gameObject);
                    break;

                case ObjectType.GoldenBox:
                    if (Random.Range(0f, 100f) < dropChance)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            inventory.AddItem(ItemType.CooldownBuff);
                        }
                        else
                        {
                            inventory.AddItem(ItemType.MpCostBuff);
                        }
                    }
                    Destroy(gameObject);
                    break;

                case ObjectType.BoobyTrap:
                    Debug.Log("부비 트랩 발동! 주변에 광역 데미지!");
                    Destroy(gameObject);
                    break;

                case ObjectType.BonusBox:
                    int totalItemTypes = System.Enum.GetValues(typeof(ItemType)).Length;
                    int randomIndex = Random.Range(0, totalItemTypes);
                    ItemType randomItem = (ItemType)randomIndex;
                    inventory.AddItem(randomItem);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}