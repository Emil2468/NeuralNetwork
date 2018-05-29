using System;
using NUnit.Framework;
using AI;
using Matrices;

namespace Tests
{
    [TestFixture]
    public class NeuralNetworkTest
    {
        NeuralNetwork nn;
        [SetUp]
        public void SetUp(){
            nn = new NeuralNetwork(new int[]{3,2,1});
        }

        [Test]
        public void TestWeigths(){
            Assert.AreEqual(2, nn.weights[0].Rows);
            Assert.AreEqual(3, nn.weights[0].Columns);
            Assert.AreEqual(1, nn.weights[1].Rows);
            Assert.AreEqual(2, nn.weights[1].Columns);
        }

        [Test]
        public void TestBiases(){
            Assert.AreEqual(2, nn.biases[0].Rows);
            Assert.AreEqual(1, nn.biases[0].Columns);
            Assert.AreEqual(1, nn.biases[1].Rows);
            Assert.AreEqual(1, nn.biases[1].Columns);
        }

        [Test]
        public void TestPerceptrons(){
            Assert.AreEqual(3, nn.layerValues[0].Rows);
            Assert.AreEqual(1, nn.layerValues[0].Columns);
            Assert.AreEqual(2, nn.layerValues[1].Rows);
            Assert.AreEqual(1, nn.layerValues[1].Columns);
            Assert.AreEqual(1, nn.layerValues[2].Rows);
            Assert.AreEqual(1, nn.layerValues[2].Columns);
        }

        [Test]
        public void TestFeedForward(){
            Matrix input = new Matrix(3,1);
            NeuralNetwork network = new NeuralNetwork(new int[]{3, 4, 5, 3});
            input.Randomize(-1,1);
            Matrix output = network.FeedForward(input);
            for (int i = 0; i < output.Rows; i++)
            {
                Console.WriteLine(output[i, 0]);
            }
        }

        [Test]
        public void TrainTest(){
            //Test is kinda useless, mostly just checking that we dont get exceptions
            NeuralNetwork network = new NeuralNetwork(new int[]{3, 3, 3, 3, 4, 5, 2, 3});
            Matrix input = new Matrix(3,1);
            Matrix target = new Matrix(3,1);
            input.Randomize(-1, 1);
            target.Randomize(-1, 1);
            ISupervisedData[] data = new ISupervisedData[]{new SupervisedData(input, target)};
            network.Train(data, 3);
        }

        [Test]
        public void LoadTest() {
            NeuralNetwork network1 = new NeuralNetwork(new int[]{3,40, 30, 3});
            network1.save("LoadTest");
            NeuralNetwork network2 = NeuralNetwork.load("LoadTest");
            Matrix input = new Matrix(3,1);
            input.Randomize(-1,1);
            Console.WriteLine("network1: \n" + network1.FeedForward(input));
            Console.WriteLine("network2: \n" + network2.FeedForward(input));
            Assert.IsTrue(network1.FeedForward(input) == network2.FeedForward(input));
        }
    }   
}
    
