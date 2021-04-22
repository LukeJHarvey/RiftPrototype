﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Attatch to Item Tagged Objects and edit its name for each item
public class ItemTag : MonoBehaviour
{
    public string attachedItemName;
    //public bool placeable = false;
    public string itemState = "Static"; 
    //"Static", "Ghost", "Created", "ToBeDestroyed", "Destroyed", "Filled", "Shot"
    //public bool created = false;
    public bool indicator = false;
    public bool destroyed = false;
    public bool infinite = false;
    public float timeTillRespawn = 0.0f;
    public float respawnTime = 5.0f;
    public GameObject Obj;
    private BoxCollider ParentCollider;
    private Material[] ObjMaterials;
    public float Transparency = 0.2f;
    //If we want the indicator to be an object above the Item
    public float arrowSpeed = .3f;
    public float minY = 1.5f;
    public float maxY = 2f;
    private Transform pointer;

    private void Start()
    {
        if(itemState == "")
            itemState = "Static";
        Transform[] ts = this.transform.parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            if(t.gameObject.name == "Pointer")
            {
                pointer = t;
            }
        }
        if(Obj != null && Obj.GetComponent<MeshRenderer>() != null)
            ObjMaterials = Obj.GetComponent<MeshRenderer>().materials;
        if(pointer != null)
            pointer.gameObject.SetActive(indicator);
        
        if(itemState == "Ghost")
        {
            foreach(Material mat in ObjMaterials)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, Transparency);
            }
        }
        ParentCollider = gameObject.transform.parent.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(itemState == "Ghost")
        {
            if(indicator)
            {
                float move = pointer.position.y + (arrowSpeed * Time.deltaTime);
                if(move > maxY)
                {
                    arrowSpeed *= -1;
                    move = maxY;
                }
                else if(move < minY)
                {
                    arrowSpeed *= -1;
                    move = minY;
                }
                pointer.position = new Vector3(pointer.position.x, move, pointer.position.z);
            }
        }
        else if(Obj != null && itemState == "Static")
        {
            if(timeTillRespawn != 0.0f)
            {
                destroyed = true;
                timeTillRespawn -= Time.deltaTime;
                if(timeTillRespawn < 0.0f)
                {
                    timeTillRespawn = 0.0f;
                    destroyed = false;
                }
            }
            if(destroyed && !infinite)
            {
                if(ParentCollider != null)
                    ParentCollider.enabled = false;
                Obj.SetActive(false);
            }
            if(!destroyed && !infinite)
            {
                if(ParentCollider != null)
                    ParentCollider.enabled = true;
                Obj.SetActive(true);
            }
        }
    }

    public void setIndicator(bool active)
    {
        //if(!created && placeable)
        if(itemState == "Ghost")
        {
            indicator = active;
            //For object above
            pointer.gameObject.SetActive(active);
            //For fading
            foreach(Material mat in ObjMaterials)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, Transparency);
            }
        }
    }
    public void setState(string newState)
    {
        itemState = newState;
        if(newState == "Created")
        {
            setIndicator(false);
            foreach(Material mat in ObjMaterials)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1.0f);
            }
        }
        else if(newState == "Ghost")
        {
            setIndicator(true);
        }
    }
}
