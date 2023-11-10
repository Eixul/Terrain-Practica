using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool dead = false;
    
    public void GameOver()
    {
        //if(Input.GetKeyDown(KeyCode.P))
        dead = true;
    }
}
