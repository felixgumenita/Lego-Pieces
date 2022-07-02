using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsManager : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard };

    public Difficulty _Difficulty = Difficulty.Easy;

    public Vector2 CurrentCellSize = new Vector2(0, 0);

    void Awake()
    {
            
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
