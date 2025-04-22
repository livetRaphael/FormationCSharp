using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comptes;

namespace Gestionnaires
{


    public class Gestionnaire
    {
        private uint _id;
        private string _type;
        private int _nbrTransactions { get; set; }
        private double _fraisTot { get; set; }
        private List<Compte> _comptes { get; set; }


        public uint Id { get => _id; set => _id = value; }
        public string Type { get => _type; set => _type = value; }
        public int NbrTransaction { get => _nbrTransactions; set => _nbrTransactions = value; }
        public double FraisTot { get => _fraisTot; set => _fraisTot = value; }
        public List<Compte> Comptes { get => _comptes; set => _comptes = value; }


        public Gestionnaire(uint id, string type, int nbrTransactions)
        {
            this._id = id;
            this._type = type;
            this._nbrTransactions = nbrTransactions;
            this._fraisTot = 0;
            this._comptes = new List<Compte> { };
        }


        public bool IsCompteAlreadyExist(uint idCompte)
        {
            return (this._comptes.Exists(cpt => cpt.Id == idCompte));
        }

        public bool IsCompteActif(uint idCompte, DateTime date)
        {
            DateTime? dateFin = this._comptes.Find(cpt => cpt.Id == idCompte).DateFin;
            return ( dateFin == null || dateFin > date);
        }


        public void CreationCompte(uint id, DateTime dateDebut, double solde)
        {
            if (this.IsCompteAlreadyExist(id))
            {
                throw new Exception("ERREUR : Compte déjà existant");
            }

            this._comptes.Add(new Compte(id, dateDebut, solde, this));
        }

        public void ClotureCompte(uint idCompte, DateTime dateFin)
        {
            if (!this.IsCompteAlreadyExist(idCompte))
            {
                throw new Exception("ERREUR : Compte non existant");
            }
            if (!this.IsCompteActif(idCompte, dateFin))
            {
                throw new Exception("ERREUR : Compte non actif");
            }
            this.CompteFromIdCompte(idCompte).DateFin = dateFin;

        }

        public void CessionCompte(uint idCompte, DateTime date, Gestionnaire gestionnaireCible)
        {
            if (!this.IsCompteAlreadyExist(idCompte))
            {
                throw new Exception("ERREUR : Compte non existant chez le gestionnaire emetteur");
            }
            if (!this.IsCompteActif(idCompte, date))
            {
                throw new Exception("ERREUR : Compte non actif");
            }

            gestionnaireCible.ReceptionCompte(idCompte, date, this);
            this.Comptes.Remove(this.CompteFromIdCompte(idCompte));

        }

        public void ReceptionCompte(uint idCompte, DateTime date, Gestionnaire gestionnaireEmetteur)
        {
            if (this.IsCompteAlreadyExist(idCompte))
            {
                throw new Exception("ERREUR : Compte déjà existant chez le gestionnaire receveur");
            }

            gestionnaireEmetteur.CompteFromIdCompte(idCompte).Gestionnaire = this;
            this.Comptes.Add(gestionnaireEmetteur.CompteFromIdCompte(idCompte));
            
        }

        public Compte CompteFromIdCompte(uint idCompte)
        {
            return this._comptes.Find(cpt => cpt.Id == idCompte);
        }

    }
}
