using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banques;
using FileControls;


namespace Traitements
{

    class Traitement
    {
        Banque _banque;

        public Traitement(Banque banque)
        {
            this._banque = banque;
        }

        public void Gestionnaires(string urlReader)
        {
            Lecture readerGestionnaires = new Lecture(urlReader);
            string[] splitLigne = new string[] { };


            while (readerGestionnaires.LireLigne(ref splitLigne))
            {
                try
                {
                    uint idTransaction = uint.Parse(splitLigne[0]);
                    string type = splitLigne[1];
                    int nbrTransactionMaxRetrait = int.Parse(splitLigne[2]);

                    this._banque.CreationGestionnaire(idTransaction, type, nbrTransactionMaxRetrait);
                }
                catch
                {

                }
            }
            
            readerGestionnaires.DisposeAndClose();
            

        }


        public void Comptes(string urlReader, string urlWriter)
        {
            Lecture readerComptes = new Lecture(urlReader);
            EcritureFile writerComptes = new EcritureFile(urlWriter);
            EcritureConsole writerConsole = new EcritureConsole();

            string[] splitLigne = new string[] { };
            string ligne = "";
            Statuts statutComptes = new Statuts();

            while (readerComptes.LireLigne(ref splitLigne))
            {
                try
                {
                    ligne = string.Join(";", splitLigne);

                    if (splitLigne.Count() != 5) { throw new Exception($"ERREUR : Nombre d'élément d'opération sur compte incorrect. Il faut 4 éléments et il y en a {splitLigne.Count()}"); }

                    uint idCompte = 0;
                    if (!uint.TryParse(splitLigne[0], out idCompte)) { throw new Exception("ERREUR : Formatage id compte"); }

                    DateTime date = new DateTime();
                    if (!DateTime.TryParse(splitLigne[1], out date)) { throw new Exception("ERREUR : Formatage date compte"); }

                    double solde = 0;
                    if (!double.TryParse(splitLigne[2], out solde))
                    {
                        if (splitLigne[2] != "") { throw new Exception("ERREUR : Formatage solde initial compte"); }
                    }

                    uint idEntree = 0;
                    if (!uint.TryParse(splitLigne[3], out idEntree))
                    {
                        if (splitLigne[3] != "") { throw new Exception("ERREUR : Formatage id gestionnaire entrée"); }
                    }

                    uint idSortie = 0;
                    if (!uint.TryParse(splitLigne[4], out idSortie))
                    {
                        if (splitLigne[4] != "") { throw new Exception("ERREUR : Formatage id gestionnaire sortie"); }
                    }

                    // CAS : CREATION DE COMPTE
                    if (idSortie == 0)
                    {
                        if (!this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idEntree)) { throw new Exception("ERREUR : Id gestionnaire n'existe pas"); }

                        this._banque.GestionnaireFromIdGestionnaire(idEntree).CreationCompte(idCompte, date, solde);
                        statutComptes.Demandes.Add(ligne, true);
                    }
                    // CAS : CLOTURE DE COMPTE
                    else if (idEntree == 0)
                    {
                        if (!this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idSortie)) { throw new Exception("ERREUR : Id gestionnaire n'existe pas"); }
                        this._banque.GestionnaireFromIdGestionnaire(idSortie).ClotureCompte(idCompte, date);
                        statutComptes.Demandes.Add(ligne, true);
                    }
                    // CAS : ECHANGE DE COMPTE
                    else
                    {
                        if (!(this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idEntree) && this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idSortie))) { throw new Exception("ERREUR : Id gestionnaire du receveur ou de l'emetteur n'existe pas"); }
                        
