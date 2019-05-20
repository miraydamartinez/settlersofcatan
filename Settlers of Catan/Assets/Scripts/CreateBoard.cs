using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using System.Drawing;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using CatanApp;
using System;

public class CreateBoard : MonoBehaviour
{
    public Board board;

    // Creating Catan Game
    Catan catan = new Catan();

    // tile templates
    public GameObject brick;
    public GameObject wheat;
    public GameObject desert;
    public GameObject lumber;
    public GameObject ore;
    public GameObject sheep;

    // chit templates
    public GameObject two;
    public GameObject three;
    public GameObject four;
    public GameObject five;
    public GameObject six;
    public GameObject eight;
    public GameObject nine;
    public GameObject ten;
    public GameObject eleven;
    public GameObject twelve;

    // settlement, road, and robber templates
    public GameObject settlement;
    public GameObject road;
    public GameObject robber;

    GameObject robberObj;

    // settlement and road visualizer components
    public SettlementVisualizerState issettlement;
    public RoadVisualizerState isroad;

    // create list of positions for tiles in board
    List<Vector3> tileposns = new List<Vector3>();
    // create list of unique settlements to avoid duplicates
    List<GameObject> uniqueset = new List<GameObject>();
    // create list of unique roads to avoid duplicates
    List<GameObject> uniqueroads = new List<GameObject>();
    // keep track of chit tiles and their associated game object
    Dictionary<int, GameObject> chitTiles = new Dictionary<int, GameObject>();
    // keep track of resource tiles and their associated game object
    Dictionary<string, GameObject> listTiles = new Dictionary<string, GameObject>();
    // maps settlements to their location on the board
    Dictionary<GameObject, Point> setlocations = new Dictionary<GameObject, Point>();
    // maps roads to their location on the board
    Dictionary<GameObject, Line> roadlocations = new Dictionary<GameObject, Line>();

    // Getting list of string player names from start menu
    List<string> _listPlayers = playerListContainer.listPlayers;

    UIDeviceInput _input;

    // buttons
    public Button rollButton;
    public Button endturnButton;
    bool _dicerolled = true;

    // gets the values of both dice
    List<int> _diceSideValues = diceSideValues.sideValues;

    // turn log message board
    string _turnlogmsg = "";
    GameObject turnlogobj;
    Text turnlogtxt;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<UIDeviceInput>();

        Dictionary<int, Tile> tiles = catan.Board.Tiles;

        computeTilePositions();

        listTiles["wood"] = lumber;
        listTiles["wheat"] = wheat;
        listTiles["brick"] = brick;
        listTiles["sheep"] = sheep;
        listTiles["ore"] = ore;
        listTiles["desert"] = desert;

        chitTiles[2] = two;
        chitTiles[3] = three;
        chitTiles[4] = four;
        chitTiles[5] = five;
        chitTiles[6] = six;
        chitTiles[8] = eight;
        chitTiles[9] = nine;
        chitTiles[10] = ten;
        chitTiles[11] = eleven;
        chitTiles[12] = twelve;

        int i = 0;
        foreach (KeyValuePair<int, Tile> tile in tiles)
        {
            // used to keep track of the locations of settlements of a tile
            List<GameObject> settlementsclose = new List<GameObject>();
            
            // tile corners (settlements)
            List<Point> corners = tile.Value.Corners;
            // tile lines (roads)
            List<Line> roads = tile.Value.Roads;

            string resource = tile.Value.Resource;
            int chit = tile.Value.Chit;
            
            // compute the location of resource tiles 
            GameObject resourceObject = GameObject.Instantiate(listTiles[resource], tileposns[i], Quaternion.identity);
            // scaling and rotating
            resourceObject.transform.localScale = new Vector3(50f, 50f, 1f);
            
            resourceObject.SetActive(true);
            
            // compute location of settlements and cities
            createSettlementObjects(settlementsclose, corners, i);
            // compute location of roads
            createRoadObjects(settlementsclose, roads, i);

            Vector3 posn = tileposns[i];

            // compute the location of chit tiles
            if (chit != 7)
            {
                posn.z = 130;
                GameObject chitObject = GameObject.Instantiate(chitTiles[chit], posn, Quaternion.identity);
                chitObject.transform.localScale = new Vector3(50f, 5f, 50f);
                chitObject.transform.rotation = Quaternion.Euler(90,0,0);
                chitObject.SetActive(true);
            }
            // set position of the robber
            else
            {
                createRobber(posn);
            }
            i++;
        }
        // create player instances for each player name entered in
        // the start menu
        foreach (string player in _listPlayers)
        {
            catan.AddPlayer(player);
        }

