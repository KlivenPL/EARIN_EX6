using CommandLine;
using EARIN_EX6.Algorithms;
using EARIN_EX6.Entities;
using EARIN_EX6.Helpers;
using EARIN_EX6.UserInputs;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;

namespace EARIN_EX6 {
    class Program {

        private const string Logo = "+-+-+-+-+-+-+-+-+-+-+\r\n|E|A|R|I|N| |E|X| |6|\r\n+-+-+-+-+-+-+-+-+-+-+\r\nOskar H\u0105cel\r\nMarcin Lisowski\r\nPW, 2021\r\n+-+-+-+-+-+-+-+-+-+-+";
        static readonly string _trainSetPath = Path.Combine(Environment.CurrentDirectory, "Dataset", "arcene_train_labeled.txt");
        static readonly string _validationSetPath = Path.Combine(Environment.CurrentDirectory, "Dataset", "arcene_valid_labeled.txt");

        public static IEnumerable<(string name, Func<MLContext, IDataView, IDataView, BinaryClassificationAlg> algFunc)> AvailableAlgorithms { get; } = new (string name, Func<MLContext, IDataView, IDataView, BinaryClassificationAlg> algFunc)[] {
                ("Auto", (context, trainSet, valid) => new AutoML(context, trainSet, valid)),
                ("Lbfgs", (context, trainSet, valid) => new LbfgsLogisticRegression(context, trainSet, valid)),
                ("LightGbm", (context, trainSet, valid) => new LightGbm(context, trainSet, valid)),
                ("RandomForest", (context, trainSet, valid) => new RandomForest(context, trainSet, valid)),
            };

        static void Main(string[] args) {
            PrintLogo();
            GetUserInput(args);
        }

        private static void GetUserInput(string[] args) {
            Parser.Default.ParseArguments<RawUserInput>(args)
                .WithParsed(o => new Program().Start(o.ToUserInput()));
        }

        private static void PrintLogo() {
            Console.ForegroundColor = EnumHelper.PickRandom(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.DarkGray);
            Console.WriteLine(Logo);
            Console.ResetColor();
            Console.WriteLine();
        }

        private void Start(UserInput userInput) {
            PrintUserInput(userInput);

            var context = new MLContext(seed: 0);
            var trainData = context.Data.LoadFromTextFile<SpectrogramData>(_trainSetPath, hasHeader: false, separatorChar: ' ');
            var validationData = context.Data.LoadFromTextFile<SpectrogramData>(_validationSetPath, hasHeader: false, separatorChar: ' ');

            if (userInput.Demo) {
                foreach (var (_, algFunc) in AvailableAlgorithms) {
                    algFunc(context, trainData, validationData).Evaluate();
                }
            }

            if (userInput.PredictFile != null) {
                var alg = userInput.ChosenAlgorithm.algFunc(context, trainData, validationData);
                var predictData = context.Data.LoadFromTextFile<UnlabeledSpectrogramData>(userInput.PredictFile.FullName, hasHeader: false, separatorChar: ' ');
                alg.Predict(predictData, userInput.PredictFile.FullName);
            }


            /*new LbfgsLogisticRegression(context, trainData, validationData).Evaluate();

            if (userInput.MarkovBlanketNode != null) {
                PrintMarkovBlanket(userInput.MarkovBlanketNode);
            }

            if (userInput.QueryNodes?.Any() == true) {
                var mcmc = new McmcGibbs(userInput);
                mcmc.EvaluateQueries();
            }*/
        }

        private void PrintUserInput(UserInput userInput) {
            Console.WriteLine("Given data:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(userInput.ToString());
            Console.ResetColor();
        }
    }
}
