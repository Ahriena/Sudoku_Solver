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
using System.Windows.Threading;

namespace Sudoku_Solver
{
    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow
    {
        public int [] grid = new int[81];
        Button? CurrButton;
        
        public MainWindow()
        {
            InitializeComponent();
            CurrButton = _00;
            for (int i = 0; i < 81; i++)
            {
                grid[i] = 0;
            }
            Fill();
        }


        // fills the entire board with the associated digits
        // Mostly a holdover from when I was testing, but it is used.
        void Fill()
        {
            for (int i = 0; i < 81; i++)
            {
                if (grid[i] != 0)
                {
                    Button_to_array_index(i).Content = grid[i];
                }
                else
                {
                    Button_to_array_index(i).Content = "";
                }
            }
        }

        // checks if a value is valid at that location
        bool Is_Valid(int digit, int index)
        {
            // checks current row
            for (int i = (index / 9) * 9; i < ((index / 9) * 9) + 9; i++)
            {
                if (grid[i] != 0 && grid[i] == digit && i != index)
                {
                    return false;
                }
            }

            // checks current column
            for (int i = index %9; i < 81; i+=9)
            {
                if (grid[i] != 0 && grid[i] == digit && i != index)
                {
                    return false;
                }
            }

            // checks current square
            for (int i = (index / 27) * 27; i < (index / 27) * 27 + 27; i += 9)
            {
                for (int j = i + (index % 9) / 3 * 3; j < i + (index % 9) / 3 * 3 + 3; j++)
                {
                    if (grid[j] != 0 && grid[j] == digit && j != index)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // finds the next empty tile, returns -1 if the grid is full
        int Find_empty(int Index)
        {
            if (Index < 0)
                {
                    return -1;
                }
            for (int i = Index; i < 81; i++)
            {
                if (grid[i] == 0) {
                    return i;
                }
            }
            return -1;
        }
        
        // resets the board to nothing
        void Clear_Board(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 81; i++)
            {
                grid[i] = 0;
                Button_to_array_index(i).Foreground = Brushes.Black;
            }
            if (CurrButton!= null)
            {
                CurrButton.Background = new SolidColorBrush(Colors.LightGray);
            }
            CurrButton = _00;
            Fill();
        }

        // changes color of the clicked square
        void OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrButton!= null) 
            { 
                CurrButton.Background = new SolidColorBrush(Colors.LightGray); 
            }

            Button? b = sender as Button;
            string Result = b.Tag.ToString();
            int Input = Int32.Parse(Result);

            CurrButton = Button_to_array_index(Input);
            CurrButton.Background = new SolidColorBrush(Colors.LightBlue);
        }

        // converts the tag to an int equivalent to the associated array location
        void Keydown1(object sender, KeyEventArgs e)
        {
            Button? b = sender as Button;
            string Result = b.Tag.ToString();
            int Input = Int32.Parse(Result);
            
            // 0, delete, or backspace
            if (e.Key == Key.NumPad0 || e.Key == Key.D0 || e.Key == Key.Delete || e.Key == Key.Back) 
            {
                Button_to_array_index(Input).Content = "";
                grid[Input] = 0;
            }

            // 1
            else if (e.Key == Key.NumPad1 || e.Key == Key.D1)
            {
                Button_to_array_index(Input).Content = "1";
                grid[Input] = 1;
            }

            // 2
            else if (e.Key == Key.NumPad2 || e.Key == Key.D2)
            {
                Button_to_array_index(Input).Content = "2";
                grid[Input] = 2;
            }

            // 3
            else if (e.Key == Key.NumPad3 || e.Key == Key.D3)
            {
                Button_to_array_index(Input).Content = "3";
                grid[Input] = 3;
            }

            // 4
            else if (e.Key == Key.NumPad4 || e.Key == Key.D4)
            {
                Button_to_array_index(Input).Content = "4";
                grid[Input] = 4;
            }

            // 5
            else if (e.Key == Key.NumPad5 || e.Key == Key.D5)
            {
                Button_to_array_index(Input).Content = "5";
                grid[Input] = 5;
            }

            // 6
            else if (e.Key == Key.NumPad6 || e.Key == Key.D6)
            {
                Button_to_array_index(Input).Content = "6";
                grid[Input] = 6;
            }

            // 7
            else if (e.Key == Key.NumPad7 || e.Key == Key.D7)
            {
                Button_to_array_index(Input).Content = "7";
                grid[Input] = 7;
            }

            // 8
            else if (e.Key == Key.NumPad8 || e.Key == Key.D8)
            {
                Button_to_array_index(Input).Content = "8";
                grid[Input] = 8;
            }

            // 9
            else if (e.Key == Key.NumPad9 || e.Key == Key.D9)
            {
                Button_to_array_index(Input).Content = "9";
                grid[Input] = 9;
            }

            // closes the program if you push f5 or f6, since I got tired of moving my mouse
            else if (e.Key == Key.F5 || e.Key == Key.F6)
            {
                Application.Current.Shutdown();
            }
        }

        //Due to using recursion, I separated the solve function into the button press
        //and the recursive solver
        //this is just the button press that resets the board color and starts solving
        void Solve(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 81; i++)
            {
                Button_to_array_index(i).Background = new SolidColorBrush(Colors.LightGray);
            }
            Solve(Find_empty(0));
        }

