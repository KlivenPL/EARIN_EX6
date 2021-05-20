using Microsoft.ML;

namespace EARIN_EX6.Algorithms {
    class LightGbm : BinaryClassificationAlg {
        public LightGbm(MLContext context, IDataView trainSet, IDataView testSet) : base(context, trainSet, testSet) {
        }

        protected override string AlgorithmName => "Light GBM";

        protected override void Train() {
            var pipeline = context.BinaryClassification.Trainers.LightGbm(numberOfIterations: 200, numberOfLeaves: 3, learningRate: 0.1);
            trainedModel = pipeline.Fit(trainSet);
        }
    }
}
