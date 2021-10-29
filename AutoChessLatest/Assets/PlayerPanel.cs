using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPanel : MonoBehaviour
{
   
   
   
   public TextMeshProUGUI liveTxt;
   public TextMeshProUGUI turnsTxt;
   public TextMeshProUGUI moneyTxt;

   public bool SimState;

   private Player playerRef;

   private void Awake()
   {
      
   }

   private void Update()
   {
      DisplayUI();
   }

   private void DisplayUI()
   {
   
   }
}