        // the actual solve function
        void Solve(int Index)
        {
            // if the board is full, returns
            if (Find_empty(Index) == -1)
            {
                return;
            }
            // if the current tile was given, it simply goes to the next one.
            if (grid[Index] != 0)
            {
                Solve(Index + 1);
                return;
            }
            // changes the text color of any unset tiles to purple so they stand out a bit
            Button_to_array_index(Index).Foreground = Brushes.Purple;

            // runs through the digits 1-9
            for (int i = 1; i < 11; i++)
            {
                // if 1-9 were invalid, it resets the tile and backtracks.
                if (i == 10)
                {
                    grid[Index] = 0;
                    Button_to_array_index(Index).Content = "";
                    Button_to_array_index(Index).Refresh();
                    return;
                }
                // otherwise, checks 1-9
                else if (Is_Valid(i, Index))
                {
                    grid[Index] = i;
                    Button_to_array_index(Index).Content = i;
                    Button_to_array_index(Index).Refresh();
                    Solve(Index + 1);
                    if (Find_empty(Index) == -1)
                    {
                        return;
                    }

                }
            }
        }

        // used to convert the array location (0-80) to the associated xy coordinate button
        Button Button_to_array_index(int Input)
        {
            return Input switch
            {

                // index(left) is for array location
                // buttons(right) are for grid location and are xy coordinates

                // Row 0
                0 => _00,
                1 => _01,
                2 => _02,
                3 => _03,
                4 => _04,
                5 => _05,
                6 => _06,
                7 => _07,
                8 => _08,
                
                // Row 1
                9 => _10,
                10 => _11,
                11 => _12,
                12 => _13,
                13 => _14,
                14 => _15,
                15 => _16,
                16 => _17,
                17 => _18,
                
                // Row 2
                18 => _20,
                19 => _21,
                20 => _22,
                21 => _23,
                22 => _24,
                23 => _25,
                24 => _26,
                25 => _27,
                26 => _28,
                
                // Row 3
                27 => _30,
                28 => _31,
                29 => _32,
                30 => _33,
                31 => _34,
                32 => _35,
                33 => _36,
                34 => _37,
                35 => _38,
                
                // Row 4
                36 => _40,
                37 => _41,
                38 => _42,
                39 => _43,
                40 => _44,
                41 => _45,
                42 => _46,
                43 => _47,
                44 => _48,
                
                //row 5
                45 => _50,
                46 => _51,
                47 => _52,
                48 => _53,
                49 => _54,
                50 => _55,
                51 => _56,
                52 => _57,
                53 => _58,
                
                //row 6
                54 => _60,
                55 => _61,
                56 => _62,
                57 => _63,
                58 => _64,
                59 => _65,
                60 => _66,
                61 => _67,
                62 => _68,
                
                //row 7
                63 => _70,
                64 => _71,
                65 => _72,
                66 => _73,
                67 => _74,
                68 => _75,
                69 => _76,
                70 => _77,
                71 => _78,
                
                //row 8
                72 => _80,
                73 => _81,
                74 => _82,
                75 => _83,
                76 => _84,
                77 => _85,
                78 => _86,
                79 => _87,
                80 => _88,
                _ => _00,
            };
        }
    }
}
