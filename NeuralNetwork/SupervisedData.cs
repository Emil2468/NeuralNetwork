using System;
using Matrices;

namespace AI
{
    public class SupervisedData : ISupervisedData
    {
        private Matrix input;
        private Matrix output;

        public SupervisedData(Matrix input, Matrix output)
        {
            this.input = input;
            this.output = output;
        }

        public Matrix GetInput() {
            return input;
        }

        public Matrix GetOutput() {
            return output;
        }
    }
}

