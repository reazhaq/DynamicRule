﻿using System;
using FluentAssertions;
using Newtonsoft.Json;
using RuleEngine.Rules;
using RuleFactory.Tests.Fixture;
using RuleFactory.Tests.Model;
using Xunit;
using Xunit.Abstractions;

namespace RuleFactory.Tests.JsonRules
{
    public class MethodCallRuleJsonTests : IClassFixture<ExpressionRulesFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Game _game1;
        private readonly Game _game2;

        public MethodCallRuleJsonTests(ExpressionRulesFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _game1 = fixture.Game1;
            _game2 = fixture.Game2;
            _testOutputHelper = testOutputHelper;
        }
        
        [Theory]
        [InlineData("Game 1", true)]
        [InlineData("game 1", true)]
        [InlineData("game 2", false)]
        [InlineData("gaMe 2", false)]
        public void CallEqualsMethodOnNameUsingConstantRule(string input1, bool expectedResult)
        {
            // call Equals method on Name string object
            // compiles to: Param_0.Name.Equals("Game 1", CurrentCultureIgnoreCase)
            var rule = new MethodCallRule<Game, bool>
            {
                ObjectToCallMethodOn = "Name",
                MethodToCall = "Equals",
                MethodParameters = { new ConstantRule<string> { Value = input1 },
                    new ConstantRule<StringComparison> { Value = "CurrentCultureIgnoreCase" } }
            };

            var compileResult = rule.Compile();
            compileResult.Should().BeTrue();
            _testOutputHelper.WriteLine($"{nameof(rule)}:{Environment.NewLine}{rule.ExpressionDebugView()}");

            var executeResult = rule.Execute(_game1);
            executeResult.Should().Be(expectedResult);

            executeResult = rule.Execute(_game2);
            executeResult.Should().Be(!expectedResult);

            // convert to json
            var ruleJson = JsonConvert.SerializeObject(rule, Formatting.Indented, new CustomRuleJsonConverter());
            _testOutputHelper.WriteLine($"{nameof(ruleJson)}:{Environment.NewLine}{ruleJson}");
            // re-hydrate from json
            var ruleFromJson = (MethodCallRule<Game, bool>)JsonConvert.DeserializeObject<Rule>(ruleJson, new CustomRuleJsonConverter());
            compileResult = ruleFromJson.Compile();
            compileResult.Should().BeTrue();
            _testOutputHelper.WriteLine($"{nameof(ruleFromJson)}:{Environment.NewLine}" +
                                        $"{ruleFromJson.ExpressionDebugView()}");

            var executeResult2 = ruleFromJson.Execute(_game1);
            executeResult2.Should().Be(expectedResult);
            executeResult2 = ruleFromJson.Execute(_game2);
            executeResult2.Should().Be(!expectedResult);
        }

