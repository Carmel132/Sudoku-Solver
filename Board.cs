using System;
using System.Collections.Generic;

namespace Sudoku_Solver
{
    struct Board // 3 x 3 x 3 x 3 (3 x 3 cell) grid
    {
        public List<Cell> Data;


        public Board(List<Cell> data)
        {
            Data = data;
        }
        public Board(Board board)
        {
            Data = board.Data;
        }
        public Board(List<string> data) // assume numbers are separated by spaces and each string is a 9 x 1 (excluding whitespace)
        {
            Data = new List<Cell>();
            foreach (string cell in data)
            {
                string[] nums = cell.Split(' ');
                List<List<Value>> push = new List<List<Value>> {new List<Value>(), new List<Value>(), new List<Value>() };
                int num = 0;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        push[i].Add(Value.Int(Convert.ToInt32(nums[num])));
                        num++;
                    }
                }
                Data.Add(push);
            }
        }

        public Board GetCopy()
        {
            return new Board(this);
        }
        public int Read(Tuple<int, int, int> coord)
        {
            return Data[coord.Item1].Data[coord.Item2][coord.Item3].Val;
        }
        public void Write(Tuple<int, int, int> coord, Value val)
        {
            Data[coord.Item1].Data[coord.Item2][coord.Item3].Val = val.Val;
        }
        public bool CheckRow(int row) // 1 based indexing, add 1 to row before call (0 never goes in)
        {
            
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            
            if (row <= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    foreach (Value v in Data[i].Data[(row - 1) % 3])
                    {
                        if (v.Val == 0) { continue; }
                        else if (numbers.Contains(v.Val))
                        {
                            numbers.Remove(v.Val);
                        }
                        else { return false; }
                    }
                }
            }
            else if (row <= 6)
            {
                for (int i = 3; i < 6; i++)
                {
                    foreach (Value v in Data[i].Data[(row - 1) % 3])
                    {
                        if (v.Val == 0) { continue; }
                        else if (numbers.Contains(v.Val))
                        {
                            numbers.Remove(v.Val);
                        }
                        else { return false; }
                    }
                }
            }
            else if (row <= 9)
            {
                for (int i = 6; i < 9; i++)
                {
                    foreach (Value v in Data[i].Data[(row - 1) % 3])
                    {
                        if (v.Val == 0) { continue; }
                        else if (numbers.Contains(v.Val))
                        {
                            numbers.Remove(v.Val);
                        }
                        else { return false; }
                    }
                }
            }


            return true;
        }
        public bool CheckColumn(int c) // 1 based indexing
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            /*int ColumnIndex = c % 3;
            int domain = c < 3 ? 0 : c < 6 ? 1 : 2;

            for (int cell = domain; cell < 3; cell += 3)
            {
                for (int row = 0; row < 3; row++)
                {
                    if (!numbers.Contains(Data[cell].Data[row][ColumnIndex].Val) && Data[cell].Data[row][ColumnIndex].Val != 0)
                    {
                        return false;
                    }
                    else { numbers.Remove(Data[cell].Data[row][ColumnIndex].Val); }
                }
            }*/

            for (int C = (int)Math.Floor( (double)(c - 1) / 3 ); C < 9; C += 3)
            {
                foreach (var i in Data[C].Data)
                {
                    if (i[(int)Math.Floor((double)(c - 1) % 3)].Val == 0) { continue; }
                    else if (numbers.Contains(i[(int)Math.Floor((double)(c - 1) % 3)].Val))
                    {
                        numbers.Remove(i[(int)Math.Floor((double)(c - 1) % 3)].Val);
                    }
                    else { return false; }
                }
            }
            return true;
        }
        public bool Solved()
        {
            //easy part
            foreach (Cell cell in Data)
            {
                if (!cell.NoDuplicates()) { return false; }
            }
            for (int i = 1; i < 10; i++)
            {
                if (!CheckRow(i) || !CheckColumn(i)) { return false; }
            }
            foreach (Cell c in Data)
            {
                foreach (var row in c.Data)
                {
                    foreach (var val in row)
                    {
                        if (val.Val == 0) { return false;}
                    }
                }
            }

            return true;
        }
        public bool IsLegal(Tuple<int, int, int> coord, Value val)
        {
            var original = this[coord.ToValueTuple()].Val;

            this[coord.ToValueTuple()].Val = val.Val;

            bool retval = CheckRow(CoordToRow(coord.ToValueTuple())) && CheckColumn(CoordToColumn(coord.ToValueTuple())) && Data[coord.Item1].NoDuplicates();
            this.Data[coord.Item1].Data[coord.Item2][coord.Item3].Val = original;
            return retval;
        }
        public int CoordToRow((int, int, int) coord)
        {
            // (cell // 3) * 3 + 1 + row
            return ((int)Math.Floor( (double)coord.Item1 / 3 ) * 3 + coord.Item2 + 1);
        }
        public int CoordToColumn((int, int, int) coord)
        {
            // cell % 3 * 3 + 1 + val
            return (coord.Item1 % 3) * 3 + 1 + coord.Item3;
        }

        
        public Value this[(int, int, int) pointer]
        {
            get => Data[pointer.Item1].Data[pointer.Item2][pointer.Item3];
            set => Data[pointer.Item1].Data[pointer.Item2][pointer.Item3].Val = value.Val;
        }

        public void Update()
        {
            for (int c = 0; c < Data.Count; c++)
            {
                for (int r = 0; r < Data[c].Data.Count; r++)
                {
                    for (int v = 0; v < Data[c].Data[r].Count; v++)
                    {
                        //Data[c].Data[r][v] is a val
                        //order doesn't matter for this function
                        this[(c, r, v)].Works.Clear();
                        for (int i = 1; i < 10; i++)
                        {
                            if (IsLegal((c, r, v).ToTuple(), Value.Int(i))) { this[(c, r, v)].Works.Add(i); }
                        }
                    }
                }
            }
        }
    }
}

