using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool _gameOver = false;
    
    void GameOver()
    {
        if(Input.GetKeyDown("Positive Button"))
        _gameOver = true;
    }
}
