using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Diagnostics;
using System.Windows.Input;


namespace Tetris
{

    class Program
    {


        [STAThread]
        static void Main(string[] args)
        {
            //Initiate new gamedata
            GameData currentGameData = new GameData();

            //Data for the game loop
            Stopwatch elapsedTime = new Stopwatch();
            int simulationTime;
            int gameTick = 100; // in ms

            //Disable the blinking cursur
            Console.CursorVisible = false;

            do
            {
                elapsedTime.Restart();

                //process input
                currentGameData.getPlayerInPut();

                //update simulation
                currentGameData.UpdateSimulation();

                //render the simulatiom
                currentGameData.RenderStringBuilder();

                //it will take at least "minGametick" amount of msec to do a loop.
                elapsedTime.Stop();
                simulationTime = (int) elapsedTime.ElapsedMilliseconds;
                if (simulationTime < gameTick)
                {
                    Thread.Sleep(gameTick - simulationTime);
                }
            } while (currentGameData.flag_BreakGameLoop == false);
        }
    }
}
