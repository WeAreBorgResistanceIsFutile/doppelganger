using System;
using Accord.MachineLearning;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Statistics.Distributions.Univariate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Images.ML.Test
{
    [TestClass]
    public class UnitTest1
    {
        private const int SAMPLE_COUNT = 50;
        private int k = 5; // the number of clusters assumed present in the data
        private double[][] observations; // the data points containing the mixture
        private KMeans kmeans;

        private DistanceNetwork network;
        private Random rand = new Random();

        private int iterations = 5;
        private double learningRate = 0.1;
        private double radius = 15;

        [TestMethod]
        public void KMeansCluster_Learning()
        {
            btnGenerateRandom_Click();

            kmeans = new KMeans(k);

            KMeansClusterCollection clustering = kmeans.Learn(observations);

            // Classify all instances in mixture data
            int[] classifications = clustering.Decide(observations);
        }

        [TestMethod]
        public void GaussianMixtureCluster_Learning2()
        {
            btnGenerateRandom_Click();

            kmeans = new KMeans(k);
            kmeans.Learn(observations);

            // Create a new Gaussian Mixture Model

            var gmm = new GaussianMixtureModel(k);


            // If available, initialize with k-means
            if (kmeans != null)
                gmm.Initialize(kmeans);

            // Compute the model

            GaussianClusterCollection clustering = gmm.Learn(observations);

            // Classify all instances in mixture data

            int[] classifications = clustering.Decide(observations);
        }

        [TestMethod]
        public void SOMCluster_Learning()
        {
            btnGenerateRandom_Click();

            SearchSolution();
        }

        private void btnGenerateRandom_Click()

        {
            observations = new double[SAMPLE_COUNT][];
            for (int i = 0; i < observations.Length; i++)
            {
                observations[i] = new double[159*106*3];

                for (int j = 0; j < observations[i].Length; j++)
                {
                    observations[i][j] = Accord.Math.Random.Generator.Random.Next(0, 255);
                }
            }

            kmeans = null;
        }



        void SearchSolution()
        {
            // Create network
            network = new DistanceNetwork(159 * 106 * 3, 100 * 100);

            // create learning algorithm
            SOMLearning trainer = new SOMLearning(network);

            // input
            double[] input = new double[3];

            double fixedLearningRate = learningRate / 10;
            double driftingLearningRate = fixedLearningRate * 9;

            // iterations
            int i = 0;

            // loop
            while (true)
            {
                trainer.LearningRate = driftingLearningRate * (iterations - i) / iterations + fixedLearningRate;
                trainer.LearningRadius = (double)radius * (iterations - i) / iterations;


                var observations = new double[159 * 106 * 3];

                for (int j = 0; j < observations.Length; j++)
                {
                    observations[j] = Accord.Math.Random.Generator.Random.Next(0, 255);
                }

                trainer.Run(observations);

                // increase current iteration
                i++;

             
                // stop ?
                if (i >= iterations)
                    break;
            }
        }
    }
}