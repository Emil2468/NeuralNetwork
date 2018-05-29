using System;
using AI;
using Matrices;
using System.IO;

namespace MNIST
{
    class MainClass
    {
        private static MnistData[] traingData; 
        private static MnistData[] testData;
        private static NeuralNetwork nn;
        private static string networkName = "MNIST";
        public static void Main(string[] args)
        {
            
            init();
            showSome(40);
            testNetwork(10000);
//            for (int i = 0; i < 100; i++)
//            {
//                Console.WriteLine ("Training...");
//                nn.Train(traingData, 10000);
//                Console.WriteLine ("Saving (Don't close)...");
//                nn.Save(networkName);
//                Console.WriteLine("Saved network");
//                testNetwork(1000);
//                Console.WriteLine();
//            }
        }

        private static void init() {
            FileStream images = new FileStream("train-images.idx3-ubyte", FileMode.Open);
            FileStream labels = new FileStream("train-labels.idx1-ubyte", FileMode.Open);

            Convertor c = new Convertor();
            traingData = (MnistData[])c.Convert(labels, images);
            images = new FileStream("t10k-images.idx3-ubyte", FileMode.Open);
            labels = new FileStream("t10k-labels.idx1-ubyte", FileMode.Open);
            //nn = new NeuralNetwork(new int[]{28*28, 100, 10});
            nn = NeuralNetwork.Load(networkName);
            testData = (MnistData[])c.Convert(labels, images);
            images.Close();
            labels.Close();
        }

        private static void testNetwork(int tests) {
            Random rnd = new Random();
            int idx = rnd.Next(testData.Length);
            int correct = 0;
            for (int i = 0; i < tests; i++)
            {
                
                Matrix output = nn.FeedForward(testData[(idx + i) % testData.Length].GetInput());
                if(largestIndex(output) == testData[(idx + i) % testData.Length].GetLabel()) {
                    correct++;
                }
            }
            Console.WriteLine("Success = " + ((float)correct/tests * 100) + "%");
        }

        private static int largestIndex(Matrix m) { //Assuming matrix only has 1 column
            int maxIndex = 0;
            float maxValue = float.NegativeInfinity;
            for (int i = 0; i < m.Rows; i++)
            {
                if(m[i, 0] > maxValue) {
                    maxIndex = i;
                    maxValue = m[i, 0];
                }
            }
            return maxIndex;
        }

        private static void showSome(int n) {
            Random rnd = new Random();
            int idx = rnd.Next(testData.Length);
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("Image:\n" + testData[(idx + i) % testData.Length]);
                Matrix output = nn.FeedForward(testData[(idx + i) % testData.Length].GetInput());
                Console.WriteLine("Output:\n" + output);
                Console.WriteLine("Guess: " + largestIndex(output));
            }
        }
    }
}
