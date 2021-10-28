using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    void Update()
    {
        foreach (Transform playerTeamCopy in this.gameObject.transform)
        {
            playerTeamCopy.tag = "Player";

        }
    }
}
