using System;
using System.Collections.Generic;
using System.Drawing;

namespace CatanApp
{
    public class Catan
    {
        // game's turn
        int _turn = 0;

        Point _setup_settlement = new Point(0,0);

        int _player_turn_ind = 0;

        bool _settlement_lock = false;
        bool _road_lock = false;

        // list containing all players
        List<Player> _players = new List<Player>();

        List<string> _colors = new List<string>()
        {"blue", "red", "green", "yellow"};

        // gameboard
        Board _gameboard;

        // game's turnlog box class
        TurnLog _turnlog = new TurnLog();

        // constructor
        public Catan()
        {
            // builds gameboard
            _gameboard = new Board();
        }

        // Seed constructor for testing
        public Catan(int seed)
        {
            _gameboard = new Board(seed);
        }

        // requires: nothing
        // effects: creates player, adds them to game's list of players, and assigns color
        public void AddPlayer(string player)
        {
            if (player != "" && player != " ")
            {
                Player new_player = new Player(player);
                new_player.Color = _colors[0];
                _colors.RemoveAt(0);
                _players.Add(new_player);
                _gameboard.AddPlayer(new_player);
                _turn -= 2;
            }
        }

        // requires: nothing
        // effects: creates player, adds them to game's list of players, and assigns color
        public void AddPlayer(Player player)
        {
            player.Color = _colors[0];
            _colors.RemoveAt(0);
            _players.Add(player);
            _gameboard.AddPlayer(player);
            _turn -= 2;
        }

        // requires: player rolled for their turn and did not get 7
        // effects: collects resources for players
        public void CollectResources(int die1, int die2)
        {
            int roll = die1 + die2;

            _turnlog.ResourcesCollectedMessage(die1, die2);
            
            for (int i = 0; i < _gameboard.Tiles.Count; i++)
            {
                if (_gameboard.Tiles[i].Chit == roll && !_gameboard.Tiles[i].Robber)
                {
                    foreach (Player player_curr in _gameboard.Tiles[i].Players)
                    {
                        //Console.WriteLine("{0} collected {1}", player_curr.Name, _gameboard.Tiles[i].Resource);
                        player_curr.AddResource(_gameboard.Tiles[i].Resource);
                    }
                }
            }
        }

        // requires: nothing
        // effects: updates gamestate and turnlog to indicate robber was moved
        public void MoveRobber(int robber_tile_id, Player player)
        {
            _gameboard.MoveRobber(robber_tile_id);
            _turnlog.RobberMovedMessage(player);
        }

        // requires: player clicked a settlement
        // effects: updates gamestate appropriately and returns string for turnlog about what happened
        public bool SettlementClicked(Point pt, Player player)
        {
            bool placed = false;
            // Player has resources to build settlement or game is still in initial setup
            if (player.CanBuildSettlement() || (_turn < 0 && !_settlement_lock))
            {
                placed = _gameboard.PlaceSettlement(pt, player);

                // removes resources if player successfully placed settlement
                if (placed && _turn >= 0)
                {
                    player.RemoveResource("wood");
                    player.RemoveResource("brick");
                    player.RemoveResource("wheat");
                    player.RemoveResource("sheep");
                }

                else if (placed && _turn < 0)
                {
                    _settlement_lock = true;
                    _setup_settlement = pt;

                    if (_road_lock && _settlement_lock)
                    {
                        _road_lock = false;
                        _settlement_lock = false;

                        // Allows for snaking turns at start of game
                        if (_turn >= -1 * _players.Count)
                        {
                            _player_turn_ind--;
                        }

                        else if (_player_turn_ind < _players.Count - 1)
                        {
                            _player_turn_ind++;
                        }
                        _turn++;
                        _turnlog.PlayerTurnMessage(this.PlayersTurn);
                    }
                }
            }

            // Update turnlog settlement message
            _turnlog.SettlementMessage(player, placed);
           
            return placed;
        }

        // requires: player clicked a road
        // effects: updates gamestate appropriately and returns string for turnlog about what happened
        public bool RoadClicked(Line line, Player player)
        {
            bool placed = false;

            bool _touches_start_settl = (line.Start == _setup_settlement || line.End == _setup_settlement);

            // Player has resources to build settlement or game is still in initial setup
            if ((player.CanBuildRoad() && _turn >= 0) || (_turn < 0 && !_road_lock && _settlement_lock && _touches_start_settl))
            {
                placed = _gameboard.PlaceRoad(line, player);
                _turnlog.RoadMessage(player, placed);

                // removes resources if player successfully placed settlement
                if (placed && _turn >= 0)
                {
                    _turnlog.EmptySetupMessage();
                    player.RemoveResource("wood");
                    player.RemoveResource("brick");
                }

                // game is still in initial setup
                else if (placed && _turn < 0)
                {
                    _road_lock = true;

                    // player has placed a road and a settlement
                    if (_road_lock && _settlement_lock)
                    {
                        _road_lock = false;
                        _settlement_lock = false;

                        // Allows for snaking turns at start of game
                        if (_turn >= -1 * _players.Count)
                        {
                            _player_turn_ind--;
                        }

                        else if (_player_turn_ind < _players.Count - 1)
                        {
                            _player_turn_ind++;
                        }
                        
                        _setup_settlement = new Point(0,0);
                        _turn++;

                        _turnlog.NewSetupTurn(this.PlayersTurn);
                        if (_turn == 0)
                        {
                            _turnlog.EndSetupMessage(this.PlayersTurn);
                        }
                    }
                }
                else if (!placed && _turn < 0)
                {
                    _turnlog.EmptySetupMessage();
                }
            }

            // Update turnlog for road placed message
            _turnlog.RoadMessage(player, placed);
            
            return placed;
        }

