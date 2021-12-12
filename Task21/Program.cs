using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Task21
{
    public class Gardener
    {
        public int startX { get; set; }
        public int x { get; set; }
        public int z { get; set; }
        public int vecX { get; set; }
        public int vecZ { get; set; }

        public Gardener(int x, int z, int vecX, int vecZ)
        {
            this.startX = x;
            this.x = x;
            this.z = z;
            this.vecX = vecX;
            this.vecZ = vecZ;
        }
    }
    internal class Program
    {
        public static int width = 5;
        public static int length = 5;
        public static int[,] map = new int[width, length];

        static void Main()
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    map[x, z] = 0;
                }
            }

            int[] gardenerArgs1 = new int[4] { 1, 5, 1, -1 };

            ParameterizedThreadStart simGardener1start = new ParameterizedThreadStart(SimulateGardener);
            Thread simGardener1thread = new Thread(simGardener1start);
            simGardener1thread.Start(gardenerArgs1);

            int[] gardenerArgs2 = new int[4] { 5, 1, -1, 1 };

            ParameterizedThreadStart simGardener2start = new ParameterizedThreadStart(SimulateGardener);
            Thread simGardener2thread = new Thread(simGardener2start);
            simGardener2thread.Start(gardenerArgs2);


            Console.ReadKey();

        }

        static void SimulateGardener(object args)
        {
            int[] gardArgs = (int[])args;
            Gardener gardener = new Gardener(gardArgs[0], gardArgs[1], gardArgs[2], gardArgs[3]);

            for (int i = 1; i < width * length; i++)
            {
                lock (Thread.CurrentThread)
                {
                    PrintGardener(gardener);
                    int gx = gardener.x;
                    int gz = gardener.z;
                    if (map[gx-1, gz-1] == 0)
                    {
                        map[gx-1, gz-1] = 1;
                    }

                    if ((gardener.x == 1 && gardener.startX != 1)
                        || (gardener.x == width && gardener.startX != width))
                    {
                        gardener.z += gardener.vecZ;
                        gardener.x = gardener.startX;
                    }

                    if (gardener.z < 1)
                    {
                        gardener.z = 1;
                    }
                    if (gardener.z > length)
                    {
                        gardener.z = length;
                    }
                    gardener.x += gardener.vecX;

                }

            }

        }
        public static void PrintGardener(Gardener gardener)
        {
            int gx = gardener.x;
            int gz = gardener.z;
            Console.WriteLine($"Садовник на клетке {gx}:{gz}");

            string cellInfo = "ещё не вспахана! Он вспахал!";
            if (map[gx - 1, gz - 1] == 1)
            {
                cellInfo = "уже вспахана!";
            }
            Console.WriteLine($"Его клетка {cellInfo}");
        }
    }
}
