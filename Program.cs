using System.Collections.Generic;
using System;
using System.Threading;

//using (int a, int b, int c, int d) as test

namespace datastructuren
{
    static class Koelkast
    {
        //int locations, int iteration

        //locations bestaat uit x Joen y Joen x koelkast y koelkast
        static List<HashSet<uint>> prelocs = new List<HashSet<uint>>();

        static List<HashSet<uint>> nextlocs = new List<HashSet<uint>>();


        //false open true muur
        static bool[,] room = new bool[1,1];
        static bool[,] dead = new bool[1, 1];//all dead corners
        static int iteration;
        static loc goal;

        //static HashSet<uint> laterAdd = new HashSet<uint>();
        static bool loop = true;

        public static void Main()
        {
            /////////input
            byte rw;
            byte rh;
            string line = Console.ReadLine();
            string[] input = line.Split(' ');
            rw = byte.Parse(input[0]);
            rh = byte.Parse(input[1]);
            char outType = char.Parse(input[2]);

            //build room and set start
            uint start = 0;

            //true is een muur
            room = new bool[rw,rh];
            for(int i = 0; i< rh; i++)
            {
                line = Console.ReadLine();
                for (int j = 0; j < rw; j++)
                {
                    if (line[j] == '.')
                        room[j, i] = false;
                    else if (line[j] == '?')
                    {
                        room[j, i] = false;
                        goal = new loc((byte)j, (byte)i);
                    }
                    else if (line[j] == '!')
                    {
                        room[j, i] = false;
                        start += CombineLocation(0, 0, (byte)j, (byte)i);
                    }
                    else if (line[j] == '+')
                    {
                        room[j, i] = false;
                        start += CombineLocation((byte)j, (byte)i, 0, 0);
                    }
                    else room[j, i] = true;
                }
            }

            dead = new bool[rw,rh];
            for (int i = 1; i < rh-1; i++)
            {
                for (int j = 1; j < rw-1; j++)
                {
                    if (room[j, i]) dead[j, i] = true;
                    if (room[j - 1, i] && room[j, i - 1]) dead[j, i] = true;
                    if (room[j - 1, i] && room[j, i + 1]) dead[j, i] = true;
                    if (room[j + 1, i] && room[j, i - 1]) dead[j, i] = true;
                    if (room[j + 1, i] && room[j, i + 1]) dead[j, i] = true;
                }
            }
            dead[goal.x, goal.y] = true;

            prelocs.Add(new HashSet<uint>());
            nextlocs.Add(new HashSet<uint>());

            prelocs[0].Add(start);
            if (room[goal.x - 1, goal.y] == false)
                nextlocs[0].Add(CombineLocation((byte)(goal.x - 1), (byte)(goal.y), (byte)goal.x, (byte)goal.y));
            if (room[goal.x + 1, goal.y] == false)
                nextlocs[0].Add(CombineLocation((byte)(goal.x + 1), (byte)(goal.y), (byte)goal.x, (byte)goal.y));
            if (room[goal.x, goal.y - 1] == false)
                nextlocs[0].Add(CombineLocation((byte)(goal.x), (byte)(goal.y - 1), (byte)goal.x, (byte)goal.y));
            if (room[goal.x, goal.y + 1] == false)
                nextlocs[0].Add(CombineLocation((byte)(goal.x), (byte)(goal.y + 1), (byte)goal.x, (byte)goal.y));

            iteration = 0;

            DateTime time = DateTime.Now;

            bool test = true;
            while (loop)
            {
                //BFS step
                prelocs.Add(new HashSet<uint>());
                test = true;
                foreach(uint key in prelocs[iteration])
                {
                    test = false;
                    
                    bytes n = SeparateLocation(key);
                    
                    MoveUp(n.Item1, n.Item2, n.Item3, n.Item4);
                    MoveDown(n.Item1, n.Item2, n.Item3, n.Item4);
                    MoveLeft(n.Item1, n.Item2, n.Item3, n.Item4);
                    MoveRight(n.Item1, n.Item2, n.Item3, n.Item4);
                    
                }
                if (test) break;

                if (!loop)
                {
                    iteration *= 2;
                    iteration++;
                    break;
                }

                ////BFS backwards step
                //nextlocs.Add(new HashSet<uint>());
                //test = true;
                //foreach (uint key in nextlocs[iteration])
                //{
                //    test = false;
                //
                //    bytes n = SeparateLocation(key);
                //
                //    PullUp(n.Item1, n.Item2, n.Item3, n.Item4);
                //    PullDown(n.Item1, n.Item2, n.Item3, n.Item4);
                //    PullLeft(n.Item1, n.Item2, n.Item3, n.Item4);
                //    PullRight(n.Item1, n.Item2, n.Item3, n.Item4);
                //}
                //if (test) break;

                //debug
                //if (iteration  <= 15)
                //{
                //    uint[,] preJoens = new uint[rw, rh]; uint[,] nextJoens = new uint[rw, rh];
                //    uint[,] preKoels = new uint[rw, rh]; uint[,] nextKoels = new uint[rw, rh];
                //    foreach (uint key in prelocs[iteration])
                //    {
                //        bytes n = SeparateLocation(key);
                //        preJoens[n.Item1, n.Item2]++;
                //        preKoels[n.Item3, n.Item4]++;
                //    }
                //    foreach (uint key in nextlocs[iteration])
                //    {
                //        bytes n = SeparateLocation(key);
                //        nextJoens[n.Item1, n.Item2]++;
                //        nextKoels[n.Item3, n.Item4]++;
                //    }
                //    Console.WriteLine("Iteration: " + iteration);
                //    Console.WriteLine("next: " + nextlocs[iteration].Count);
                //    
                //
                //    for (int i = 0; i < rh/2; i++)
                //    {
                //        for (int j = 0; j < rw/3; j++)
                //        {
                //            if (room[j, i])
                //                Console.Write("#" + nextJoens[j, i] + nextKoels[j, i]);
                //            else
                //            {
                //                Console.Write("." + nextJoens[j, i] + nextKoels[j, i]);
                //            }
                //        }
                //        Console.WriteLine("");
                //    }
                //    
                //
                //    Console.WriteLine(" ");
                //    
                //}
                ///debug
                iteration++;

                if (!loop)
                {
                    iteration *= 2;
                }

            }
            if (test)
            {
                Console.WriteLine("No solution");
                return;
            }
            Console.WriteLine(iteration);
            if (outType == 'P')
            {

            }

            Console.WriteLine((DateTime.Now - time).TotalSeconds);

        }

