﻿using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using RuleEngine.Common;
using RuleEngine.Interfaces;
using RuleEngine.Interfaces.Compilers;
using RuleEngine.Interfaces.Rules;
using RuleEngine.RuleCompilers;

namespace RuleEngine.Rules
{
    public class RegExRule<T> : Rule, IRegExRule<T>
    {
        private Func<T, bool> CompiledDelegate { get; set; }
        private static readonly IRegExRuleCompiler<T> RegExRuleCompiler = new RegExRuleCompiler<T>();

        public string RegExToUse;
        public string OperatorToUse;
        public string ObjectToValidate { get; set; }

        public override Expression BuildExpression(params ParameterExpression[] parameters)
        {
            if (parameters == null || parameters.Length != 1)
                throw new RuleEngineException($"{nameof(BuildExpression)} must call with one parameter");

            var expression = RegExRuleCompiler.BuildExpression(parameters[0], this);
            Debug.WriteLine($"  {nameof(expression)}: {expression}");
            return expression;
        }

        public override bool Compile()
        {
            CompiledDelegate = RegExRuleCompiler.CompileRule(this);
            return CompiledDelegate != null;
        }

        public bool IsMatch(T targetObject)
        {
            if (CompiledDelegate == null)
                throw new Exception("A Rule must be compiled first");

            return CompiledDelegate(targetObject);
        }
    }
}