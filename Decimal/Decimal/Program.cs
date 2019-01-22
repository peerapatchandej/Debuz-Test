using System;
using System.Linq;

namespace Decimal
{
    class Program
    {
        static decimal number1, number2;
        static int fractionPoint = 0;
        static string recurring;

        static void Main(string[] args)
        {
            InputData();
            FindRecurringCount();
        }

        static void InputData()
        {
            bool valid = false;

            while (valid == false)
            {
                Console.Write("Input number 1 : ");
                string inputData = Console.ReadLine();

                if (decimal.TryParse(inputData, out number1))
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("\nplease try again.\n");
                }
            }

            valid = false;

            while (valid == false)
            {
                Console.Write("Input number 2 : ");
                string inputData = Console.ReadLine();

                if (decimal.TryParse(inputData, out number2) && number2 != 0)
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("\nplease try again.\n");
                }
            }

            valid = false;

            while (valid == false)
            {
                Console.Write("Input fraction : ");
                string inputData = Console.ReadLine();

                if (int.TryParse(inputData, out fractionPoint) && fractionPoint != 0)
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("\nplease try again.\n");
                }
            }

            Console.WriteLine();
        }

        static void Calculate(int fracCount)
        {
            int answer = 0;
            int fraction = 0;

            if(number1 == 0)
            {
                Console.WriteLine("Answer : 0\n");
                return;
            }

            if (decimal.Remainder(fractionPoint, fracCount) != 0)
            {
                decimal integer = decimal.Truncate(decimal.Divide(fractionPoint, fracCount));

                fraction = (int)(fractionPoint - integer * fracCount);
                answer = int.Parse(recurring.ElementAt(fraction - 1).ToString()); 
            }
            else
            {
                answer = int.Parse(recurring.Last().ToString());
            }

            Console.WriteLine("Answer : " + answer + "\n");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }

        static void FindRecurringCount()
        {
            decimal firstRemain = decimal.Remainder(number1, number2);
            decimal divided = firstRemain * 10;
            decimal lastRemain = 0;
            decimal tempValue = 0;
            int fractionCount = 0;

            //Console.WriteLine(number1 + " / " + number2 + " = " + decimal.Divide(number1, number2));

            if (firstRemain != 0)
            {
                try
                {
                    if (!PrimeNumber(number2))
                    {
                        while (firstRemain != lastRemain)
                        {
                            lastRemain = decimal.Remainder(divided, number2);
                            tempValue = decimal.Divide(divided, number2);

                            //Console.WriteLine(divided + " / " + number2 + " = " + decimal.Divide(divided, number2));

                            divided *= 10;
                            fractionCount++;
                        }
                    }
                    else
                    {
                        fractionCount = int.Parse((number2 - 1).ToString());
                    }

                    string result = decimal.Divide(number1, number2).ToString();
                    int index = result.IndexOf('.');
                    recurring = result.Substring(index + 1, fractionCount);

                    //Console.WriteLine("Fraction count : " + fractionCount + " result is " + recurring);

                    Calculate(fractionCount);
                }
                catch
                {
                    string result = decimal.Divide(number1, number2).ToString();
                    int index = result.IndexOf('.');
                    char answer = ' ';
                    
                    recurring = result.Remove(0, index + 1);
                    //Console.WriteLine("recurring  " + recurring);

                    if (fractionPoint <= recurring.Length)
                    {
                        Console.WriteLine("Answer : " + recurring.ElementAt(fractionPoint - 1) + "\n");
                    }
                    else
                    {
                        if (!PrimeNumber(number2))
                        {
                            if (recurring.Length >= 28)
                            {
                                answer = recurring.Last();
                            }
                            else
                            {
                                answer = '0';
                            }

                            Console.WriteLine("Answer : " + answer + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Can not find answer.\n");
                        }
                    }

                    Console.WriteLine("Press enter to close...");
                    Console.ReadLine();
                }
            }
        }

        static bool PrimeNumber(decimal number)
        {
            int i = 0;

            if (number != 2 && number != 5)
            {
                if (number != 1)
                {
                    for (i = 2; i < number; i++)
                    {
                        if (number % i == 0)
                        {
                            return false;
                        }
                    }
                }
                else return false;

                if (i == number) return true;
            }

            return false;
        }
    }
}
