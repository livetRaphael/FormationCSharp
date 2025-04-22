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

            try
            {
                while (readerGestionnaires.LireLigne(ref splitLigne))
                {
                    uint idTransaction = uint.Parse(splitLigne[0]);
                    string type = splitLigne[1];
                    int nbrTransactionMaxRetrait = int.Parse(splitLigne[2]);

                    this._banque.CreationGestionnaire(idTransaction, type, nbrTransactionMaxRetrait);
                }
            }
            finally
            {
                readerGestionnaires.DisposeAndClose();
            }

        }


        public void Comptes(string urlReader, string urlWriter)
        {
            Lecture readerComptes = new Lecture(urlReader);
            EcritureFile writerComptes = new EcritureFile(urlWriter);
            EcritureConsole writerConsole = new EcritureConsole();

            string[] splitLigne = new string[] { };
            Statuts statutComptes = new Statuts();

            try
            {
                while (readerComptes.LireLigne(ref splitLigne))
                {
                    string ligne = string.Join(";", splitLigne);

                    uint idCompte = uint.Parse(splitLigne[0]);
                    DateTime date = readerComptes.StringToDateTime(splitLigne[1]);
                    double solde = (splitLigne[2] != "") ? double.Parse(splitLigne[2], CultureInfo.InvariantCulture) : 0;
                    uint idEntree = (splitLigne[3] != "") ? uint.Parse(splitLigne[3]) : 0;
                    uint idSortie = (splitLigne[4] != "") ? uint.Parse(splitLigne[4]) : 0;


                    // CAS : CREATION DE COMPTE
                    if (idSortie == 0)
                    {
                        statutComptes.Demandes.Add(ligne, (this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idEntree)) ? this._banque.GestionnaireFromIdGestionnaire(idEntree).CreationCompte(idCompte, date, solde) : false);
                    }
                    // CAS : CLOTURE DE COMPTE
                    else if (idEntree == 0)
                    {
                        statutComptes.Demandes.Add(ligne, (this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idSortie)) ? this._banque.GestionnaireFromIdGestionnaire(idSortie).ClotureCompte(idCompte, date) : false);
                    }
                    // CAS : ECHANGE DE COMPTE
                    else
                    {
                        statutComptes.Demandes.Add(ligne, (this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idEntree) && this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idSortie)) ? this._banque.GestionnaireFromIdGestionnaire(idEntree).CessionCompte(idCompte, date, this._banque.GestionnaireFromIdGestionnaire(idSortie)) : false);
                    }
                }

                writerComptes.WriteAllStatutsResults(statutComptes);
                writerConsole.WriteAllStatutsResults(statutComptes);
            }
            finally
            {
                readerComptes.DisposeAndClose();
                writerComptes.DisposeAndClose();
            }
        }


        public void Transactions(string urlReader, string urlWriter)
        {
            Lecture readerTransactions = new Lecture(urlReader);
            EcritureFile writerTransactions = new EcritureFile(urlWriter);
            EcritureConsole writerConsole = new EcritureConsole();

            string[] splitLigne = new string[] { };
            Statuts statutTransactions = new Statuts();

            try
            {
                while (readerTransactions.LireLigne(ref splitLigne))
            {
                string ligne = string.Join(";", splitLigne);

                uint idTransaction = uint.Parse(splitLigne[0]);
                DateTime date = readerTransactions.StringToDateTime(splitLigne[1]);
                double montant = double.Parse(splitLigne[2], CultureInfo.InvariantCulture);
                uint idCompteSrc = uint.Parse(splitLigne[3]);
                uint idCompteDst = uint.Parse(splitLigne[4]);

                
                // Cas DEPOT
                if (idCompteSrc == 0)
                {
                    statutTransactions.Demandes.Add(ligne, (this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteDst)) ? this._banque.GestionnaireFromIdCompte(idCompteDst).CompteFromIdCompte(idCompteDst).DepotArgent(idTransaction, date, montant) : false);
                }
                // Cas RETRAIT
                else if (idCompteDst == 0)
                {
                    statutTransactions.Demandes.Add(ligne, (this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteSrc)) ? this._banque.GestionnaireFromIdCompte(idCompteSrc).CompteFromIdCompte(idCompteSrc).RetirerArgent(idTransaction, date, montant) : false);
                }
                // Cas PRELEVEMENT/VIREMENT
                else
                {
                    statutTransactions.Demandes.Add(ligne, (this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteSrc) && this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteDst)) ? this._banque.GestionnaireFromIdCompte(idCompteDst).CompteFromIdCompte(idCompteDst).Prelevement(idTransaction, date, montant, this._banque.GestionnaireFromIdCompte(idCompteSrc).CompteFromIdCompte(idCompteSrc)) : false);
                }
            }

                writerTransactions.WriteAllStatutsResults(statutTransactions);
                writerConsole.WriteAllStatutsResults(statutTransactions);

                Metrologie metro = new Metrologie(this._banque, statutTransactions);
                writerConsole.WriteMetrologie(metro);
            }
            finally
            {
                readerTransactions.DisposeAndClose();
                writerTransactions.DisposeAndClose();
            }


}

        
    }

}



