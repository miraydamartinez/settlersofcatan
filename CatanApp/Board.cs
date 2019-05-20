using System;
using System.Collections.Generic;
using System.Drawing;

namespace CatanApp
{
    public class Board
    {
        // Stores board's tiles with int identifiers
        // int identifiers correspond to position, with 0 in top left and 18 in bottom right
        Dictionary<int, Tile> _board  = new Dictionary<int, Tile>();

        // Contains all possible chit values for tiles
        List<int> chits = new List<int>()
        {2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12};

        // // Tile that holds the robber
        // Tile _tile_robber;

        int robber_tile = 0;

        // Dictionary containing claimed settlements
        Dictionary<Point, Player> _claimed_settlements = new Dictionary<Point, Player>();

        // Dictionary containing claimed roads
        Dictionary<Line, Player> _claimed_roads = new Dictionary<Line, Player>();

        // List of players
        List<Player> _players = new List<Player>();

        // Contains all possible resource values for tiles
        List<string> resources = new List<string>()
        {"wood", "wood", "wood", "wood",
        "wheat","wheat","wheat","wheat",
        "brick","brick","brick",
        "sheep","sheep","sheep","sheep",
        "ore","ore","ore",
        "desert"};

        // Adds corner game logic coordinates to each tile
        //static List<List<Point>> _tile_pts = new List<List<Point>>();

        // Controls randomization and seeds for testing
        Random random_res;
        Random random_chit;

        // Simple constructor for unit testing
        public Board()
        {
            random_res = new Random();
            random_chit = new Random();
            RandomizeBoard();
            AddTilePts();
            
            bool roads_added = AddTileRoads();

            if (!roads_added)
            {
                throw new Exception("Roads not properly added");
            }
        }

        // Testing constructor using seed
        public Board(int seed)
        {
            
            random_res = new Random(seed);
            random_chit = new Random(seed);
            RandomizeBoard();
            AddTilePts();
            bool roads_added = AddTileRoads();

            if (!roads_added)
            {
                throw new Exception("Roads not properly added");
            }
        }

        // requires: nothing
        // effects: adds player to game
        public void AddPlayer(Player player)
        {
            Console.WriteLine("Player was added");
            _players.Add(player);
        }

        public void AddPlayer(string player)
        {
            Player new_player = new Player(player);
            _players.Add(new_player);
        }

        // requires: valid game board
        // effects: initializes gameboard and randomizes positions and chits of resource tiles
        public void RandomizeBoard()
        {
            // Sets up each of board's 19 tiles
            for (int i = 0; i < 19; i++)
            {
                Tile tile_curr = new Tile();

                // Sets resource for current tile
                string res = resources[random_res.Next(0, resources.Count)];
                resources.Remove(res);            
                tile_curr.Resource = res;


                // Sets chit for tile

                // Tile is not desert
                if (tile_curr.Resource != "desert")
                {
                    int chit = chits[random_chit.Next(0, chits.Count)];
                    chits.Remove(chit);
                    tile_curr.Chit = chit;
                }

                // Tile is desert, Robber will start there
                else
                {
                    tile_curr.Robber = true;
                    //_tile_robber = tile_curr; 
                }

                //tile_curr.Corners = _tile_pts[i];

                _board.Add(i, tile_curr);
            }
        }

