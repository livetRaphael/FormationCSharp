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


namespace Projet_1
{
    class Program
    {

        public static void Main(string[] args)
        {
            Banque banque = new Banque();
            Traitement traitement = new Traitement(banque);

            // INSTANCIATION DE LA LECTURE DES FICHIERS D'ENTREE
            Lecture readerComptes = new Lecture("C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_1\\Comptes_2.txt");
            Lecture readerTransactions = new Lecture("C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_1\\Transactions_2.txt");

            // INSTANCIATION DE L'ECRITURE DES FICHIERS DE SORTIE
            EcritureFile writerFile = new EcritureFile("C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_1\\outputStatut.txt");
            EcritureConsole writerConsole = new EcritureConsole(banque);

            // COMPTES
            traitement.Comptes(readerComptes);
            // TRANSACTIONS
            traitement.Transaction(readerTransactions, writerFile, writerConsole);


            writerFile.DisposeAndClose();
            readerComptes.DisposeAndClose();
            readerTransactions.DisposeAndClose();



            Console.ReadKey();
        }
    }


}
    



