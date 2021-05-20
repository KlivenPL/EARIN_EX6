using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace EARIN_EX6.Algorithms {
    abstract class BinaryClassificationAlg {
        protected IDataView trainSet;
        protected IDataView validationSet;
        protected MLContext context;
        protected ITransformer trainedModel;
        protected BinaryClassificationMetrics metrics;

        protected BinaryClassificationAlg(MLContext context, IDataView trainSet, IDataView validationSet) {
            this.context = context;
            this.trainSet = trainSet;
            this.validationSet = validationSet;

            var modelFileInfo = new FileInfo(SavedModelName);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Initalizing {AlgorithmName}");
            Console.ResetColor();

            if (modelFileInfo.Exists) {
                Console.WriteLine($"\tLoading saved trained model of {AlgorithmName}");
                Console.WriteLine($"\t(Please remove {SavedModelName} to retrain the net)");
                LoadModel();
                Console.WriteLine($"\tModel loaded");
            } else {
                Console.WriteLine($"\tTraining {AlgorithmName}, please wait");
                Train();
                Console.WriteLine($"\tTraining complete");
            }
            Console.WriteLine();
        }

        private string SavedModelName => $"{MakeValidFileName(AlgorithmName)}.zip";
        protected abstract string AlgorithmName { get; }

        public void Evaluate() {
            Validate();
            PrintMetrics();
            SaveModel();
        }

        protected abstract void Train();

        protected virtual void LoadModel() {
            trainedModel = context.Model.Load(SavedModelName, out _);
        }

        protected virtual void Validate() {
            var predictions = trainedModel.Transform(validationSet);
            metrics = context.BinaryClassification.EvaluateNonCalibrated(predictions);
        }

        protected virtual void PrintMetrics() {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{AlgorithmName} metrics:");
            Console.ResetColor();
            Console.WriteLine($"\tAccuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"\tAUC: {metrics.AreaUnderPrecisionRecallCurve:P2}");
            Console.WriteLine($"\tF1: {metrics.F1Score:P2}");
            Console.WriteLine($"\tROC curve: {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"\tConfusion matrix:{Environment.NewLine}\t{metrics.ConfusionMatrix.GetFormattedConfusionTable().Replace(Environment.NewLine, $"{Environment.NewLine}\t")}");
            Console.WriteLine();
        }

        public void Predict(IDataView dataToPredict, string fileName) {
            Console.WriteLine("\tPredicting, please wait...");
            var predictions = trainedModel.Transform(dataToPredict);
            var cancer = predictions.GetColumn<bool>("PredictedLabel");
            FileInfo fi = new FileInfo(Path.ChangeExtension(fileName, ".output"));

            using var stream = fi.Open(FileMode.Create);
            using BinaryWriter bw = new BinaryWriter(stream);
            byte[] textbyte = Encoding.Unicode.GetBytes(string.Join(Environment.NewLine, cancer.Select(b => b ? "1" : "-1")));
            bw.Write(textbyte);
            stream.Close();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\tPredicted {cancer.Count()} rows using {AlgorithmName}.{Environment.NewLine}\tFile saved as {fi.FullName}");
            Console.ResetColor();
        }

        protected void SaveModel() {
            context.Model.Save(trainedModel, trainSet.Schema, SavedModelName);
        }

        private static string MakeValidFileName(string name) {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }
    }
}
