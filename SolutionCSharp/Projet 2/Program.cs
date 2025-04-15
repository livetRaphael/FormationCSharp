using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompteBancaires;
using Transactions;


namespace Semaine_2
{
    class Banque
    {

        public List<CompteBancaire> listComptes = new List<CompteBancaire> { };
        

        public static void Main(string[] args)
        {
            Banque banque = new Banque();

            string inputCompte = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet\\inputCompte.txt";
            // Lecture de chaque ligne du fichier entrée des comptes
            using (TextReader readerCompte = new StreamReader(inputCompte))
            {
                string ligne;
                while ((ligne = readerCompte.ReadLine()) != null && ligne != string.Empty)
                {
                    string[] splitLigne = ligne.Split(';');
                    uint id = uint.Parse(splitLigne[0]);
                    

                    if (banque.IsCompteAlreadyExist(id))
                    {
                        continue;
                    }

                    banque.listComptes.Add(new CompteBancaire(id));
                    if (splitLigne[1] != string.Empty)
                    {
                        double solde = double.Parse(splitLigne[1], CultureInfo.InvariantCulture);
                        banque.listComptes[banque.listComptes.Count - 1]._solde = solde;
                    }
                }
            }

            string labelSoldesCompte = "";
            for (int i = 1; i < banque.listComptes.Count + 1; i++)
            {
                labelSoldesCompte += "Solde compte " + i + "  ";
            }
            Console.WriteLine($"Sorties   {labelSoldesCompte}");
            string soldesCompte = "";
            foreach (CompteBancaire cpt in banque.listComptes)
            {
                soldesCompte += "     " + cpt._solde.ToString("0000.00") + "     ";
            }
            Console.WriteLine($"        {soldesCompte}");



            string outputStatut = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet\\outputStatut.txt";
            using (TextWriter writerStatut = new StreamWriter(outputStatut))
            {
                string inputTransaction = "C:\\Users\\Formation\\source\\repos\\livetRaphael\\Projet\\inputTransaction.txt";
                using (TextReader readerTransaction = new StreamReader(inputTransaction))
                {
                    string ligne;
                    // Lecture de chaque ligne du fichier entrée des transactions
                    while ((ligne = readerTransaction.ReadLine()) != null && ligne != string.Empty)
                    {
                        string[] splitLigne = ligne.Split(';');

                        uint id = uint.Parse(splitLigne[0]);
                        double montant = double.Parse(splitLigne[1], CultureInfo.InvariantCulture);
                        uint idCompteSrc = uint.Parse(splitLigne[2]);
                        uint idCompteDst = uint.Parse(splitLigne[3]);

                        if (banque.IsTransactionAlreadyExist(id))
                        {
                            continue;
                        }

                        bool statut = false;
                        // Cas DEPOT
                        if (idCompteSrc == 0)
                        {
                            CompteBancaire compteDst = banque.findCompteFromId(idCompteDst);
                            statut = compteDst.DepotArgent(id, montant);
                        }
                        // Cas RETRAIT
                        else if (idCompteDst == 0)
                        {
                            CompteBancaire compteSrc = banque.findCompteFromId(idCompteSrc);
                            statut = compteSrc.RetirerArgent(id, montant);
                        }
                        // Cas PRELEVEMENT/VIREMENT
                        else
                        {
                            CompteBancaire compteDst = banque.findCompteFromId(idCompteDst);
                            CompteBancaire compteSrc = banque.findCompteFromId(idCompteSrc);
                            
                            statut = compteDst.Prelevement(id, montant, compteSrc);
                        }
                        string labelStatut = statut ? "OK": "KO";
                        writerStatut.WriteLine($"{id};{labelStatut}");

                        soldesCompte = "";
                        foreach (CompteBancaire cpt in banque.listComptes)
                        {
                            soldesCompte += "     " + cpt._solde.ToString("0000.00") + "     ";
                        }

                        Console.WriteLine($" {id};{labelStatut}   {soldesCompte}");

                        
                    }
                }
            }

            Console.ReadKey();
        }

        public bool IsCompteAlreadyExist(uint id)
        {
            return (this.listComptes.Where(cpt => cpt._id == id).Count()>0);
        }

        public bool IsTransactionAlreadyExist(uint id)
        {
            return (this.listComptes.Where(cpt => cpt._histo.Where(trans => trans._id == id).Count() > 0).Count() > 0);
        }


        public CompteBancaire findCompteFromId(uint id)
        {
            return this.listComptes.Where(cpt => cpt._id == id).ToList()[0];
        }
    }


}
