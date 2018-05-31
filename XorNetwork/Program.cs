using System;
using Matrices;
using AI;


namespace XorNetwork
{
    class MainClass
    {
        static NeuralNetwork nn;
        static ISupervisedData[] trainingData;
        public static void Main(string[] args)
        {
            nn = new NeuralNetwork(new int[]{2, 4, 1});
            trainingData = new SupervisedData[4];
            trainingData[0] = new SupervisedData(new Matrix(new float[,]{{0},{0}}), new Matrix(new float[,]{{0}}));
            trainingData[1] = new SupervisedData(new Matrix(new float[,]{{0},{1}}), new Matrix(new float[,]{{1}}));
            trainingData[2] = new SupervisedData(new Matrix(new float[,]{{1},{0}}), new Matrix(new float[,]{{1}}));
            trainingData[3] = new SupervisedData(new Matrix(new float[,]{{1},{1}}), new Matrix(new float[,]{{0}}));

            nn.Train(trainingData, 100000);
            Console.WriteLine("Input = 0, 0");
            Console.WriteLine("Output = " + nn.FeedForward(trainingData[0].GetInput()));
            Console.WriteLine("Input = 0, 1");
            Console.WriteLine("Output = " + nn.FeedForward(trainingData[1].GetInput()));
            Console.WriteLine("Input = 1, 0");
            Console.WriteLine("Output = " + nn.FeedForward(trainingData[2].GetInput()));
            Console.WriteLine("Input = 1, 1");
            Console.WriteLine("Output = " + nn.FeedForward(trainingData[3].GetInput()));

        }
    }
}
