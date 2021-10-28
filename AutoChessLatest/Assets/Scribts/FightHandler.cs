using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FightHandler : MonoBehaviour
{

    public List<GameObject> playerTeamRef;
    public List<GameObject> enemyTeamRef;
    
    public List<GameObject> newEnemyTeamRef;
    public List<GameObject> newPlayerTeamRef;

    public Transform layoutPlayer;
    public Transform layouEnemy;

    public AudioSource hitSFX;
    
    public bool enemyWin;
    public bool playerWin;
    public bool draw;

    public bool over;



   private void Awake()
    {
        GameManager.gm.AddToEnemyTeam();
    }
    
    
   private void Start()
    {
        SpawnSetUp();
        
        SpawnTeam(playerTeamRef,layoutPlayer,newPlayerTeamRef);
        
       
        SpawnTeam(enemyTeamRef,layouEnemy,newEnemyTeamRef);
        
        
    }

    void Update()
    {
        
    }

    private void SpawnTeam(List<GameObject> gos, Transform targetLayoutGroup,List<GameObject> _newPlayerTeamRef)
    {
        foreach (GameObject element in gos)
        {
          GameObject tmpElement =  Instantiate(element);
         
          GameManager.gm.ResetTranformToPanel(tmpElement,targetLayoutGroup.gameObject);

          _newPlayerTeamRef.Add(tmpElement);
            
        }
    }

    private void SpawnSetUp()
    {
        playerTeamRef = Player.playerInstance.playerTeam;
        enemyTeamRef = GameManager.gm.enemyTeam;
    } 
    
    // Button for Testing
    public void FightButton()
    {
        if (newEnemyTeamRef.Count <= 0 || newPlayerTeamRef.Count <= 0)
        {
            Debug.Log("Game already over");
            return;
        }
        
        Fight(newPlayerTeamRef,newEnemyTeamRef);
        CheckForDead();
    }

    private void Fight(List<GameObject>playerTeam, List<GameObject>enemyTeam)
    { 
        int inxPlayer = playerTeam.Count -1; 
        int inxEnemy = enemyTeam.Count -1;
        
        Element frontPlayer = playerTeam[inxPlayer].gameObject.GetComponent<Element>(); 
        Element frontEnemy = enemyTeam[inxEnemy].gameObject.GetComponent<Element>();

        if (frontPlayer.hasEffect)
        {
            frontPlayer.ApplyEffect();
            frontPlayer.DisableEffect();
        }
        if (frontEnemy.hasEffect)
        {
            frontEnemy.ApplyEffect();
            frontEnemy.DisableEffect();
        }

        frontEnemy.onDamage(frontPlayer.damage);
        frontPlayer.onDamage(frontEnemy.damage);
        
        hitSFX.Play();

        if (frontPlayer.health <= 0)
        {
            newPlayerTeamRef.Remove(frontPlayer.gameObject);    
            frontPlayer.gameObject.SetActive(false);
            frontPlayer.gameObject.transform.SetParent(null);
        }

        if (frontEnemy.health <= 0)
        {
            newEnemyTeamRef.Remove(frontEnemy.gameObject);
            frontEnemy.gameObject.SetActive(false);
            frontEnemy.gameObject.transform.SetParent(null);
        }
    }

    private void CheckForDead()
    {
        
        if (newPlayerTeamRef.Count <= 0 && newEnemyTeamRef.Count >= 1)
        {
            enemyWin = true;
            GameManager.gm.OnFightEnd(playerWin,enemyWin,draw); // Muss drin sein weil er sonst jedes mal bei Fight wieder turns erhöht
        }
        if(newEnemyTeamRef.Count <= 0 && newPlayerTeamRef.Count >= 1)
        {
            playerWin = true;
            GameManager.gm.OnFightEnd(playerWin,enemyWin,draw);
          
        }

        if (newPlayerTeamRef.Count <= 0 && newEnemyTeamRef.Count <=0)
        {
            draw = true;
            GameManager.gm.OnFightEnd(playerWin,enemyWin,draw);
        }

    }
    
    
}
