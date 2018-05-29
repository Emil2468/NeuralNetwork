using System;
using Matrices;
using System.IO;
//using NeuralNetwork;

namespace AI
{
    public class NeuralNetwork
    {
        public int[] layerSizes {get; private set;}
        public int layers {get; private set;}
        public Matrix[] weights {get; private set;}
        public Matrix[] biases {get; private set;}
        public Matrix[] layerValues {get; private set;}
        public float learingRate;
        /// <summary>
        /// Initializes a new instance of the <see cref="NeuralNetwork.NeuralNetwork"/> class.
        /// </summary>
        /// <param name="layerSizes">Array containing sizes of all layers</param>
        public NeuralNetwork(int[] layerSizes, float learingRate = 0.1f)
        {
            this.layerSizes = layerSizes;
            layers = layerSizes.Length; 
            weights = new Matrix[layers - 1];
            biases = new Matrix[layers - 1];
            layerValues = new Matrix[layers];
            this.learingRate = learingRate;

            for (int i = 0; i < layerValues.Length; i++)
            {
                layerValues[i] = new Matrix(layerSizes[i], 1);
            }

            for (int i = 0; i < biases.Length; i++)
            {
                biases[i] = new Matrix(layerSizes[i + 1], 1);
                biases[i].Randomize(-1, 1);
            }

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = new Matrix(layerSizes[i + 1], layerSizes[i]);
                weights[i].Randomize(-1, 1);
            }
        }

        public Matrix FeedForward(Matrix input)
        {
            if (input.Rows != layerSizes[0])
            {
                throw new ArgumentException("Input does not match network");
            }
            else
            {
                layerValues[0] = input.copy();
                for (int i = 0; i < weights.Length; i++)
                {
                    layerValues[i + 1] = weights[i] * layerValues[i];
                    layerValues[i + 1] += biases[i];
                    Matrix.Map(layerValues[i + 1], sigmoid);
                }
                return layerValues[layers - 1];
            }
        }


        public void Train(ISupervisedData[] data, int rounds) {
            Random rnd = new Random();
            for (int k = 0; k < rounds; k++)
            {   
                int index = rnd.Next(data.Length);
                Matrix output = FeedForward(data[index].GetInput());
                Matrix outputError = data[index].GetOutput() - output;
                Matrix gradients = Matrix.Map(output, sigmoidPrime);
                gradients = gradients ^ outputError;
                gradients = gradients * learingRate;

                //Transpose layer rigth before output
                Matrix transposedLayer = Matrix.Transpose(layerValues[layers - 2]);
                Matrix weigthDeltas = gradients * transposedLayer;
                //Add deltas to last layer of weigths and biases
                weights[weights.Length - 1] += weigthDeltas;
                biases[biases.Length - 1] += gradients;

                for (int i = weights.Length - 2; i >= 0; i--)
                {
                    Matrix transposedWeigths = Matrix.Transpose(weights[i + 1]);
    //                Console.WriteLine("Output error: " + outputError.Rows + ", " + outputError.Columns);
    //                Console.WriteLine("transposedWeigths: " + transposedWeigths.Rows + ", " + transposedWeigths.Columns);
    //                Console.WriteLine();
                    outputError = transposedWeigths * outputError; 
                    gradients = Matrix.Map(layerValues[i + 1], sigmoidPrime);

                    gradients = gradients ^ outputError;
                    gradients = gradients * learingRate;
                    transposedLayer = Matrix.Transpose(layerValues[i]);
    //                Console.WriteLine("transposedLayer: " + transposedLayer.Rows + ", " + transposedLayer.Columns);
    //                Console.WriteLine("gradients: " + gradients.Rows + ", " + gradients.Columns);
    //                Console.WriteLine();
                    weigthDeltas =  gradients * transposedLayer;
    //                Console.WriteLine("weights[]: " + weights[i].Rows + ", " + weights[i].Columns);
    //                Console.WriteLine("weigthDeltas: " + weigthDeltas.Rows + ", " + weigthDeltas.Columns);
    //                Console.WriteLine();
                    weights[i] += weigthDeltas;
    //                Console.WriteLine("biases[i]: " + biases[i].Rows + ", " + biases[i].Columns);
    //                Console.WriteLine("gradients: " + gradients.Rows + ", " + gradients.Columns);
    //                Console.WriteLine();
                    biases[i] += gradients;
                }
            }

        }

