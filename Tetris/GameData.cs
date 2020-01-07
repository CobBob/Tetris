using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
//using System.Windows.Input;

namespace Tetris
{
    public enum BlockOptions
    {
        Square,
        Straight,
        Normal_L,
        Reverse_L,
        Normal_S,
        Reverse_S,
        T_piece,
    };

    class GameData
    {
        //Characters for rendering
        private char code_WallVer = '\u2551';
        private char code_WallHor = '\u2550';
        private char code_WallTL = '\u2554';
        private char code_WallTR = '\u2557';
        private char code_WallBL = '\u255A';
        private char code_WallBR = '\u255D';
        private char code_Block = 'B';  //'\u2586';
        private char code_Filler = 'F'; //'\u2B1B';
        private char code_Empty = ' ';


        public bool flag_BreakGameLoop { get; private set; }

        private int fieldHeight;
        private int fieldWidth;

        private char[,] playField;
        private double fallSpeed;

        //The stringBuilder used for the display
        private StringBuilder toDisPstring;

        //The score
        private int score;

        //Bool to store whether the block has landed
        private bool blockHasLanded;
        //Bool for gameover
        private bool isGameOver;

        //Block properties
        private double y_speed_Block;
        private double x_speed_Block;
        private List<double> x_coord_Block_Float;
        private List<double> y_coord_Block_Float;

        //Random number
        Random rand;

        //the current and next block to be played
        BlockOptions nextBlock;


        //PlayerInputs
        bool input_GoLeft = false;
        bool input_GoRight = false;
        bool input_Boost = false;
        bool input_Rotate = false;

        public GameData()
        {
            flag_BreakGameLoop = false;

            fieldHeight = 20;
            fieldWidth = 15;

            playField = new char[fieldWidth, fieldHeight];
            fallSpeed = 0.27;

            //The stringBuilder used for the display
            toDisPstring = new StringBuilder();

            //The score
            score = 0;

            //Bool to store whether the block has landed
            blockHasLanded = false;

            //Block properties
            y_speed_Block = 0;
            x_speed_Block = 0;
            y_coord_Block_Float = new List<double>();
            x_coord_Block_Float = new List<double>();

            //Random number
            rand = new Random();

            //the next block to be played
            nextBlock = GetRandomBlock();

            //Draw an empty field
            for (int i = 0; i < fieldHeight; i++)
            {
                for (int j = 0; j < fieldWidth; j++)
                {
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            playField[j, i] = Convert.ToChar(code_WallTL);
                        }
                        else if (j == fieldWidth - 1)
                        {
                            playField[j, i] = Convert.ToChar(code_WallTR);
                        }
                        else
                        {
                            playField[j, i] = Convert.ToChar(code_WallHor);
                        }
                    }
                    else if (i == fieldHeight - 1)
                    {
                        if (j == 0)
                        {
                            playField[j, i] = Convert.ToChar(code_WallBL);
                        }
                        else if (j == fieldWidth - 1)
                        {
                            playField[j, i] = Convert.ToChar(code_WallBR);
                        }
                        else
                        {
                            playField[j, i] = Convert.ToChar(code_WallHor);
                        }
                    }
                    else if (j == 0 || j == fieldWidth - 1)
                    {
                        playField[j, i] = Convert.ToChar(code_WallVer);
                    }
                    else
                    {
                        playField[j, i] = code_Empty;
                    }
                }
            }