        static void InsertPre(byte b, byte b2, byte b3, byte b4)
        {
            uint input = CombineLocation(b, b2, b3, b4);

            for (int i = iteration + 1; i >= 0; i -= 2)
            {
                if(prelocs[i].Contains(input))
                {
                    return;
                }
            }
            if (nextlocs[0].Contains(input))
            {
                //endcode
                loop = false;
            }
            prelocs[iteration + 1].Add(input);
        }

        //bit of code copy but ok
        static void InsertNext(byte b, byte b2, byte b3, byte b4)
        {
            uint input = CombineLocation(b, b2, b3, b4);

            for (int i = iteration + 1; i >= 0; i -= 2)
            {
                if (nextlocs[i].Contains(input))
                {
                    return;
                }
            }
            if(prelocs[iteration+1].Contains(input))
            {
                //end code
                loop = false;
            }

            nextlocs[iteration + 1].Add(input);
        }


        static void MoveUp(byte jx, byte jy, byte kx, byte ky)
        {
            if (room[jx, jy - 1] != false)
            {
                return;
            }
            if (jx == kx && jy -1 == ky)
            {
                if (!dead[jx, jy - 2])
                {
                    InsertPre(jx, (byte)(jy - 1), kx, (byte)(ky - 1));
                }
                return;
            }
            InsertPre(jx, (byte)(jy-1), kx, ky);
        }
        static void MoveDown(byte jx, byte jy, byte kx, byte ky)
        {
            if (room[jx, jy + 1] != false)
            {
                return;
            }
            if (jx == kx && jy + 1 == ky)
            {
                if (!dead[jx, jy + 2])
                {
                    InsertPre(jx, (byte)(jy + 1), kx, (byte)(ky + 1));
                }
                return;
            }
            InsertPre(jx, (byte)(jy+1), kx, ky);
        }
        static void MoveLeft(byte jx, byte jy, byte kx, byte ky)
        {
            if (room[jx - 1, jy] != false)
            {
                return;
            }
            if (jx - 1 == kx && jy == ky)
            {
                if (!dead[jx - 2, jy])
                {
                    InsertPre((byte)(jx - 1), jy, (byte)(kx - 1), ky);
                }
                return;
            }
            InsertPre((byte)(jx-1), jy, kx, ky);
        }
        static void MoveRight(byte jx, byte jy, byte kx, byte ky)
        {
            if (room[jx + 1, jy] != false)
            {
                return;
            }
            if (jx + 1 == kx && jy == ky)
            {
                if (!dead[jx + 2, jy])
                {
                    InsertPre((byte)(jx + 1), jy, (byte)(kx + 1), ky);
                }
                return;
            }
            InsertPre((byte)(jx+1), jy, kx, ky);
        }


