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

        public Lecture(string inputUrl)
        {
            this._fileStream = File.OpenRead(inputUrl);
            this._reader = new StreamReader(this._fileStream);
        }

        public bool LireLigne(ref string[] splitLigne)
        {
            string ligne = "";
            if (!((ligne = this._reader.ReadLine()) != null && ligne != string.Empty))
            {
                return false;
            }

            splitLigne = ligne.Split(';');
            return true;
            ;
        }

        public DateTime StringToDateTime(string date)
        {
            return new DateTime(int.Parse(date.Substring(6, 4)), int.Parse(date.Substring(3, 2)), int.Parse(date.Substring(0, 2)));
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

        public void WriteLineStatutTransaction(string id, bool statut)
        {
            string labelStatut = statut ? "OK" : "KO";
            this._writer.WriteLine($"{id};{labelStatut}");
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
        private Banque _banque;

        public EcritureConsole(Banque banque)
        {
            this._banque = banque;
        }

        public void WriteLineLabelTransaction()
        {
            string labelComptes = "     ";
            for (int i = 0; i < this._banque.Comptes.Count; i++)
            {
                labelComptes += " Solde Compte" + this._banque.Comptes[i]._id + " ";
            }
            Console.WriteLine(labelComptes);
        }

        public void WriteLineSoldeComptes()
        {
            Console.Write("    ");
            Console.WriteLine(this.GetSoldeComptes());
        }

        public void WriteLineTransaction(string id, bool statut)
        {
            string labelStatut = statut ? "OK" : "KO";
            Console.Write($"{id};{labelStatut}");
            Console.WriteLine(this.GetSoldeComptes());
        }

        public string GetSoldeComptes()
        {
            string soldeComptes = "";
            for (int i = 0; i < this._banque.Comptes.Count; i++)
            {
                soldeComptes += $"     {this._banque.Comptes[i]._solde : 0000.00}  ";
            }
            return soldeComptes;
        }

        
    }



}
    



