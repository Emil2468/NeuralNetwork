using System;
using Matrices;

namespace AI
{
    public interface ISupervisedData
    {
        Matrix GetInput();
        Matrix GetOutput();
    }
}

