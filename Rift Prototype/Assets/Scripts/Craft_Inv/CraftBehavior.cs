﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The crafting behavior, this script is the core of the crafting system

public class CraftBehavior : MonoBehaviour
{

    private Item requestedSize = null;

    private Item requestedType = null;

    private Item requestedMaterial = null;

    private Item resetHelper;

    private Dictionary<(string, string, string), string> recipe;

    private Inventory inventory;

    private ItemDatabase allItems;

    //Note to self/others the below is called frequently it can probably be replaced by a fixed event that occurs whenever something is dropped into the crafting slots

    public void Update()
    {
        if(requestedSize != resetHelper && requestedType != resetHelper && requestedMaterial != resetHelper)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

    public void SetItemDatabase(ItemDatabase data)
    {
        this.allItems = data;
    }

    public void Setreset()
    {

        requestedSize = null;

        requestedType = null;

        requestedMaterial = null;
    }

    public void Craft()
    {
        Debug.Log(requestedSize.itemSize + " " + requestedType.itemType + " " + requestedMaterial.itemMaterial);
        if (recipe.TryGetValue((requestedSize.itemSize, requestedType.itemType, requestedMaterial.itemMaterial), out string value))
        {
            // Key was in dictionary; "value" contains corresponding value
            Debug.Log("craft Success: You made " + value);
            inventory.CraftRemoval(requestedSize, requestedType, requestedMaterial);
            inventory.AddItem(allItems.FindItem(value));
        }
        else
        {
            // Key wasn't in dictionary; "value" is now 0
            Debug.Log("Craft Fail");
        }
    }

    public void SetSize(Item item)
    {
        requestedSize = item;
    }

    public void SetType(Item item)
    {
        requestedType = item;
    }

    public void SetMaterial(Item item)
    {
        requestedMaterial = item;
    }

    //public void SetRecipe(Dictionary<(Item.ItemSize, Item.ItemType, Item.ItemMaterial), Item.ItemName> data)
    public void SetRecipe(Dictionary<(string, string, string), string> data)
    {
        recipe = data;
    }

    public void ResetMaterial()
    {
        requestedMaterial = resetHelper;
    }
}
