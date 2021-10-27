using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{


    
 public void OnPressReadyBtn()
 {
  GameManager.gm.LoadSimScene();
 }


 public void OnPressBackToChoose()
 {
  GameManager.gm.LoadChooseScene();
 }

 public void OnPressPlayMainMenuBtn()
 {
  GameManager.gm.LoadFirstScene();
 }
 
}
