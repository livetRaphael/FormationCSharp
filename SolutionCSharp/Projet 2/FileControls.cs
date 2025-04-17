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

        public void WriteLineStatutTransaction(string demande, bool statut)
        {
            string labelStatut = statut ? "OK" : "KO";
            this._writer.WriteLine($"{demande};{labelStatut}");
        }

        public void WriteLineStatutCompte(string demande, bool statut)
        {
            string labelStatut = statut ? "OK" : "KO";
            this._writer.WriteLine($"{demande};{labelStatut}");
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


        public void WriteLineTransaction(string demande, bool statut)
        {
            string labelStatut = statut ? "OK" : "KO";

            Console.WriteLine($"{demande};{labelStatut}");
        }

        public void WriteLineCompte(string demande, bool statut)
        {
            string labelStatut = statut ? "OK" : "KO";
            
            Console.WriteLine($"{demande};{labelStatut}");
        }
    }



}
    