        // requires: player clicked a city
        // effects: updates gamestate appropriately and returns string for turnlog about what happened
        public bool CityClicked(Point pt, Player player)
        {
            bool placed  = false;
            // Player has resources to build settlement
            if (player.CanBuildCity())
            {
                placed = _gameboard.PlaceCity(pt, player);
                _turnlog.CityMessage(player, placed);

                // removes resources if player successfully placed settlement
                if (placed && _turn != 0)
                {
                    Console.WriteLine("Entered resource removal");
                    player.RemoveResource("ore");
                    player.RemoveResource("ore");
                    player.RemoveResource("ore");
                    player.RemoveResource("wheat");
                    player.RemoveResource("wheat");
                }
            }

            // Player does not have resources to build a city
            _turnlog.CityMessage(player, placed);

            return placed;
        }

        // requires: nothing
        // effects: advances turn
        public void EndTurn()
        {
            _turn += 1;
            _turnlog.EmptySetupMessage();
            _turnlog.PlayerTurnMessage(this.PlayersTurn);
        }

        // returns a list containing all players
        public List<Player> Players
        {
            get { return _players; }
        }

        // returns int representing the game's turn
        public int Turn
        {
            get { return _turn; }
        }

        // returns the player whose turn it is
        public Player PlayersTurn
        {
            get { 
                    if (_turn >= 0)
                    {
                        return _players[Math.Abs(_turn % _players.Count)];
                    }

                    else
                    {
                        return _players[_player_turn_ind];
                    }
                }
        }

        public int PTInd
        {
            get { return _player_turn_ind; }
        }

        // returns gameboard
        public Board Board
        {
            get { return _gameboard; }
        }

        // returns game's turnlog
        public TurnLog TurnLog
        {
            get { return _turnlog; }
        }
    }

    public class Player
    {
        string _name;

        // color respresenting player on board
        string _color;

        // how many victory points player has
        int _victory_pts = 0;

        // list containing resources user has
        List<string> _resources = new List<string>();

        // contains all possible resources player could obtain
        List<string> _possib_resources = new List<string>()
        {"sheep", "wood", "wheat", "brick", "ore"};

        // number of knights used by this player
        int _knights_used;

        public Player(string name)
        {
            _name = name;
        }

        // requires: nothing
        // effects: if string matches a valid resource, adds resource to player's list of resources
        public string AddResource(string resource)
        {
            if (_possib_resources.Contains(resource))
            {
                _resources.Add(resource);
                return resource;
            }

            return "";
        }

        // requires: nothing
        // effects: removes selected resource from player's hand
        public void RemoveResource(string resource)
        {
            if (_resources.Contains(resource))
            {
                _resources.Remove(resource);
            }
        }

        // requires: valid resource name
        // effects: returns how many of desired resource player has
        public int ResourceCount(string resource)
        {
            int count = 0;
            foreach (string res_curr in _resources)
            {
                if (res_curr == resource)
                {
                    count++;
                }
            }
            return count;
        }

        // requires: nothing
        // effects: returns bool indicating whether player has resources to build settlement
        public bool CanBuildSettlement()
        {
            bool canbuild = true;

            if (ResourceCount("wood") < 1)
            {
                canbuild = false;
            }

            if (ResourceCount("brick") < 1)
            {
                canbuild = false;
            }

            if (ResourceCount("sheep") < 1)
            {
                canbuild = false;
            }

            if (ResourceCount("wheat") < 1)
            {
                canbuild = false;
            }

            return canbuild;
        }

        // requires: nothing
        // effects: returns bool indicating whether player has resources to build road
        public bool CanBuildRoad()
        {
            bool canbuild = true;

            if (ResourceCount("wood") < 1)
            {
                canbuild = false;
            }

            if (ResourceCount("brick") < 1)
            {
                canbuild = false;
            }

            return canbuild;
        }

        // requires: nothing
        // effects: returns bool indicating whether player has resources to build city
        public bool CanBuildCity()
        {
            bool canbuild = true;

            if (ResourceCount("wheat") < 2)
            {
                canbuild = false;
            }

            if (ResourceCount("ore") < 3)
            {
                canbuild = false;
            }

            return canbuild;
        }

        // requires: nothing
        // effects: returns bool indicating whether player has resources to buy development card
        public bool CanBuyDevelopmentCard()
        {
            bool canbuild = true;

            if (ResourceCount("wheat") < 1)
            {
                canbuild = false;
            }

            if (ResourceCount("sheep") < 1)
            {
                canbuild = false;
            }

            if (ResourceCount("ore") < 1)
            {
                canbuild = false;
            }

            return canbuild;
        }

        // requires: player earned a victory point
        // effects: adds one victory point to player's total
        public void AddVictoryPoint()
        {
            _victory_pts++;
        }

        // requires: player used a knight
        // effects: adds one  knight used to player's knight count, 
        // checks if player can get largest army
        public void UsedKnight()
        {
            _knights_used++;
            if (_knights_used > 2)
            {
                _victory_pts += 2;
            }
        }

        // equals and gethashcode overrides
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            Player player = (Player) obj;

            return _name == player.Name;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public string Name
        {
            get { return _name; }
        }

        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public List<string> Resources
        {
            get { return _resources; }
        }

        public int VictoryPoints
        {
            get { return _victory_pts; }
        }

        public List<string> PossibleResources
        {
            get { return _possib_resources; }
        }
    }

}
