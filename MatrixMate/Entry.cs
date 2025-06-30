using System;

namespace MatrixMate
{
    public class Entry
    {
        public int numerator;
        public int denominator;

        public Entry(int a)
        {
            this.numerator = a;
            this.denominator = 1;
        }
        public Entry()
        {
            this.numerator = 1;
            this.denominator = 1;
        }
        public Entry(int numerator, int denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;
            this.Simplify();
        }

        public Entry(Entry Copy)
        {
            this.numerator = Copy.numerator;
            this.denominator = Copy.denominator;
        }
        public void Addition(Entry A)
        {
            if (this.denominator == A.denominator)
            {
                this.numerator += A.numerator;
                this.Simplify();
                return;
            }
            else
            {
                int temp = this.denominator;
                this.denominator *= A.denominator;
                this.numerator *= A.denominator;

                A.denominator *= temp;
                A.numerator *= temp;

                this.numerator += A.numerator;
                this.Simplify();
            }
        }

        private void Simplify()
        {
            int basis = (this.numerator > this.denominator ? this.numerator : this.denominator);
            while (numerator % basis != 0 || denominator % basis != 0)
            {
                basis--;
            }
            this.numerator = this.numerator / basis;
            this.denominator = this.denominator / basis;
            if (this.denominator < 0)
            {
                this.denominator *= -1;
                this.numerator *= -1;
            }
            return;
        }

        public Entry Multiplication(Entry A)
        {
            Entry newMat = new Entry(this.numerator, this.denominator);
            newMat.numerator *= A.numerator;
            newMat.denominator *= A.denominator;
            newMat.Simplify();
            return newMat;
        }
        public Entry Multiplication(int a)
        {
            Entry newMat = new Entry(this.numerator, this.denominator);
            newMat.numerator *= a;
            newMat.Simplify();
            return newMat;
        }
        public void Input(int Entry)
        {
            this.numerator = Entry;
            this.denominator = 1;
        }
        public void Input(Entry Value)
        {
            this.numerator = Value.numerator;
            this.denominator = Value.denominator;
        }

        public override string ToString()
        {
            return this.numerator + (this.denominator != 1 ? "/" + this.denominator : (string.Empty));
        }
    }
}
