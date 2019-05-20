using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using CatanApp;

public static class playerListContainer
{
    public static List<string> listPlayers = new List<string>();
}

public class CreateStartMenu : MonoBehaviour
{
    public InputField player1Field, player2Field, player3Field, player4Field; 
    public Text p1Text, p2Text, p3Text, p4Text, errorText;
    public Button startButton;
    public GameObject logo;
    bool _p1Used = false, _p2Used = false, _p3Used = false, _p4Used = false; 
    string _Player1, _Player2, _Player3, _Player4;

    
    // Start is called before the first frame update
    void Start()
    {
        p1Text.text = "Player 1: ";
        p2Text.text = "Player 2: ";
        p3Text.text = "Player 3: ";
        p4Text.text = "Player 4: ";
        player1Field.text = "Enter Player 1 name here";
        player2Field.text = "Enter Player 2 name here";
        player3Field.text = "Enter Player 3 name here";
        player4Field.text = "Enter Player 4 name here";
        player1Field.onEndEdit.AddListener(delegate {AcceptStringInput1(); });
        player2Field.onEndEdit.AddListener(delegate {AcceptStringInput2(); });
        player3Field.onEndEdit.AddListener(delegate {AcceptStringInput3(); });
        player4Field.onEndEdit.AddListener(delegate {AcceptStringInput4(); });
        startButton.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AcceptStringInput1()
    {
        _Player1 = player1Field.text;
        if (_p1Used)
        {
            playerListContainer.listPlayers.RemoveAt(0);
        }
        playerListContainer.listPlayers.Insert(0, _Player1);
        _p1Used = true;
        p1Text.text = "Player 1: " + _Player1;
        InputComplete(player1Field, 1);
    }

    void AcceptStringInput2()
    {
        _Player2 = player2Field.text;
        if (_p2Used)
        {
            playerListContainer.listPlayers.RemoveAt(1);
        }
        if(playerListContainer.listPlayers.Count == 1)
        {
            playerListContainer.listPlayers.Insert(1, _Player2);
            _p2Used = true;
            p2Text.text = "Player 2: " + _Player2;
            InputComplete(player2Field, 2);
        }
    }

    void AcceptStringInput3()
    {
        _Player3 = player3Field.text;
        if (_p3Used)
        {
            playerListContainer.listPlayers.RemoveAt(2);
        }
        if(playerListContainer.listPlayers.Count == 2)
        {
            playerListContainer.listPlayers.Insert(2, _Player3);
            _p3Used = true;
            p3Text.text = "Player 3: " + _Player3;
            InputComplete(player3Field, 3);
        }    
    }

    void AcceptStringInput4()
    {
        _Player4 = player4Field.text;
        if (_p4Used)
        {
            playerListContainer.listPlayers.RemoveAt(3);
        }
        if(playerListContainer.listPlayers.Count == 3)
        {
            playerListContainer.listPlayers.Insert(3, _Player4);
            _p4Used = true;
            p4Text.text = "Player 4: " + _Player4;
            InputComplete(player4Field, 4);
        }
    }

    void InputComplete(InputField thisField, int field)
    {
        thisField.text = "Edit player name here";
    }

    void StartGame()
    {
        if (playerListContainer.listPlayers.Count > 1)
        {
            SceneManager.LoadScene("boardgame");  
        }
        else
        {
            errorText.text = "Need more than one player's name entered to start.";
        }
    }

}

