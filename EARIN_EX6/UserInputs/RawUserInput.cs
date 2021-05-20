using CommandLine;
using EARIN_EX6.Helpers;
using System.IO;
using System.Linq;

namespace EARIN_EX6.UserInputs {
    class RawUserInput {
        [Option('p', "predict", Required = false, HelpText = "Path to file with mass-spectrometric data to predict. Creates a same-named file (with .output extension) containing results.")]
        public string PredictPath { get; set; }

        [Option('a', "algorithm", Required = false, HelpText = "Specifies an algorithm. Available options: Auto, Lbfgs, LightGbm, RandomForest")]
        public string ChosenAlgorithm { get; set; }

        [Option('d', "demo", Required = false, HelpText = "If set, shows trained models' results")]
        public bool Demo { get; set; }

        public UserInput ToUserInput() {
            if (string.IsNullOrWhiteSpace(PredictPath) && string.IsNullOrWhiteSpace(ChosenAlgorithm) && Demo == false) {
                ExceptionHelper.ThrowAndExit(ExceptionHelper.ExceptionType.InvalidInput, "Invalid input. Use --help to see available options.");
            }

            if (!string.IsNullOrWhiteSpace(PredictPath) && string.IsNullOrWhiteSpace(ChosenAlgorithm)) {
                ExceptionHelper.ThrowAndExit(ExceptionHelper.ExceptionType.InvalidInput, "Algorithm not specified.");
            }

            if (string.IsNullOrWhiteSpace(PredictPath) && !string.IsNullOrWhiteSpace(ChosenAlgorithm)) {
                ExceptionHelper.ThrowAndExit(ExceptionHelper.ExceptionType.InvalidInput, "Predict file not specified. To see models's results use --demo");
            }

            UserInput userInput = new UserInput { Demo = Demo };

            if (!string.IsNullOrWhiteSpace(PredictPath)) {
                var file = new FileInfo(PredictPath);
                if (!file.Exists) {
                    ExceptionHelper.ThrowAndExit(ExceptionHelper.ExceptionType.InvalidInput, $"Predict file does not exist: {file.FullName}");
                }
                userInput.PredictFile = file;
            }

            if (!string.IsNullOrWhiteSpace(ChosenAlgorithm)) {
                var algFuncPair = Program.AvailableAlgorithms.FirstOrDefault(x => x.name.ToLower() == ChosenAlgorithm.ToLower());
                if (algFuncPair == default) {
                    ExceptionHelper.ThrowAndExit(ExceptionHelper.ExceptionType.InvalidInput, $"Invalid algorithm specified: {ChosenAlgorithm}. See --help to check available algorithms");
                }
                userInput.ChosenAlgorithm = algFuncPair;
            }

            return userInput;
        }
    }
}