        // requires: user selected location on board corresponding to a potential settlement
        // effects: returns bool determining whether user could buy location
        public bool PlaceSettlement(Point pt, Player player)
        {
            // Settlement already claimed
            if (_claimed_settlements.ContainsKey(pt))
            {
                Console.WriteLine("Point already in dict");
                return false;
            }

            int x = pt.X;
            int y = pt.Y;

            // Checks no adjacent settlements
            // settlements of the form (even,even) and (odd,odd) check below, left, and right
            if ((x % 2 == 0  && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1))
            {
                //Console.WriteLine("Entered even y condtional");
                if (_claimed_settlements.ContainsKey(new Point(pt.X, pt.Y + 1)))
                {
                    Console.WriteLine("Settlement below");
                    return false;
                }

                if (_claimed_settlements.ContainsKey(new Point(pt.X - 1, pt.Y)))
                {
                    Console.WriteLine("Settlement to left");
                    return false;
                }

                if (_claimed_settlements.ContainsKey(new Point(pt.X + 1, pt.Y)))
                {
                    Console.WriteLine("Settlement to right");
                    return false;
                }
            }

            // settlements of the form (odd,even) and (even,odd) check below, left, and right
            else
            {
                if (_claimed_settlements.ContainsKey(new Point(pt.X, pt.Y - 1)))
                {
                    Console.WriteLine("Settlement above");
                    return false;
                }

                if (_claimed_settlements.ContainsKey(new Point(pt.X - 1, pt.Y)))
                {
                    Console.WriteLine("Settlement to left");
                    return false;
                }

                if (_claimed_settlements.ContainsKey(new Point(pt.X + 1, pt.Y)))
                {
                    Console.WriteLine("Settlement to right");
                    return false;
                }
            }

            bool r_val = false;

            // checks that 4 players have each placed 2 initial roads
            Console.WriteLine("Number of players: {0}", _players.Count);
            if (_claimed_settlements.Count >= 2 * _players.Count)
            {
                Console.WriteLine("Not touching an existing thing");
                foreach (Line line_curr in _claimed_roads.Keys)
                {
                    Point start_curr = line_curr.Start;

                    Point end_curr = line_curr.End;

                    // Settlement touches the end of one of player's roads
                    if ((start_curr == pt || end_curr == pt) && _claimed_roads[line_curr] == player)
                    {
                        r_val = true;
                        player.AddVictoryPoint();
                        break;
                    }
                }
            }
            else
            {
                r_val = true;
                player.AddVictoryPoint();
            }

            // none of player's roads were desired touching settlement
            if (!r_val)
            {
                return r_val;
            }

            // add settlement to claimed settlements and adds player to list of players touching tiles for appropriate tiles
            _claimed_settlements.Add(pt, player);
            AddNewResources(pt, player);
            return true;            
        }

        // requires: user selected one of their settlements to upgrade
        // effects: returns a bool determining whether the user could upgrade that location
        public bool PlaceCity(Point pt, Player player)
        {
            if (_claimed_settlements.ContainsKey(pt))
            {
                if (_claimed_settlements[pt] == player)
                {
                    AddNewResources(pt, player);
                    player.AddVictoryPoint();
                    return true;
                }
            }
            return false;
        }

        // requires: user selected a road location to buy
        // effects: returns bool determining whether use could buy road
        public bool PlaceRoad(Line line, Player player)
        {
            // Road already claimed
            if (_claimed_roads.ContainsKey(line))
            {
                return false;
            }

            bool r_val = false;
            Point start = line.Start;
            Point end = line.End;

            // Player selected a road attached to a settlement
            if (_claimed_settlements.ContainsKey(start) && _claimed_settlements[start] == player)
            {
                r_val = true;
            }
            if (_claimed_settlements.ContainsKey(end) && _claimed_settlements[end] == player)
            {
                r_val = true;
            }

            // Checks whether selected road touches one of player's existing roads
            if (!r_val)
            {
                foreach (Line line_curr in _claimed_roads.Keys)
                {
                    if (_claimed_roads[line_curr] == player)
                    {
                        Point start_curr = line_curr.Start;
                        Point end_curr = line_curr.End;

                        if (start_curr == start || start_curr == end || end_curr == start || end_curr == end)
                        {
                            r_val = true;
                        }
                    }
                }
            }

            // User selected invalid road
            if (!r_val)
            {
                return r_val;
            }

            _claimed_roads.Add(line, player);
            return r_val;
        }

