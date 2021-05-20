using Microsoft.ML;

namespace EARIN_EX6.Algorithms {
    class RandomForest : BinaryClassificationAlg {
        public RandomForest(MLContext context, IDataView trainSet, IDataView testSet) : base(context, trainSet, testSet) {
        }

        protected override string AlgorithmName => "Random forest";

        protected override void Train() {
            var pipeline = context.BinaryClassification.Trainers.FastForest(numberOfTrees: 200, minimumExampleCountPerLeaf: 2, numberOfLeaves: 10);
            trainedModel = pipeline.Fit(trainSet);
        }
    }
}
