using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random =  UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public static GameManager gm;

    public List<GameObject> elementsList;


    public List<GameObject> playerTeam = new List<GameObject>();
    public List<GameObject> enemyTeam = new List<GameObject>();
    public List<GameObject> shopItems = new List<GameObject>();

    public Player playerRef;
  
    

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    
    void Start()
    {
       DontDestroyOnLoad(this.gameObject); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddToPlayerTeam();
        } 
        if (Input.GetKeyDown(KeyCode.E))
        {
           
        }
 
    }
    
    
// Set Teams
    public void AddToPlayerTeam()
    {
        playerTeam = GetRandomItemsFromList<GameObject> (elementsList, 5);
        
    } 
    public void AddToEnemyTeam()
    {
        enemyTeam = GetRandomItemsFromList<GameObject> (elementsList,5 );
        
    }

    public void AddToShop()
    {
         shopItems = GetRandomItemsFromList<GameObject> (elementsList, 5);
    }
  
 

    
    // Player Funktions
    public void StorePlayerTeam(List<GameObject> _team)
    {
        playerTeam = _team;
    }

    public List<GameObject> GetPlayerTeam()
    {
        return playerTeam;
    }

    public void GetPlayerTeamInfo()
    {
        Debug.Log("Player team: "+ playerTeam.Count + " Elements");
    }
   
    
    
    // Get Random Elements from Pool
    public static List<T> GetRandomItemsFromList<T> (List<T> list, int number)
    {
        // this is the list we're going to remove picked items from
        List<T> tmpList = new List<T>(list);
        // this is the list we're going to move items to
        List<T> newList = new List<T>();
 
        // make sure tmpList isn't already empty
        while (newList.Count < number && tmpList.Count > 0)
        {
            int index = Random.Range(0, tmpList.Count);
            newList.Add(tmpList[index]);
            tmpList.RemoveAt(index);
        }
 
        return newList;
    }

    
    
    public void OnFightEnd(bool playerWin, bool playerLost , bool draw)
    {
      
        if (playerWin)
        {
            playerRef.IncreaseWin();
        }
        else if (playerLost)
        {
            playerRef.ReducePlayerLife();
        }
        else if (draw)
        {
          Debug.Log("Draw"); 
        }
        playerRef.IncreasePlayerTurn();
            
       
    }
    
 
// Loading 

    public void LoadFirstScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void LoadSimScene()
    {
        StoreGMPlayerListInGamgeMangerObject();
        //DeleteGamgeManagerObjectChildren();

       
        
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadChooseScene()
    {
        SceneManager.LoadScene(1);
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
    public void ResetTranformToPanel(GameObject start, GameObject target)
    {
        start.transform.SetParent(target.transform);
        start.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
    }

    public void StoreGMPlayerListInGamgeMangerObject()
    {
        foreach (GameObject child in playerTeam)
        {
            child.transform.SetParent(this.transform);
        }
    }  
    public void DeleteGamgeManagerObjectChildren()
    {
        foreach (GameObject child in playerTeam)
        {
          Destroy(child);
        }
    }


}



