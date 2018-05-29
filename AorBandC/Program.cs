using System;
using Matrices;
using AI;
//using NeuralNetwork;

namespace AorBandC
{
    class MainClass
    {
        static NeuralNetwork nn;
        static ISupervisedData[] trainingData;
        public static void Main(string[] args)
        {   
            nn = new NeuralNetwork(new int[]{3, 4, 5, 4, 1}, 0.2f);

            nn = NeuralNetwork.load("Test");
            trainingData = new ISupervisedData[8];
            //Output = (A or B) and c
            trainingData[0] = new SupervisedData(new Matrix(new float[,]{{0},{0}, {0}}), new Matrix(new float[,]{{0}}));
            trainingData[1] = new SupervisedData(new Matrix(new float[,]{{0},{0}, {1}}), new Matrix(new float[,]{{0}}));
            trainingData[2] = new SupervisedData(new Matrix(new float[,]{{0},{1}, {0}}), new Matrix(new float[,]{{0}}));
            trainingData[3] = new SupervisedData(new Matrix(new float[,]{{0},{1}, {1}}), new Matrix(new float[,]{{1}}));
            trainingData[4] = new SupervisedData(new Matrix(new float[,]{{1},{0}, {0}}), new Matrix(new float[,]{{0}}));
            trainingData[5] = new SupervisedData(new Matrix(new float[,]{{1},{0}, {1}}), new Matrix(new float[,]{{1}}));
            trainingData[6] = new SupervisedData(new Matrix(new float[,]{{1},{1}, {0}}), new Matrix(new float[,]{{0}}));
            trainingData[7] = new SupervisedData(new Matrix(new float[,]{{1},{1}, {1}}), new Matrix(new float[,]{{1}}));

            nn.Train(trainingData, 100000);
            nn.save("Test");
            for (int i = 0; i < trainingData.Length; i++)
            {
                Console.Write("Input = \n" + trainingData[i].GetInput());

                Console.Write("\nExpected = \n" + trainingData[i].GetOutput());

                Matrix output = nn.FeedForward(trainingData[i].GetInput());
                Console.Write("\nActual = \n" + output);

                Console.WriteLine("\n");

            }

        }
        }
    }