        // requires: _tile_pts initialized as a List<List<Point>>
        // effects: adds Lists containing points of each tile's corners to _tile_pts
        public void AddTilePts()
        {
            // Adds pts for tile 0
            List<Point> pts0 = new List<Point>()
            {
                
                new Point(4,1),
                new Point(3,1),
                new Point(2,1),
                new Point(2,0),
                new Point(3,0),
                new Point(4,0)
            };
            _board[0].Corners = pts0;
            //_tile_pts.Add(pts0);

            // Adds pts for tile 1
            List<Point> pts1 = new List<Point>()
            {
                
                new Point(6,1),
                new Point(5,1),
                new Point(4,1),
                new Point(4,0),
                new Point(5,0),
                new Point(6,0)
            };
            _board[1].Corners = pts1;
            //_tile_pts.Add(pts1);

            // Adds pts for tile 2
            List<Point> pts2 = new List<Point>()
            {
                
                new Point(8,1),
                new Point(7,1),
                new Point(6,1),
                new Point(6,0),
                new Point(7,0),
                new Point(8,0)
            };
            _board[2].Corners = pts2;
            //_tile_pts.Add(pts2);

            // Adds pts for tile 3
            List<Point> pts3 = new List<Point>()
            {
                new Point(3,2),
                new Point(2,2),
                new Point(1,2),
                new Point(1,1),
                new Point(2,1),
                new Point(3,1)
            };
            _board[3].Corners = pts3;
            //_tile_pts.Add(pts3);

            // Adds pts for tile 4
            List<Point> pts4 = new List<Point>()
            {
                
                new Point(5,2),
                new Point(4,2),
                new Point(3,2),
                new Point(3,1),
                new Point(4,1),
                new Point(5,1)
            };
            _board[4].Corners = pts4;
            //_tile_pts.Add(pts4);

            // Adds pts for tile 5
            List<Point> pts5 = new List<Point>()
            {
                
                new Point(7,2),
                new Point(6,2),
                new Point(5,2),
                new Point(5,1),
                new Point(6,1),
                new Point(7,1)
            };
            _board[5].Corners = pts5;
            //_tile_pts.Add(pts5);

            // Adds pts for tile 6
            List<Point> pts6 = new List<Point>()
            {
                
                new Point(9,2),
                new Point(8,2),
                new Point(7,2),
                new Point(7,1),
                new Point(8,1),
                new Point(9,1)
            };
            _board[6].Corners = pts6;
            //_tile_pts.Add(pts6);

            // Adds pts for tile 7
            List<Point> pts7 = new List<Point>()
            {
                
                new Point(2,3),
                new Point(1,3),
                new Point(0,3),
                new Point(0,2),
                new Point(1,2),
                new Point(2,2)
            };
            _board[7].Corners = pts7;
            //_tile_pts.Add(pts7);

            // Adds pts for tile 8
            List<Point> pts8 = new List<Point>()
            {
                
                new Point(4,3),
                new Point(3,3),
                new Point(2,3),
                new Point(2,2),
                new Point(3,2),
                new Point(4,2)
            };
            _board[8].Corners = pts8;
            //_tile_pts.Add(pts8);

            // Adds pts for tile 9
            List<Point> pts9 = new List<Point>()
            {
                
                new Point(6,3),
                new Point(5,3),
                new Point(4,3),
                new Point(4,2),
                new Point(5,2),
                new Point(6,2)
            };
            _board[9].Corners = pts9;
            //_tile_pts.Add(pts9);

            // Adds pts for tile 10
            List<Point> pts10 = new List<Point>()
            {
                
                new Point(8,3),
                new Point(7,3),
                new Point(6,3),
                new Point(6,2),
                new Point(7,2),
                new Point(8,2)
            };
            _board[10].Corners = pts10;
            //_tile_pts.Add(pts10);

            // Adds pts for tile 11
            List<Point> pts11 = new List<Point>()
            {
                
                new Point(10,3),
                new Point(9,3),
                new Point(8,3),
                new Point(8,2),
                new Point(9,2),
                new Point(10,2)
            };
            _board[11].Corners = pts11;
            //_tile_pts.Add(pts11);

            // Adds pts for tile 12
            List<Point> pts12 = new List<Point>()
            {
                
                new Point(3,4),
                new Point(2,4),
                new Point(1,4),
                new Point(1,3),
                new Point(2,3),
                new Point(3,3)
            };
            _board[12].Corners = pts12;
            //_tile_pts.Add(pts12);

            // Adds pts for tile 13
            List<Point> pts13 = new List<Point>()
            {
                
                new Point(5,4),
                new Point(4,4),
                new Point(3,4),
                new Point(3,3),
                new Point(4,3),
                new Point(5,3)
            };
            _board[13].Corners = pts13;
            //_tile_pts.Add(pts13);

            // Adds pts for tile 14
            List<Point> pts14 = new List<Point>()
            {
                
                new Point(7,4),
                new Point(6,4),
                new Point(5,4),
                new Point(5,3),
                new Point(6,3),
                new Point(7,3)
            };
            _board[14].Corners = pts14;
            //_tile_pts.Add(pts14);
            
            // Adds pts for tile 15
            List<Point> pts15 = new List<Point>()
            {
                
                new Point(9,4),
                new Point(8,4),
                new Point(7,4),
                new Point(7,3),
                new Point(8,3),
                new Point(9,3)
            };
            _board[15].Corners = pts15;
            //_tile_pts.Add(pts15);

            // Adds pts for tile 16
            List<Point> pts16 = new List<Point>()
            {
                
                new Point(4,5),
                new Point(3,5),
                new Point(2,5),
                new Point(2,4),
                new Point(3,4),
                new Point(4,4)
            };
            _board[16].Corners = pts16;
            //_tile_pts.Add(pts16);

            // Adds pts for tile 17
            List<Point> pts17 = new List<Point>()
            {
                
                new Point(6,5),
                new Point(5,5),
                new Point(4,5),
                new Point(4,4),
                new Point(5,4),
                new Point(6,4)
            };
            _board[17].Corners = pts17;
            //_tile_pts.Add(pts17);

            // Adds pts for tile 18
            List<Point> pts18 = new List<Point>()
            {
                
                new Point(8,5),
                new Point(7,5),
                new Point(6,5),
                new Point(6,4),
                new Point(7,4),
                new Point(8,4)
            };
            _board[18].Corners = pts18;
            //_tile_pts.Add(pts18);
        }