            //Place a block
            PlaceBlock(GetRandomBlock(),(fieldWidth/2));
            //Update Playfield with new block 
            UpdatePlayFieldAddBlock();
        }

        private void PlaceBlock(BlockOptions chosenBlock, int x_coord_Offset = 0)
        {
            y_speed_Block = fallSpeed;
            x_speed_Block = 0;

            switch (chosenBlock)
            {
                case BlockOptions.Square:
                    {
                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(1);

                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(1);

                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(2);

                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(2);
                        break;
                    }
                case BlockOptions.Straight:
                    {
                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(1);

                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(2);

                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(3);

                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(4);
                        break;
                    }
                case BlockOptions.Normal_L:
                    {
                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(1);

                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(2);

                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(3);

                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(3);
                        break;
                    }
                case BlockOptions.Reverse_L:
                    {
                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(1);

                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(2);

                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(3);

                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(3);
                        break;
                    }
                case BlockOptions.Normal_S:
                    {
                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(2);

                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(2);

                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(1);

                        x_coord_Block_Float.Add(3);
                        y_coord_Block_Float.Add(1);
                        break;
                    }
                case BlockOptions.Reverse_S:
                    {
                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(1);

                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(1);

                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(2);

                        x_coord_Block_Float.Add(3);
                        y_coord_Block_Float.Add(2);
                        break;
                    }
                case BlockOptions.T_piece:
                    {
                        x_coord_Block_Float.Add(1);
                        y_coord_Block_Float.Add(1);

                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(1);

                        x_coord_Block_Float.Add(3);
                        y_coord_Block_Float.Add(1);

                        x_coord_Block_Float.Add(2);
                        y_coord_Block_Float.Add(2);
                        break;
                    }
            }
            for(int i = 0; i < x_coord_Block_Float.Count(); i++)
            {
                x_coord_Block_Float[i] += x_coord_Offset;
            }
        }
        private void UpdatePlayFieldRemoveBlock()
        {
            for (int i = 0; i < x_coord_Block_Float.Count(); i++)
            {
                playField[(int) (x_coord_Block_Float[i] + 0.5), 
                    (int) (y_coord_Block_Float[i] + 0.5)] = code_Empty;
            }
        }
        private void UpdatePlayFieldAddBlock()
        {
            for (int i = 0; i < x_coord_Block_Float.Count(); i++)
            {
                playField[(int) (x_coord_Block_Float[i] + 0.5), 
                    (int) (y_coord_Block_Float[i] + 0.5)] = code_Block;
            }
        }
        private void MoveBlockVertical()
        {
            //Check if there are no landings
            blockHasLanded = false;
            for (int i = 0; i < y_coord_Block_Float.Count(); i++)
            {
                if(playField[(int) (x_coord_Block_Float[i] + 0.5),
                    (int) (y_coord_Block_Float[i] + y_speed_Block + 0.5)] == code_Filler
                    || playField[(int) (x_coord_Block_Float[i] + 0.5),
                    (int) (y_coord_Block_Float[i] + y_speed_Block + 0.5)] == code_WallHor)
                {
                    blockHasLanded = true;
                }
            }

            //If there are no landings Updata y coordinates of block
            if(!blockHasLanded)
            {
                for (int i = 0; i < y_coord_Block_Float.Count(); i++)
                {
                    y_coord_Block_Float[i] += y_speed_Block;
                }
            }
        }
        private void MoveBlockHorizontal()
        {
            //Check if the block doesn't bumb into something
            for (int i = 0; i < y_coord_Block_Float.Count(); i++)
            {
                if (playField[(int) (x_coord_Block_Float[i] + x_speed_Block + 0.5),
                    (int) (y_coord_Block_Float[i] + 0.5)] == code_Filler
                    || playField[(int) (x_coord_Block_Float[i] + x_speed_Block + 0.5),
                    (int) (y_coord_Block_Float[i] + 0.5)] == code_WallVer)
                {
                    //return function if a block at the new position is filler or wall
                    return;
                }
            }

            //If there are no bumbs Updata x coordinates of block
            for (int i = 0; i < y_coord_Block_Float.Count(); i++)
            {
                    x_coord_Block_Float[i] += x_speed_Block;
            }
        }
        private void TurnBlockToFiller()
        {
            for (int i = 0; i < y_coord_Block_Float.Count(); i++)
            {
                playField[(int) (x_coord_Block_Float[i] + 0.5),
                    (int) (y_coord_Block_Float[i] + 0.5)] = code_Filler;
            }
        }
        private void ClearBlockParameters()
        {
            y_coord_Block_Float.Clear();
            x_coord_Block_Float.Clear();
            y_speed_Block = 0;
            x_speed_Block = 0;
        }
        private BlockOptions GetRandomBlock()
        {
            BlockOptions blockPicked;
            Array values = Enum.GetValues(typeof(BlockOptions));
            blockPicked = (BlockOptions)values.GetValue(rand.Next(values.Length));

            return blockPicked;
        }
        private bool BlockCanBeAddedToPlayField()
        {
            for (int i = 0; i < y_coord_Block_Float.Count(); i++)
            {
                if (playField[(int) (x_coord_Block_Float[i] + 0.5),
                    (int) (y_coord_Block_Float[i] + 0.5)] == code_Filler)
                {
                    return false;
                }
            }
            return true;
        }
        private void RotateBlock()
        {
            double x_coord_OldPos = x_coord_Block_Float[1];
            double y_coord_OldPos = y_coord_Block_Float[1];

            double x_coord_new;
            double y_coord_new;

            List<double> x_coord_NewList = new List<double>();
            List<double> y_coord_NewList = new List<double>();

            for (int i = 0; i < x_coord_Block_Float.Count(); i++)
            {
                x_coord_new = -1*(y_coord_Block_Float[i] - y_coord_OldPos);
                y_coord_new = (x_coord_Block_Float[i] - x_coord_OldPos);

                x_coord_NewList.Add(x_coord_new + x_coord_OldPos);
                y_coord_NewList.Add(y_coord_new + y_coord_OldPos);
            }

            //If a found coordinate is out of bounds end this method
            //Also if a new position corresponds with a filller or edge block
            for (int i = 0; i < x_coord_Block_Float.Count(); i++)
            {
                if (x_coord_NewList[i] > fieldWidth-1
                    || x_coord_NewList[i] < 0
                    || y_coord_NewList[i] > fieldHeight-1
                    || y_coord_NewList[i] < 0)
                {
                    return;
                }
                if (playField[(int) (x_coord_NewList[i] + 0.5),
                    (int) (y_coord_NewList[i] + 0.5)] == code_Filler
                    || playField[(int)(x_coord_NewList[i] + 0.5),
                    (int)(y_coord_NewList[i] + 0.5)] == code_WallVer
                    || playField[(int)(x_coord_NewList[i] + 0.5),
                    (int)(y_coord_NewList[i] + 0.5)] == code_WallHor)
                {
                    return;
                }
            }

            //If the runtime gets here we can use the new list of coordinates
            x_coord_Block_Float.Clear();
            y_coord_Block_Float.Clear();

            x_coord_Block_Float = x_coord_NewList;
            y_coord_Block_Float = y_coord_NewList;
        }
        private void RemoveCompletedRows()
        {
            int k;
            //char[] tempLine = new char[fieldWidth];
            for (int i = 2; i < fieldHeight-1; i++)
            {
                for (int j = 1; j < fieldWidth-1; j++)
                {
                    if (playField[j,i] != code_Filler)
                    {
                        break;
                    }
                    if (j == fieldWidth-2)
                    {
                        //If the runtime reaches this point, it means that a completed line is present
                        //Write line above to this line and repeat untill top
                        k = i;
                        score += 500;
                        do
                        {
                            for (int p = 1; p < fieldWidth - 1; p++)
                            {
                                playField[p, k] = playField[p, k - 1];
                            }
                            k--;
                        } while (k > 2);
                    }
                }
            }
        }
        public void getPlayerInPut()
        {
            //Reset all inputs back to false
            input_GoLeft = false;
            input_GoRight = false;
            input_Boost = false;
            input_Rotate = false;

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Left))
            {
                input_GoLeft = true;
            }
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Right))
            {
                input_GoRight = true;
            }
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Down))
            {
                input_Boost = true;
            }
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Up))
            {
                input_Rotate = true;
            }
        }
        private void ProcessPlayerInPut()
        {
            if(input_GoLeft)
            {
                x_speed_Block = -1.01;
            }
            else if(input_GoRight)
            {
                x_speed_Block = 1.01;
            }
            else
            {
                x_speed_Block = 0;
            }

            if(input_Boost)
            {
                y_speed_Block = 1;
            }
            else
            {
                y_speed_Block = fallSpeed;
            }

            if (input_Rotate)
            {
                RotateBlock();
            }
        }
        private void BuildStringBuilder()
        {
            toDisPstring.Clear();

            if (isGameOver)
            {
                toDisPstring.Append("GAME OVER                           \n");
            }
            else
            {
                toDisPstring.Append("                                           \n");
            }

            toDisPstring.Append("Score: " + score + "                        \n\n");

            for (int i = 0; i < fieldHeight; i++)
            {
                for (int j = 0; j < fieldWidth; j++)
                {
                    toDisPstring.Append(playField[j, i]);
                }
                toDisPstring.Append("\n");
            }

            toDisPstring.Append("\nNext Block: " + nextBlock.ToString() + "     ");
        }
        public void RenderStringBuilder()
        {
            //Update the string to be drawn with current data
            BuildStringBuilder();

            toDisPstring.ToString();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(toDisPstring);
        }
        public void UpdateSimulation()
        {
            //Proces player input
            UpdatePlayFieldRemoveBlock();
            ProcessPlayerInPut();
            UpdatePlayFieldAddBlock();

            //If it is game over end this method
            if (isGameOver)
            {
                return;
            }

            //Move the block
            UpdatePlayFieldRemoveBlock();
            MoveBlockHorizontal();
            MoveBlockVertical();
            UpdatePlayFieldAddBlock();

            //If a block has landed
            if (blockHasLanded)
            {
                //Turn block into filler
                TurnBlockToFiller();
                ClearBlockParameters();
                //Check for complete rows and delete them + update score
                RemoveCompletedRows();
                //put the next block at the top of the field
                PlaceBlock(nextBlock, (fieldWidth / 2));
                if (!BlockCanBeAddedToPlayField())
                {
                    isGameOver = true;
                    return;
                }
                //Get a new next block
                nextBlock = GetRandomBlock();
            }
            else
            {
                //UpdatePlayFieldAddBlock();
            }

            //Give a point to score
            score++;
        }
    }
}