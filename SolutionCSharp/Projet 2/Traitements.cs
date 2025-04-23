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


        public void ComptesTransactions(string urlReaderComptes, string urlWriterComptes, string urlReaderTransactions, string urlWriterTransactions, string urlMetrologie)
        {
            EcritureConsole writerConsole = new EcritureConsole();

            Lecture readerComptes = new Lecture(urlReaderComptes);
            EcritureFile writerComptes = new EcritureFile(urlWriterComptes);
            
            Lecture readerTransactions = new Lecture(urlReaderTransactions);
            EcritureFile writerTransactions = new EcritureFile(urlWriterTransactions);
            EcritureFile writerMetrologie = new EcritureFile(urlMetrologie);


            Statuts statutComptes = new Statuts();
            Statuts statutTransactions = new Statuts();

            
            string[] splitLigneCompte = new string[] { };
            string[] splitLigneTransaction = new string[] { };

            readerComptes.LireLigne(ref splitLigneCompte);
            readerTransactions.LireLigne(ref splitLigneTransaction);

            while (!readerComptes.IsOver || !readerTransactions.IsOver)
            {
                try
                {
                    string[] splitLigneTemp = new string[] { };
                    DateTime dateCompte;
                    DateTime dateTransaction;
                    if (!DateTime.TryParse(splitLigneCompte[1], out dateCompte)) { throw new Exception("ERREUR : Formatage date compte"); }
                    if (!DateTime.TryParse(splitLigneTransaction[1], out dateTransaction)) { throw new Exception("ERREUR : Formatage date transaction"); }

                    if (readerComptes.IsOver)
                    {
                        this.Transaction(splitLigneTransaction, statutTransactions);
                        readerTransactions.LireLigne(ref splitLigneTransaction);
                    }
                    else if (readerTransactions.IsOver)
                    {
                        this.Compte(splitLigneCompte, statutComptes);
                        readerComptes.LireLigne(ref splitLigneCompte);
                    }
                    else
                    {
                        if (dateCompte <= dateTransaction)
                        {
                            this.Compte(splitLigneCompte, statutComptes);
                            readerComptes.LireLigne(ref splitLigneCompte);
                        }
                        else
                        {
                            this.Transaction(splitLigneTransaction, statutTransactions);
                            readerTransactions.LireLigne(ref splitLigneTransaction);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (e.Message == "ERREUR : Formatage date compte")
                    {
                        statutComptes.Demandes.Add(string.Join(";", splitLigneCompte), false);
                    }
                    else if (e.Message == "ERREUR : Formatage date transaction")
                    {
                        statutComptes.Demandes.Add(string.Join(";", splitLigneTransaction), false);
                    }
                }

            }
           

            writerComptes.WriteAllStatutsResults(statutComptes);
            writerConsole.WriteAllStatutsResults(statutComptes);
            readerComptes.DisposeAndClose();
            writerComptes.DisposeAndClose();

            writerTransactions.WriteAllStatutsResults(statutTransactions);
            writerConsole.WriteAllStatutsResults(statutTransactions);
            readerTransactions.DisposeAndClose();
            writerTransactions.DisposeAndClose();


            Metrologie metro = new Metrologie(this._banque, statutTransactions);
            writerMetrologie.WriteTransactionsMetrologie(metro);
            writerConsole.WriteTransactionsMetrologie(metro);
            writerMetrologie.DisposeAndClose();


        }


        // TRAITEMENT D'UNE LIGNE D'OPERATION SUR COMPTE
        public void Compte(string[] splitLigne, Statuts statutComptes)
        {
            string ligne = string.Join(";", splitLigne);
            try
            {
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

        // TRAITEMENT D'UNE LIGNE DE TRANSACTION
        public void Transaction(string[] splitLigne, Statuts statutTransactions)
        {
            string ligne = string.Join(";", splitLigne);
            try
            {
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
            catch
            {
                statutTransactions.Demandes.Add(ligne, false);
            }
        }
        


        // BATCH DE TOUS LES OBJETS SANS CONSIDERATION DE DATE
        public void Gestionnaires(string urlReader)
        {
            Lecture readerGestionnaires = new Lecture(urlReader);
            string[] splitLigne = new string[] { };


            while (!readerGestionnaires.IsOver)
            {
                try
                {
                    readerGestionnaires.LireLigne(ref splitLigne);
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
            Statuts statutComptes = new Statuts();

            while (!readerComptes.IsOver)
            {
                readerComptes.LireLigne(ref splitLigne);
                this.Compte(splitLigne, statutComptes);
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
            Statuts statutTransactions = new Statuts();

            
            while (!readerTransactions.IsOver)
            {
                readerTransactions.LireLigne(ref splitLigne);
                this.Transaction(splitLigne, statutTransactions);
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