        [Fact]
        public void CallAVoidMethod()
        {
            // call FlipActive method on the game object
            // compiles to: Param_0.FlipActive()
            var rule = new MethodVoidCallRule<Game>
            {
                MethodToCall = "FlipActive"
            };

            var compileResult = rule.Compile();
            compileResult.Should().BeTrue();
            _testOutputHelper.WriteLine($"{nameof(rule)}:{Environment.NewLine}{rule.ExpressionDebugView()}");

            var currentActiveState = _game1.Active;
            rule.Execute(_game1);
            _game1.Active.Should().Be(!currentActiveState);

            // convert to json
            var ruleJson = JsonConvert.SerializeObject(rule, Formatting.Indented, new CustomRuleJsonConverter());
            _testOutputHelper.WriteLine($"{nameof(ruleJson)}:{Environment.NewLine}{ruleJson}");
            // re-hydrate from json
            var ruleFromJson = (MethodVoidCallRule<Game>)JsonConvert.DeserializeObject<Rule>(ruleJson, new CustomRuleJsonConverter());
            compileResult = ruleFromJson.Compile();
            compileResult.Should().BeTrue();
            _testOutputHelper.WriteLine($"{nameof(ruleFromJson)}:{Environment.NewLine}" +
                                        $"{ruleFromJson.ExpressionDebugView()}");
            var currentActiveState2 = _game1.Active;
            ruleFromJson.Execute(_game1);
            _game1.Active.Should().Be(!currentActiveState2);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(1000, false)]
        public void CheckToSeeIfPlayerExistsInAGame(int id, bool expectedResult)
        {
            // call HasPlayer method on the game object
            // compiles to: Param_0.HasPlayer(1000)
            var rule = new MethodCallRule<Game, bool>
            {
                MethodToCall = "HasPlayer",
                MethodParameters = { new ConstantRule<int> { Value = id.ToString() } }
            };

            var compileResult = rule.Compile();
            compileResult.Should().BeTrue();
            _testOutputHelper.WriteLine($"{nameof(rule)}:{Environment.NewLine}" +
                                        $"{rule.ExpressionDebugView()}");

            var executeResult = rule.Execute(_game1);
            executeResult.Should().Be(expectedResult);

            // convert to json
            var ruleJson = JsonConvert.SerializeObject(rule, Formatting.Indented, new CustomRuleJsonConverter());
            _testOutputHelper.WriteLine($"{nameof(ruleJson)}:{Environment.NewLine}{ruleJson}");
            // re-hydrate from json
            var ruleFromJson = (MethodCallRule<Game, bool>)JsonConvert.DeserializeObject<Rule>(ruleJson, new CustomRuleJsonConverter());
            compileResult = ruleFromJson.Compile();
            compileResult.Should().BeTrue();
            _testOutputHelper.WriteLine($"{nameof(ruleFromJson)}:{Environment.NewLine}" +
                                        $"{ruleFromJson.ExpressionDebugView()}");
            var executeResult2 = ruleFromJson.Execute(_game1);
            executeResult2.Should().Be(expectedResult);
        }

        [Fact]
        public void CallAStringMethodOnDescriptionObject()
        {
            // Description is a string - Call Contains method on Description
            // compiles to: Param_0.Description.Contains("cool")
            var rule = new MethodCallRule<Game, bool>
            {
                MethodToCall = "Contains",
                ObjectToCallMethodOn = "Description",
                MethodParameters = { new ConstantRule<string> { Value = "cool" } }
            };

            var compileResult = rule.Compile();
            compileResult.Should().BeTrue();
            _testOutputHelper.WriteLine($"{nameof(rule)}:{Environment.NewLine}" +
                                        $"{rule.ExpressionDebugView()}");

            // check to see if _game1 description contains keyword "cool"
            var executeResult = rule.Execute(_game1);
            executeResult.Should().BeFalse();

            // check to see if _game2 description contains keyword "cool"
            executeResult = rule.Execute(_game2);
            executeResult.Should().BeTrue();

            // convert to json
            var ruleJson = JsonConvert.SerializeObject(rule, Formatting.Indented, new CustomRuleJsonConverter());
            _testOutputHelper.WriteLine($"{nameof(ruleJson)}:{Environment.NewLine}{ruleJson}");
            // re-hydrate from json
            var ruleFromJson = (MethodCallRule<Game, bool>)JsonConvert.DeserializeObject<Rule>(ruleJson, new CustomRuleJsonConverter());
            compileResult = ruleFromJson.Compile();
            compileResult.Should().BeTrue();
            _testOutputHelper.WriteLine($"{nameof(ruleFromJson)}:{Environment.NewLine}" +
                                        $"{ruleFromJson.ExpressionDebugView()}");
            // check to see if _game1 description contains keyword "cool"
            var executeResult2 = ruleFromJson.Execute(_game1);
            executeResult2.Should().BeFalse();

            // check to see if _game2 description contains keyword "cool"
            executeResult2 = ruleFromJson.Execute(_game2);
            executeResult2.Should().BeTrue();
        }
    }
}