using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompteBancaires;


namespace Banques
{
    class Banque
    {

        public List<CompteBancaire> Comptes = new List<CompteBancaire> { };
        public bool IsCompteAlreadyExist(uint id)
        {
            return (this.Comptes.Where(cpt => cpt._id == id).Count() > 0);
        }

        public bool IsCompteValid(double solde)
        {
            return (solde >= 0);
        }

        public bool IsTransactionAlreadyExist(uint id)
        {
            return (this.Comptes.Where(cpt => cpt._histo.Where(trans => trans._id == id).Count() > 0).Count() > 0);
        }

        public bool CreationCompte(uint idCompte, double solde)
        {
            if (IsCompteAlreadyExist(idCompte))
            {
                return false;
            }
            if (!IsCompteValid(solde))
            {
                return false;
            }

            this.Comptes.Add(new CompteBancaire(idCompte, solde));
            return true;
        }

        

        public CompteBancaire CompteFromIdCompte(uint id)
        {

            return this.Comptes.Where(cpt => cpt._id == id).ToList()[0];
        }
    }


}
