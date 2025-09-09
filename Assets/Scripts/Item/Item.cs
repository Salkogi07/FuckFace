using UnityEngine;

public abstract class Item
{
    public string itemName;
    public Sprite icon;
    public abstract void Use();
}

public class HealPotion : Item
{
    public int value = 20;
    public int useMp = 0;

    public override void Use()
    {
        
    }
}

public class BuffItem : Item
{
    public float Value = 5f;
    public int useMp = 0;

    public override void Use()
    {
        
    }
}
