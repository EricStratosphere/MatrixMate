using System;
using System.ComponentModel;
using System.Data.Common;
using System.Security.Cryptography;

namespace MatrixMate
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to MatrixMate! A Console Application for Basic Matrix Operations.");
            while (true)
            {
                Console.WriteLine("Enter matrix row size: ");
                int row = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter matrix column size");
                int column = Convert.ToInt32(Console.ReadLine());
                if (row > 0 && column > 0)
                {
                    if (row == column)
                    {
                        Menu1(row, column);
                    }
                    else
                    {
                        Menu2(row, column);
                    }
                    Console.WriteLine("Operation finished. Would you like to perform another one? [Y] [N]");
                    char resp = char.ToLower(Console.ReadLine().ToCharArray()[0]);
                    if (resp == 'y')
                    {
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Closing program, thank you for using the application!");
                        return;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid inputs!");
                }
            }
        }

        public static Entry[,] FetchMatrix(int Row, int Column)
        {
            Console.WriteLine("Enter matrix values (Note: Please enter the values one row at a time):");
            Entry[,] Matrix = new Entry[Row, Column];
            for (int i = 0; i < Row; i++)
            {
                Entry entry;
                string response;

                Console.WriteLine($"Matrix Row {i}: ");
                response = Console.ReadLine();
                var MatrixRow = response.Split(' ');
                bool isValidInput = true;

                if (MatrixRow.Length != Column)
                {
                    isValidInput = false;
                }
                for (int k = 0; k < Column; k++)
                {
                    int numerator, denominator;
                    if (int.TryParse(MatrixRow[k], out numerator))
                    {
                        entry = new Entry(numerator);
                    }
                    else
                    {
                        if (MatrixRow[k].IndexOf("/") == -1)
                        {
                            isValidInput = false;
                            break;
                        }
                        else
                        {
                            int.TryParse(MatrixRow[k].Substring(0, response.IndexOf("/")), out numerator);
                            int nextVal = MatrixRow[k].IndexOf("/") + 1;
                            if (nextVal == MatrixRow[k].Length)
                            {
                                isValidInput = false;
                                break;
                            }
                            int.TryParse(MatrixRow[k].Substring(nextVal), out denominator);
                            if (denominator == 0)
                            {
                                isValidInput = false;
                                break;
                            }
                            entry = new Entry(numerator, denominator);
                        }
                    }
                    Matrix[i, k] = entry;
                }
                if (!isValidInput)
                {
                    i--;
                    Console.WriteLine("Invalid input! Try again.");
                    continue;
                }
            }
            return Matrix;
        }
        static void Menu1(int row, int column)
        {
            int resp;
            Console.WriteLine("[0] Calculate Inverse");//Unfinished      
            Console.WriteLine("[1] Perform Matrix Addition");//Done
            Console.WriteLine("[2] Perform Matrix Multiplication");//Done
            Console.WriteLine("[3] Find Determinant"); //Unfinished
            try
            {
                resp = Convert.ToInt32(Console.ReadLine().ToCharArray()[0]) - '0';
                if (resp < 0 || resp > 3)
                {
                    Console.Clear();
                    Console.WriteLine(resp);
                    Console.WriteLine("Invalid output! Try again?");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("Invalid output! Try again?" + e.Message);
                return;
            }
            finally
            {

            }
            switch (resp)
            {
                case 0: CalculateInverse(row, column); return;
                case 1: MatrixAddition(row, column); return;
                case 2: MatrixMultiplication(row, column); return;
                case 3: FindDeterminant(row, column); return;
            }
        }
        static void Menu2(int row, int column)
        {
            while (true)
            {
                int resp;
                Console.WriteLine("[0] Perform Matrix Addition");//Done
                Console.WriteLine("[1] Find Reduced Row Echelon");//Done
                Console.WriteLine("[2] Perform Matrix Multiplication");//Done
                try
                {
                    resp = Convert.ToInt32(Console.ReadLine().ToCharArray()[0]) - '0';
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid output! Try again" + e.Message);
                    return;
                }
                if (resp < 0 || resp > 2)
                {
                    Console.Clear();
                    Console.WriteLine(resp);
                    Console.WriteLine("Invalid output! Try again");
                    return;
                }
                switch (resp)
                {
                    case 0: MatrixAddition(row, column); return;
                    case 1: ReducedRowEchelon(row, column); return;
                    case 2: MatrixMultiplication(row, column); return;
                }
            }
        }

        //To Do: 
        static int FindDeterminant(int Row, int Column)
        {
            bool isNegated = false;
            bool allAreZeroes = true;
            Entry[,] Matrix = FetchMatrix(Row, Column);

            for (int i = 0; i < Column; i++)
            {
                if (Matrix[i, i].numerator == 0)
                {
                    allAreZeroes = true;
                    for (int j = i; j < Row; j++)
                    {
                        if (Matrix[j, i].numerator != 0)
                        {
                            allAreZeroes = false;
                            SwapRows(Matrix, Row, Column, i, j);
                            isNegated = !isNegated;
                            break;
                        }
                    }
                    if (allAreZeroes)
                    {
                        Console.WriteLine("0");
                        return 0;
                    }
                }
                for (int j = i + 1; j < Row; j++)
                {
                    Entry Reciprocal = new Entry(Matrix[i, i].denominator, Matrix[i, i].numerator);
                    //Console.WriteLine($"Reciprocal: {Reciprocal}");
                    ElementaryRowFunction(Matrix, Reciprocal, Row, Column, i, j);
                }
                //Console.WriteLine("One operation Done: ");
                //DispMatrix(Matrix, Row, Column);
            }
            //int product = 1;
            if (isNegated)
            {
                for (int i = 0; i < Row; i++)
                {
                    for (int j = 0; j < Column; j++)
                    {
                        Matrix[i, j].numerator *= -1;
                    }
                }
            }
            Entry Determinant = new Entry(1);
            for (int i = 0; i < Row; i++)
            {
                Determinant = Determinant.Multiplication(Matrix[i, i]);
            }
            Console.WriteLine(Determinant);
            return 0;
        }
        //Hatagi nig pangan jusko.
        static void ElementaryRowFunction(Entry[,] Matrix, Entry Reciprocal, int RowSize, int ColumnSize, int SourceRow, int DestinationRow)
        {
            Entry[] TempMatrix = new Entry[ColumnSize];
            Entry Multiple = new Entry(Reciprocal);
            //Console.WriteLine("DestinationRow + SourceRow: " + Matrix[DestinationRow, SourceRow]);
            Reciprocal = Reciprocal.Multiplication(-1).Multiplication(Matrix[DestinationRow, SourceRow]);
            //Console.WriteLine("Reciprocal: " + Reciprocal);
            //Console.WriteLine("Values of TempMatrix: ");
            for (int k = 0; k < ColumnSize; k++)
            {
                TempMatrix[k] = (Matrix[SourceRow, k].Multiplication(Reciprocal));
                //Console.WriteLine(TempMatrix[k]);
            }
            for (int k = 0; k < ColumnSize; k++)
            {
                Matrix[DestinationRow, k].Addition(TempMatrix[k]);
            }
        }

        static void CalculateInverse(int Row, int Column)
        {
            //Creating the Identity Matrix.
            Entry[,] IdentityMatrix = new Entry[Row, Column];
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    Entry entry = new Entry(0);
                    if (i == j)
                        entry.Input(1);
                    IdentityMatrix[i, j] = entry;
                }
            }
            DispMatrix(IdentityMatrix, Row, Column);

            Entry[,] Matrix = FetchMatrix(Row, Column);

            Console.Clear();
            Console.WriteLine("Original values:");
            DispMatrix(Matrix, Row, Column);
            for (int j = 0; j < Column; j++)
            {
                bool allAreZeroesFlag = false;
                //Case 1: Matrix[j, j] == 0
                //Scans the first column for any leading ones or values that are not one. If there aren't any, then the matrix is indivisible. 
                if (Matrix[j, j].numerator == 0)
                {
                    allAreZeroesFlag = true;
                    //Console.WriteLine("Zero found! Displaying coordinates: " + j + "\nValue: " + Matrix[j,j]);
                    for (int i = j; i < Row; i++)
                    {
                        if (Matrix[i, j].numerator != 0)
                        {
                            allAreZeroesFlag = false;
                            SwapRows(Matrix, Row, Column, i, j);
                            SwapRows(IdentityMatrix, Row, Column, i, j);
                            //Console.WriteLine("Zero found! Swapping rows.");
                            //DispMatrix(Matrix, Row, Column);
                            break;
                        }
                    }
                }
                //Console.WriteLine("test");
                //Console.WriteLine("One operation done.");
                if (allAreZeroesFlag == true)
                {
                    Console.WriteLine("The system does not have an inverse! Terminating operation.\nShowing status of matrix before termination:");
                    DispMatrix(Matrix, Row, Column);
                    return;
                }

                Entry Multiple = new Entry(Matrix[j, j].denominator, Matrix[j, j].numerator);
                //If the number is, let's say, 2. Then the denominator will become the numerator of the multiple and the numerator will become the nominator.
                //So the value of the multiple will be 1/2. The multiple will then be multiplied to each column in the current row to create a leading one.

                for (int k = 0; k < Column; k++)
                {
                    Matrix[j, k] = Matrix[j, k].Multiplication(Multiple);
                    IdentityMatrix[j, k] = IdentityMatrix[j, k].Multiplication(Multiple);
                }
                Multiple = null;
                GC.Collect();
                for (int i = 0; i < Row; i++)
                {
                    if (i != j)
                    {
                        AddMultipleOfOneRowToIdentityMatrix(Matrix, IdentityMatrix, Row, Column, j, i);
                    }
                }
            }
            Console.WriteLine("Final Matrix: ");
            DispMatrix(Matrix, Row, Column);
            Console.WriteLine("Inverse Matrix: ");
            DispMatrix(IdentityMatrix, Row, Column);
        }
        static void MatrixMultiplication(int Row, int Column)
        {
            Entry[,] Matrix = FetchMatrix(Row, Column);
            int SecondColumn;
            Console.WriteLine("Enter size of second matrix' column");
            SecondColumn = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Second matrix values");
            Entry[,] SecondMatrix = FetchMatrix(Column, SecondColumn);


            /*{1, 2, 3},
               {4, 5, 6}
            
             *
             
             {1, 2},
             {3, 4},
            {5, 6}*/

            //Output = {a, b},
            //         {c, d}
            Entry[,] Product = new Entry[Row, SecondColumn];
            int ProductRow = Row;
            int ProductColumn = SecondColumn;
            for (int i = 0; i < ProductRow; i++)
            {
                for (int j = 0; j < ProductColumn; j++)
                {
                    Entry prod = new Entry(0);
                    for (int k = 0; k < Column; k++)
                    {
                        prod.Addition(Matrix[i, k].Multiplication(SecondMatrix[k, j]));
                    }
                    Product[i, j] = prod;
                }
            }
            DispMatrix(Product, Row, SecondColumn);
        }
        static void MatrixAddition(int Row, int Column)
        {
            Console.WriteLine("First Matrix");
            Entry[,] Matrix = FetchMatrix(Row, Column);



            Console.WriteLine("Second Matrix");
            Entry[,] SecondMatrix = FetchMatrix(Row, Column);
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    Matrix[i, j].Addition(SecondMatrix[i, j]);
                }
            }
            DispMatrix(Matrix, Row, Column);
        }

        public static void ReducedRowEchelon(int Row, int Column)
        {
            if (Column - 1 != Row)
            {
                Console.WriteLine("You have discovered an unfinished feature! Stay tuned for future updates to use these features hehehe");
            }
            Entry[,] Matrix = FetchMatrix(Row, Column);
            Console.Clear();
            Console.WriteLine("Original values:");
            DispMatrix(Matrix, Row, Column);
            for (int j = 0; j < Column - 1; j++)
            {
                bool allAreZeroesFlag = false;
                //Case 1: Matrix[j, j] == 0
                //Scans the first column for any leading ones or values that are not one. If there aren't any, then the matrix is indivisible. 
                if (Matrix[j, j].numerator == 0)
                {
                    allAreZeroesFlag = true;
                    //Console.WriteLine("Zero found! Displaying coordinates: " + j + "\nValue: " + Matrix[j,j]);
                    for (int i = j; i < Row; i++)
                    {
                        if (Matrix[i, j].numerator != 0)
                        {
                            allAreZeroesFlag = false;
                            SwapRows(Matrix, Row, Column, i, j);
                            //Console.WriteLine("Zero found! Swapping rows.");
                            //Console.WriteLine();
                            //Console.WriteLine();
                            //DispMatrix(Matrix, Row, Column);
                            break;
                        }
                    }
                }
                //Console.WriteLine("test");
                //Console.WriteLine("One operation done.");
                if (allAreZeroesFlag == true)
                {
                    Console.WriteLine("The system cannot be solved! Terminating operation.\nShowing status of matrix before termination:");
                    DispMatrix(Matrix, Row, Column);
                    return;
                }
                //Console.WriteLine("Current Row: " + j);
                //Console.WriteLine("Matrix[j,j] = " + Matrix[j,j].numerator + "/" + Matrix[j,j].denominator);
                //Early Fractions for the first row.

                //NOTE: Actually mugana na diay sha bisag zero ang official denominator kay tungod sa swapping nga algorithm kanina.
                Entry Multiple = new Entry(Matrix[j, j].denominator, Matrix[j, j].numerator);
                //If the number is, let's say, 2. Then the denominator will become the numerator of the multiple and the numerator will become the nominator.
                //So the value of the multiple will be 1/2. The multiple will then be multiplied to each column in the current row to create a leading one.

                for (int k = j; k < Column; k++)
                {
                    Matrix[j, k] = Matrix[j, k].Multiplication(Multiple);
                }
                Multiple = null;
                GC.Collect();
                for (int i = 0; i < Row; i++)
                {
                    if (i != j)
                        AddMultipleOfOneRowToAnother(Matrix, Row, Column, j, i);
                }

            }
            Console.WriteLine("Final Matrix: ");
            DispMatrix(Matrix, Row, Column);
        }

        public static void SwapRows(Entry[,] Matrix, int RowSize, int ColumnSize, int Destination, int Source)
        {
            Entry[] TempMatrix = new Entry[ColumnSize];
            for (int i = 0; i < ColumnSize; i++)
            {
                TempMatrix[i] = new Entry(Matrix[Source, i]);
                Matrix[Source, i].numerator = Matrix[Destination, i].numerator;
                Matrix[Source, i].denominator = Matrix[Destination, i].denominator;
                Matrix[Destination, i].numerator = TempMatrix[i].numerator;
                Matrix[Destination, i].denominator = TempMatrix[i].denominator;
            }
            return;
        }
        public static void AddMultipleOfOneRowToAnother(Entry[,] Matrix, int RowSize, int ColumnSize, int SourceRow, int DestinationRow)
        {

            //Console.WriteLine("Destination Row: " + DestinationRow + " Source Row: " + SourceRow);
            //Console.WriteLine("Multiple to be referenced: " + Matrix[DestinationRow, SourceRow]);

            Entry[] TempMatrix = new Entry[ColumnSize];
            if (DestinationRow == RowSize || DestinationRow < 0)
                return;

            for (int i = 0; i < ColumnSize; i++)
            {
                //Example: 
                /*[1, 2, 3,
                 * -4, 5, 6,
                 * 7, 8, 9]*/

                //arr[0][0] {1} will be multiplied to (-1 * arr[1][0](-4) ) (4)
                TempMatrix[i] = (Matrix[SourceRow, i].Multiplication(-1)).Multiplication(Matrix[DestinationRow, SourceRow]);
            }
            for (int i = 0; i < ColumnSize; i++)
            {
                Matrix[DestinationRow, i].Addition(TempMatrix[i]);
            }

            // Console.WriteLine("After adding multiple of one row to another:");
            //DispMatrix(Matrix, RowSize, ColumnSize);
        }
        public static void AddMultipleOfOneRowToIdentityMatrix(Entry[,] Matrix, Entry[,] IdentityMatrix, int RowSize, int ColumnSize, int SourceRow, int DestinationRow)
        {
            //Console.WriteLine("Destination Row: " + DestinationRow + " Source Row: " + SourceRow);
            //Console.WriteLine("Multiple to be referenced: " + Matrix[DestinationRow, SourceRow]);

            Entry[] TempMatrix = new Entry[ColumnSize];
            if (DestinationRow == RowSize || DestinationRow < 0)
                return;

            Entry numReference = new Entry(Matrix[DestinationRow, SourceRow]);
            //Console.WriteLine($"NumReference: {numReference}");
            for (int i = 0; i < ColumnSize; i++)
            {
                //Example: 
                /*[1, 2, 3,
                 * -4, 5, 6,
                 * 7, 8, 9]*/

                /*[1, 0, 0]
                  [0, 1, 0]
                  [0, 0, 1]*/

                //arr[0][0] {1} will be multiplied to (-1 * arr[1][0](-4) ) (4)

                TempMatrix[i] = (Matrix[SourceRow, i].Multiplication(-1)).Multiplication(numReference);
                Matrix[DestinationRow, i].Addition(TempMatrix[i]);
                TempMatrix[i] = (IdentityMatrix[SourceRow, i].Multiplication(-1)).Multiplication(numReference);
                IdentityMatrix[DestinationRow, i].Addition(TempMatrix[i]);
            }


            // Console.WriteLine("After adding multiple of one row to another:");
            //DispMatrix(Matrix, RowSize, ColumnSize);
        }
        public static Entry[] GetRow(Entry[,] Matrix, int RowSize, int ColumnSize, int Source)
        {
            Entry[] newMatrix = new Entry[ColumnSize];
            for (int i = 0; i < ColumnSize; i++)
            {
                newMatrix[i] = Matrix[Source, i];
            }
            return newMatrix;
        }
        public static void DispMatrix(Entry[,] Matrix, int Row, int Column)
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    Console.Write(Matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}