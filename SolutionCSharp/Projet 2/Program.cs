using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banques;
using FileControls;
using Traitements;



namespace Projet_2
{
    class Program
    {

        public static void Main(string[] args)
        {
            Banque banque = new Banque();
            Traitement traitement = new Traitement(banque);

            // INSTANCIATION DE LA LECTURE DES FICHIERS D'ENTREE
            Lecture readerGestionnaires = new Lecture("C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\gestionnaires.txt");
            Lecture readerComptes = new Lecture("C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\comptes.txt");
            Lecture readerTransactions = new Lecture("C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\transactions.txt");


            // INSTANCIATION DE L'ECRITURE DES FICHIERS DE SORTIE
            EcritureFile writerComptes = new EcritureFile("C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\statutOperations.txt");
            EcritureFile writerTransactions = new EcritureFile("C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\statutTransactions.txt");
            EcritureConsole writerConsole = new EcritureConsole(banque);


            // GESTIONNAIRES
            traitement.Gestionnaires(readerGestionnaires);

            readerGestionnaires.DisposeAndClose();

            // COMPTES
            traitement.Comptes(readerComptes, writerComptes, writerConsole);
            readerComptes.DisposeAndClose();
            writerComptes.DisposeAndClose();

            // TRANSACTIONS
            traitement.Transactions(readerTransactions, writerTransactions, writerConsole);
            readerTransactions.DisposeAndClose();
            writerTransactions.DisposeAndClose();


            Console.ReadKey();
        }
    }



}
    