        public void Save(string name) {
            using(StreamWriter writer = new StreamWriter(name + ".nn")) {
                for (int i = 0; i < layers; i++)
                {
                    writer.Write(layerSizes[i]);
                    if(i != layers - 1) {
                        writer.Write(", ");
                    }
                }
                writer.WriteLine(", " + this.learingRate);
                for (int i = 0; i < weights.Length; i++)
                {
                    writer.WriteLine(weights[i]);
                }

                for (int i = 0; i < biases.Length; i++)
                {
                    writer.WriteLine(biases[i]);
                }
            }
        }

        //Maybe this could be a constructor
        public static NeuralNetwork Load(string name){
            NeuralNetwork nn;
            using(StreamReader reader = new StreamReader(name + ".nn")){
                string[] sizesStr = reader.ReadLine().Split(',');
                //We subtract one because the last element is the learning rate
                int[] sizesInt = new int[sizesStr.Length - 1];
                for (int i = 0; i < sizesInt.Length; i++)
                {
                    sizesInt[i] = int.Parse(sizesStr[i]);
                }
                nn = new NeuralNetwork(sizesInt, float.Parse(sizesStr[sizesStr.Length - 1]));

                for (int i = 0; i < nn.weights.Length; i++)
                {
                    string matrixString = "";
                    for (int j = 0; j < nn.weights[i].Rows; j++) {
                        matrixString += reader.ReadLine();
                        if (j != nn.weights[i].Rows - 1) {
                            matrixString += "\n";
                        }
                    }
                    nn.weights[i] = new Matrix(matrixString);
                }
                for (int i = 0; i < nn.biases.Length; i++)
                {
                    string matrixString = "";
                    for (int j = 0; j < nn.biases[i].Rows; j++) {
                        matrixString += reader.ReadLine();
                        if (j != nn.biases[i].Rows - 1) {
                            matrixString += "\n";
                        }
                    }
                    nn.biases[i] = new Matrix(matrixString);
                }
            }
            return nn;
        }

        public static NeuralNetwork mutate(NeuralNetwork n) {
            NeuralNetwork newNetwork = new NeuralNetwork(n.layerSizes);   
            for (int i = 0; i < n.weights.Length; i++)
            {
                newNetwork.weights[i] = Matrix.Map(n.weights[i], changeValue);
            }
            for (int i = 0; i < n.biases.Length; i++)
            {
                newNetwork.biases[i] = Matrix.Map(n.biases[i], changeValue);
            }
            return newNetwork;
        }

        //Method used when mutating network
        private static float changeValue(float value) {
            Random rnd = new Random();
            float chance = map(rnd.Next(0, 100000001), 0, 100000000, 0, 1);
            //would be nice to not hard code these values
            if (chance <= 0.02){
                if (rnd.Next(0,2) == 0) {
                    return value + chance;
                } else {
                    return value - chance;
                }
            } else {
                return value;
            }
        }

        //TODO: maybe change this
        private static float sigmoid(float x) {
            return 1.0f / (1.0f + (float)Math.Exp(-x));
        }

        /// <summary>
        /// Derivative of sigmoid
        /// </summary>
        /// <returns>The prime.</returns>
        /// <param name="x">x value should be value that has allready passed through sigmoid, i think</param>
        private static float sigmoidPrime(float x) {
            return x * (1 - x);
        }

        /// <summary>
        /// Maps value from range [minVal, maxVal] to [minMap, maxMap]
        /// </summary>
        private static float map(float value, float minVal, float maxVal, float minMap, float maxMap) {
            return (value - minVal) / (maxVal - minVal) * (maxMap - minMap) + minMap;
        }
    }
}

