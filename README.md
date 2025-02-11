<h3>
<p align="center">
  QR-factorization of matrices using Householder transformations on top of Math.NET.
</p>
</h3>

A plain use of Math.NET stuff in order to solve a linear system of equations
might looks like this:

```
Matrix<double> A = ...
Matrix<double> b = ...

var result = A.solve(b);
```

In case the result is wrong, inexact, or contains invalid values (NaN) - which is
probably more often than not - you might want to use this extension as alternative:

```
Matrix<double> A = ...
Matrix<double> b = ...

var qr = new HouseholderQR(A);
var result = qr.Solve(b);
```

This extension provides a QR factorization of matrices using Householder
transformations with addition of column pivoting. The implementation is closely
based on the algorithm as found in the Eigen C++ library and can therefore be
seen as a copy/clone of it.