        static void PullUp(byte jx, byte jy, byte kx, byte ky)
        {
            if (room[jx, jy - 1] != false)
            {
                return;
            }
            if (jx == kx && jy - 1 == ky)
            {
                return;
            }
            if (jx == kx && jy + 1 == ky)
            {
                InsertNext(jx, (byte)(jy - 1), kx, (byte)(ky - 1));
            }
            InsertNext(jx, (byte)(jy - 1), kx, ky);
        }
        static void PullDown(byte jx, byte jy, byte kx, byte ky)
        {
            if (room[jx, jy + 1] != false)
            {
                return;
            }
            if (jx == kx && jy + 1 == ky)
            {
                return;
            }
            if (jx == kx && jy - 1 == ky)
            {
                InsertNext(jx, (byte)(jy + 1), kx, (byte)(ky + 1));
            }
            InsertNext(jx, (byte)(jy + 1), kx, ky);
        }
        static void PullLeft(byte jx, byte jy, byte kx, byte ky)
        {
            if (room[jx - 1, jy] != false)
            {
                return;
            }
            if (jx - 1 == kx && jy == ky)
            {
                return;
            }
            if (jx + 1 == kx && jy == ky)
            {
                InsertNext((byte)(jx - 1), jy, (byte)(kx - 1), ky);
            }
            InsertNext((byte)(jx - 1), jy, kx, ky);
        }
        static void PullRight(byte jx, byte jy, byte kx, byte ky)
        {
            if (room[jx + 1, jy] != false)
            {
                return;
            }
            if (jx + 1 == kx && jy == ky)
            {
                return;
            }
            if (jx - 1 == kx && jy == ky)
            {
                InsertNext((byte)(jx + 1), jy, (byte)(kx + 1), ky);
            }
            InsertNext((byte)(jx + 1), jy, kx, ky);
        }

        static uint CombineLocation(byte joenx, byte joeny, byte koelx, byte koely)
        {
            return ((uint)joenx <<24) + ((uint)joeny << 16) + ((uint) koelx<<8) + koely;
        }

        static bytes SeparateLocation(uint loc)
        {
            return new bytes((byte)(loc >> 24), (byte)(loc >> 16), (byte)(loc >> 8), (byte)(loc));
        }

    }

    internal struct loc
    {
        internal byte x;
        internal byte y;

        internal loc(byte nx, byte ny)
        {
            x = nx;
            y = ny;
        }

    }

    internal struct bytes
    {
        internal byte Item1;
        internal byte Item2;
        internal byte Item3;
        internal byte Item4;

        internal bytes(byte b1, byte b2, byte b3, byte b4)
        {
            Item1 = b1;
            Item2 = b2;
            Item3 = b3;
            Item4 = b4;
        }

    }




}