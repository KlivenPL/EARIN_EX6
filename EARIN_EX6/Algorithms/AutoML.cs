using EARIN_EX6.Entities;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using System;
using System.Linq;

namespace EARIN_EX6.Algorithms {
    class AutoML : BinaryClassificationAlg {
        public AutoML(MLContext context, IDataView trainSet, IDataView testSet) : base(context, trainSet, testSet) {
        }

        protected override string AlgorithmName => "Auto ML";

        protected override void Train() {
            var settings = new BinaryExperimentSettings {
                MaxExperimentTimeInSeconds = 30 * 60,

            };

            var set1 = context.Data.CreateEnumerable<SpectrogramData>(trainSet, false);
            var set2 = context.Data.CreateEnumerable<SpectrogramData>(validationSet, false);

            var combinedSets = context.Data.LoadFromEnumerable(set1.Concat(set2));
            var trainTestSplit = context.Data.TrainTestSplit(combinedSets, 0.5);

            var progressHandler = new Progress<RunDetail<Microsoft.ML.Data.BinaryClassificationMetrics>>(ph => {
                if (ph.ValidationMetrics != null) {
                    Console.WriteLine($"Current trainer - {ph.TrainerName} with accuracy {ph.ValidationMetrics.Accuracy}");
                }
            });

            var experiment = context.Auto().CreateBinaryClassificationExperiment(settings);

            // Run the experiment
            Console.WriteLine("Running the experiment...");
            var experimentResult = experiment.Execute(trainData: trainTestSplit.TrainSet, validationData: trainTestSplit.TestSet, progressHandler: progressHandler);
            Console.WriteLine($"Best run ({experimentResult.BestRun.TrainerName}):");
            trainedModel = experimentResult.BestRun.Model;
            metrics = experimentResult.BestRun.ValidationMetrics;
        }
    }
}
