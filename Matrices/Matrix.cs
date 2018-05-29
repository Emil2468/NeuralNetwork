using System;

namespace Matrices
{
    public class Matrix
    {
        private float[,] values; 
        public int Rows;
        public int Columns;

        //Initialize new matrix with zeors in all entries
        public Matrix(int rows, int columns) {
          values = new float[rows, columns];
          this.Columns = columns;
          this.Rows = rows;
        }


        public Matrix(float[,] values) {
            this.values = values;
            this.Columns = values.GetLength(1);
            this.Rows = values.GetLength(0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrices.Matrix"/> class.
        /// </summary>
        /// <param name="values">Each column should be serperated by comma, and each row by linebreak</param>
        public Matrix(string values) {
            string[] rowArray = values.Split('\n');
            string[] columnArray = rowArray[0].Split(',');
            this.Rows = rowArray.Length;
            this.Columns = columnArray.Length;
            this.values = new float[Rows, Columns];
            for (int i = 0; i < rowArray.Length; i++)
            {
                columnArray = rowArray[i].Split(',');
                for (int j = 0; j < columnArray.Length; j++) {
                    this[i,j] = float.Parse(columnArray[j]);

                }
            }
        }

        public Matrix copy() {
            float[,] newValues = new float[Rows, Columns];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++) {
                    newValues[i,j] = values[i,j];
                }
            }
            return new Matrix(newValues);
        }

        public static Matrix operator+ (Matrix a, Matrix b) {
            if (a.Columns != b.Columns || a.Rows != b.Rows) {
                throw new ArgumentException("Matricies does not have the same size");
            } else {
                float[,] newValues = new float[a.Rows, a.Columns];
                for (int i = 0; i < a.Rows; i++)
                {
                    for (int j = 0; j < a.Columns; j++) {
                        newValues[i,j] = a[i,j] + b[i,j];
                    }
                }
                return new Matrix(newValues);
            }
        }

        public static Matrix operator- (Matrix a, Matrix b) {
            if (a.Columns != b.Columns || a.Rows != b.Rows) {
                throw new ArgumentException("Matricies does not have the same size");
            } else {
                float[,] newValues = new float[a.Rows, a.Columns];
                for (int i = 0; i < a.Rows; i++)
                {
                    for (int j = 0; j < a.Columns; j++) {
                        newValues[i,j] = a[i,j] - b[i,j];
                    }
                }
                return new Matrix(newValues);
            }
        }

        public static Matrix operator* (Matrix a, Matrix b) {
            if (a.Columns != b.Rows) {
                throw new ArgumentException("Can not multiply matricies where a.columns != b.rows");
            } else {
                float[,] newValues = new float[a.Rows, b.Columns];
                for (int k = 0; k < a.Columns; k++)
                {
                    for (int i = 0; i < a.Rows; i++)
                    {
                        for (int j = 0; j < b.Columns; j++) {
                            newValues[i,j] += a[i, k] * b[k, j];
                        }
                    }
                }
                return new Matrix(newValues);
            }
        }

        public static Matrix operator* (Matrix a, float b) {
            float[,] newValues = new float[a.Rows, a.Columns];
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++) {
                    newValues[i,j] += a[i, j] * b;
                }
            }
            return new Matrix(newValues);
        }

        /// <summary>
        /// Multiply two matricies elementwise
        /// </summary>
        public static Matrix operator ^ (Matrix a, Matrix b) {
            if (a.Columns != b.Columns || a.Rows != b.Rows) {
                throw new ArgumentException("Matricies does not have the same size");
            } else {
                float[,] newValues = new float[a.Rows, a.Columns];
                for (int i = 0; i < a.Rows; i++)
                {
                    for (int j = 0; j < a.Columns; j++) {
                        newValues[i,j] = a[i,j] * b[i,j];
                    }
                }
                return new Matrix(newValues);
            }
        }

        public static bool operator== (Matrix a, Matrix b) {
            if (a.Columns != b.Columns || a.Rows != b.Rows) {
                return false;
            } else {
                for (int i = 0; i < a.Rows; i++)
                {
                    for (int j = 0; j < a.Columns; j++) {
                        //Checking for equality, taking floating point precision into account
                        if((float)Math.Abs(a[i,j] - b[i,j]) > 0.00001f) {
                            return false;
                        }
                    }
                }
                return true;
            }
        }



        public static bool operator!= (Matrix a, Matrix b) {
            return !(a == b);
        }

        public override string ToString(){
            string str = "";
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++) {
                    str += this[i,j];
                    if (j != this.Columns - 1) {
                        str += ", ";
                    }
                }
                if (i != this.Rows - 1)
                {
                    str += "\n";
                }
            }
            return str;
        }

        public static Matrix Transpose(Matrix m){
            Matrix newMatrix = new Matrix(m.Columns, m.Rows);
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++) {
                    newMatrix[j,i] = m[i,j];
                }
            }
            return newMatrix;
        }

        /// <summary>
        /// Randomize the matrix's entries with new values between min and max, both inclusive.
        /// </summary>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        public void Randomize(float min, float max) {
            Random rnd = new Random();
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++) {
                    this[i,j] = map(rnd.Next(0, 100000001), 0, 100000000, min, max); 
                }
            }
        }

        public delegate float Mapper(float val);  
        public static Matrix Map(Matrix m, Mapper mapper) {
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++) {
                    m[i, j] = mapper(m[i, j]);
                }
            }
            return m;
        }

        public float this[int i, int j] {
            get{
              return values[i, j];
            }
            set{
              values[i, j] = value;
            }
        }

        /// <summary>
        /// Maps value from range [minVal, maxVal] to [minMap, maxMap]
        /// </summary>
        private static float map(float value, float minVal, float maxVal, float minMap, float maxMap) {
            return (value - minVal) / (maxVal - minVal) * (maxMap - minMap) + minMap;
        }
    }
}

