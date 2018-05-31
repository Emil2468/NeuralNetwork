using System;
using NUnit.Framework;
using Matrices;

namespace Tests {
  [TestFixture]
  public class MatrixTests {
    Matrix m;

    [SetUp]
    public void init(){
      m = new Matrix(3, 3);
    }

    [Test]
    public void IsZero(){
      Assert.AreEqual(0, m[1, 1]);
      Assert.AreEqual(0, m[2, 1]);
      Assert.AreEqual(0, m[1, 2]);
    }
    
    [Test]
    public void SetTest(){
        m[1, 1] = 4;
        Assert.AreEqual(4, m[1, 1]);
    }

    [Test]
    public void SpecifiedVals(){
        float[,] vals = { { 3, 3 }, { 4, 5 }, { 1, 2 } };
        Matrix mat = new Matrix(vals);
        Assert.AreEqual(vals[0, 0], mat[0, 0]);
        Assert.AreEqual(vals[2, 1], mat[2, 1]);
    }

    [Test]
    public void CopyTest(){
        Matrix n = m.copy();
        n[1,2] = 4;
        Assert.AreNotEqual(n[1,2], m[1,2]);
        Assert.AreEqual(n[1,1], m[1,1]);
    }

    [Test]
    public void TestEqual(){
        Matrix a = new Matrix(new float[,] {{2, 2}, {3, 3}, {4, 4}});
        Matrix b = new Matrix(new float[,] {{2, 2}, {3, 3}, {4, 4}});
        Matrix c = new Matrix(new float[,] {{1, 2}, {3, 3}, {4, 4}});
        Matrix d = new Matrix(2,3);
        Matrix e = new Matrix(2,3);
        Assert.AreEqual(true, a == b);
        Assert.AreEqual(false, b == c);
        Assert.AreEqual(false, d == c);
        Assert.AreEqual(true, d == e);
    }

    [Test]
    public void AdditionTest(){
        Matrix a = new Matrix(new float[,] {{1, 2}, {2, 3}, {5, 4}});
        Matrix b = new Matrix(new float[,] {{2, 2}, {3, 3}, {4, 4}});
        Matrix c = new Matrix(new float[,] {{3, 4}, {5, 6}, {9, 8}});
        Matrix d = new Matrix(1,2);

        Assert.AreEqual(true, a + b == c);
        Assert.Throws<ArgumentException>(() => d = a + d);
    }

    [Test]
    public void SubtractionTest(){
        Matrix a = new Matrix(new float[,] {{1, 2}, {2, 3}, {5, 4}});
        Matrix b = new Matrix(new float[,] {{2, 2}, {3, 3}, {4, 4}});
        Matrix c = new Matrix(new float[,] {{-1, 0}, {-1, 0}, {1, 0}});
        Matrix d = new Matrix(1,2);

        Assert.AreEqual(true, a - b == c);
        Assert.Throws<ArgumentException>(() => d = a + d);
    }

    [Test]
    public void MultTest(){
        Matrix a = new Matrix(new float[,] {{1, 2}, {0, 1}});
        Matrix b = new Matrix(new float[,] {{1, 3}, {-1, 0}});
        Matrix ab = new Matrix(new float[,] {{-1, 3}, {-1, 0}});
        Matrix ba = new Matrix(new float[,] {{1, 5}, {-1, -2}});
        Assert.IsTrue(a * b == ab);
        Assert.IsTrue(b * a == ba);
    }

    [Test]
    public void MultTest2(){
        Matrix a = new Matrix(new float[,] {{1,2,3}, {1,2,3}});
        Matrix b = new Matrix(new float[,] {{1}, {2}, {3}});
        Matrix c = new Matrix(new float[,] {{14},{14}});
        Assert.IsTrue(a * b == c);
    }

    [Test]
    public void RandomTest(){
        Matrix a = new Matrix(40,40);
        a.Randomize(-1,1);
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Columns; j++) {
                Assert.IsTrue(a[i, j] >= -1 && a[i,j] <= 1);
            }
        }
    }

    [Test]
    public void MapTest(){
        Matrix a = new Matrix(3,3);
        a = Matrix.Map(a, add1);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++) {
                    Assert.AreEqual(1, a[i,j]);
                }
            }
    }

    private float add1(float val) {
        return val + 1;
    }

    [Test]
    public void TransposeTest(){
        Matrix a = new Matrix(new float[,]{{1},{3}});
        Matrix b = new Matrix(new float[,]{{1,3}});
        Assert.IsTrue(Matrix.Transpose(a) == b);
    }

    [Test]
    public void MultByNumberTest(){
        Matrix a = new Matrix(new float[,]{{1,2},{3,4}}) * 2;
        Assert.AreEqual(2, a[0,0]);
        Assert.AreEqual(4, a[0,1]);
        Assert.AreEqual(6, a[1,0]);
        Assert.AreEqual(8, a[1,1]);
    }

    [Test]
    public void FromStringTest(){
        Matrix a = new Matrix("1,2\n3,4");
        Matrix b = new Matrix("1, 2 \n 3, 4");
        Matrix c = new Matrix(new float[,]{{1,2},{3,4}});
        Assert.IsTrue(a == b);
        Assert.IsTrue(a == c);
    }
  }
}