        // requires: nothing
        // effects: adds roads to each tile. returns false if any tile's road-adding process fails
        public bool AddTileRoads()
        {
            bool r_val = true;
            for (int i = 0; i < 19; i++)
            {
                bool road_added = _board[i].AddRoads();

                // tile's road-adding process failed
                if (!road_added)
                {
                    r_val = false;
                }
            }
            return r_val;
        }

        // requires: valid player and point parameters and that user just placed city or settlement
        // effects: adds player to list of players touching a given settlement after player places
        //          settlement or city
        private void AddNewResources(Point pt, Player player)
        {
            for (int i = 0; i < 19; i++)
            {
                if(_board[i].Corners.Contains(pt))
                {
                    _board[i].AddPlayer(player);

                    // Player gets resource if placing an initial settlement
                    if (_claimed_settlements.Count <= 2 * _players.Count)
                    {
                        //Console.WriteLine("{0} added {1} by claiming a settlement", player.Name, _board[i].Resource);
                        //Console.WriteLine("Number of claimed settlements: {0}", _claimed_settlements.Count);
                        player.AddResource(_board[i].Resource);
                    }
                }
            }
        }

        public void MoveRobber(int tile_id)
        {
            Tile new_robbertile = _board[tile_id];

            Tile curr_robber = _board[0];

            for(int i = 0; i < _board.Count; i++)
            {
                if (_board[i].Robber)
                {
                    curr_robber = _board[i];
                }
            }
            curr_robber.Robber = false;
            new_robbertile.Robber = true;

            robber_tile = tile_id;
        }

        // // getters and setters for tile robber is on
        // public Tile RobberTile
        // {
        //     get {  return _tile_robber; }
        //     set { 
        //             _tile_robber.Robber = false;
        //             _tile_robber = value; 
        //             _tile_robber.Robber = true;
        //         }
        // }

        // Returns dictionary containing board's tiles
        public Dictionary<int, Tile> Tiles
        {
            get { return _board; }
        }

        public int RobberTileID
        {
            get { return robber_tile;  }
        }
    }

    // Contains information for each tile on board
    public class Tile
    {
        // defaults to 7 for desert
        private int _chit = 7;
        private string _resource;

