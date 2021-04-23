﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Attach to Player to detect Item collisions

public class ItemTrigger : MonoBehaviour
{
    public Collider currentCol = null;
    public ItemTag currentItem = null;
    public bool reportBoo = false;
    private GameObject global_variables;
    
    void Start()
    {
        global_variables = this.gameObject.transform.parent.GetComponent<BasicMovement>().global_variables;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            ItemTag item = collision.gameObject.GetComponent<ItemTag>();
            currentItem = item;

            //if(item.placeable)
            if(new []{"Created", "Filled", "Shot", "Ghost", "ToBeDestroyed", "Destroyed"}.Contains(item.itemState))
            {
                string action = "Enter "+ item.itemState+" " + item.attachedItemName;
                //Uncomment if we want to call an action when entering a placeable item zone
                global_variables.GetComponent<StateMachine>().handleAction("Player", onAction: action);
            }
        }
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerStay(Collider collision)
    {

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Item")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            ItemTag item = collision.gameObject.GetComponent<ItemTag>();
            currentItem = item;
            item.setIndicator(true);

            //if(item.placeable)
            if(new []{"Created", "Filled", "Shot", "Ghost", "ToBeDestroyed", "Destroyed"}.Contains(item.itemState))
            {
                string action = "Enter "+ item.itemState+" " + item.attachedItemName;
                if(item.itemState != "Ghost" && item.itemState != "Destroyed")
                {
                    //action += item.attachedItemName;
                    string itemPrompt = "Press 'E' to use " + item.attachedItemName;
                    global_variables.GetComponent<GlobalScript>().Overlay.GetComponent<Overlay>().changePromptActive(true);
                    global_variables.GetComponent<GlobalScript>().Overlay.GetComponent<Overlay>().changePrompt(itemPrompt);
                }
                
                //Uncomment if we want to call an action when entering a placeable item zone
                //global_variables.GetComponent<StateMachine>().handleAction("Player", onAction: action);
            }
            else
            {
                //Set Overlay
                if(!item.destroyed)
                {
                    string itemPrompt = "Press 'E' to pick up " + item.attachedItemName;
                    global_variables.GetComponent<GlobalScript>().Overlay.GetComponent<Overlay>().changePromptActive(true);
                    global_variables.GetComponent<GlobalScript>().Overlay.GetComponent<Overlay>().changePrompt(itemPrompt);
                }
                
            }
            currentCol = collision;
        } else if(collision.gameObject.tag == "Report")
        {
            reportBoo = true;
            string name = collision.gameObject.name;
            string report = "";
            switch (name)
            {
                case "PoliceReport":
                    report = "Police Report";
                    break;
                case "GoldbergSafetyReport":
                    report = "Goldberg Safety Report";
                    break;
                case "ForemansReport":
                    report = "Foreman's Report";
                    break;
            }
            string itemPrompt = "Press 'E' to view " + report;
            global_variables.GetComponent<GlobalScript>().Overlay.GetComponent<Overlay>().changePromptActive(true);
            global_variables.GetComponent<GlobalScript>().Overlay.GetComponent<Overlay>().changePrompt(itemPrompt);
            currentCol = collision;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            ItemTag item = collision.gameObject.GetComponent<ItemTag>();
            item.setIndicator(false);

            global_variables.GetComponent<GlobalScript>().Overlay.GetComponent<Overlay>().changePromptActive(false);
            currentCol = null;
            currentItem = null;
        } else if(collision.gameObject.tag == "Report")
        {
            global_variables.GetComponent<GlobalScript>().Overlay.GetComponent<Overlay>().changePromptActive(false);
            reportBoo = false;
            currentCol = null;
        }
    }
}