        endturnButton.onClick.AddListener(endTurnClicked);

        // getting the turnlog scroll view object
        turnlogobj = GameObject.Find("turnlog/Viewport/Content/Text");
        // set text of the turnlogobj which is initially empty
        turnlogtxt = turnlogobj.GetComponent<Text>();
        turnlogtxt.text = "Player 1 goes first. \n \nWelcome to Digital Catan! To start, each player will take turns placing one road and one settlement. The order snakes, until each player has placed 2 roads and 2 settlements.";

    }

    // requires: nothing
    // effects: computes the location of each tile on the board
    private void computeTilePositions()
    {
        int xpos = 300;
        int ypos = 580;
        int zpos = 140;
        for (int p = 0; p < 5; p++)
        {
            // 5 tiles
            if (p == 2)
            {
                xpos -= 3*60;
                for (int j = 0; j < 5; j++)
                {
                    tileposns.Add(new Vector3(xpos,ypos,zpos));
                    xpos += 130;
                }
            }
            // 3 tiles
            if (p == 0 || p == 4)
            {
                if (p == 0)
                {
                    xpos -= 60;
                }
                else
                {
                    xpos -= 46;
                }
                for (int j = 0; j < 3; j++)
                {
                    tileposns.Add(new Vector3(xpos,ypos,zpos));
                    xpos += 130;
                } 
            }
            // 4 tiles
            if (p == 1 || p == 3)
            {
                if (p == 1)
                {
                    xpos -= 2*60;
                }
                else
                {
                    xpos -= 2*57;
                }
                for (int j = 0; j < 4; j++)
                {
                    tileposns.Add(new Vector3(xpos,ypos,zpos));
                    xpos += 130;
                }
            }
            xpos = 300;
            ypos -= 110;
        }
    }

    // requires: a valid list of game objects containing all of the settlements close by
    // to avoid the creation of many game objects, a valid list of points containing
    // the relative location of the settlements to each tile, and a valid index
    // effects: creates a set of settlement / city objects for each tile
    private void createSettlementObjects(List<GameObject> settlementsclose, List<Point> corners, int i)
    {
        float theta = 30*Mathf.Deg2Rad;
        int r = 70;
        
        for (int j = 0; j < 6; j++)
        {
            float x = r*Mathf.Cos(theta) + tileposns[i].x;
            float y = r*Mathf.Sin(theta) + tileposns[i].y; 
            Vector3 settlementpos = new Vector3(x, y, tileposns[i].z);

            GameObject settlementObject = GameObject.Instantiate(settlement, settlementpos, Quaternion.identity);
            theta += 60*Mathf.Deg2Rad;

            Bounds settlementbounds = settlementObject.GetComponent<Renderer>().bounds;
            bool iscollides = false;
                            
            List<GameObject> tmp = new List<GameObject>(uniqueset);
            foreach (GameObject gameobj in tmp)
            {
                Bounds otherbounds = gameobj.GetComponent<Renderer>().bounds;
                if (settlementbounds.Intersects(otherbounds))
                {
                    iscollides = true;
                    settlementsclose.Add(gameobj);
                    break;
                }
            }
            if (iscollides == false)
            {
                uniqueset.Add(settlementObject);
                setlocations[settlementObject] = corners[corners.Count-1-j];
                settlementsclose.Add(settlementObject);
                settlementObject.SetActive(true);
            }
        }
    }

    // requires: a valid list of game objects containing all of the settlements close by
    // to avoid the creation of many game objects, a valid list of lines containing
    // the start and end position points of a road relative to each tile, and a valid index
    // effects: creates a set of road objects and rotates them accordingly
    private void createRoadObjects(List<GameObject> settlementsclose, List<Line> roads, int i)
    {
        float ctheta = 90*Mathf.Deg2Rad + 30;
        int cr = 70;

        for (int k = 0; k < 6; k++)
        {
            float cx = cr*Mathf.Cos(ctheta) + tileposns[i].x;
            float cz = cr*Mathf.Sin(ctheta) + tileposns[i].z; 
            Vector3 roadpos = new Vector3(cx, tileposns[i].y, cz);

            GameObject roadObject = GameObject.Instantiate(road, roadpos, Quaternion.identity);
            ctheta += 60*Mathf.Deg2Rad;
            
            // rotate roads to align with settlements
            if (k < settlementsclose.Count - 1)
            {
                Vector3 pos1 = settlementsclose[k].transform.position;
                Vector3 pos2 = settlementsclose[k+1].transform.position;
                Vector3 dir = pos2 - pos1;
                roadObject.transform.position = 0.5f * (pos2 + pos1);
                roadObject.transform.localScale = new Vector3(17f, 0.6f * dir.magnitude, 17f);
                roadObject.transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(90,0,0);
            }
            else
            {
                Vector3 pos1 = settlementsclose[0].transform.position;
                Vector3 pos2 = settlementsclose[5].transform.position;
                Vector3 dir = pos2 - pos1;
                roadObject.transform.position = 0.5f * (pos2 + pos1);
                roadObject.transform.localScale = new Vector3(17f, 0.6f * dir.magnitude, 17f);
                roadObject.transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(90,0,0);
            }
            
            // calculate bounds 
            Bounds roadbounds = roadObject.GetComponent<Renderer>().bounds;
            bool ccollides = false;
                            
            List<GameObject> tmp = new List<GameObject>(uniqueroads);
            foreach (GameObject gameobj in tmp)
            {
                Bounds otherbounds = gameobj.GetComponent<Renderer>().bounds;
                if (roadbounds.Intersects(otherbounds) && roadObject.transform.position == gameobj.transform.position)
                {
                    ccollides = true;
                    break;
                }
            }
            if (ccollides == false)
            {
                uniqueroads.Add(roadObject);
                roadlocations[roadObject] = roads[k];
                roadObject.SetActive(true);
            }

        }
    }

    // requires: a valid position 
    // effects: creates a robber (game object) at the given position on
    // the game board
    private void createRobber(Vector3 posn)
    {
        robberObj = GameObject.Instantiate(robber, posn, Quaternion.identity);
        robberObj.transform.localScale = new Vector3(30f, 30f, 0f);
        robberObj.SetActive(true);
    }

    // requires: the dice was rolled 
    // effects: determines the value of the dice and updates the board
    // accordingly; also makes the roll button un-interactable if dice
    // have been rolled
    private void updateRolled()
    {
        int die1 = _diceSideValues[_diceSideValues.Count-2];
        int die2 = _diceSideValues[_diceSideValues.Count-1];
        int sum = die1 + die2;
        if (sum == 7)
        {
            StartCoroutine("sevenRolled");
        }
        else
        {
            catan.CollectResources(die1, die2);
            _turnlogmsg = catan.TurnLog.Message;
            turnlogtxt.text = _turnlogmsg + Environment.NewLine + turnlogtxt.text;
            // update cards after rolling
            updateResourceCards(catan.PlayersTurn);
        }
        _dicerolled = true;
        rollButton.interactable = false;    
    }

    // requires: a seven was rolled
    // effects: updates the game board message and moves the robber 
    private IEnumerator sevenRolled()
    {
        catan.TurnLog.RobberMessage();
        _turnlogmsg = catan.TurnLog.Message;
        turnlogtxt.text = _turnlogmsg + Environment.NewLine + turnlogtxt.text;
        int index = 0;
        bool tileclicked = false;
        while (!tileclicked)
        {
            Transform checktransform = _input.ComputeHitObject();
            bool buttonPressed = _input.ButtonPressed();
            if (checktransform != null)
            {
                Mesh mesh = checktransform.gameObject.GetComponent<MeshFilter>().mesh;
                if (mesh.name == "Cylinder Instance" && buttonPressed == true)
                {
                    Vector3 posn = checktransform.position;
                    Vector3 robberposn = new Vector3(posn.x, posn.y, 120);
                    robberObj.transform.position = robberposn;

                    foreach (Vector3 objposn in tileposns)
                    {
                        Vector3 newposn = new Vector3(posn.x, posn.y, 140);
                        if (newposn == objposn)
                        {
                            index = tileposns.IndexOf(objposn);
                        }
                    }
                    tileclicked = true;
                }
            }
            yield return null;
        }
        catan.MoveRobber(index, catan.PlayersTurn);
        _turnlogmsg = catan.TurnLog.Message;
        turnlogtxt.text = _turnlogmsg + Environment.NewLine + turnlogtxt.text;
    }
    
    // requires: end button was clicked
    // requires: end button was clicked
    // effects: makes roll button interactable and advances the game
    private void endTurnClicked()
    {
        if (catan.Turn >= 0 && diceSideValues._dicerolled)
        {
            _dicerolled = false;
            diceSideValues._dicerolled = false;
            rollButton.interactable = true;
            catan.EndTurn();
            checkWinner();
            // if not at the end of the game, update cards to reflect
            // current player's holdings
            updateResourceCards(catan.PlayersTurn);
        }
    }

    // requires nothing
    // effects: updates the resource cards to show the current 
    // players' holdings
    private void updateResourceCards(Player player)
    {
        List<string> resources = player.Resources;
    
        foreach(string resource in player.PossibleResources)
        {
            int resourceq = player.ResourceCount(resource);
            string findobj = $"{resource}Q";
            GameObject resourceobjtxt = GameObject.Find(findobj);
            Text txt = resourceobjtxt.GetComponent<Text>();
            txt.text = $"x{resourceq}";
        }
    }

    // requires: nothing
    // effects: determines whether someone has won the game and if so,
    // changes the scene to display the winner
    private void checkWinner()
    {
        foreach (Player player in catan.Players)
        {
            if (player.VictoryPoints >= 10)
            {
                winner._playername = player.Name;
                SceneManager.LoadScene("victoryscreen");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform checktransform = _input.ComputeHitObject();
        bool buttonPressed = _input.ButtonPressed();

        // User cannot make a move if they have not yet rolled
        if (checktransform != null && _dicerolled == true)
        {
            Mesh mesh = checktransform.gameObject.GetComponent<MeshFilter>().mesh;

            // Player clicked a settlement
            if (mesh.name == "Cube Instance" && buttonPressed == true)
            {
                issettlement = checktransform.gameObject.GetComponent<SettlementVisualizerState>();
                Player player = catan.PlayersTurn;
                
                bool canbuy = catan.SettlementClicked(setlocations[checktransform.gameObject],player);
                _turnlogmsg = catan.TurnLog.Message;
                turnlogtxt.text = _turnlogmsg + Environment.NewLine + turnlogtxt.text;
                if (canbuy)
                {
                    issettlement.PlayerColor = player.Color;
                    issettlement.Selected = true;
                    updateResourceCards(catan.PlayersTurn);
                }

                // Checks whether player tried to upgrade a settlement
                else if (!canbuy && issettlement.PlayerColor == player.Color)
                {
                    canbuy = catan.CityClicked(setlocations[checktransform.gameObject],player);
                    if (canbuy)
                    {
                        issettlement.UpdateCityColor();
                        updateResourceCards(catan.PlayersTurn);
                    }
                } 
            }

            // Player clicked a road
            else if (mesh.name == "Cylinder Instance" && buttonPressed == true && checktransform.gameObject.GetComponent<RoadVisualizerState>() != null)
            {
                isroad = checktransform.gameObject.GetComponent<RoadVisualizerState>();
                Player player = catan.PlayersTurn;
                bool canbuy = catan.RoadClicked(roadlocations[checktransform.gameObject], player);

                _turnlogmsg = catan.TurnLog.Message;
                turnlogtxt.text = catan.TurnLog.SetupMessage + _turnlogmsg + Environment.NewLine + turnlogtxt.text;

                // Player can buy selected road
                if (canbuy)
                {
                    isroad.PlayerColor = player.Color;
                    isroad.Selected = true;
                    updateResourceCards(catan.PlayersTurn);
                }
            }
        }

        // Updates turnlog message
        if (_turnlogmsg != catan.TurnLog.Message)
        {
            _turnlogmsg = catan.TurnLog.Message;
            turnlogtxt.text = _turnlogmsg + Environment.NewLine + turnlogtxt.text;
        }

        // check if rolled
        if (diceSideValues._dicerolled == true && rollButton.interactable == true)
        {
            updateRolled();
        }
    }
}