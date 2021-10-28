using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player playerInstance;

    public int playerStats_Money = 10;
    public int playerStats_Lifes = 3;


    public int player_Turn;
    public int player_Wins;
    

    public List<GameObject> playerTeam;
    public List<Element> playerTeamElements;

    private void Awake()
    {
        if (playerInstance ==null)
        {
            playerInstance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        
    }

    public void ReducePlayerLife()
    {
        this.playerStats_Lifes--;
    }
    
    void Start()
    {
    }

    public void PayRoll()
    {
        this.playerStats_Money--;
    }

    public void PayShop()
    {
        this.playerStats_Money -= 3;
    }

    public void GetCoin()
    {
        this.playerStats_Money++;
    }

    public void GetMoney()
    {
        this.playerStats_Money += 10;
    }

    public void IncreaseWin()
    {
        this.player_Wins++;
    }  
    public void IncreasePlayerTurn()
    {
        this.player_Turn++;
    }

    // Speichert das Player Team am Ende der Choose Runde die Liste im GM( die verändert wurde)
    public void StoreTeamList(List<GameObject> gos)
    {
        playerTeam.Clear();
        playerTeamElements.Clear();
        foreach (GameObject children in gos)
        {
                playerTeam.Add(children);
                playerTeamElements.Add(children.GetComponent<Element>());
        
        }
    }
}

  