using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace EARIN_EX6.Algorithms {
    class LbfgsLogisticRegression : BinaryClassificationAlg {
        public LbfgsLogisticRegression(MLContext context, IDataView trainSet, IDataView testSet) : base(context, trainSet, testSet) {
        }

        protected override string AlgorithmName => "Logistic regression L-BFGS";

        protected override void Train() {
            var pipeline = context.BinaryClassification.Trainers.LbfgsLogisticRegression(historySize: 40);
            trainedModel = pipeline.Fit(trainSet);
        }
    }
}
