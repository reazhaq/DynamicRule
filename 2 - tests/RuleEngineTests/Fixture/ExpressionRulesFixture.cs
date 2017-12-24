﻿using System;
using System.Linq;
using RuleEngineTests.Model;

namespace RuleEngineTests.Fixture
{
    public class ExpressionRulesFixture : IDisposable
    {
        public void Dispose(){}
        public Game Game1 { get; }
        public Game Game2 { get; }

        public ExpressionRulesFixture()
        {
            var someRandomNumber = new Random();
            Game1 = new Game
            {
                Name = "Game 1",
                Description = "super boring game",
                Active = false,
                Ranking = 99
            };
            Game1.Players.AddRange(Enumerable.Range(1, 40).Select(x => new Player
            {
                Id = x,
                Name = $"Player{x}",
                Country = new Country
                {
                    CountryCode = Country.Countries[someRandomNumber.Next(x, Country.Countries.Length - 1)]
                },
                CurrentScore = 100 - x,
                CurrentCoOrdinates = new CoOrdinate { X = x, Y = x }
            }));

            Game2 = new Game
            {
                Name = "Game 2",
                Description = "super cool game",
                Active = true,
                Ranking = 98
            };
            Game2.Players.AddRange(Enumerable.Range(1, 60).Select(x => new Player
            {
                Id = x,
                Name = $"Player{x}",
                Country = new Country
                {
                    CountryCode = Country.Countries[someRandomNumber.Next(x, Country.Countries.Length - 1)]
                },
                CurrentScore = 100 - x,
                CurrentCoOrdinates = new CoOrdinate { X = x, Y = x }
            }));
        }
    }
}