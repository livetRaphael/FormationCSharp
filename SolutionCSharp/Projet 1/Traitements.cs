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

        public void Comptes(Lecture lecteurComptes)
        {
            string[] splitLigne = new string[] { };
            while (lecteurComptes.LireLigne(ref splitLigne))
            {
                uint idCompte = uint.Parse(splitLigne[0]);
                double solde = (splitLigne[1] != string.Empty) ? double.Parse(splitLigne[1], CultureInfo.InvariantCulture) : 0;

                this._banque.CreationCompte(idCompte, solde);
            }
        }

        public void Transactions(Lecture lecteurTransactions, EcritureFile writerFile, EcritureConsole writerConsole)
        {
            string[] splitLigne = new string[] { };
            writerConsole.WriteLineLabelTransaction();
            writerConsole.WriteLineSoldeComptes();

            while (lecteurTransactions.LireLigne(ref splitLigne))
            {
                uint idTransaction = uint.Parse(splitLigne[0]);
                double montant = double.Parse(splitLigne[1], CultureInfo.InvariantCulture);
                uint idCompteSrc = uint.Parse(splitLigne[2]);
                uint idCompteDst = uint.Parse(splitLigne[3]);


                bool statut = false;

                // Cas DEPOT
                if (idCompteSrc == 0)
                {
                    // Cas ERREUR
                    if (idCompteDst == 0)
                    {
                        statut = false;
                    }
                    else
                    {
                        if (this._banque.IsCompteAlreadyExist(idCompteDst))
                        {
                            statut = this._banque.CompteFromIdCompte(idCompteDst).DepotArgent(idTransaction, montant);
                        }
                        else
                        {
                            statut = false;
                        }
                    }
                }
                // Cas RETRAIT
                else if (idCompteDst == 0)
                {
                    if (this._banque.IsCompteAlreadyExist(idCompteSrc))
                    {
                        statut = this._banque.CompteFromIdCompte(idCompteSrc).RetirerArgent(idTransaction, montant);
                    }
                    else
                    {
                        statut = false;
                    }
                }
                // Cas PRELEVEMENT/VIREMENT
                else
                {
                    if (this._banque.IsCompteAlreadyExist(idCompteSrc) && this._banque.IsCompteAlreadyExist(idCompteDst))
                    {
                        statut = this._banque.CompteFromIdCompte(idCompteDst).Prelevement(idTransaction, montant, this._banque.CompteFromIdCompte(idCompteSrc));
                    }
                    else
                    {
                        statut = false;
                    }
                }

                writerFile.WriteLineStatutTransaction(splitLigne[0], statut);
                writerConsole.WriteLineTransaction(splitLigne[0], statut);
            }
        }


    }

}



