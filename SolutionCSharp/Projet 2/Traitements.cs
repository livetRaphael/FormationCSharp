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

        public void Gestionnaires(Lecture lecteurGestionnaires)
        {
            string[] splitLigne = new string[] { };
            while (lecteurGestionnaires.LireLigne(ref splitLigne))
            {
                uint idTransaction = uint.Parse(splitLigne[0]);
                string type = splitLigne[1];
                int nbrTransactionMaxRetrait = int.Parse(splitLigne[2]);

                this._banque.CreationGestionnaire(idTransaction, type, nbrTransactionMaxRetrait);
            }
        }


        public void Comptes(Lecture lecteurComptes, EcritureFile writerFile, EcritureConsole writerConsole)
        {
            string[] splitLigne = new string[] { };
            bool statut = false;

            while (lecteurComptes.LireLigne(ref splitLigne))
            {

                uint idCompte = uint.Parse(splitLigne[0]);
                DateTime date = lecteurComptes.StringToDateTime(splitLigne[1]);
                double solde = (splitLigne[2] != "") ? double.Parse(splitLigne[2], CultureInfo.InvariantCulture) : 0;
                uint idEntree = (splitLigne[3] != "") ? uint.Parse(splitLigne[3]) : 0;
                uint idSortie = (splitLigne[4] != "") ? uint.Parse(splitLigne[4]) : 0;


                // CAS : CREATION DE COMPTE
                if (idSortie == 0)
                {
                    statut = (this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idEntree)) ? this._banque.GestionnaireFromIdGestionnaire(idEntree).CreationCompte(idCompte, date, solde) : false;
                }
                // CAS : CLOTURE DE COMPTE
                else if (idEntree == 0)
                {
                    statut = (this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idSortie)) ? this._banque.GestionnaireFromIdGestionnaire(idSortie).ClotureCompte(idCompte, date) : false;
                }
                // CAS : ECHANGE DE COMPTE
                else
                {
                    statut = (this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idEntree) && this._banque.IsGestionnaireAlreadyExistFromIdGestionnaire(idSortie)) ? this._banque.GestionnaireFromIdGestionnaire(idEntree).CessionCompte(idCompte, date, this._banque.GestionnaireFromIdGestionnaire(idSortie)) : false;
                }

                writerFile.WriteLineStatutCompte(string.Join(";", splitLigne), statut);
                writerConsole.WriteLineCompte(string.Join(";", splitLigne), statut);

                
            }
            Console.WriteLine();

        }



        public void Transactions(Lecture lecteurTransactions, EcritureFile writerFile, EcritureConsole writerConsole)
        {
            string[] splitLigne = new string[] { };
            

            while (lecteurTransactions.LireLigne(ref splitLigne))
            {

                uint idTransaction = uint.Parse(splitLigne[0]);
                DateTime date = lecteurTransactions.StringToDateTime(splitLigne[1]);
                double montant = double.Parse(splitLigne[2], CultureInfo.InvariantCulture);
                uint idCompteSrc = uint.Parse(splitLigne[3]);
                uint idCompteDst = uint.Parse(splitLigne[4]);

                bool statut = false;
                // Cas DEPOT
                if (idCompteSrc == 0)
                {
                    statut = (this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteDst)) ? this._banque.GestionnaireFromIdCompte(idCompteDst).CompteFromIdCompte(idCompteDst).DepotArgent(idTransaction, date, montant) : false;
                }
                // Cas RETRAIT
                else if (idCompteDst == 0)
                {
                    statut = (this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteSrc)) ? this._banque.GestionnaireFromIdCompte(idCompteSrc).CompteFromIdCompte(idCompteSrc).RetirerArgent(idTransaction, date, montant) : false;
                }
                // Cas PRELEVEMENT/VIREMENT
                else
                {
                    statut = (this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteSrc) && this._banque.IsGestionnaireAlreadyExistFromIdCompte(idCompteDst)) ? this._banque.GestionnaireFromIdCompte(idCompteDst).CompteFromIdCompte(idCompteDst).Prelevement(idTransaction, date, montant, this._banque.GestionnaireFromIdCompte(idCompteSrc).CompteFromIdCompte(idCompteSrc)) : false;
                }

                writerFile.WriteLineStatutTransaction(string.Join(";", splitLigne), statut);
                writerConsole.WriteLineTransaction(string.Join(";", splitLigne), statut);
            }
        }


    }

}