                        this._banque.GestionnaireFromIdGestionnaire(idEntree).CessionCompte(idCompte, date, this._banque.GestionnaireFromIdGestionnaire(idSortie));
                        statutComptes.Demandes.Add(ligne, true);
                    }
                }
                catch
                {
                    statutComptes.Demandes.Add(ligne, false);
                }


            }

            writerComptes.WriteAllStatutsResults(statutComptes);
            writerConsole.WriteAllStatutsResults(statutComptes);
            readerComptes.DisposeAndClose();
            writerComptes.DisposeAndClose();
        } 
            
        


        public void Transactions(string urlReader, string urlWriter, string urlMetrologie)
        {
            Lecture readerTransactions = new Lecture(urlReader);
            EcritureFile writerTransactions = new EcritureFile(urlWriter);
            EcritureFile writerMetrologie = new EcritureFile(urlMetrologie);
            EcritureConsole writerConsole = new EcritureConsole();

            string[] splitLigne = new string[] { };
            string ligne = "";
            Statuts statutTransactions = new Statuts();

            
            while (readerTransactions.LireLigne(ref splitLigne))
            {
                try
                {
                    ligne = string.Join(";", splitLigne);

                    if (splitLigne.Count() != 5) { throw new Exception($"ERREUR : Nombre d'élément de transaction incorrect. Il faut 5 éléments et il y en a {splitLigne.Count()}"); }


                    uint idTransaction = 0;
                    if (!uint.TryParse(splitLigne[0], out idTransaction)) { throw new Exception("ERREUR : Formatage id transaction incorrect"); }

                    DateTime date = new DateTime();
                    if (!DateTime.TryParse(splitLigne[1], out date)) { throw new Exception("ERREUR : Formatage date transaction incorrect"); }

                    double montant = 0;
                    if (!double.TryParse(splitLigne[2], out montant)) { throw new Exception("ERREUR : Formatage montant transaction incorrect"); }

                    uint idCompteSrc = 0;
                    if (!uint.TryParse(splitLigne[3], out idCompteSrc)) { throw new Exception("ERREUR : Formatage id compte source transaction incorrect"); }

                    uint idCompteDst = 0;
                    if (!uint.TryParse(splitLigne[4], out idCompteDst)) { throw new Exception("ERREUR : Formatage id compte destination transaction incorrect"); }

                    // Cas DEPOT
                    if (idCompteSrc == 0)
                    {
                        if (!this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteDst)) { throw new Exception("ERREUR : Id compte destinataire n'existe pas ou n'est associé à aucun gestionnaire"); }
                        
                        this._banque.GestionnaireFromIdCompte(idCompteDst).CompteFromIdCompte(idCompteDst).DepotArgent(idTransaction, date, montant);
                        statutTransactions.Demandes.Add(ligne, true);
                    }

                    // Cas RETRAIT
                    else if (idCompteDst == 0)
                    {
                        if (!this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteSrc)) { throw new Exception("ERREUR : Id compte source n'existe pas ou n'est associé à aucun gestionnaire"); }

                        this._banque.GestionnaireFromIdCompte(idCompteSrc).CompteFromIdCompte(idCompteSrc).RetirerArgent(idTransaction, date, montant);
                        statutTransactions.Demandes.Add(ligne, true);
                    }

                    // Cas PRELEVEMENT/VIREMENT
                    else
                    {
                        if (!(this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteSrc) && this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteDst))) { throw new Exception("ERREUR : Id compte source ou id compte destinataire n'existe pas ou n'est associé à aucun gestionnaire"); }
                        
                        this._banque.GestionnaireFromIdCompte(idCompteDst).CompteFromIdCompte(idCompteDst).Prelevement(idTransaction, date, montant, this._banque.GestionnaireFromIdCompte(idCompteSrc).CompteFromIdCompte(idCompteSrc));
                        statutTransactions.Demandes.Add(ligne, true);
                    }
                }
                catch (Exception e)
                {
                    statutTransactions.Demandes.Add(ligne, false);
                    Console.WriteLine(e);
                }
            }
            
            
            writerTransactions.WriteAllStatutsResults(statutTransactions);
            writerConsole.WriteAllStatutsResults(statutTransactions);

            Metrologie metro = new Metrologie(this._banque, statutTransactions);
            writerMetrologie.WriteTransactionsMetrologie(metro);
            writerConsole.WriteTransactionsMetrologie(metro);

            readerTransactions.DisposeAndClose();
            writerTransactions.DisposeAndClose();
            writerMetrologie.DisposeAndClose();
            


}

        
    }

}