        private bool _robber = false;

        // list of players with settlements touching tile
        List<Player> _players_touching = new List<Player>();

        List<Point> _corners = new List<Point>();

        List<Line> _roads = new List<Line>();


        // requires: valid player that tried to place a settlement or city on tile
        // effects: adds player to list of players touching tile. if player is
        //          upgrading to a city, they will be added to the list a second time
        public void AddPlayer(Player player)
        {
            _players_touching.Add(player);
        }

        // requires: nothing
        // effects: adds tile's roads to _roads based on tile's corners. returns false if corners not yet set
        public bool AddRoads()
        {
            if (_corners.Count != 6)
            {
                return false;
            }

            else
            {
                for (int i = 5; i >= 0; i--)
                {
                    // catch negative indices
                    int low_index = i-1;
                    if (low_index < 0)
                    {
                        low_index = 5;
                    }

                    Line line_curr = new Line(_corners[i], _corners[low_index]);
                    _roads.Add(line_curr);
                }
            }
            return true;
        }

        public int Chit
        {
            get { return _chit; }
            set { _chit = value; }
        }

        public string Resource
        {
            get { return _resource; }
            set { _resource = value; }
        }

        public bool Robber
        {
            get { return _robber; }
            set { _robber = value; }
        }

        public List<Player> Players
        {
            get { return _players_touching; }
        }

        public List<Point> Corners
        {
            get { return _corners; }
            set { _corners = value; }
        }

        public List<Line> Roads
        {
            get { return _roads; }
            set { _roads = value; }
        }
    }

    // line class used for roads
    // can be interacted with by accessing x1, x2, y1, y2, or with start and end points
    public class Line
    {
        int _X1 = 0;
        int _X2 = 0;
        int _Y1 = 0;
        int _Y2 = 0;

        Point _start = new Point(0,0);
        Point _end = new Point(0,0);
        
        // Empty constructor
        public Line()
        {
            _X1 = 0;
        }

        // Constructor that takes in points
        public Line(Point start, Point end)
        {   
            _start = start;
            _end = end;
            _X1 = _start.X;
            _Y1 = _start.Y;
            _X2 = _end.X;
            _Y2 = _end.Y;
        }

        public override string ToString()
        {
            string x1_str = _X1.ToString();
            string y1_str = _Y1.ToString();
            string x2_str = _X2.ToString();
            string y2_str = _Y2.ToString();
            return "("+x1_str+","+y1_str+") ("+x2_str+","+y2_str+")";
        }

        public Point Start
        {
            get { return new Point(_X1, _Y1); }
            set { 
                    _start = value; 
                    _X1 = _start.X;
                    _Y1 = _start.Y;
                }
        }

        public Point End
        {
            get { return new Point(_X2, _Y2); }
            set { 
                    _end = value; 
                    _X2 = _end.X;
                    _Y2 = _end.Y;
                }
        }

        public int X1
        {
            get { return _X1; }
            set {
                    _X1 = value;
                    _start = new Point(value, _Y1);
                }
        }

        public int X2
        {
            get { return _X2; }
            set {
                    _X2 = value;
                    _end = new Point(value, _Y2);
                }
        }

        public int Y1
        {
            get { return _Y1; }
            set {
                    _Y1 = value;
                    _start = new Point(_X1, value);
                }
        }

        public int Y2
        {
            get { return _Y2; }
            set {
                    _Y2 = value;
                    _end = new Point(_X2, value);
                }
        }

        // Equals override so lines with same points are equal, even if they're in different order
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            Line line = (Line) obj;

            // Lines should be equal even if start and end points are different
            if ((line.Start == _start && line.End == _end) || (line.Start == _end && line.End == _start))
            {
                return true;
            }

            return false;
        }

        // Overrides GetHashCode so that objects deemed equal have same hashcode
        public override int GetHashCode()
        {
            int hash_start = _start.GetHashCode();
            int hash_end = _end.GetHashCode();

            int hash_sum = hash_start + hash_end;

            return hash_sum.GetHashCode();
        }
    }
}