﻿using System.Collections.Generic;
using System.Linq;

namespace SampleModel
{
    public class Game
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Ranking { get; set; }
        public bool Active { get; set; }

        public string Rating { get; set; }
        public List<Player> Players { get; } = new List<Player>();

        public bool HasPlayer(int id) => Players.Any(p => p.Id == id);

        public void FlipActive() => Active = !Active;

        public static Game CreateGame() => new Game {Active = false, Name = "new"};
        public static Game CreateGame(string name) => new Game {Active = false, Name = name};
    }
}