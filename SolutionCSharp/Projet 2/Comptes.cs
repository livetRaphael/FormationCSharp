using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;
using Gestionnaires;

namespace Comptes
{


    public class Compte
    {
        private uint _id;
        private double _solde;
        private double _maxRetrait;
        private Gestionnaire _gestionnaire;
        private DateTime _dateDebut;
        private DateTime? _dateFin;
        private List<Transaction> _histo;

        public uint Id { get => _id; set => _id = value; }
        public double Solde { get => _solde; set => _solde = value; }
        public double MaxRetrait { get => _maxRetrait; set => _maxRetrait = value; }
        public Gestionnaire Gestionnaire { get => _gestionnaire; set => _gestionnaire = value; }
        public DateTime DateDebut { get => _dateDebut; set => _dateDebut = value; }
        public DateTime? DateFin { get => _dateFin; set => _dateFin = value; }
        public List<Transaction> Histo { get => _histo; set => _histo = value; }



        public Compte(uint id, DateTime dateDebut, double solde, Gestionnaire gestionnaire)
        {
            this._id = id;
            this._dateDebut = dateDebut;
            this._solde = solde;
            this._gestionnaire = gestionnaire;
            this._dateFin = null;
            this._maxRetrait = 1000;
            this._histo = new List<Transaction> { };
        }

        private bool IsMaxRetraitReached(double montant, DateTime date)
        {
            double sumTransactions = 0;

            List<Transaction> NbrTransactions = this._histo.Where(t => t.IdCompteSrc == this._id).Reverse().Take(this._gestionnaire.NbrTransaction).ToList();
            foreach (Transaction transaction in NbrTransactions)
            {
                sumTransactions += transaction.Montant;
            }
            if ((montant + sumTransactions > this._maxRetrait))
            {
                return true;
            }

            sumTransactions = 0;
            List<Transaction> TimeTransactions = this._histo.Where(t => t.IdCompteSrc == this._id && t.Date <= date && t.Date >= date - TimeSpan.FromDays(7)).ToList();
            foreach (Transaction transaction in TimeTransactions)
            {
                sumTransactions += transaction.Montant;
            }
            if ((montant + sumTransactions > 2000))
            {
                return true;
            }

            return false;
        }

        private bool IsDepotArgentValid(double montant, DateTime date)
        {
            return (montant >= 0 && date >= this.DateDebut && (this.DateFin == null || date <= this.DateFin));
        }
        private bool IsRetirerArgentValid(double montant, DateTime date)
        {
            return (montant > 0 && this._solde >= montant && date >= this.DateDebut && (this.DateFin == null || date <= this.DateFin) && !IsMaxRetraitReached(montant, date));
        }


        public bool DepotArgent(uint id, DateTime date, double montant)
        {
            if (!this.IsDepotArgentValid(montant, date))
            {
                return false;
            }

            this._solde += montant;
            this._histo.Add(new Transaction(id, date, montant, 0, 0, this.Id));
            return true;
        }

        public bool RetirerArgent(uint id, DateTime date, double montant)
        {
            if (!this.IsRetirerArgentValid(montant, date))
            {
                return false;
            }

            this._solde -= montant;
            this._histo.Add(new Transaction(id, date, montant, 0, this.Id, 0));
            return true;
        }

        public bool Prelevement(uint id, DateTime date, double montant, Compte compteSrc)
        {
            if (!this.IsDepotArgentValid(montant, date))
            {
                return false;
            }

            if (!compteSrc.Virement(id, date, montant, this))
            {
                return false;
            }
            return true;
        }

        public bool Virement(uint id, DateTime date, double montant, Compte compteDst)
        {
            
            if (!this.IsRetirerArgentValid(montant, date))
            {
                return false;
            }
            
            this._solde -= montant;
            this._histo.Add(new Transaction(id, date, montant, 0, this.Id, compteDst.Id));


            double frais = CalculerFrais(montant, this._gestionnaire, compteDst._gestionnaire);
            compteDst._solde += montant - frais;
            compteDst._histo.Add(new Transaction(id, date, montant, frais, this.Id, compteDst.Id));

            return true; ;
        }

        public double CalculerFrais(double montant, Gestionnaire gestionnaireCompteSrc, Gestionnaire gestionnaireCompteDst)
        {
            double frais = 0;
            if (gestionnaireCompteSrc == gestionnaireCompteDst)
            {
                return frais;
            }
            else switch (gestionnaireCompteSrc.Type)
                {
                    case "Particulier":
                        frais = montant * 0.01;
                        break;
                    case "Entreprise":
                        frais = 10;
                        break;
                    default:
                        throw new Exception("Type de gestionnaire inconnu !");
                }
            return frais;
        }

        

    }
}
