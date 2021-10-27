using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Element : MonoBehaviour
{
    public int health;
    public int damage;

    public int level = 1;


    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI damageTxt;
    public TextMeshProUGUI levelTxt;

    public bool hasEffect;
    public bool selected;


    private Button btn;

    public int iD;

    private void Start()
    {
        btn = GetComponent<Button>();
        
        btn.onClick.AddListener(OnSelected);
   
        
    }
    

    private void Update()
    {
        DisplayStats();
        if (selected == true)
        {
            levelTxt.text = "SELECTED";
        }
        
            
        
    }
    
    

    public void onDamage(int dmgAmount)
    {
        health -= dmgAmount;
    }

    private void Die()
    {
      gameObject.transform.SetParent(null);
      gameObject.SetActive(false);
    }

    private void DisplayStats()
    {
        healthTxt.text = health.ToString();
        damageTxt.text = damage.ToString();
        levelTxt.text = level.ToString();
    }

    public void ApplyEffect()
    {
        this.health += 2;

    }

    public void ManipulateStats(int x)
    {
        this.health += x;
        UpdateLevel();
    }

    public void DisableEffect()
    {
        this.hasEffect = false;
    }

    public void OnSelected()
    {
        this.selected = !this.selected;
    }

    public void DeSelect()
    {
        this.selected = false;
    }

    public void UpdateLevel()
    {
        this.level++;
    }
    
}
