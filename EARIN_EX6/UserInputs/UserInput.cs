using EARIN_EX6.Algorithms;
using Microsoft.ML;
using System;
using System.IO;
using System.Text;

namespace EARIN_EX6.UserInputs {
    class UserInput {
        public FileInfo PredictFile { get; set; }
        public (string name, Func<MLContext, IDataView, IDataView, BinaryClassificationAlg> algFunc) ChosenAlgorithm { get; set; }
        public bool Demo { get; set; }

        public override string ToString() {
            var sb = new StringBuilder();

            sb.AppendLine($"\tDemo mode: {(Demo ? "true" : "false")}");
            sb.AppendLine($"\tPredict file: {(PredictFile == null ? "(none)" : PredictFile.Name)}");
            sb.AppendLine($"\tChosen algorithm: {(ChosenAlgorithm == default ? "(none)" : ChosenAlgorithm.name)}");

            return sb.ToString();
        }
    }
}
