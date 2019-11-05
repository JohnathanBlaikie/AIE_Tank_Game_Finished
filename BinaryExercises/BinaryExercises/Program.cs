using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryExercises
{
    class Program
    {
        const byte CHAINSAW = 0X01;
        const byte PISTOL = 0X01 << 1;
        const byte SHOTGUN = 0X01 << 2;
        const byte SUPER_SHOTGUN = 0X01 << 3;
        const byte CHAINGUN = 0X01 << 4;
        const byte ROCKET_LAUNCHER = 0X01 << 5;
        const byte PLASMA_GUN = 0X01 << 6;
        const byte BFG9000 = 0X01 << 7;



        static void Main(string[] args)
        {
            bool mainLoop = true;
            while (mainLoop)
            {
                Console.Clear();
                Console.WriteLine("Hello! Please Select an Exercise:\n[1] Doom Inventory" +
                    "\n[2] Decimal to Binary Conversion\n[3] Binary to Decimal Conversion");
                int.TryParse(Console.ReadLine(), out int listInt);
                if (listInt == 1)
                {
                    byte inventory = 0;
                    Console.ReadKey();
                    bool loop = true;
                    while (loop)
                    {
                        
                        inventory |= PLASMA_GUN;
                        inventory |= PISTOL;
                        inventory |= CHAINSAW;

                        inventory ^= BFG9000;

                        PrintInventory(inventory);
                        Console.ReadLine();
                    }
                }
                else if (listInt == 2)
                {
                    bool loop = true;
                    while (loop)
                    {
                        Console.WriteLine("Please Enter a Number:");
                        int binNum;
                        int.TryParse(Console.ReadLine(), out binNum);
                        string binFin = Convert.ToString(binNum, 2);
                        Console.WriteLine(binFin);
                        Console.WriteLine("\nWould you Like to try another number? [Y] / [N]");
                        char.TryParse((Console.ReadLine()), out char temp);
                        char.ToLower(temp);
                        if (temp == 'n')
                        {
                            loop = false;
                        }
                        else { }

                    }
                }
                else if (listInt == 3)
                {
                    bool loop = true;
                    while (loop)
                    {
                        Console.WriteLine("Please Enter a Number:");
                        int binNum;
                        int.TryParse(Console.ReadLine(), out binNum);
                        string nuBinNum = Convert.ToString(binNum);
                        string binFin = Convert.ToInt32(nuBinNum, 2).ToString();
                        Console.WriteLine(binFin);
                        Console.WriteLine("\nWould you Like to try another number? [Y] / [N]");
                        char.TryParse((Console.ReadLine()), out char temp);
                        char.ToLower(temp);
                        if (temp == 'n')
                        {
                            loop = false;
                        }
                        else { }

                    }
                }
            }
        }


        #region DoomInv
        public static void AddToInventory(ref byte inventory, byte weapon)
        {
            inventory |= weapon;
        }

        public static readonly string[] weapons = { "Fists", "Chainsaw", "Pistol", "Shotgun", "Super Shotgun",
        "Chaingun", "Rocket Launcher", "Plasma Gun", "BFG 9000" };


        public static void PrintInventory(byte inventory)
        {
            Console.Write("{0} | ", weapons[0]);

            for(int i = 1; i < weapons.Length; i++)
            {
                int mask = 0x01 << i - 1;
                if((inventory & mask) == mask)
                {
                    Console.Write("{0} | ", weapons[i]);
                }
            }
            Console.Write("\n");

        }
        #endregion DoomInv
    }
}

