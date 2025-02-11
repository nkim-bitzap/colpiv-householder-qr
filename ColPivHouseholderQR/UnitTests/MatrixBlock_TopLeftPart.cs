using MathNet.Numerics.LinearAlgebra;
using ColPivHouseholderQR;

namespace UnitTests;

public class MatrixBlock_TopLeftPart {

    readonly Matrix<double> sampleMatrix =
      Matrix<double>.Build.DenseOfColumns([
        [ 0, 2, 0, 1, -4 ],
        [-3, 3, -1, 0, 1],
        [1, 0, 0, 1, 3],
        [4, 1, 2, 0, 3]
      ]);

    //--------------------------------------------------------------------------

    [Fact]
    public void Test_MatrixBlock_TopLeftPart_1() {
      var matrix = sampleMatrix.Clone();

      var part = MatrixBlock.TopLeftPart(matrix, 1, 1);

      Assert.Equal(1, part.RowCount);
      Assert.Equal(1, part.ColumnCount);
      Assert.Equal(0, part[0, 0]);
    }

    //--------------------------------------------------------------------------

    [Fact]
    public void Test_MatrixBlock_TopLeftPart_2() {
      var matrix = sampleMatrix.Clone();

      var part = MatrixBlock.TopLeftPart(matrix, 1, 2);

      Assert.Equal(1, part.RowCount);
      Assert.Equal(2, part.ColumnCount);
      Assert.Equal(0, part[0, 0]);
      Assert.Equal(-3, part[0, 1]);
    }

    //--------------------------------------------------------------------------

    [Fact]
    public void Test_MatrixBlock_TopLeftPart_3() {
      var matrix = sampleMatrix.Clone();

      var part = MatrixBlock.TopLeftPart(matrix, 2, 1);

      Assert.Equal(2, part.RowCount);
      Assert.Equal(1, part.ColumnCount);
      Assert.Equal(0, part[0, 0]);
      Assert.Equal(2, part[1, 0]);
    }

    //--------------------------------------------------------------------------

    [Fact]
    public void Test_MatrixBlock_TopLeftPart_4() {
      var matrix = sampleMatrix.Clone();

      var part = MatrixBlock.TopLeftPart(matrix, 2, 2);

      Assert.Equal(2, part.RowCount);
      Assert.Equal(2, part.ColumnCount);
      Assert.Equal(0, part[0, 0]);
      Assert.Equal(2, part[1, 0]);
      Assert.Equal(-3, part[0, 1]);
      Assert.Equal(3, part[1, 1]);
    }

    //--------------------------------------------------------------------------

    [Fact]
    public void Test_MatrixBlock_TopLeftPart_5() {
      var matrix = sampleMatrix.Clone();

      var part =
        MatrixBlock.TopLeftPart(matrix, matrix.RowCount, matrix.ColumnCount);

      Assert.Equal(matrix.ColumnCount, part.ColumnCount); 
      Assert.Equal(matrix.RowCount, part.RowCount);

      for (int row = 0; row < matrix.RowCount; ++row) {
        for (int col = 0; col < matrix.ColumnCount; ++col) {
          Assert.Equal(matrix[row, col], part[row, col]);
        }
      }
    }
}