# MatrixMate
A console-based application created for Linear Algebra students. This application is capable, in its current state, of performing basic Matrix operations.

# Features and Tools Used
MatrixMate so far is made entirely in C#. As of today, it is capable of performing these features:
- Matrix Addition
- Matrix Multiplication (Matrix-to-Matrix and Scalar-to-Matrix)
- Inverse Matrix Calculation
- Gauss-Jordan Elimination (Conversion of a System of Linear Equations to Reduced-Row Echelon)
- Calculating Determinants

Future Features to be implemented:
- A Graphical User Interface for ease of use.
- LU-factorization for Matrix.

# How to use
Upon opening of the application, you will immediately be met by the program asking for two user-inputs. The row-size of your matrix, and the column-size. The menu that follows after your response depends on whether the row-size and column-size are equal. If they are, you will be able to perform exclusive operations such as calculating determinants and inverses of a matrix. When asked to input the entries of a matrix, you are to strictly enter them one row at a time separated by a space. A single entry can also be input as a fraction; two numbers separated by a forward slash without spaces.

For example:
- "1 2 3 4/5" and "1 2 3 4" are valid inputs.
- "1 2 3 4 / 5" and "1 2.6 3 4" are not.
- Inputs where an entry is separated by more than one space is also invalid.

Note: As of now, the feature to calculate reduced-row-echelon only works on systems that have the same number of unknowns as equations. Should there be more unknowns than equations or vice versa, errors may occur.

# How to run

<br>Clone the repository and run the executable MatrixMate application file. If the operating system prevents you from running the unrecognized app, click more info then run anyway.

# License 
Distributed under GNU General Public License v2.0. See LICENSE.txt for more information.
