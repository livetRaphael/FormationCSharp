using System;

namespace Semaine1
{
    class Program
    {

        static void Main(string[] args)
        {
            BasicOperation(3, 4, '+');
            BasicOperation(6, 2, '/');
            BasicOperation(3, 0, '/');
            BasicOperation(6, 9, 'L');

            IntegerDivision(12, -4);
            IntegerDivision(13, -4);
            IntegerDivision(12, 0);

            Pow(2, 2);
            Pow(4, -2);

            PyramidConstruction(5, true);

            PyramidConstruction(10, false);

            Console.WriteLine(GoodDay(15));

            Console.WriteLine(IterativeFactorielle(3));
            Console.WriteLine(IterativeFactorielle(0));

            Console.WriteLine(RecursiveFactorielle(3));
            Console.WriteLine(RecursiveFactorielle(0));

            Console.ReadKey();


        }

        static void BasicOperation(int a, int b, char ope)
        {
            switch (ope)
            {
                case char o when o == '+':
                    Console.WriteLine($"{a} {ope} {b} = {a + b}");
                    break;
                case char o when o == '-':
                    Console.WriteLine($"{a} {ope} {b} = {a - b}");
                    break;
                case char o when o == '*':
                    Console.WriteLine($"{a} {ope} {b} = {a * b}");
                    break;
                case char o when o == '/' && b != 0:
                    Console.WriteLine($"{a} {ope} {b} = {a / b}");
                    break;
                default:
                    Console.WriteLine($"{a} {ope} {b} = Opération invalide.");
                    break;
            }
        }

        static void IntegerDivision(int a, int b)
        {
            int q;
            int r;

            if (b != 0)
            {
                q = a / b;
                r = a % b;
                if (r != 0)
                {
                    Console.WriteLine($"{a} = {q} * {b} + {r}");
                }
                else
                {
                    Console.WriteLine($"{a} = {q} * {b}");
                }
            }
            else
            {
                Console.WriteLine($"{a} : {b} = Opération invalide.");
            }
        }    

        static void Pow(int a, int b)
        {  
            if (b < 0)
            {
                Console.WriteLine($"{a} ^ {b} = Opération invalide.");
            }
            else
            {
                Console.WriteLine($"{a} ^ {b} = {Math.Pow(a, b)}");
            }
        }

        static string GoodDay(int heure)
        {
            string message;
            switch (heure)
            {
                case int h when h < 6:
                    message = "Merveilleuse nuit !";
                    break;
                case int h when h >= 6 && h < 12:
                    message = "Bonne matinée !";
                    break;
                case int h when h == 12:
                    message = "Bonne appétit !";
                    break;
                case int h when h > 12 && h < 18:
                    message = "Profitez de votre après-midi !";
                    break;
                case int h when h > 18:
                    message = "Passez une bonne soirée !";
                    break;
                default:
                    message = "ERREUR HORAIRE INCONNUE";
                    break;
            }
            return $"Il est {heure}H, {message}";
        }

        static void PyramidConstruction(int n, bool isSmooth)
        {
            for (int i = 1; i <= n; i++)
            {
                int gauche = n - i + 1;
                int droite = n + i ;
                
                if (!isSmooth && i%2==0) {
                    for (int j = 1; j <= 1+2*(n-1); j++)
                    {
                        if (j >= gauche && j < droite)
                        {
                            Console.Write("-");
                        }
                        else
                        {
                            Console.Write(" ");
                        }

                    }
                
                }
                else
                {
                    for (int j = 1; j <= 1 + 2 * (n - 1); j++)
                    {
                        if (j >= gauche && j < droite)
                        {
                            Console.Write("*");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                }
                Console.WriteLine("");
            }
        }

        static int IterativeFactorielle(int n)
        {
            int res = 1;
            for (int i = 1; i <= n; i++)
            {
                res *= i;
            }
            return res;
        }

        static int RecursiveFactorielle(int n)
        {
            if (n <= 1) {
                return 1;
            }
            else
            {
                return n * RecursiveFactorielle(n - 1);
            }
        }



    }
}
