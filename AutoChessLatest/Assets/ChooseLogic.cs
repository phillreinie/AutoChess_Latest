using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLogic : MonoBehaviour
{
    
    public Transform layoutShop;
    public Transform layoutPlayerTeam;

    public List<GameObject> shopList;
    

    
    void Start()
    {
      Roll();
      Player.playerInstance.GetMoney();
      SpawnTeam(GameManager.gm.playerTeam,layoutPlayerTeam);
      
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
            tmpElement.transform.SetParent(targetLayoutGroup);
            tmpElement.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            
        }
    } 
    private void SpawnShop(List<GameObject> gos, Transform targetLayoutGroup)
    {
        foreach (GameObject element in gos)
        {
            GameObject tmpElement =  Instantiate(element, transform.position, Quaternion.identity);
            tmpElement.transform.SetParent(targetLayoutGroup);
            tmpElement.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            

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

                GameManager.gm.playerTeam.Add(child.gameObject);
                child.gameObject.transform.SetParent(layoutPlayerTeam);
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

    public void Sell()
    {
        if (Player.playerInstance.playerStats_Money <= 0) 
            return;

        List<GameObject> selectedElements = CheckIfSelected(layoutPlayerTeam);
        if (selectedElements.Count == 1)
        {
            int childs = layoutShop.transform.childCount;
       
            GameObject child =   selectedElements[0].gameObject;
               
            GameManager.gm.playerTeam.Remove(child.gameObject);
            Destroy(child.gameObject);
            Player.playerInstance.GetCoin();
            // muss player team aktualisieren
        }
    }
    
    
    public void Combine()
    {
        List<GameObject> selectedElements = CheckIfSelected(layoutPlayerTeam);
        int selectedElementsCount = selectedElements.Count;
        Debug.Log(selectedElements);
        if (selectedElementsCount == 2)
        {
             Element element1 = selectedElements[0].GetComponent<Element>();
             Element element2 = selectedElements[1].GetComponent<Element>();
             int id1 =element1.iD;
             int id2 = element2.iD;
             if (id1 == id2)
             {
                 GameManager.gm.playerTeam.Remove(selectedElements[1].gameObject); // muss erst finden um zu wissen welcher indeyx
                 Destroy(selectedElements[1].gameObject);
                 Element element= selectedElements[0].GetComponent<Element>();
             
                 element.ManipulateStats(2);
             }
             else
             {
                 element1.DeSelect();
                 element2.DeSelect();
             }
           

             /* for (int i = childs - 1; i >= 0; i--) 
              {
                  GameObject child =   layoutPlayerTeam.transform.GetChild(i).gameObject;
                  bool flag =  child.GetComponent<Element>().selected;
                  if (flag)
                  {
                      child.GetComponent<Element>().ManipulateStats(10);
                  }
                  child.GetComponent<Element>().DeSelect();
              }*/
        }
        
     
        
    }

    public List<GameObject> CheckIfSelected(Transform parent)
    {
        List<GameObject> temp = new List<GameObject>();
        
        int childs = parent.transform.childCount;
        
        for (int i = childs - 1; i >= 0; i--) 
        {
            GameObject child =   parent.transform.GetChild(i).gameObject;
            bool flag =  child.GetComponent<Element>().selected;
            if (flag)
            {
                temp.Add(child);
            }
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
                Debug.Log(gos[i].name);
            }
            
        }  
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
    
    
    
    

   // Sell
   // Checker der unter den ausgewählten gleiche findet ++
  

}
