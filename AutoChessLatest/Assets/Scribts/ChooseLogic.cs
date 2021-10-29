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
    
    
    private GameManager gameMangerRef;
    private List<GameObject> gameMangerList;
     
    private Player playerRef;
    private List<GameObject> playerTeamList;



    void Awake()
    {
        if (chooseLogic == null)
        {
            chooseLogic = this;
        }
        
        gameMangerRef = GameManager.gm;
        playerRef = Player.playerInstance;

        gameMangerList = gameMangerRef.playerTeamListGM;
        playerTeamList = playerRef.playerTeamList;
        



    }
    
    void Start()
    { 
        
        
        
        
        Roll();
        playerRef.GetMoney();
        
        // Läd das in der am Ende der letzten Runde gespeicherte Team
       
        GameManager.gm.StorePlayerTeam(playerTeamList);
        
        
        SpawnTeam(playerTeamList,layoutPlayerTeam);
        
   
        
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

        if (playerRef.playerStats_Money >= 3)
        {
            if (selectedElements.Count == 1)
            {
                int childs = layoutShop.transform.childCount;

                GameObject child = selectedElements[0].gameObject;
                Element childElement = child.GetComponent<Element>();

                gameMangerList.Add(child.gameObject);
                
                // Set its self id to the id in the team List ( so know witch pos in team) -> elegantere Lösung
                childElement.SetInxInGM(gameMangerList.Count-1);
                 
                // Set Parent to Layoutgroup
                child.gameObject.transform.SetParent(layoutPlayerTeam);
                gameMangerRef.ResetTranformToPanel(child.gameObject,layoutPlayerTeam.gameObject);
                
                playerRef.PayShop();
                childElement.DeSelect();
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

          
            for (int i = 0; i < gameMangerList.Count; i++)
            {

                if (gameMangerList[i].GetComponent<Element>().GetInxInGMList() == child.GetComponent<Element>().GetInxInGMList())
                {
                    Debug.Log("Copy Found");
                    gameMangerList[i].GetComponent<Element>().sold = true;
                    gameMangerList.Remove(gameMangerList[i]);
                    // loop threw gameMangerList and see if any of it is already in player list if not than deacrease
                    foreach (GameObject go in gameMangerList)
                    {
                        if (!playerTeamList.Contains(go))
                        {
                            go.GetComponent<Element>().DecreaseINxInGM();
                        }
                      
                    } 
                    
                    //
                    foreach (GameObject go in playerTeamList)
                    {
                        if (gameMangerList.Contains(go))
                        {
                            go.GetComponent<Element>().DecreaseINxInGM();
                        }
                      
                    }
                    Destroy(child);
                    break;

                } // if
       
            } // for
        }// if
        else
            {
                selectedElements[0].gameObject.GetComponent<Element>().DeSelect(); // loop for deselect
            }
    } // still buggy but ok for now
    
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
                 for (int i = 0; i < selectedElements.Count; i++)
                 {
                     if (gameMangerList.Contains(selectedElements[i]))
                     {
                         Debug.Log("Copy Found");
                         gameMangerList.RemoveAt(i);
                     }
                 }
                 gameMangerList.Add(selectedElements[0]);
             
                
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
