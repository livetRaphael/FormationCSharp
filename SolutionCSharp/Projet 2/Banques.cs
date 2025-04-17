using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestionnaires;




namespace Banques
{
    class Banque
    {


        public List<Gestionnaire> _gestionnaires = new List<Gestionnaire> { };

        public bool IsGestionnaireAlreadyExistFromIdGestionnaire(uint idGestionnaire)
        {
            return this._gestionnaires.Exists(g => g.Id == idGestionnaire);
        }

        public bool IsGestionnaireAlreadyExistFromIdCompte(uint idCompte)
        {
            return this._gestionnaires.Exists(g => g.Comptes.Exists(cpt => cpt.Id == idCompte));
        }


        public bool CreationGestionnaire(uint idGestionnaire, string type, int nbrTransactionsMaxRetrait)
        {
            if (this.IsGestionnaireAlreadyExistFromIdGestionnaire(idGestionnaire))
            {
                return false;
            }
            this._gestionnaires.Add(new Gestionnaire(idGestionnaire, type, nbrTransactionsMaxRetrait));
            return true;
        }

        public Gestionnaire GestionnaireFromIdGestionnaire(uint idGestionnaire)
        {
            if (!IsGestionnaireAlreadyExistFromIdGestionnaire(idGestionnaire))
            {
                throw new Exception("Gestionnaire non existant !");
            }
            return this._gestionnaires.Find(g => g.Id == idGestionnaire);
        }

        public Gestionnaire GestionnaireFromIdCompte(uint idCompte)
        {

            if (!IsGestionnaireAlreadyExistFromIdCompte(idCompte))
            {
                throw new Exception("Gestionnaire non existant !");
            }
            return this._gestionnaires.Find(g => g.Comptes.Exists(cpt => cpt.Id == idCompte));
        }


    }



    }



