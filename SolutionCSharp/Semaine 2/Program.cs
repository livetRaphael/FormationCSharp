using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semaine_2
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class CompteBancaire
    {
        public uint _id;
        public double _solde;
        public double _maxRetrait;
        public List<Transaction> histo;

        CompteBancaire(uint id)
        {
            this._id = id;
            this._solde = 0;
            this._maxRetrait = 1000;
            this.histo = new List<Transaction> { };
        }

        private bool IsMaxRetraitReached(double montant)
        {
            double sumPastTransactions = 0;

            List<Transaction> lastTenTransactions = this.histo.Where(t => t._compteSrc._id == this._id).Reverse().Take(10).ToList();

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
            if (!IsDepotArgentValid(montant))
            {
                return false;
            }

            this._solde += montant;
            this.histo.Add(new Transaction(id, montant, 0, this._id));
            return true;
        }
   
        public bool RetirerArgent(uint id, double montant)
        {
            if (!IsRetirerArgentValid(montant))
            {
                return false;
            }

            this._solde -= montant;
            this.histo.Add(new Transaction(id, montant, this._id, 0));
            return true;
        }

        public bool Virement(uint id, double montant, uint idCompteDst)
        {
            if (!IsRetirerArgentValid(montant))
            {
                return false;
            }
            





            return true; ;
        }
    
    
    
    
    
    }

    class Transaction
    {
        public uint _id;
        public double _montant;
        public uint _idCompteSrc;
        public uint _idCompteDst;

        

        public Transaction(uint id, double montant, uint idCompteSrc, uint idCompteDst)
        {
            this._id = id;
            this._montant = montant;
            this._idCompteSrc = idCompteSrc;
            this._idCompteDst = idCompteDst;
        }


    }

}
