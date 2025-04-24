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
            string urlReaderGestionnaires = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\Gestionnaires_5.txt";
            traitement.Gestionnaires(urlReaderGestionnaires);
            

            // COMPTES
            string urlReaderComptes = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\Comptes_5.txt";
            string urlWriterComptes = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\statutOperations_5.txt";
            //traitement.Comptes(urlReaderComptes, urlWriterComptes);


            // TRANSACTIONS
            string urlReaderTransactions = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\Transactions_5.txt";
            string urlWriterTransactions = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\statutTransactions_5.txt";
            string urlWriterMetrologie = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet_2\\metrologie_5.txt";
            //traitement.Transactions(urlReaderTransactions, urlWriterTransactions, urlWriterMetrologie);

            traitement.ComptesTransactions(urlReaderComptes, urlWriterComptes, urlReaderTransactions, urlWriterTransactions, urlWriterMetrologie);

            Console.ReadKey();
        }
    }



    



}
    



