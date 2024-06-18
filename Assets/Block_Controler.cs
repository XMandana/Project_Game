using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Controler : MonoBehaviour
{
    public string Color_Name;
    public Game_Master Game_Maste;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Choise()
    {
        Game_Maste.Check_The_Color(Color_Name);
    }
}
