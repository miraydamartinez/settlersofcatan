using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using CatanApp;

public static class winner
{
    public static string _playername; 
}

public class CreateVictoryScreen : MonoBehaviour
{

    GameObject victoryobj;
    Text victorytxt;

    void Start()
    {
        victoryobj = GameObject.Find("vicboard");
        victorytxt = victoryobj.GetComponent<Text>();
        victorytxt.text = $"Congratulations, {winner._playername}, you have won Catan!";
    }
}