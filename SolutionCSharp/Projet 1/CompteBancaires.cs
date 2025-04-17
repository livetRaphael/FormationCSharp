using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;

namespace CompteBancaires
{


    public class CompteBancaire
    {
        public uint _id;
        public double _solde;
        public double _maxRetrait;
        public List<Transaction> _histo;

        public CompteBancaire(uint id, double solde)
        {
            this._id = id;
            this._solde = solde;
            this._maxRetrait = 1000;
            this._histo = new List<Transaction> { };
        }

        private bool IsMaxRetraitReached(double montant)
        {
            double sumPastTransactions = 0;

            List<Transaction> lastTenTransactions = this._histo.Where(t => t._idCompteSrc == this._id).Reverse().Take(10).ToList();

            foreach (Transaction transaction in lastTenTransactions)
            {
                sumPastTransactions += transaction._montant;
            }
            
            return (montant + sumPastTransactions > this._maxRetrait);
        }

        private bool IsDepotArgentValid(double montant)
        {
            return (montant >= 0);
        }
        private bool IsRetirerArgentValid(double montant)
        {
            return (montant > 0 && this._solde >= montant && !IsMaxRetraitReached(montant));
        }

        public bool DepotArgent(uint id, double montant)
        {
            if (!this.IsDepotArgentValid(montant))
            {
                return false;
            }

            this._solde += montant;
            this._histo.Add(new Transaction(id, montant, 0, this._id));
            return true;
        }

        public bool RetirerArgent(uint id, double montant)
        {
            if (!this.IsRetirerArgentValid(montant))
            {
                return false;
            }

            this._solde -= montant;
            this._histo.Add(new Transaction(id, montant, this._id, 0));
            return true;
        }

        public bool Virement(uint id, double montant, CompteBancaire compteDst)
        {
            
            if (!this.IsRetirerArgentValid(montant))
            {
                return false;
            }
            
            this._solde -= montant;
            this._histo.Add(new Transaction(id, montant, this._id, compteDst._id));

            compteDst._solde += montant;
            compteDst._histo.Add(new Transaction(id, montant, this._id, compteDst._id));

            return true; ;
        }

        public bool Prelevement(uint id, double montant, CompteBancaire compteSrc)
        {
            if (!this.IsDepotArgentValid(montant))
            {
                return false;
            }
            
            if (!compteSrc.Virement(id, montant, this))
            {
                return false;
            }
            return true;
        }

    }
    }
