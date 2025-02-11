//------------------------------------------------------------------------------
// @project: ColPivHouseholderQR
// @file: MatrixBlock.cs
// @author: NK
// @date: 11.2.2025

using System.Diagnostics;

using MatrixType = MathNet.Numerics.LinearAlgebra.Matrix<double>;
using VectorType = MathNet.Numerics.LinearAlgebra.Vector<double>;

namespace ColPivHouseholderQR {

  //----------------------------------------------------------------------------
  // @class MatrixBlock
  // @brief Static methods for extracting matrix parts.
  //
  // @note This is far from complete, but sufficient for the current implemen-
  //   tation. Might get extended later though, if other factorization methods
  //   become desirable.
  public static class MatrixBlock {

    public static void SwapValues(VectorType array, int i, int j) {
      Debug.Assert(0 <= i && i < array.Count);
      Debug.Assert(0 <= j && j < array.Count);

      (array[j], array[i]) = (array[i], array[j]);
    }

    //--------------------------------------------------------------------------

    public static void SwapColumns(MatrixType matrix, int ci, int cj) {
      int numRows = matrix.RowCount;
      int numColumns = matrix.ColumnCount;

      if (0 == numRows || 0 == numColumns) return;

      Debug.Assert(ci < numColumns && cj < numColumns);

      for (int i = 0; i < numRows; ++i) {
        (matrix[i, ci], matrix[i, cj]) =
        (matrix[i, cj], matrix[i, ci]);
      }
    }

    //--------------------------------------------------------------------------

    public static VectorType ColumnTail(MatrixType matrix,
                                        int column,
                                        int tailSize)
    {
      int numRows = matrix.RowCount;
      int numColumns = matrix.ColumnCount;

      Debug.Assert(0 <= column && column < numColumns);
      Debug.Assert(0 <= tailSize && tailSize <= numRows);

      return matrix.Column(column).SubVector(
        numRows - tailSize, tailSize);
    }

    //--------------------------------------------------------------------------

    public static double MaxTailValue(VectorType array,
                                      int tailSize,
                                      ref int maxIndex)
    {
      Debug.Assert(0 < tailSize && tailSize <= array.Count);

      var tailVector =
        array.SubVector(array.Count - tailSize, tailSize);

      maxIndex = tailVector.MaximumIndex();
      return tailVector[maxIndex];
    }

    //--------------------------------------------------------------------------

    public static MatrixType MatrixPart(MatrixType matrix,
                                        int startRow,
                                        int startColumn,
                                        int numPartRows,
                                        int numPartColumns)
    {
      int numRows = matrix.RowCount;
      int numColumns = matrix.ColumnCount;

      Debug.Assert(0 <= startRow && startRow < numRows);
      Debug.Assert(0 <= startColumn && startColumn < numColumns);

      Debug.Assert(1 <= numPartRows &&
                   numPartRows <= numRows - startRow);
      Debug.Assert(1 <= numPartColumns &&
                   numPartColumns <= numColumns - startColumn);


      return matrix.SubMatrix(
        startRow, numPartRows, startColumn, numPartColumns);
    }

    //--------------------------------------------------------------------------

    public static MatrixType TopRows(MatrixType matrix, int numRows) {
      return MatrixPart(matrix, 0, 0, numRows, matrix.ColumnCount);
    }

    //--------------------------------------------------------------------------

    public static MatrixType BottomRows(MatrixType matrix, int numRows) {
      return MatrixPart(
        matrix, matrix.RowCount - numRows, 0, numRows, matrix.ColumnCount);
    }

    //--------------------------------------------------------------------------

    public static MatrixType TopLeftPart(MatrixType matrix,
                                         int numRows,
                                         int numColumns)
    {
      return MatrixPart(matrix, 0, 0, numRows, numColumns);
    }

    //--------------------------------------------------------------------------

    public static MatrixType BottomRightPart(MatrixType matrix,
                                             int numRows,
                                             int numColumns)
    {
      return MatrixPart(matrix,
                        matrix.RowCount - numRows,
                        matrix.ColumnCount - numColumns,
                        numRows,
                        numColumns);
    }

    //--------------------------------------------------------------------------

    public static void MakeUpperTriangular(MatrixType matrix) {
      for (int i = 1; i < matrix.RowCount; ++i) 
        for (int j = 0; j < Math.Min(i, matrix.ColumnCount); ++j)
          matrix[i, j] = 0;
    }

    //--------------------------------------------------------------------------

    public static MatrixType ApplyHouseholderLeft(MatrixType block,
                                                  VectorType essential,
                                                  double tau)
    {
      int numBlockRows = block.RowCount;
      int numBlockColumns = block.ColumnCount;

      if (1 == numBlockRows) {  block *= (1 - tau); }
      else if (0 != tau) {

        MatrixType bottom = MatrixBlock.BottomRightPart(
          block, numBlockRows - 1, numBlockColumns);

        var tmp = (essential * bottom) + block.Row(0);

        block.SetRow(0, block.Row(0) - tau * tmp);

        // Basic math: if A is a matrix of dimension n x m and B has dimension
        // m x k, then the product (A * B) has the dimension n x k.
        bottom -=
          MatrixType.Build.DenseOfColumnVectors(tau * essential) *
          MatrixType.Build.DenseOfRowVectors(tmp);

        block.SetSubMatrix(1, 0, bottom);
      }

      return block;
    }
  }
}


