using System;
using AI;
using Matrices;

namespace MNIST
{
    public class MnistData : ISupervisedData
    {
        private Matrix input;
        private Matrix output;
        private byte label;

        public MnistData(Matrix input, byte label)
        {
            this.input = input;
            this.label = label;
            this.output = new Matrix(10, 1);
            this.output[this.label, 0] = 1; //Set the correct node to 1 based on number current image depicts
        }

        public Matrix GetInput() {
            return input;
        }

        public Matrix GetOutput() {
            return output;
        }

        public byte GetLabel() {
            return label;
        }

        public override string ToString() {
          string s = "";
          for (int i = 0; i < 28; i++)
          {
            for (int j = 0; j < 28; j++)
            {
              if (this.input[28 * i + j,0] < 0.01)
                s += " "; // white
              else if (this.input[28 * i + j,0] > 0.99)
                s += "O"; // black
              else
                s += "."; // gray
            }
            s += "\n";
          }
          s += this.label;
          return s;
        }
    }
}

