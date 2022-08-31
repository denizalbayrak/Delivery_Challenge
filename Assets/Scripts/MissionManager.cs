using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using DG.Tweening;

public class MissionManager : MonoBehaviour
{
    public int mushroomNum =0;
    public Text MushroomNumText;
    public Text Mission;
    public Text Mushroom;
    public GameObject TalkingBox;
  

    public Text WhoTalking;
    public Text SayWhat;
 

    public void TakeMission()
    {
        Mushroom.gameObject.SetActive(true);
        Mission.text = "I need to find a hoe to collect mushroom.";
        MushroomNumText.text = mushroomNum.ToString();
        Mission.DOFade(1, 1f);
    }


}
