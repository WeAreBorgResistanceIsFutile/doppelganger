using System;
using System.Collections.Generic;

using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Doppelganger.Image.ML
{
    public class ImageSimilarityVerificationTrainer
    {
        private readonly IEnumerable<MyData> _Data;

        public ImageSimilarityVerificationTrainer(IEnumerable<MyData> data)
        {
            this._Data = data;
        }

        public class MyData
        {
            //private const int BYTE_ARRAY_SIZE = 106 * 159 * 4;
            private const int BYTE_ARRAY_SIZE = 3;

            [ColumnName("Input1")]
            public byte[] Image1 { get; set; }

            [ColumnName("Input2")]
            public byte[] Image2 { get; set; }

            [ColumnName("Label")]
            public float AreIdentical;

            public MyData(byte[] image1, byte[] image2)
            {
                if (image1 is null || image1.Length != BYTE_ARRAY_SIZE)
                    throw new ArgumentException($"{nameof(image1)} should be a {BYTE_ARRAY_SIZE} length byte array");

                if (image2 is null || image2.Length != BYTE_ARRAY_SIZE)
                    throw new ArgumentException($"{nameof(image2)} should be a {BYTE_ARRAY_SIZE} length byte array");

                Image1 = image1;
                Image2 = image2;
            }
        }

        // IrisPrediction is the result returned from prediction operations
        public class MyPrediction
        {
            [ColumnName("Label")]
            public float AreIdentical;
        }

        public void SomeFunc()
        {
            // STEP 2: Create a ML.NET environment
            MLContext mlContext = new MLContext();

            IDataView trainingDataView = mlContext.Data.LoadFromEnumerable<MyData>(_Data);

            var pipeline = mlContext.Transforms.Concatenate("Features", "Input1", "Input2")
                .Append(mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent());

            pipeline.Fit(trainingDataView);
        }
    }
}