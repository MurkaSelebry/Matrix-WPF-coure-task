using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MatrixKirill
{
    internal class Matrix
    {

        private double[][] MatrixData;
        public uint Nrows { get; set; }

        public uint Ncols { set; get; }

        public Matrix()
        {
            Nrows = Ncols = 0;
        }

        public Matrix(uint n)
        {
            Nrows = Ncols = n;
            MatrixData = new double[Nrows][];
            for (int i = 0; i < Nrows; i++) MatrixData[i] = new double[Ncols];
        }


        public Matrix(uint nrows, uint ncols, double fill_value = 0.0)
        {
            Nrows = nrows;
            Ncols = ncols;
            MatrixData = new double[Nrows][];
            for (uint i = 0; i < Nrows; i++)
            {
                MatrixData[i] = new double[Ncols];
                for (uint j = 0; j < Ncols; j++) MatrixData[i][j] = fill_value;
            }
        }

        public double this[int i, int j]
        {
            get => MatrixData[i][j];
            set => MatrixData[i][j] = value;
        }

        public double[] this[int i]
        {
            get => MatrixData[i];
            set => MatrixData[i] = value;
        }

        public uint[] GetSize() => new uint[] { Nrows, Ncols };


        // Вывод матрицы с клавиатуры
        public void PrintMatrix()
        {
            for (int i = 0; i < Nrows; i++)
            {
                for (int j = 0; j < Ncols; j++)
                {
                    Console.Write(MatrixData[i][j] + "\t");
                }

                Console.WriteLine();
            }
        }

        // Умножение матрицы А на матрицу Б
        public static Matrix? MultiplyMatrix(Matrix a, Matrix b)
        {
            if (a.Ncols != b.Nrows) return null;
            var res = new Matrix(a.Nrows, b.Ncols);
            for (int i = 0; i < a.Nrows; i++)
            {
                for (int j = 0; j < b.Ncols; j++)
                {
                    for (int k = 0; k < b.Nrows; k++)
                    {
                        res[i, j] += a[i, k] * b[k, j];
                    }
                }
            }

            return res;
        }

        public static int GaussTransformation(Matrix matrix, double[] arrayB, double[] x)
        {
            int i, j, k;
            var gaussMatrix = new Matrix(matrix.Nrows);
            var b = new double[matrix.Nrows];
            for (i = 0; i < matrix.Nrows; i++)
                for (j = 0; j < matrix.Nrows; j++)
                    gaussMatrix[i][j] = matrix[i, j];
            for (i = 0; i < matrix.Nrows; i++) b[i] = arrayB[i];
            for (k = 0; k < matrix.Nrows; k++)
            {
                var max = Math.Abs(gaussMatrix[k, k]);
                var r = k;
                for (i = k + 1; i < matrix.Nrows; i++)
                    if (Math.Abs(gaussMatrix[i, k]) > max)
                    {
                        max = Math.Abs(gaussMatrix[i, k]);
                        r = i;
                    }

                double c;
                for (j = 0; j < matrix.Nrows; j++)
                {
                    c = gaussMatrix[k, j];
                    gaussMatrix[k, j] = gaussMatrix[r, j];
                    gaussMatrix[r, j] = c;
                }

                c = b[k];
                b[k] = b[r];
                b[r] = c;
                for (i = k + 1; i < matrix.Nrows; i++)
                {
                    double m;
                    for (m = gaussMatrix[i, k] / gaussMatrix[k, k], j = k; j < matrix.Nrows; j++)
                        gaussMatrix[i, j] -= m * gaussMatrix[k, j];
                    b[i] -= m * b[k];
                }

            }

            if (gaussMatrix[Convert.ToInt32(matrix.Nrows) - 1, Convert.ToInt32(matrix.Nrows) - 1] == 0)
                return b[Convert.ToInt32(matrix.Nrows) - 1] == 0 ? -1 : -2;
            for (i = Convert.ToInt32(matrix.Nrows) - 1; i >= 0; i--)
            {
                double s;
                for (s = 0, j = i + 1; j < matrix.Nrows; j++)
                    s += gaussMatrix[i, j] * x[j];
                x[i] = (b[i] - s) / gaussMatrix[i, i];
            }
            return 0;
        }

        public static Matrix? InverseMatrix(Matrix matrix)
        {
            if (DeterminantOfMatrix(matrix) == 0.0)
            {
                MessageBox.Show("Determinant is zero!");
                return null;
            }
            var inverseMatrix = new Matrix(matrix.Nrows, matrix.Ncols);
            var res = 0;
            var b = new double[matrix.Nrows];
            var x = new double[matrix.Nrows];
            for (var i = 0; i < matrix.Nrows; i++)
            {
                for (var j = 0; j < matrix.Nrows; j++)
                    if (j == i) b[j] = 1;
                    else b[j] = 0;
                res = GaussTransformation(matrix, b, x);
                if (res != 0) break;
                for (var j = 0; j < matrix.Nrows; j++)
                {
                    inverseMatrix[j, i] = x[j];
                }
            }
            return res != 0 ? null: inverseMatrix;
        }

        public static double DeterminantOfMatrix(Matrix matrix)
        {

            double det = 1;
            var a = new Matrix(matrix.Nrows);
            for (int i = 0; i < matrix.Nrows; i++)
                for (int j = 0; j < matrix.Nrows; j++)
                    a[i, j] = matrix[i, j];
            for (int k = 0; k < matrix.Nrows; k++)
            {
                var max = Math.Abs(a[k, k]);
                int change_det = k;
                for (int i = k + 1; i < matrix.Nrows; i++)
                    if (Math.Abs(a[i, k]) > max)
                    {
                        max = Math.Abs(a[i, k]);
                        change_det = i;
                    }
                if (change_det != k) det = -det;
                for (int j = 0; j < matrix.Nrows; j++)
                    (a[k, j], a[change_det, j]) = (a[change_det, j], a[k, j]);


                for (int i = k + 1; i < matrix.Nrows; i++)
                    for (int j = k; j < matrix.Nrows; j++)
                        a[i, j] -= a[i, k] / a[k, k] * a[k, j];


            }

            for (int i = 0; i < matrix.Nrows; i++)
                det *= a[i, i];
            if (Math.Abs(det) < 0.001) det = 0.0;
            return det;

        }
        public static double[]? SLAE_Solution(Matrix a, double[] y)
        {
            const double eps = 0.00001;  // точность
            var x = new double[a.Nrows];
            var k = 0;
            while (k < a.Nrows)
            {
                // Поиск строки с максимальным a[i][k]
                var max = Math.Abs(a[k, k]);
                var index = k;
                for (int i = k + 1; i < a.Nrows; i++)
                {
                    if (Math.Abs(a[i, k]) > max)
                    {
                        max = Math.Abs(a[i, k]);
                        index = i;
                    }
                }
                // Перестановка строк
                if (max < eps)
                {
                    // нет ненулевых диагональных элементов
                    Console.WriteLine($"Решение получить невозможно из-за нулевого столбца {index} матрицы A ");
                    return null;
                }

                double temp;
                for (int j = 0; j < a.Nrows; j++)
                {
                    temp = a[k, j];
                    a[k][j] = a[index][j];
                    a[index][j] = temp;
                }
                temp = y[k];
                y[k] = y[index];
                y[index] = temp;
                // Нормализация уравнений
                for (int i = k; i < a.Nrows; i++)
                {
                    temp = a[i][k];
                    if (Math.Abs(temp) < eps) continue; // для нулевого коэффициента пропустить
                    for (int j = 0; j < a.Nrows; j++)
                        a[i][j] /= temp;
                    y[i] /= temp;
                    if (i == k) continue; // уравнение не вычитать само из себя
                    for (int j = 0; j < a.Nrows; j++)
                        a[i][j] -= a[k][j];
                    y[i] -= y[k];
                }
                k++;
            }
            // обратная подстановка
            for (k = Convert.ToInt32(a.Nrows - 1); k >= 0; k--)
            {
                x[k] = y[k];
                for (int i = 0; i < k; i++)
                    y[i] -= a[i][k] * x[k];
            }
            return x;
        }
    }
}
