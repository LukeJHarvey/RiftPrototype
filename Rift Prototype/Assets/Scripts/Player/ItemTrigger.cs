﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach to Player to detect Item collisions

public class ItemTrigger : MonoBehaviour
{
    public Collider currentCol = null;
    public ItemTag currentItem = null;
    private GameObject global_variables;
    
    void Start()
    {
        global_variables = this.gameObject.transform.parent.GetComponent<BasicMovement>().global_variables;
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerEnter(Collider collision)
    {

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Item")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Item Detected");
            ItemTag item = collision.gameObject.GetComponent<ItemTag>();
            currentItem = item;
            item.setIndicator(true);

            if(item.placeable)
            {
                string action = "Enter PlaceableItem ";
                if(!item.created)
                    action += "Ghost ";
                else
                    action += "Item ";
                
                global_variables.GetComponent<StateMachine>().handleAction("Player", onAction: action + item.attachedItemName);
            }
            else
            {
                //Set Overlay
                string itemPrompt = "Press 'E' to pick up " + item.attachedItemName;
                global_variables.GetComponent<GlobalScript>().Overlay.GetComponent<Overlay>().changePromptActive(true);
                global_variables.GetComponent<GlobalScript>().Overlay.GetComponent<Overlay>().changePrompt(itemPrompt);
            }
            currentCol = collision;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Debug.Log("Left Item Collider Area");
            ItemTag item = collision.gameObject.GetComponent<ItemTag>();
            item.setIndicator(false);

            global_variables.GetComponent<GlobalScript>().Overlay.GetComponent<Overlay>().changePromptActive(false);
            currentCol = null;
            currentItem = null;
        }
    }
}