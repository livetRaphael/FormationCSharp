﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Semaine1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            /*
            Serie1 serie1 = new Serie1();
            serie1.test();
            
            Module3 module3 = new Module3();
            module3.test();

            Serie2 serie2 = new Serie2();
            serie2.test();
               
            Module4 module4 = new Module4();
            module4.test();
            
            Serie3 serie3 = new Serie3();
            serie3.test();
                 

            Percolation.PercolationSimulation perSim;
            Percolation.PercolationSimulation.PclData result = perSim.MeanPercolationValue(20, 100);
            Console.WriteLine($"{result.mean} ; {result.std}");

            */

            Serie4 serie4 = new Serie4();
            serie4.test();



            Console.ReadKey();

        }
    }


    class Maze
    {
        public struct Cell
        {

            // true si paroi et false si ouvert
            // haut, bas, gauche, droite
            public bool[] walls;

            public bool visited;

            // 0 cellule simple, 1 entree, 2 sortie
            public byte state;

            public Cell(bool[] Walls, bool Visited, byte State)
            {

                this.walls = Walls;
                this.visited = Visited;
                this.state = State;
            }
        }

        private int N;
        private int M;
        private Cell[,] grid;
        public Maze(int N, int M)
        {
            this.N = N;
            this.M = M;
            grid = new Cell[N, M];
        }


        public bool IsOpen(int i, int j, int w)
        {
            if (i < 0 || i >= N || j < 0 || j >= M)
                return false;

            return !(grid[i, j].walls[w]);
        }

        public bool IsMazeStart(int i, int j)
        {
            return (grid[i, j].state == 1);
        }

        public bool IsMazeEnd(int i, int j)
        {
            return (grid[i, j].state == 2);
        }

        public void Open(int i, int j, int w)
        {
            grid[i, j].walls[w] = false;

            switch (w)
            {
                case 0:
                    if (i != 0)
                    {
                        grid[i - 1, j].walls[1] = false;
                    }
                    break;
                case 1:
                    if (i != N)
                    {
                        grid[i + 1, j].walls[0] = false;
                    }
                    break;
                case 2:
                    if (j != 0)
                    {
                        grid[i, j - 1].walls[3] = false;
                    }
                    break;
                case 3:
                    if (j != M)
                    {
                        grid[i, j + 1].walls[2] = false;
                    }
                    break;
                default:
                    break;
            }
        }

        public Dictionary<int, int> CloseNeighbors(int i, int j)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();

            if (i > 0)
            {
                result.Add(i - 1, j);
            }
            if (i < N - 1)
            {
                result.Add(i + 1, j);
            }
            if (j > 0)
            {
                result.Add(i, j - 1);
            }
            if (j < M - 1)
            {
                result.Add(i, j + 1);
            }

            return result;
        }


    }


    class Serie4
    {

        public void test()
        {
            string code = "=.=.=.=...=...=.===.=.=...=.===.=.=...===.===.===.....=.===.===...===.===.===...=.===.=...=.===.=.=...===.=.=";


            Console.WriteLine(LettersCount(code));
            Console.WriteLine(WordsCount(code));

            Console.Write($"{code} :");
            Console.Write($" {MorseTranslation(code)}");
            Console.WriteLine();

            Console.Write($"{code} :");
            Console.Write($" {EfficientMorseTranslation(code)}");
            Console.WriteLine();

            string phrase = "BONNE NUIT";
            string phraseCode = MorseEncryption(phrase);

            Console.Write($"{phrase} :");
            Console.Write($" {phraseCode} :");
            Console.Write($" {EfficientMorseTranslation(phraseCode)}");
            Console.WriteLine();

            Console.Write($"[[()] : {BracketControls("[[()]")}  ");

            Console.WriteLine();
            PhoneBook phoneBook = new PhoneBook(new Dictionary<string, string>());
            phoneBook.AddPhoneNumber("0651013987", "Raphael");
            phoneBook.AddPhoneNumber("0651513987", "Sullivan");
            phoneBook.AddPhoneNumber("0651693987", "George");
            phoneBook.DeletePhoneNumber("0651013987");
            phoneBook.DisplayPhoneBook();

            BusinessSchedule schedule = new BusinessSchedule(new SortedDictionary<DateTime, TimeSpan>());
            schedule.AddBusinessMeeting(new DateTime(2022, 3, 10, 14, 0, 0), new TimeSpan(1, 0, 0));
            schedule.AddBusinessMeeting(new DateTime(2022, 3, 10, 18, 0, 0), new TimeSpan(1, 0, 0));
            schedule.AddBusinessMeeting(new DateTime(2022, 3, 10, 16, 0, 0), new TimeSpan(1, 0, 0)); 
            schedule.DisplayMeetings();
            schedule.ClearMeetingPeriod(new DateTime(2022, 3, 10, 14, 0, 0), new DateTime(2022, 3, 10, 16, 30, 0));
            schedule.DisplayMeetings();

        }

        /*
        Le dictionnaire a l'air particulièrement adapté puisqu'on a une correspondance 
        clair entre chaque lettre et son code morse.
        */

        static int LettersCount(string code)
        {
            return code.Replace(".....", "...").Split(new[] { "..." }, StringSplitOptions.None).Count();
        }

        static int WordsCount(string code)
        {
            return code.Split(new[] { "....." }, StringSplitOptions.None).Count();
        }

        static string MorseTranslation(string code)
        {
            Dictionary<char, string> morseCode = new Dictionary<char, string>()
        {
            {'A', "=.==="},
            {'B', "===.=.=.="},
            {'C', "===.=.===.="},
            {'D', "===.=.="},
            {'E', "="},
            {'F', "=.=.===.="},
            {'G', "===.===.="},
            {'H', "=.=.=.="},
            {'I', "=.="},
            {'J', "=.===.===.==="},
            {'K', "===.=.==="},
            {'L', "=.===.=.="},
            {'M', "===.==="},
            {'N', "===.="},
            {'O', "===.===.==="},
            {'P', "=.===.===.="},
            {'Q', "===.===.=.==="},
            {'R', "=.===.="},
            {'S', "=.=.="},
            {'T', "==="},
            {'U', "=.=.==="},
            {'V', "=.=.=.==="},
            {'W', "=.===.==="},
            {'X', "===.=.=.==="},
            {'Y', "===.=.===.==="},
            {'Z', "===.===.=.="}
        };
            // Inversion du dictionnaire : morse -> caractère
            var reversedMorseCode = morseCode.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

            string decode = "";

            string[] words = code.Split(new[] { "....." }, StringSplitOptions.None);
            foreach (string word in words)
            {

                string[] letters = word.Split(new[] { "..." }, StringSplitOptions.None);
                foreach (string letter in letters)
                {

                    if (reversedMorseCode.TryGetValue(letter, out char decodedLetter))
                    {
                        decode += decodedLetter;
                    }
                    else
                    {
                        decode += "?";
                    }
                }
                decode += " ";
            }
            return decode;
        }

        static string EfficientMorseTranslation(string code)
        {
            Dictionary<char, string> morseCode = new Dictionary<char, string>()
        {
            {'A', "=.==="},
            {'B', "===.=.=.="},
            {'C', "===.=.===.="},
            {'D', "===.=.="},
            {'E', "="},
            {'F', "=.=.===.="},
            {'G', "===.===.="},
            {'H', "=.=.=.="},
            {'I', "=.="},
            {'J', "=.===.===.==="},
            {'K', "===.=.==="},
            {'L', "=.===.=.="},
            {'M', "===.==="},
            {'N', "===.="},
            {'O', "===.===.==="},
            {'P', "=.===.===.="},
            {'Q', "===.===.=.==="},
            {'R', "=.===.="},
            {'S', "=.=.="},
            {'T', "==="},
            {'U', "=.=.==="},
            {'V', "=.=.=.==="},
            {'W', "=.===.==="},
            {'X', "===.=.=.==="},
            {'Y', "===.=.===.==="},
            {'Z', "===.===.=.="}
        };
            // Inversion du dictionnaire : morse -> caractère
            var reversedMorseCode = morseCode.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

            string decode = "";

            string[] words = Regex.Split(code, @"\.{5,}"); ;
            foreach (string word in words)
            {

                string[] letters = word.Split(new[] { "...", "...." }, StringSplitOptions.None);
                foreach (string letter in letters)
                {

                    if (reversedMorseCode.TryGetValue(letter.Replace("..", "."), out char decodedLetter))
                    {
                        decode += decodedLetter;
                    }
                    else
                    {
                        decode += "?";
                    }
                }
                decode += " ";
            }
            return decode;
        }

        static string MorseEncryption(string sentence)
        {
            Dictionary<char, string> morseCode = new Dictionary<char, string>()
        {
            {'A', "=.==="},
            {'B', "===.=.=.="},
            {'C', "===.=.===.="},
            {'D', "===.=.="},
            {'E', "="},
            {'F', "=.=.===.="},
            {'G', "===.===.="},
            {'H', "=.=.=.="},
            {'I', "=.="},
            {'J', "=.===.===.==="},
            {'K', "===.=.==="},
            {'L', "=.===.=.="},
            {'M', "===.==="},
            {'N', "===.="},
            {'O', "===.===.==="},
            {'P', "=.===.===.="},
            {'Q', "===.===.=.==="},
            {'R', "=.===.="},
            {'S', "=.=.="},
            {'T', "==="},
            {'U', "=.=.==="},
            {'V', "=.=.=.==="},
            {'W', "=.===.==="},
            {'X', "===.=.=.==="},
            {'Y', "===.=.===.==="},
            {'Z', "===.===.=.="}
        };

            string code = "";
            foreach (char letter in sentence)
            {
                if (letter == ' ')
                {
                    code += ".....";
                }
                else
                {
                    if (morseCode.TryGetValue(letter, out string codedLetter))
                    {
                        code += codedLetter;
                    }
                    else
                    {
                        code += "?";
                    }
                    code += "...";
                }
            }
            return code.Trim('.');
        }

        /*
        Je pense qu'un stack de caractère, ça parait pratique d'avoir un tableau
        variable pour insérer les parenthèses ouvrantes et remove lorsqu'elle trouve leurs
        parenthèses fermantes --> LIFO
        */

        static bool BracketControls(string sentence)
        {
            List<char> typeParOuvr = new List<char> { '{', '(', '[' };
            List<char> typeParFerm = new List<char> { '}', ')', ']' };
            Stack<char> par = new Stack<char> { };

            bool result = true;

            foreach (char c in sentence)
            {
                if (typeParOuvr.Contains(c)) {
                    par.Push(c);
                }

                // Verification que c est une parenthèse fermante
                if (typeParFerm.Contains(c))
                {
                    // Verification que la dernière parenthèse ouvrante correspond à cette
                    // parenthèse fermante
                    if (par.Count != 0 && par.Peek() == typeParOuvr[typeParFerm.LastIndexOf(c)])
                    {
                        par.Pop();
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            if (par.Count != 0)
            {
                result = false;
            }
            return result;
        }


        /*
        Puisque le numero de telephone est unique et considerant le sens de fonctionnement
        de l'annuaire, je ferais un dictionnaire ayant pour clé le numero et pour valeur
        le nom du contact.
        */

        struct PhoneBook
        {
            public Dictionary<string, string> book;

            public PhoneBook(Dictionary<string, string> Book)
            {
                this.book = Book;
        }

            public bool IsValidPhoneNumber(string phoneNumber)
            {
                return (phoneNumber.Length == 10 && phoneNumber[0] == 0 && phoneNumber[1] != 0);
            }

            public bool ContainsPhoneContact(string phoneNumber)
            {
                return this.book.ContainsKey(phoneNumber);
            }

            public void PhoneContact(string phoneNumber)
            {
                if (ContainsPhoneContact(phoneNumber))
                {
                    Console.WriteLine(this.book[phoneNumber]);
                }
                else
                {
                    throw new Exception("Numero de téléphone pas présent");
                }
            }

            public bool AddPhoneNumber(string phoneNumber, string name)
            {
                bool result = true;
                if (ContainsPhoneContact(phoneNumber))
                {
                    result = false;
                }
                else
                {
                    this.book.Add(phoneNumber, name);
                }
                return result;
            }

            public bool DeletePhoneNumber(string phoneNumber)
            {
                bool result = true;
                if (ContainsPhoneContact(phoneNumber))
                {
                    this.book.Remove(phoneNumber);
                }
                else
                {
                    result = false;
                }
                return result;
            }

            public void DisplayPhoneBook()
            {
                Console.WriteLine("Annuaire téléphonique :");
                Console.WriteLine("-----------------------");

                if (this.book.Count != 0)
                {
                    foreach (KeyValuePair<string, string> phone in this.book)
                    {
                        Console.WriteLine($"{phone.Key} : {phone.Value}");
                    }
                }
                else
                {
                    Console.WriteLine("Pas de numéros téléphoniques");
                }
                Console.WriteLine("-----------------------");

            }

        }

        /*
        Au début : il faut tout décaler d'un donc il y a N inversions
        Au milieu : il faut décaler la moitié de la liste d'un donc il y a N/2 inversions
        A la fin, il n'y a pas d'inversion, on ajoute juste à la suite dans la mémoire
        */

        /*
        Si la liste est non trié, il faudra au maximum parcourir toute la liste 
        pour trouver un élément donné.
        Si la liste est trié, une recherche dichotomique permet de trouver l'élément
        en log(n)/log(2) comparaisons au maximum.
        */

        struct BusinessSchedule
        {
            public SortedDictionary<DateTime, TimeSpan> schedule;
            public DateTime startSchedule;
            public DateTime endSchedule;

            public BusinessSchedule(SortedDictionary<DateTime, TimeSpan> Schedule)
            {
                this.schedule = Schedule;
                this.startSchedule = new DateTime(2020, 1, 1);
                this.endSchedule = new DateTime(2030, 12, 31);
            }

            public bool IsEmpty()
            {
                return (this.schedule.Count == 0);
            }

            public void SetRangeOfDates(DateTime begin, DateTime end)
            {
                if (begin > end)
                {
                    throw new Exception("Date de début et de fin invalide");
                }
                if (IsEmpty())
                {
                    this.startSchedule = begin;
                    this.endSchedule = end;
                }
                else
                {
                    throw new Exception("Emploi du temps pas vide de toute réunion");
                }

            }

            public void DisplayMeetings(DateTime begin = new DateTime(), DateTime end = new DateTime())
            {
                if (begin == new DateTime())
                {
                    begin = this.startSchedule;
                }
                if (end == new DateTime())
                {
                    end = this.endSchedule;
                }

                Console.WriteLine($"Emploi du temps : {begin.ToString("dd/MM/yyyy HH:mm:ss")} - {end.ToString("dd/MM/yyyy HH:mm:ss")}");
                Console.WriteLine("-----------------------------------------------------------");

                int i = 1;
                List<DateTime> rdvDates = this.schedule.Keys.Where(date => date >= begin && date <= end).ToList();

                if (IsEmpty())
                {
                    Console.WriteLine("Pas de réunions programmées");
                }
                else
                {
                    foreach (DateTime rdvDate in rdvDates)
                    {
                        TimeSpan rdvDuration = this.schedule[rdvDate];
                        Console.WriteLine($"Réunion {i}       : {rdvDate.ToString("dd/MM/yyyy HH:mm:ss")} - {rdvDate.Add(rdvDuration).ToString("dd/MM/yyyy HH:mm:ss")}");
                        i++;
                    }

                }
                Console.WriteLine("-----------------------------------------------------------");
            }

            public KeyValuePair<DateTime?, DateTime?> ClosestElements(DateTime beginMeeting)
            {
                DateTime? preDate;
                try
                {
                    preDate = this.schedule.Keys.Where(date => date <= beginMeeting).Last();
                }
                catch (Exception)
                {
                    preDate = null;
                }

                DateTime? postDate;
                try
                {
                    postDate =  this.schedule.Keys.Where(date => date > beginMeeting).First();
                }
                catch (Exception)
                {
                    postDate = null;
                }

                return new KeyValuePair<DateTime?, DateTime?>(preDate, postDate);
            }

            public bool AddBusinessMeeting(DateTime date, TimeSpan duration)
            {

                bool result;
                if (!(date > this.startSchedule && date < this.endSchedule))
                {
                    result = false;
                }
                else if (!(date + duration > this.startSchedule && date + duration < this.endSchedule))
                {
                    result = false;
                }
                else
                {
                    KeyValuePair<DateTime?, DateTime?> closestElements = ClosestElements(date);
                    
                    if (closestElements.Key != null && closestElements.Key.Value + this.schedule[closestElements.Key.Value] <= date)
                    {
                        if(closestElements.Value != null && closestElements.Value.Value >= date + duration)
                        {
                            this.schedule.Add(date, duration);
                            result = true;
                        }
                        else if (closestElements.Value == null)
                        {
                            this.schedule.Add(date, duration);
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else if (closestElements.Key == null)
                    {
                        this.schedule.Add(date, duration);
                        result = true;
                    }
                    else
                    {
                        result = false;

                    }
                }
                return result;
            }

            public bool DeleteBusinessMeeting(DateTime date)
            {
                bool result;

                SortedDictionary<DateTime, TimeSpan> s = this.schedule;
                DateTime? delDate;
                try
                {
                    delDate = this.schedule.Keys.Where(d => d <= date && d + s[d] > date).First();
                    this.schedule.Remove(delDate.Value);
                    result = true;
                }
                catch (Exception)
                {
                    delDate = null;
                    result = false;
                }

                return result;
            }

            public int ClearMeetingPeriod(DateTime begin, DateTime end)
            {
                int i = 0;
                if (!(begin > this.startSchedule && begin < this.endSchedule))
                {
                    throw new Exception("Début de période supprimer hors période de l'emploi du temps");
                }
                else if (!(end > this.startSchedule && end < this.endSchedule))
                {
                    throw new Exception("Fin de période supprimer hors période de l'emploi du temps");
                }
                else if (begin > end)
                {
                    throw new Exception("Période de suppression invalide : début après fin");
                }
                else
                {
                    DateTime? nextElement = begin;
                    DateTime? tempElement;
                    while (nextElement != null && nextElement < end)
                    {
                        tempElement = ClosestElements(nextElement.Value).Value;
                        DeleteBusinessMeeting(nextElement.Value);
                        nextElement = tempElement;
                        i++;
                    }
                }
                return i;
            }
            

}


    }





    class Percolation
    {
        public bool[,] open;
        public bool[,] full;
        public int N;

        public Percolation(int N)
        {
            this.N = N;
            open = new bool[N, N];
            full = new bool[N, N];
        }

        public bool IsOpen(int i, int j)
        {
            return open[i, j];
        }

        public bool IsFull(int i, int j)
        {
            return full[i, j];
        }

        public bool Percolate()
        {
            int i = N-1;
            for (int j = 0; j < N; j++)
            {
                if (IsFull(i, j))
                {
                    return true;
                }
            }
            return false;
        }

        public List<KeyValuePair<int, int>> CloseNeighbors(int i, int j)
        {
            List<KeyValuePair<int, int>> result = new List<KeyValuePair<int, int>> { };

            if (i > 0)
            {
                result.Add(new KeyValuePair<int, int> (i - 1, j));
            }
            if (i < N - 1)
            {
                result.Add(new KeyValuePair<int, int>(i + 1, j));
            }
            if (j > 0)
            {
                result.Add(new KeyValuePair<int, int>(i, j - 1));
            }
            if (j < N - 1)
            {
                result.Add(new KeyValuePair<int, int>(i, j + 1));
            }

            return result;
        }

        public bool Open(int i, int j)
        {
            bool IsNewOpen = false;
            // Verification si au moins un voisin est plein de (i,j)
            bool IsOneNeighborFull = false;
            List<KeyValuePair<int, int>> Voisins = CloseNeighbors(i, j);
            foreach (KeyValuePair<int, int> vois in Voisins)
            {
                if (IsFull(vois.Key, vois.Value))
                {
                    IsOneNeighborFull = true;
                }
            }


            if ((i==0 || IsOneNeighborFull) && !(IsFull(i,j)))
            {

                full[i, j] = true;
                IsNewOpen = true;

                for (int k = 0; k < Voisins.Count; k++)
                {
                    if (IsOpen(Voisins[k].Key, Voisins[k].Value) && !IsFull(Voisins[k].Key, Voisins[k].Value)) {
                        Open(i, j);
                    }
                    
                }
            }
            return IsNewOpen;


        }

        /*
        b) Dans le pire des cas, toutes les cases sauf la derniere ligne 
        sont pleines avant qu'une seule case de la derniere ligne ne soit ouverte
        Il faut alors N * (N-1) + 1 Open réussi pour qu'il y est percolation.

        c) Au debut, il y a une chance sur N qu'une case de la derniere ligne
        soit selectionnée. De plus, cette chance augmente à chaque open pour
        atteindre N/(N+1) à l'avant dernier cas possible. Donc peu probable 
        d'atteindre le cas limite.
        */

        public void displayGrid(bool[,] tableau)
        {

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write($"{tableau[i, j]} ");
                }
                Console.WriteLine();
            }


        }

        public struct PercolationSimulation
        {
            public struct PclData
            {
                public double mean;
                public double std;
            }

            public double PercolationValue(int size)
            {
                Percolation perc = new Percolation(size);
                Random rnd = new Random();

                int cpt = 0;
                while (!perc.Percolate()) {
                    int i = rnd.Next(0, size);
                    int j = rnd.Next(0, size);

                    if (perc.Open(i, j))
                    {
                        cpt++;
                    }
                    
                }
                
                return cpt / Math.Pow(size, 2);
            }

            public PclData MeanPercolationValue(int size, int t)
            {
                PclData result;

                result.mean = 0;
                result.std = 0;

                for (int i = 0; i < t; i++)
                {
                    
                    result.mean += PercolationValue(size);
                    result.std += Math.Pow(PercolationValue(size), 2);
                }
                result.mean /= t;
                result.std = Math.Sqrt(result.std / t - Math.Pow(result.mean, 2));

                return result;
            }
        }
        
    }
    
    class Serie3
    {
        public void test()
        {

            string url = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\";
            SchoolMeans(url + "entree.txt", url + "sortie.txt");


            int[] array1 = new int[] { 6, 4, 8, 2, 9, 3, 9, 4, 7, 6, 1 };
            InsertionSort(array1);
            Module3.displayTabUni(array1);

            int[] array2 = new int[] { 6, 4, 8, 2, 9, 3, 9, 4, 7, 6, 1 };
            QuickSort(array2);
            Module3.displayTabUni(array2);


            DisplayPerformances(new List<int> {1000, 2000, 5000, 10000, 20000}, 50);
            

        }

        struct Moyenne
        {
            public string nom;
            public string matiere;
            public double note;

            public Moyenne(string Nom, string Matiere, double Note)
            {
                this.nom = Nom;
                this.matiere = Matiere;
                this.note = Note;
            }
        }


        static void SchoolMeans(string input, string output)
        {
            List<Moyenne> moys = new List<Moyenne> { };
            string[] splitLigne;

            // Lecture de chaque ligne du fichier entrée
            using (TextReader reader = new StreamReader(input))
            {
                string ligne;
                while ((ligne = reader.ReadLine()) != null && ligne != string.Empty)
                {
                    splitLigne = ligne.Split(';');
                    double note = double.Parse(splitLigne[2], CultureInfo.InvariantCulture);
                    moys.Add(new Moyenne(splitLigne[0], 
                                         splitLigne[1], 
                                         note));
                }
            }

            // Traitement des moyennes
            List<Moyenne> moysMat = new List<Moyenne> { };
            while (moys.Count != 0)
            {
                string currentMat = moys[0].matiere;
                double somme = 0;
                int diviseur = 0;
                for (int i = 0; i < moys.Count; i++)
                {
                    
                    if (moys[i].matiere == currentMat)
                    {
                        somme += moys[i].note;
                        diviseur++;
                        
                        moys.Remove(moys[i]);
                        i--;
                    }
                }
                moysMat.Add(new Moyenne(string.Empty, currentMat, somme / diviseur));
            }

            // Ecriture de chaque ligne du fichier sortie
            using (TextWriter writer = new StreamWriter(output))
            {
                foreach (Moyenne moyMat in moysMat)
                {
                    string format = string.Format("##.0");
                    string ligne = $"{moyMat.matiere};{moyMat.note.ToString(format).Replace(',','.')}";
                    writer.WriteLine(ligne);
                }

                
                
            }

            return;
        }

        static void InsertionSort(int[] array)
        {
            int tempValue = 0;
            int pivot = 0;
            for (int i = 1; i < array.Length; i++)
            {
                int j = i-1;
                pivot = array[j+1];
                while (j >= 0 && pivot < array[j])
                {
                    tempValue = array[j];
                    array[j] = pivot;
                    array[j + 1] = tempValue;
                    j--;
                }
            }
        }

        static void QuickSort(int[] array)
        {
            if (array.Length <= 1)
            {
                return;
            }

            QuickSortRecursive(array, 0, array.Length - 1);
        }

        static void QuickSortRecursive(int[] array, int low, int high)
        {
            if (low < high)
            {
                
                int pivotIndex = Partition(array, low, high);

                QuickSortRecursive(array, low, pivotIndex - 1);
                QuickSortRecursive(array, pivotIndex + 1, high);
            }
        }

        static int Partition(int[] array, int low, int high)
        {
            int pivot = array[low];
            int i = low + 1;
            int j = high;

            while (i <= j)
            {
                while (i <= high && array[i] <= pivot)
                {
                    i++;
                }
                while (array[j] > pivot)
                {
                    j--;
                }
                if (i < j)
                {
                    Swap(ref array[i], ref array[j]);
                }
            }
            Swap(ref array[low], ref array[j]);
            return j;
        }

        static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }


        static long UseInsertionSort(int[] array)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            InsertionSort(array);
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        static long UseQuickSort(int[] array)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            QuickSort(array);
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        /*
        Puisque la génération des tableaux est random, un tableau peut etre 
        generer déjà trié. Donc il faut multiplier les tableaux pour avoir des
        données viables (dont le résultat ne dépend pas du random de la génération
        des tableaux).
        */

        static List<int[]> ArraysGenerator(int size)
        {
            int[] tab1 = new int[size];
            int[] tab2 = new int[size];
            Random rnd = new Random();

            for (int i = 0; i < size; i++)
            {
                tab1[i] = rnd.Next(-1000, 1001);
                tab2[i] = tab1[i];
            }
            return new List<int[]> { tab1, tab2 };
        }

        struct SortData
        {
            public double insertionMean;
            public double insertionStd;
            public double quickMean;
            public double quickStd;

            public SortData(double InsertionMean, double InsertionStd, double QuickMean, double QuickStd)
            {
                this.insertionMean = InsertionMean;
                this.insertionStd = InsertionStd;
                this.quickMean = QuickMean;
                this.quickStd = QuickStd;

            }
        }

        static SortData PerformanceTest(int size, int count)
        {
            SortData result = new SortData(0,0,0,0);

            for (int i = 0; i < count; i++)
            {
                List<int[]> tabs = ArraysGenerator(size);

                long insertionTime = UseInsertionSort(tabs[0]);
                long quickTime = UseQuickSort(tabs[1]);

                result.insertionMean += insertionTime;
                result.quickMean += quickTime;

                result.insertionStd += Math.Pow(insertionTime, 2);
                result.quickStd += Math.Pow(quickTime, 2);
            }

            result.insertionMean /= count;
            result.quickMean /= count;
            result.insertionStd = Math.Sqrt(result.insertionStd / count - Math.Pow(result.insertionMean, 2));
            result.quickStd = Math.Sqrt(result.quickStd / count - Math.Pow(result.quickMean, 2));

            return result;
        }


        static List<SortData> PerformancesTest(List<int> sizes, int count)
        {
            List<SortData> results = new List<SortData> { }; 
            foreach (int size in sizes)
            {
                results.Add(PerformanceTest(size, count));
            }
            return results;
        }

        static void DisplayPerformances(List<int> sizes, int count)
        {
            Console.WriteLine("Calculs des performances :");
            List <SortData> results = PerformancesTest(sizes, count);

            Console.WriteLine("Affichages des performances :");
            Console.WriteLine("n ;MeanInsertion ;StdInsertion ;MeanQuick ;StdQuick");
            for (int i = 0; i < sizes.Count; i++)
            {
                SortData result = results[i];
                Console.WriteLine($"{sizes[i]};{result.insertionMean};{result.insertionStd};{result.quickMean};{result.quickStd}");
                
            }
        }

    }
        
    class Module4
    {
        public void test()
        {
            displayNumeros();

            string url = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\test.txt";
            lireTextFile(url);
            ecrireTextFile(url);
        }

        static void displayNumeros()
        {
            StringBuilder numeros = new StringBuilder();

            for (int i = 1; i < 101; i++)
            {
                numeros.Append($"{i}-");
            }

            Console.WriteLine(numeros.ToString().TrimEnd('-'));
        }

        static void lireTextFile(string url)
        {
            using (TextReader reader = new StreamReader(url))
            {
                string ligne;
                while ((ligne = reader.ReadLine()) != null && ligne != string.Empty)
                {
                    Console.WriteLine(ligne);
                }
            }
        }

        static void ecrireTextFile(string url) {

            TextWriter writer = new StreamWriter(url);

            string input;
            while ((input = Console.ReadLine()) != string.Empty)
            {
                writer.WriteLine(input);
            }
            writer.Close();

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

            Console.WriteLine("EratosthenesSieve n=100");
            int[] erato = EratosthenesSieve(100);
            Module3.displayTabUni(erato);

            Console.WriteLine("QCM");
            Qcm q1 = new Qcm("Q1", new List<string> { "R1", "R2", "R3" }, 2, 3);
            Qcm q2 = new Qcm("Q2", new List<string> { "R1", "R2", "R3" }, 1, 3);

            //AskQuestion(q1);

            AskQuestions(new Qcm[] { q1, q2 });



        }

        static bool LinearSearchBool(int[] tableau, int valeur)
        {
            bool res = false;
            if (tableau.Length == 0)
            {
                return false;
            }

            
            for (int i = 0; i < tableau.Length; i++)
            {
                if (tableau[i] == valeur)
                {
                    res = true;
                }
            }
            return res;
        }

        static int LinearSearch(int[] tableau, int valeur)
        {
            if (tableau.Length == 0)
            {
                return -1;
            }

            int res = -1;
            int i = 0;
            while ((res == -1) && (i < tableau.Length))
            {
                if (tableau[i] == valeur)
                {
                    res = i;
                }
                i++;
            }
            return res;
        }

        /*
        Dans le pire des cas, tout le tableau est parcouru sans trouver la valeur
        */

        public static int BinarySearch(int[] tableau, int valeur)
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
                if (tableau[res] == valeur)
                {
                    return res;
                }
                else if (tableau[res] < valeur)
                {
                    sup = res;
                }
                else
                {
                    inf = res;
                }
            }
            return -1;
        }

        /*
        Dans le pire des cas, soit n la taille du tableau, il faudra log(n)/log(2)
        éléments lus.
        */


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

        static int[] EratosthenesSieve(int n)
        {
            List<int> result = new List<int> { };

            for (int i = 0; i < n - 1; i++)
            {
                result.Add(i + 2);
            }

            int iTraite = 0;
            while (result[iTraite] < Math.Sqrt(result.Max()))
            {

                foreach (int c in result.GetRange(iTraite + 1, result.Count - iTraite - 1))
                {
                    if (c % result[iTraite] == 0)
                    {
                        result.Remove(c);
                    }
                }
                iTraite++;
            }
            return result.ToArray();

        }

        static int findMax(List<int> list)
        {
            int res = list[0];
            foreach (int l in list)
            {
                if (res > l)
                {
                    res = l;
                }
            }
            return res;
        }

        public struct Qcm
        {
            public string question;
            public List<string> answers;
            public byte solution;
            public int weight;
            public Qcm(string Question, List<string> Answers, byte Solution,
                int Weight)
            {
                this.question = Question;
                this.answers = Answers;
                this.solution = Solution;
                this.weight = Weight;
            }
        }

        /*
        Les questions et les reponses sont des chaines de caractères et les 
        reponses sont multiples pour chaque question d'ou la liste.
        La solution est un byte parce que c'est l'indice de la reponse juste,
        on suppose qu'une question n'a pas plus de 255 réponses.
        Pour le poids de chaque question, on met un entier pour que l'utilisateur
        puisse définir les poids de ses questions aussi finement que voulu.
        */

        static bool QcmValidity(Qcm qcm)
        {
            bool res = true;

            if (qcm.solution < 0 || qcm.solution >= qcm.answers.Count)
            {
                res = false;
            }
            if (qcm.weight < 0)
            {
                res = false;
            }
            return res;
        }

        static int AskQuestion(Qcm qcm)
        {
            if (!QcmValidity(qcm))
            {
                throw new ArgumentException("QCM INVALIDE");
            }

            Console.WriteLine($"{qcm.question}");

            string answers = "";
            for (int i = 0; i < qcm.answers.Count(); i++)
            {
                answers += $"{i + 1}. {qcm.answers[i]}    ";
            }
            Console.WriteLine(answers);

            int answer = -1;
            while (!(answer > 0 && answer <= qcm.answers.Count))
            {
                Console.Write("Réponse : ");
                answer = int.Parse(Console.ReadLine());
            }

            if (answer == qcm.solution)
            {
                return qcm.weight;
            }
            else
            {
                return 0;
            }
            
        }

        static void AskQuestions(Qcm[] qcms)
        {
            int res = 0;
            int weights = 0;
            foreach (Qcm q in qcms)
            {
                res += AskQuestion(q);
                weights += q.weight;
            }
            Console.WriteLine($"Résultats du questionnaire : {res} / {weights}");
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

        /*
        1.
        a) Pour un niveau j, il y a 1 + 2 * (j-1) blocs.
        b) Donc au niveau N, il y a 1 + 2 * (N-1) blocs.
        
        2.
        a) Le sommet de la pyramide est à la position N.
        b) gauche(j) = N - j + 1
           droite(j) = N + j
        */

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

        /*
        La version itérative est plus efficace. Il y a moins d'instruction à
        faire à chaque "boucle".
        */


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


