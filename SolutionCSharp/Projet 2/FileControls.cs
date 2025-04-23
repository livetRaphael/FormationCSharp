using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banques;


namespace FileControls
{
    

    class Lecture
    {
        private FileStream _fileStream;
        private TextReader _reader;
        private bool _isOver;

        public bool IsOver { get => _isOver; set => _isOver = value; }

        public Lecture(string inputUrl)
        {
            this._fileStream = File.OpenRead(inputUrl);
            this._reader = new StreamReader(this._fileStream);
            this._isOver = false;
        }


        public void LireLigne(ref string[] splitLigne)
        {
            string ligne = "";
            if (!((ligne = this._reader.ReadLine()) != null && ligne != string.Empty))
            {
                this._isOver = true;
            }
            else
            {
                splitLigne = ligne.Split(';');
            }
            
        }

        public void DisposeAndClose()
        {
            this._fileStream.Dispose();
            this._reader.Dispose();

            this._fileStream.Close();
            this._reader.Close();
        }

    }

    class EcritureFile
    {
        private FileStream _fileStream;
        private StreamWriter _writer;

        public EcritureFile(string inputUrl)
        {
            this._fileStream = File.Create(inputUrl);
            this._writer = new StreamWriter(this._fileStream);
        }

        public void WriteLine(string demande, bool statut)
        {
            
        }

        public void WriteAllStatutsResults(Statuts statut)
        {
            foreach (KeyValuePair<string, bool> demande in statut.Demandes)
            {
                string labelStatut = demande.Value ? "OK" : "KO";
                this._writer.WriteLine($"{demande.Key};{labelStatut}");
            }
        }

        public void WriteTransactionsMetrologie(Metrologie metro)
        {

            this._writer.WriteLine("Statistique :");
            this._writer.WriteLine($"Nombre de comptes : {metro.NbrComptes}");
            this._writer.WriteLine($"Nombre de transactions : {metro.NbrTransactions}");
            this._writer.WriteLine($"Nombre de réussites : {metro.NbrReussites}");
            this._writer.WriteLine($"Nombre d'échecs : {metro.NbrEchecs}");
            this._writer.WriteLine($"Montant total des réussites : {metro.MontantReussites} euros");
            this._writer.WriteLine();

            this._writer.WriteLine("Frais de gestion :");
            foreach (KeyValuePair<uint, double> gestionnaire in metro.Frais)
            {
                this._writer.WriteLine($"{gestionnaire.Key} : {gestionnaire.Value} euros");
            }
        }

        public void DisposeAndClose()
        {
            this._writer.Dispose();
            this._fileStream.Dispose();
           
            this._fileStream.Close();
            this._writer.Close();
        }
    }

    class EcritureConsole
    {

        public void WriteAllStatutsResults(Statuts statut)
        {
            Console.WriteLine();
            foreach (KeyValuePair<string, bool> demande in statut.Demandes)
            {
                string labelStatut = demande.Value ? "OK" : "KO";
                Console.WriteLine($"{demande.Key};{labelStatut}");
            }
        }

        public void WriteTransactionsMetrologie(Metrologie metro)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Statistique :");
            Console.WriteLine($"Nombre de comptes : {metro.NbrComptes}");
            Console.WriteLine($"Nombre de transactions : {metro.NbrTransactions}");
            Console.WriteLine($"Nombre de réussites : {metro.NbrReussites}");
            Console.WriteLine($"Nombre d'échecs : {metro.NbrEchecs}");
            Console.WriteLine($"Montant total des réussites : {metro.MontantReussites} euros");
            Console.WriteLine();

            Console.WriteLine("Frais de gestion :");
            foreach (KeyValuePair<uint, double> gestionnaire in metro.Frais)
            {
                Console.WriteLine($"{gestionnaire.Key} : {gestionnaire.Value} euros");
            }
        }


    }


    class Statuts
    {
        private Dictionary<string, bool> _demandes;

        public Dictionary<string, bool> Demandes { get => _demandes; set => _demandes = value; }
    
        public Statuts()
        {
            this._demandes = new Dictionary<string, bool> { };
        }
    }

    class Metrologie
    {
        private int _nbrComptes;
        private int _nbrTransactions;
        private int _nbrReussites;
        private int _nbrEchecs;
        private double _montantReussites;
        private Dictionary<uint, double> _frais;

        public int NbrComptes { get => _nbrComptes; set => _nbrComptes = value; }
        public int NbrTransactions { get => _nbrTransactions; set => _nbrTransactions = value; }
        public int NbrReussites { get => _nbrReussites; set => _nbrReussites = value; }
        public int NbrEchecs { get => _nbrEchecs; set => _nbrEchecs = value; }
        public double MontantReussites { get => _montantReussites; set => _montantReussites = value; }
        public Dictionary<uint, double> Frais { get => _frais; set => _frais = value; }


        public Metrologie(Banque banque, Statuts statutTransactions)
        {
            this._nbrComptes = CountAllComptes(banque);
            this._nbrTransactions = CountAllTransactions(statutTransactions);
            this._nbrReussites = CountAllTransactionsReussites(statutTransactions);
            this._nbrEchecs = CountAllTransactionsEchecs(statutTransactions);
            this._montantReussites = SumTotMontantReussites(statutTransactions);
            this._frais = FraisGestionPerGestionnaire(banque);
        }

        public int CountAllComptes(Banque banque)
        {
            return banque.Gestionnaires.Select(g => g.Comptes.Count).Sum();
        }

        public int CountAllTransactions(Statuts statutTransactions)
        {
            return statutTransactions.Demandes.Count();
        }

        public int CountAllTransactionsReussites(Statuts statutTransactions)
        {
            return statutTransactions.Demandes.Count(d => d.Value == true);
        }

        public int CountAllTransactionsEchecs(Statuts statutTransactions)
        {
            return statutTransactions.Demandes.Count(d => d.Value == false);
        }

        public double SumTotMontantReussites(Statuts statutTransactions)
        {
            List<KeyValuePair<string, bool>> statutReussites = statutTransactions.Demandes.Where(d => d.Value == true).ToList();
            // On recupère et somme tous les montants
            double sum = 0;
            double mtt;
            foreach(KeyValuePair<string, bool> s in statutReussites)
            {
                double.TryParse(s.Key.Split(';')[2], out mtt);
                sum += mtt;
            }
            return sum;
        }

        public Dictionary<uint, double> FraisGestionPerGestionnaire(Banque banque)
        {
            Dictionary<uint, double> result = new Dictionary<uint, double> { };
            List<uint> idGestionnaires = banque.Gestionnaires.Select(g => g.Id).ToList();

            for (int i = 0; i < idGestionnaires.Count(); i++)
            { 
                 result.Add(idGestionnaires[i], banque.GestionnaireFromIdGestionnaire(idGestionnaires[i]).FraisTot);
            }
            return result;
        }

    }


}
    



