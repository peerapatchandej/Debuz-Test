using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Master_Mind
{
    class Program
    {
        static Random rand = new Random();
        static List<int> tempNumber = new List<int>();
        static List<int> randNumber = new List<int>();
        static bool[] isCheck = new bool[] { false, false, false, false };
        static int[] inputNumber = new int[4];
        static int predictNum = 0;
        static int predictPos = 0;
        static int predictCount = 0;
        static bool play = true;

        static void Main(string[] args)
        {
            while (play)
            {
                Console.Clear();
                RandomNumber();
                PredictAnswer();
                PlayAgain();
            }
        }

        static void RandomNumber()
        {
            for (int i = 0; i < 10; i++)
            {
                tempNumber.Add(i);
            }

            for (int i = 0; i < 4; i++)
            {
                int index = rand.Next(0, tempNumber.Count);

                randNumber.Add(tempNumber[index]);
                tempNumber.RemoveAt(index);
            }

            Console.WriteLine("Random number : " + randNumber[0] + randNumber[1] + randNumber[2] + randNumber[3] + "\n");
        }

        static void InputNumber()
        {
            bool valid = false;
            Regex regex = new Regex("^[0-9]+$");

            while(valid == false)
            {
                Console.Write("Input number : ");
                string input = Console.ReadLine();

                if (regex.IsMatch(input))
                {
                    if (input.Length == 4)
                    {
                        for (int i = 0; i < inputNumber.Length; i++)
                        {
                            inputNumber[i] = int.Parse(input.Substring(i, 1));
                        }

                        valid = true;
                    }
                    else
                    {
                        valid = false;
                        Console.WriteLine("\nplease try again.\n");
                    }

                    //Console.WriteLine(inputNumber[0] + " " + inputNumber[1] + " " + inputNumber[2] + " " + inputNumber[3]);
                }
                else
                {
                    Console.WriteLine("\nplease try again.\n");
                }
            }
        }

        static void PredictAnswer()
        {
            while(predictNum != randNumber.Count || predictPos != randNumber.Count)
            {
                Console.WriteLine("////////////////////////////////////////////\n");

                for(int i = 0; i < isCheck.Length; i++)
                {
                    isCheck[i] = false;
                }

                predictNum = 0;
                predictPos = 0;

                InputNumber();

                for (int i = 0; i < inputNumber.Length; i++)
                {
                    int index = randNumber.IndexOf(inputNumber[i]);

                    if (index != -1 )
                    {
                        if (!isCheck[index])
                        {
                            isCheck[index] = true;
                            predictNum++;

                            if (randNumber.IndexOf(inputNumber[i]) == i)
                            {
                                predictPos++;
                            }
                        }
                        else if (randNumber.IndexOf(inputNumber[i]) == i)
                        {
                            predictPos++;
                        }
                    }
                }

                predictCount++;

                Console.WriteLine("\nPredict number : " + predictNum + " Predict postion : " + predictPos + "\n");
                
            }

            Console.WriteLine("Predict count : " + predictCount + "\n");
            Console.WriteLine("////////////////////////////////////////////\n");
        }

        static void PlayAgain()
        {
            string input = "";

            while(input != "y" && input != "n")
            {
                Console.Write("Play again (y/n) : ");
                input = Console.ReadLine();
            }

            if (input == "y")
            {
                tempNumber.Clear();
                randNumber.Clear();

                predictNum = 0;
                predictPos = 0;
                predictCount = 0;

                play = true;
                Console.WriteLine();
            }
            else if (input == "n")
            {
                play = false;
            }
        }
    }
}
