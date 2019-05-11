
//Conversion code found at: https://jamesmccaffrey.wordpress.com/2013/11/23/reading-the-mnist-data-set-with-c/
using System;
using Matrices;
using AI;
using System.IO;

namespace MNIST
{
    public class Convertor
    {
        public ISupervisedData[] Convert(Stream labels, Stream images) {
            BinaryReader brLabels = new BinaryReader(labels);
            BinaryReader brImages = new BinaryReader(images);

            int magicNumberImages = ReadBigInt32(brImages); //Not to be used
            int numImages = ReadBigInt32(brImages);
            int numRows = ReadBigInt32(brImages);
            int numCols = ReadBigInt32(brImages);

            int magicNumberLabels = ReadBigInt32(brLabels); //Not to be used
            int numLabels = ReadBigInt32(brLabels);

            ISupervisedData[] result = new MnistData[numImages];
            for (int i = 0; i < result.Length; i++) {
                Matrix input = new Matrix(numCols * numRows, 1);
                for (int j = 0; j < numCols * numRows; j++) {
                    byte pixelValue = brImages.ReadByte();
                    input[j,0] = map(pixelValue, 0, 255, 0, 1);
                }
                result[i] = new MnistData(input, brLabels.ReadByte());
            }
            brLabels.Close();
            brImages.Close();
            return result;
             
        }
        /// <summary>
        /// Maps value from range [minVal, maxVal] to [minMap, maxMap]
        /// </summary>
        private static float map(float value, float minVal, float maxVal, float minMap, float maxMap) {
            return (value - minVal) / (maxVal - minVal) * (maxMap - minMap) + minMap;
        }

        private static int ReadBigInt32(BinaryReader br)
        {
          var bytes = br.ReadBytes(sizeof(Int32));
          if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
          return BitConverter.ToInt32(bytes, 0);
        }


    }
}
