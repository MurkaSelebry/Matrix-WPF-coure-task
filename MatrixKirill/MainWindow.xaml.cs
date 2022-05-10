using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
namespace MatrixKirill
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^-?[0-9][0-9,\.]+$");
            e.Handled = regex.IsMatch(e.Text);
        }
    // Вспомогательная функция для размещения текстовых полей на сетке
    TextBox create_text_box(Grid gd, int rowIndex, int colIndex)
        {
            // Динамически создаём текстовое поле и добавляем его на
            // сетку белокурого (кремового) цвета
            TextBox t = new TextBox();
            gd.Children.Add(t);
            // Выставляем выравнивание текстового поля
            // по левой границе и по верхней границе соответственно
            t.HorizontalAlignment = HorizontalAlignment.Left;
            t.VerticalAlignment = VerticalAlignment.Top;
            const int size = 30;
            // Делаем размер текстового поля квадратным 30х30
            t.Width = size;
            t.Height = size;
            // Задаём расположение текстового поля на сетке относительно
            // левой и верхней её границы.
            t.Margin = new Thickness(size * colIndex, size * rowIndex, 0, 0);
            // Задаём выравнивание текста по центру
            t.TextAlignment = TextAlignment.Center;
            t.PreviewTextInput+=NumberValidationTextBox;
            return t;
        }

    private TextBox[,] _slauMatrix;
    private TextBox[] _unknownVars;
    private TextBox[] _rightSide;
        // Обработчик события кнопки задания количества уравнений СЛАУ
        private void click_set_size_slau(object sender, RoutedEventArgs e)
        {
            bool isSuccess = int.TryParse(slau_size_tb.Text, out int n);
            if (!isSuccess||n<=0) return;
            // Очищаем сетку от предыдущих текстовых полей
            
            slau_grid.Children.Clear();
            // Создаём матрицу для хранения текстовых полей
            // для ввода матрицы СЛАУ
            _slauMatrix = new TextBox[n, n];
            // Создаём массив для хранения текстовых полей
            // для вывода неизвестных СЛАУ
            _unknownVars = new TextBox[n];
            // Создаём матрицу для хранения текстовых полей
            // для ввода вектора-столбца свободных членов СЛАУ
            _rightSide = new TextBox[n];
            for (int i = 0; i < n; i++)
            {
                // Создаём и размещаем на сетке текстовые поля
                // для ввода коэффициентов матрицы СЛАУ
                for (int j = 0; j < n; j++)
                    _slauMatrix[i, j] = create_text_box(slau_grid, i, j);
                // Создаём и размещаем на сетке текстовые поля
                // для вывода неизвестных СЛАУ, делаем фон
                // ярко-бирюзовым и выводим x1, x2 и т.д.
                TextBox t = create_text_box(slau_grid, i, n + 1);
                t.Background = new SolidColorBrush(Color.FromRgb(220, 255, 255));
                t.Text = $"x{i}";
                _unknownVars[i] = t;
                // Создаём и размещаем на сетке текстовые поля
                // для ввода свободных членов СЛАУ
                _rightSide[i] = create_text_box(slau_grid, i, n + 3);
            }
        }

        private TextBox[,] _reverseMatrix;
        private void click_set_size_reverse(object sender, RoutedEventArgs e)
        {
            bool isSuccess = int.TryParse(reverse_size_tb.Text, out int n);
            if (!isSuccess || n <= 0) return;
            // Очищаем сетку от предыдущих текстовых полей

            reverse_grid.Children.Clear();
            // Создаём матрицу для хранения текстовых полей
            _reverseMatrix = new TextBox[n, n];
            
            for (int i = 0; i < n; i++)
            {
                // Создаём и размещаем на сетке текстовые поля
                for (int j = 0; j < n; j++)
                    _reverseMatrix[i, j] = create_text_box(reverse_grid, i, j);
                
            }
        }
        private TextBox[,] _matrixA;
        private TextBox[,] _matrixB;

        private void click_set_size_multy(object sender, RoutedEventArgs e)
        {
            bool isSuccess = int.TryParse(multy1_rows_tb.Text, out int a_rows);
            if (!isSuccess || a_rows <= 0) return;
            isSuccess = int.TryParse(multy1_cols_tb.Text, out int a_cols);
            if (!isSuccess || a_cols <= 0) return;
            isSuccess = int.TryParse(multy2_rows_tb.Text, out int b_rows);
            if (!isSuccess || b_rows <= 0) return;
            isSuccess = int.TryParse(multy2_cols_tb.Text, out int b_cols);
            if (!isSuccess || b_cols <= 0) return;
            // Очищаем сетку от предыдущих текстовых полей

            multy_grid.Children.Clear();
            // Создаём матрицу для хранения текстовых полей
            _matrixA = new TextBox[a_rows, a_cols];
            _matrixB = new TextBox[b_rows, b_cols];

            for (int i = 0; i < a_rows; i++)
            {
                // Создаём и размещаем на сетке текстовые поля
                for (int j = 0; j < a_cols; j++)
                    _matrixA[i, j] = create_text_box(multy_grid, i, j);

            }
            for (int i = 0; i < b_rows; i++)
            {
                // Создаём и размещаем на сетке текстовые поля
                for (int j = 0; j < b_cols; j++)
                    _matrixB[i, j] = create_text_box(multy_grid, i, j+a_cols+1);

            }
        }

        private void click_solve_slau(object sender, RoutedEventArgs e)
        {
            var matrixSLAU = new Matrix(Convert.ToUInt32(_slauMatrix.GetLength(0)));
            var rightSideDouble = new double[_rightSide.GetLength(0)];
            for (int i = 0; i < _slauMatrix.GetLength(0); i++)
            {
                double number;
                var isSuccess = double.TryParse(_rightSide[i].Text, out number);
                if (!isSuccess) return;
                rightSideDouble[i]=number;
                for (int j = 0; j < _slauMatrix.GetLength(1); j++)
                {
                    isSuccess = double.TryParse(_slauMatrix[i, j].Text, out number);
                    if (!isSuccess) return;
                    matrixSLAU[i, j] = number;
                }
            }

            var solve = Matrix.SLAE_Solution(matrixSLAU, rightSideDouble);
            if (solve != null)
                for (int i = 0; i < solve.GetLength(0); i++)
                    _unknownVars[i].Text = solve[i].ToString();
            else MessageBox.Show("СЛАУ не может быть решено!");
        }
        private void click_solve_reverse(object sender, RoutedEventArgs e)
        {
            var reverseMatrix = new Matrix(Convert.ToUInt32(_reverseMatrix.GetLength(0)));
            for (int i = 0; i < _reverseMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < _reverseMatrix.GetLength(1); j++)
                {
                    double number;
                    var isSuccess = double.TryParse(_reverseMatrix[i,j].Text, out number);
                    if (!isSuccess) return;
                    reverseMatrix[i, j] = number;
                }
            }

            var solve = Matrix.InverseMatrix(reverseMatrix);
            if (solve != null)
            {
                var _reverseMatrixSolve = new TextBox[_reverseMatrix.GetLength(0),_reverseMatrix.GetLength(1)];
                for(int i = 0; i < _reverseMatrixSolve.GetLength(0);i++)
                for (int j = 0; j < _reverseMatrixSolve.GetLength(1); j++)
                {
                    _reverseMatrixSolve[i, j] = create_text_box(reverse_grid, i, j+_reverseMatrixSolve.GetLength(1)+1);
                    _reverseMatrixSolve[i, j].Background = new SolidColorBrush(Color.FromRgb(220, 255, 255));
                    _reverseMatrixSolve[i, j].Text = solve[i, j].ToString();
                }

            }
            else MessageBox.Show("Обратная матрица не может быть найдена!!");

        }
        private void click_solve_multy(object sender, RoutedEventArgs e)
        {
            var MatrixA = new Matrix(Convert.ToUInt32(_matrixA.GetLength(0)), Convert.ToUInt32(_matrixA.GetLength(1)));
            for (int i = 0; i < _matrixA.GetLength(0); i++)
            {
                for (int j = 0; j < _matrixA.GetLength(1); j++)
                {
                    double number;
                    var isSuccess = double.TryParse(_matrixA[i, j].Text, out number);
                    if (!isSuccess) return;
                    MatrixA[i, j] = number;
                }
            }
            var MatrixB = new Matrix(Convert.ToUInt32(_matrixB.GetLength(0)), Convert.ToUInt32(_matrixB.GetLength(1)));
            for (int i = 0; i < _matrixB.GetLength(0); i++)
            {
                for (int j = 0; j < _matrixB.GetLength(1); j++)
                {
                    double number;
                    var isSuccess = double.TryParse(_matrixB[i, j].Text, out number);
                    if (!isSuccess) return;
                    MatrixB[i, j] = number;
                }
            }

            var solve = Matrix.MultiplyMatrix(MatrixA, MatrixB);
            if (solve != null)
            {
                var _mulMatrix = new TextBox[solve.Nrows, solve.Ncols];
                for (int i = 0; i < _mulMatrix.GetLength(0); i++)
                for (int j = 0; j < _mulMatrix.GetLength(1); j++)
                {
                    _mulMatrix[i, j] = create_text_box(multy_grid, i+Convert.ToInt32(MatrixA.Nrows), j);
                    _mulMatrix[i, j].Background = new SolidColorBrush(Color.FromRgb(220, 255, 255));
                    _mulMatrix[i, j].Text = solve[i, j].ToString();
                }

            }
            else MessageBox.Show("Матрицы не могут быть умножены!");

        }


    }
}
