using Microsoft.ML.Data;

namespace EARIN_EX6.Entities {
    class SpectrogramData {

        [LoadColumn(0), ColumnName("Label")]
        public bool Cancer { get; set; }

        [LoadColumn(1, 10000), VectorType(10000)]
        public float[] Features { get; set; }
    }

    class UnlabeledSpectrogramData {
        [LoadColumn(0, 9999), VectorType(10000)]
        public float[] Features { get; set; }

        [LoadColumn(10000), ColumnName("Label")]
        public bool Cancer { get; set; }
    }
}
