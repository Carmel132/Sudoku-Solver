using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku_Solver
{
    struct Cell // 9 x 9 grid
    {
        public List<List<Value>> Data;

        //Bunch of arbitrary values that might be in handy later
        public Value[] H_TopRow;
        public Value[] H_MidRow;
        public Value[] H_BotRow;

        public Value[] V_LeftRow;
        public Value[] V_MidRow;
        public Value[] V_RightRow; // I know these are columns shut

        public Cell(List<List<Value>> StartingData)
        {
            Data = StartingData;
            H_TopRow = new Value[] { Data[0][0], Data[0][1], Data[0][2] }; // H_TopRow[1] == Data[0][1]
            H_MidRow = new Value[] { Data[1][0], Data[1][1], Data[1][2] }; // H_MidRow[1] == Data[1][1]
            H_BotRow = new Value[] { Data[2][0], Data[2][1], Data[2][2] };

            V_LeftRow = new Value[] { Data[0][0], Data[1][0], Data[2][0] };
            V_MidRow = new Value[] { Data[0][1], Data[1][1], Data[2][1] };
            V_RightRow = new Value[] { Data[0][2], Data[1][2], Data[2][2] };
        }

        public bool NoDuplicates() // this was hell (this aged well)
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            foreach (List<Value> row in Data)
            {
                foreach (Value cell in row)
                {
                    if (numbers.Count(func => func == cell.Val) == 1)
                    {
                        numbers[Array.IndexOf(numbers, cell.Val)] = 0;
                    }
                    else if (cell.Val != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static implicit operator Cell(List<List<Value>> v)
        {
            return new Cell(v);
        }

        public void update()
        {
            H_TopRow = new Value[] { Data[0][0], Data[0][1], Data[0][2] };
            H_MidRow = new Value[] { Data[1][0], Data[1][1], Data[1][2] }; 
            H_BotRow = new Value[] { Data[2][0], Data[2][1], Data[2][2] };

            V_LeftRow = new Value[] { Data[0][0], Data[1][0], Data[2][0] };
            V_MidRow = new Value[] { Data[0][1], Data[1][1], Data[2][1] };
            V_RightRow = new Value[] { Data[0][2], Data[1][2], Data[2][2] };
        }
    }
}

