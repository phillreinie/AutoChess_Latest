using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLogic : MonoBehaviour
{

    public static ChooseLogic chooseLogic;
    
    public Transform layoutShop;
    public Transform layoutPlayerTeam;

    public List<GameObject> shopList;



    void Awake()
    {
        if (chooseLogic == null)
        {
            chooseLogic = this;
        }
    }
    
    void Start()
    { 
        Roll();
        Player.playerInstance.GetMoney();
        
        // Läd das in der am Ende der letzten Runde gespeicherte Team
        List<GameObject> obj = Player.playerInstance.playerTeam;
        GameManager.gm.StorePlayerTeam(obj);
        
        
        SpawnTeam(Player.playerInstance.playerTeam,layoutPlayerTeam);
        
   
        
    }

    void Update()
    {
        Test();
        
        
    }
    
    private void SpawnTeam(List<GameObject> gos, Transform targetLayoutGroup)
    {
        foreach (GameObject element in gos)
        {
            GameObject tmpElement =  Instantiate(element, transform.position, Quaternion.identity);
            GameManager.gm.ResetTranformToPanel(tmpElement,targetLayoutGroup.gameObject);
        }
    } 
    
    private void SpawnShop(List<GameObject> gos, Transform targetLayoutGroup)
    {
        foreach (GameObject element in gos)
        {
            GameObject tmpElement =  Instantiate(element, transform.position, Quaternion.identity);
            GameManager.gm.ResetTranformToPanel(tmpElement,targetLayoutGroup.gameObject);
        }
    }

    public void Roll()
    {
        Player playerRef = Player.playerInstance;
        int money = playerRef.playerStats_Money;
        
        if (money <= 0)
           return;
        
        playerRef.PayRoll();
        
        int childs = layoutShop.transform.childCount;
        for (int i = childs - 1; i >= 0; i--) 
        {
            GameObject.Destroy( layoutShop.transform.GetChild(i).gameObject );
        }
        
        CreateShopList();
        SpawnShop(shopList,layoutShop);
    }

    private void CreateShopList()
    {
        GameManager.gm.AddToShop();
        shopList = GameManager.gm.shopItems;
    }


    public void Buy()
    {
        List<GameObject> selectedElements = CheckIfSelected(layoutShop);

        if (Player.playerInstance.playerStats_Money >= 3)
        {
            if (selectedElements.Count == 1)
            {
                int childs = layoutShop.transform.childCount;

                GameObject child = selectedElements[0].gameObject;

                GameManager.gm.playerTeamListGM.Add(child.gameObject);
                
                child.GetComponent<Element>().SetInxInGM(GameManager.gm.playerTeamListGM.Count-1);
                child.gameObject.transform.SetParent(layoutPlayerTeam);
                GameManager.gm.ResetTranformToPanel(child.gameObject,layoutPlayerTeam.gameObject);
                Player.playerInstance.PayShop();
                child.GetComponent<Element>().DeSelect();
            }
        }
        else
        {
            for (int i = 0; i < selectedElements.Count; i++)
            {
                selectedElements[i].GetComponent<Element>().DeSelect();
            }
        }
    }

    
    // hat auch probleme dynamisch zu löschen
    public void Sell()
    {
        List<GameObject> selectedElements = CheckIfSelected(layoutPlayerTeam);
        if (selectedElements.Count == 1)
        {
            GameObject child =   selectedElements[0].gameObject;

            // if (GameManager.gm.playerTeam.Contains(child))
            // {
            //     GameManager.gm.playerTeam.Remove(child);
            // } 
            for (int i = 0; i < GameManager.gm.playerTeamListGM.Count; i++)
            {

                if (GameManager.gm.playerTeamListGM[i].GetComponent<Element>().GetInxInGMList() == child.GetComponent<Element>().GetInxInGMList())
                {
                    Debug.Log("Copy Found");
                    GameManager.gm.playerTeamListGM[i].GetComponent<Element>().sold = true;
                    GameManager.gm.playerTeamListGM.Remove(GameManager.gm.playerTeamListGM[i]);
                    foreach (GameObject go in GameManager.gm.playerTeamListGM)
                    {
                        if (!GameManager.gm.playerTeamListGM.Contains(go))
                        {
                            
                            go.GetComponent<Element>().DecreaseINxInGM();
                        }
                      
                    }
                    Destroy(child);
                    break;

                }
       
            }
      

           
             // findet bestehende elemente nicht (deswegen ab 2 Runde fehlerhaft)
        }
        else
        {
            selectedElements[0].gameObject.GetComponent<Element>().DeSelect(); // loop for deselect
        }
    }
    
    // zeigt probleme mit Laden neu laden in Scene - Team muss besser gespeichert werden 
    public void Combine()
    {
        List<GameObject> selectedElements = CheckIfSelected(layoutPlayerTeam);
        int selectedElementsCount = selectedElements.Count;
        //Debug.Log(selectedElements);
        if (selectedElementsCount == 2)
        {
             Element element1 = selectedElements[0].GetComponent<Element>();
             Element element2 = selectedElements[1].GetComponent<Element>();
             int id1 =element1.iD;
             int id2 = element2.iD;
             if (id1 == id2)
             {
                 GameManager.gm.playerTeamListGM.RemoveAt(GameManager.gm.playerTeamListGM.Count -1); // muss erst finden um zu wissen welcher indeyx
                // GameManager.gm.playerTeam.Remove(selectedElements[1].gameObject); // muss erst finden um zu wissen welcher indeyx
                 Destroy(selectedElements[1].gameObject);
                 Element element= selectedElements[0].GetComponent<Element>();
             
                 element.ManipulateStats(2);
             }
             else
             {
                 element1.DeSelect();
                 element2.DeSelect();
             }
        }
    }

    public List<GameObject> CheckIfSelected(Transform parent)
    {
        if (parent.transform.childCount == 0)
            return new List<GameObject>();
        
        List<GameObject> temp = new List<GameObject>();
        
        int children = parent.transform.childCount;
        
        for (int i = children - 1; i >= 0; i--) 
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            bool flag =  child.GetComponent<Element>().selected;
            if (flag)
            {
                temp.Add(child);
            }
        }

        return temp;
    }


    
    public List<GameObject> FindAllChildrenInGameObject(Transform parent)
    {
        List<GameObject> temp = new List<GameObject>();
        
        int childs = parent.transform.childCount;
        
        for (int i = childs - 1; i >= 0; i--) 
        {
            GameObject child = parent.transform.GetChild(i).gameObject; 
            temp.Add(child);
        }
        return temp;
    }


    
    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            List<GameObject> gos = FindAllChildrenInGameObject(layoutPlayerTeam);
            for (int i = 0; i < gos.Count; i++)
            {
                //Debug.Log(gos[i].name);
            }
            
        }  
    }

    public List<GameObject> GetPlayerTeamPanel()
    {
        return FindAllChildrenInGameObject(layoutPlayerTeam);
    }

    private void OnDisable()
    {
        
    }

    private void UpdateTeam()
    {
        //Debug.Log(GetPlayerTeamPanel().Count);

    }

    public void SetPanelOfPlayerTeamToPlayerTeam()
    {
       List<GameObject> temp =GetPlayerTeamPanel();
       for (int i = 0; i < temp.Count; i++)
       {
           temp[i].transform.SetParent(Player.playerInstance.gameObject.transform);
       }
    }
    
    
    
    

   // Sell
   // Checker der unter den ausgewählten gleiche findet ++
  

}
