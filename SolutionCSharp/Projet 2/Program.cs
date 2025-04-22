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

            // GESTIONNAIRES
            string urlReaderGestionnaires = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\gestionnaires.txt";
            traitement.Gestionnaires(urlReaderGestionnaires);
            
            // COMPTES
            string urlReaderComptes = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\comptes.txt";
            string urlWriterComptes = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\statutOperations.txt";
            traitement.Comptes(urlReaderComptes, urlWriterComptes);

            // TRANSACTIONS
            string urlReaderTransactions = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\transactions.txt";
            string urlWriterTransactions = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\statutTransactions.txt";
            string urlWriterMetrologie = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\metrologie.txt";
            traitement.Transactions(urlReaderTransactions, urlWriterTransactions, urlWriterMetrologie);


            Console.ReadKey();
        }
    }



    



}
    



