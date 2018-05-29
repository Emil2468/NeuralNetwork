using System;
using Matrices;
using AI;


namespace XorNetwork
{
    class MainClass
    {
        static NeuralNetwork nn;
        static Matrix[][] trainingData;
        public static void Main(string[] args)
        {
            nn = new NeuralNetwork(new int[]{2, 4, 1});
            trainingData = new Matrix[4][];
            for (int i = 0; i < trainingData.Length; i++)
            {
                trainingData[i] = new Matrix[2];
            }
            trainingData[0][0] = new Matrix(new float[,]{{0},{0}});
            trainingData[0][1] = new Matrix(new float[,]{{0}});
            trainingData[1][0] = new Matrix(new float[,]{{0},{1}});
            trainingData[1][1] = new Matrix(new float[,]{{1}});
            trainingData[2][0] = new Matrix(new float[,]{{1},{0}});
            trainingData[2][1] = new Matrix(new float[,]{{1}});
            trainingData[3][0] = new Matrix(new float[,]{{1},{1}});
            trainingData[3][1] = new Matrix(new float[,]{{0}});

            Random rnd = new Random();
            for (int i = 0; i < 100000; i++)
            {
                int idx = rnd.Next(0, 4);
                nn.Train(trainingData[idx][0], trainingData[idx][1]);
            }
            Console.WriteLine("Input = 0, 0");
            Console.WriteLine("Output = " + nn.FeedForward(trainingData[0][0])[0,0]);
            Console.WriteLine("Input = 0, 1");
            Console.WriteLine("Output = " + nn.FeedForward(trainingData[1][0])[0,0]);
            Console.WriteLine("Input = 1, 0");
            Console.WriteLine("Output = " + nn.FeedForward(trainingData[2][0])[0,0]);
            Console.WriteLine("Input = 1, 1");
            Console.WriteLine("Output = " + nn.FeedForward(trainingData[3][0])[0,0]);

        }
    }
}
