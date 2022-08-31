using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryManager : MonoBehaviour
{
    public MissionManager missionManager;
    public GameObject player;
    public List<Image> InventorySprites = new List<Image>();
    public List<Image> InventoryBoxes = new List<Image>();
    public List<bool> Inventory = new List<bool>();
    public List<Text> InventoryNumber = new List<Text>();
    public List<string> InventoryName = new List<string>();
    public Sprite Mushroom;
    public Sprite Hoe;
    Color Brown;
    public Text MushroomNum;
    public int MushroomTemp;
    public int ScrollTemp =0;
    
    void Start()
    {
        Brown = InventoryBoxes[0].color;
        InventoryBoxes[0].color = Color.red;
        for (int i = 0; i < InventorySprites.Count; i++)
        {
            InventoryName.Add("empty");
            Inventory.Add(false);
        }
    }

     public void putInventory(string takedObj)
    {
        if (takedObj.Equals("Hoe"))
        {
          
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (!Inventory[i])
                {
                    InventoryName[i] = "Hoe";
                       InventorySprites[i].gameObject.SetActive(true);
                    InventoryNumber[i].gameObject.SetActive(true);
                    int num = int.Parse(InventoryNumber[i].text.ToString()) +1;
                    InventoryNumber[i].text = num.ToString();
                    InventorySprites[i].sprite = Hoe;
                    Inventory[i] = true;
                    break;
                }
            }
        }

        else if (takedObj.Equals("Mushroom"))
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (!Inventory[i])
                {
                   
                    if (missionManager.mushroomNum == 1)
                    {
                        InventoryName[i] = "Mushroom";
                        Inventory[i] = true;
                        InventorySprites[i].gameObject.SetActive(true);
                        InventorySprites[i].sprite = Mushroom;
                        InventoryNumber[i].gameObject.SetActive(true);
                        InventoryNumber[i].text = missionManager.mushroomNum.ToString();
                        MushroomTemp = i;
                    }

                    else
                    {
                        InventoryNumber[MushroomTemp].text = missionManager.mushroomNum.ToString();
                    }
                 
                    break;
                }
            }
            
        }
    }


    public void removeInventory(string takedObj)
    {
        if (takedObj.Equals("Mushroom"))
        {
            if (missionManager.mushroomNum>0)
            {
                InventoryNumber[MushroomTemp].text = missionManager.mushroomNum.ToString();
            }
            else
            {
                Inventory[MushroomTemp] = false;
                InventorySprites[MushroomTemp].gameObject.SetActive(false);
                InventoryNumber[MushroomTemp].text = missionManager.mushroomNum.ToString();
                InventoryNumber[MushroomTemp].gameObject.SetActive(false);
            }
        
           }
    }

    public void ScrollObjects(int ScrollNum)
    {
         InventoryBoxes[ScrollTemp].color = Brown;
        if (ScrollNum == -1)
        {
            if (ScrollTemp == 0)
            {
                ScrollTemp = 1;
            }
            else if (ScrollTemp == 1)
            {
                ScrollTemp = 2;

            }
            else if (ScrollTemp == 2)
            {
                ScrollTemp = 0;

            }
        }
        if (ScrollNum == 1)
        {
            if (ScrollTemp == 0)
            {
                ScrollTemp = 2;
            }
            else if (ScrollTemp == 1)
            {
                ScrollTemp = 0;

            }
            else if (ScrollTemp == 2)
            {
                ScrollTemp = 1;
            }
        }

        InventoryBoxes[(int)ScrollTemp].color = Color.red;
        if (InventoryName[(int)ScrollTemp] == "Hoe")
        {
            player.GetComponent<PlayerController>().HoldHoe();
        }
        if (InventoryName[(int)ScrollTemp] == "Mushroom")
        {

            player.GetComponent<PlayerController>().HoldMushroom();
        }
        if (InventoryName[(int)ScrollTemp] == "empty")
        {
            player.GetComponent<PlayerController>().HoldEmpty();
        }
    }
}
