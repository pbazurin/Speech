﻿using Lisa.Helpers;
using Lisa.Resources;
using Microsoft.Speech.Recognition;

namespace Lisa.Modules
{
    public class MathModule : AbstractModule
    {
        public override void Init(SpeechRecognitionEngine recognizer)
        {
            var numbers = new Choices();

            for(var i = 1; i < 20; i++)
            {
                numbers.Add(i.ToString());
            }

            var grammarBuilder = new GrammarBuilder();

            grammarBuilder.Append(new SemanticResultKey("firstNumber", numbers));
            grammarBuilder.Append(i18n.MathModule_Plus);
            grammarBuilder.Append(new SemanticResultKey("secondNumber", numbers));

            recognizer.LoadGrammar(new Grammar(grammarBuilder)
            {
                Name = this.GetGrammarName()
            });

            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (!e.Result.IsValid(this.GetGrammarName()))
            {
                return;
            }

            var firstNumber = int.Parse(e.Result.Semantics["firstNumber"].Value.ToString());
            var secondNumber = int.Parse(e.Result.Semantics["secondNumber"].Value.ToString());

            Lisa.Say((firstNumber + secondNumber).ToString());
        }
    }
}
