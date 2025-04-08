using System;
using System.Collections.Generic;

namespace Semaine1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            /*
            Serie1 serie1 = new Serie1();
            serie1.test();
            */
            Module3 module3 = new Module3();
            module3.test();

            Serie2 serie2 = new Serie2();
            serie2.test();


            Console.ReadKey();

        }
    }



    class Serie2
    {
        public void test()
        {
            int[] A = new int[] { 1, -5, 10, 3, 0, 4, 2, -7 };
            Console.WriteLine(LinearSearch(A, -5));

            int[] B = new int[] { -7, -5, 0, 1, 2, 3, 4, 10 };
            Console.WriteLine(BinarySearch(B, 5));

            int[] C = new int[] { };
            Console.WriteLine(BinarySearch(C, 5));

            Console.WriteLine("F");
            int[] D = new int[] { 1, 2, 3 };
            int[] E = new int[] { -1, -4, 0 };
            int[][] F = BuildingMatrix(D, E);
            Module3.displayTabBi(F);

            Console.WriteLine("I");
            int[] G = new int[] { 1, 2, 3 };
            int[] H = new int[] { -1, -4, 0 };
            int[][] I = BuildingMatrix(H, G);
            Module3.displayTabBi(I);

            Console.WriteLine("J=F+I");
            int[][] J = Addition(F, I);
            Module3.displayTabBi(J);

            Console.WriteLine("K=F-I");
            int[][] K = Substraction(F, I);
            Module3.displayTabBi(K);

            int[][] L = new int[3][] 
            {
                new int[] {1, 2},
                new int[] {4, 6},
                new int[] {-1, 8},
            };
            int[][] M = new int[2][]
            {
                new int[] {-1, 5, 0},
                new int[] {-4, 0, 1},
            };
            Console.WriteLine("N=L*M");
            int[][] N = Multiplication(L, M);
            Module3.displayTabBi(N);


        }
        
        static int LinearSearch(int[] tableau, int valeur)
        {
            if (tableau.Length == 0)
            {
                return -1;
            }

            int res = -1;
            for (int i = 0; i < tableau.Length; i++)
            {
                if (tableau[i] == valeur)
                {
                    res = i;
                }
            }
            return res;
        }

        static int BinarySearch(int[] tableau, int valeur)
        {
            int res = -1;
            int sup = tableau.Length;
            int inf = 0;

            if (sup == 0)
            {
                return -1;
            }

            while (res != (sup - inf) / 2)
            {
                res = (sup - inf) / 2;
                if (tableau[res] == valeur) {
                    return res;
                }
                else if (tableau[res] < valeur) {
                    sup = res;
                }
                else
                {
                    inf = res;
                }
            }
            return -1;
        }
        
        static int[][] BuildingMatrix(int[] leftVector, int[] rightVector)
        {
            int[][] matrix = new int[leftVector.Length][];

            for (int i = 0; i < leftVector.Length; i++)
            {
                matrix[i] = new int[rightVector.Length];

                for (int j = 0; j < rightVector.Length; j++)
                {
                    matrix[i][j] = leftVector[i] * rightVector[j];                    
                }
            }
            return matrix;
        }
        
        static int[][] Addition(int[][] leftMatrix, int[][] rightMatrix)
        {
            int[][] matrix = new int[leftMatrix.Length][];

            for (int i = 0; i < leftMatrix.Length; i++)
            {
                matrix[i] = new int[leftMatrix[0].Length];
                for (int j = 0; j < leftMatrix[0].Length; j++)
                {
                    matrix[i][j] = leftMatrix[i][j] + rightMatrix[i][j];
                }
            }
            return matrix;
        }

        static int[][] Substraction(int[][] leftMatrix, int[][] rightMatrix)
        {
            int[][] matrix = new int[leftMatrix.Length][];

            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new int[leftMatrix[0].Length];
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    matrix[i][j] = matrix[i][j] - rightMatrix[i][j];
       
                }
            }
            return matrix;
        }

        static int[][] Multiplication(int[][] leftMatrix, int[][] rightMatrix)
        {
            int[][] matrix = new int[leftMatrix.Length][];

            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new int[rightMatrix[0].Length];
                for (int j = 0; j < rightMatrix[0].Length; j++)
                {
                    for (int k = 0; k < leftMatrix[0].Length; k++)
                    {
                        matrix[i][j] += leftMatrix[i][k] * rightMatrix[k][j];
                    }
                }
            }
            return matrix;

        }


    }


    class Module3
    {
        public void test()
        {
            Voiture C3 = new Voiture("Citroen C3", 15, 5.00, "Roule", "Gule", "Jeanne");
            Console.WriteLine(C3.toString());

            Console.WriteLine("Tableaux : ");
            int[] tableau = new int[] { 1, 2, 3, 5, 454 };
            displayTabUni(tableau);

            int[][] tableau2 = new int[][]
            {
                new int[] {1, 3, 4765},
                new int[] {14, 35},
                new int[] {15, 34, 455},
                new int[] {17, 31},
            };

            Console.WriteLine(maFonction(5, 4, 5, 6));

            List<string> mots = new List<string>() 
            {
                "orange", "violet" , "bleu", "rouge"
            };
            



        }

        static int maFonction(params int[] tableau)
        {
            int somme = 0;
            foreach (int element in tableau)
            {
                somme += element;
            }
            return somme;
        }

        public static void displayTabUni(int[] tableau)
        {
            
            for (int i = 0; i < tableau.Length; i++)
            {
                Console.Write($"{tableau[i]} ");
            }
            Console.WriteLine();

        }

        public static void displayTabBi(int[][] tableau)
        {

            for (int i = 0; i < tableau[0].Length; i++)
            {
                displayTabUni(tableau[i]);
            }
            

        }

        public enum Etat : byte
        {
            Roule,
            RoulePas,
            RoulePeutEtre,
            Autre,
        }

        public struct Voiture
        {

            string modele;
            int age;
            double prix;
            Etat etat;
            string nomProprio;
            string prenomProprio;


            public Voiture(string Modele, int Age,
                double Prix, string Etat, string NomProprio, string PrenomProprio)
            {
                this.modele = Modele;
                this.age = Age;
                this.prix = Prix;
                this.nomProprio = NomProprio;
                this.prenomProprio = PrenomProprio;

                switch (Etat) {
                    case "Roule":
                        this.etat = Module3.Etat.Roule;
                        break;
                    case "RoulePas":
                        this.etat = Module3.Etat.RoulePas;
                        break;
                    case "RoulePeutEtre":
                        this.etat = Module3.Etat.RoulePeutEtre;
                        break;
                    default:
                        this.etat = Module3.Etat.Autre;
                        break;
                }
               
            }

            public string toString()
            {
                return $"{this.modele} - {this.prix} - {this.etat}";
            }

        }

    }

    class Serie1
    {

        public void test()
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

            DisplayPrimes();

            Console.WriteLine(Gcd(25, 10));
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
                int droite = n + i;

                if (!isSmooth && i % 2 == 0)
                {
                    for (int j = 1; j <= 1 + 2 * (n - 1); j++)
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
            if (n <= 1)
            {
                return 1;
            }
            else
            {
                return n * RecursiveFactorielle(n - 1);
            }
        }

        static bool IsPrime(int value)
        {
            bool res = true;
            int b = 2;
            while (b <= Math.Sqrt(value))
            {
                if (value % b == 0)
                {
                    res = false;
                }
                b++;
            }
            return res;
        }

        static void DisplayPrimes()
        {
            for (int i = 1; i <= 100; i++)
            {
                if (IsPrime(i))
                {
                    Console.WriteLine(i);
                }
            }
        }

        static int Gcd(int a, int b)
        {
            if (b != 0)
            {
                int q = a / b;
                int r = a % b;

                if (r == 0)
                {
                    return b;
                }
                else
                {
                    return Gcd(b, r);
                }
            }
            else
            {
                return -1;
            }
        }




    }

}
