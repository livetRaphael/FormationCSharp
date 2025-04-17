using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comptes;

namespace Transactions
{
   
    public class Transaction
    {
        private uint _id;
        private double _montant;
        private double _frais;
        private DateTime _date;
        private uint _idCompteSrc;
        private uint _idCompteDst;


        public uint Id { get => _id; set => _id = value; }
        public double Montant { get => _montant; set => _montant = value; }
        public double Frais { get => _frais; set => _frais = value; }
        public DateTime Date { get => _date; set => _date = value; }
        public uint IdCompteSrc { get => _idCompteSrc; set => _idCompteSrc = value; }
        public uint IdCompteDst { get => _idCompteDst; set => _idCompteDst = value; }




        public Transaction(uint id, DateTime date, double montant, double frais, uint idCompteSrc, uint idCompteDst)
        {
            this._id = id;
            this._montant = montant;
            this._frais = frais;
            this._date = date;
            this._idCompteSrc = idCompteSrc;
            this._idCompteDst = idCompteDst;
        }


    }

}
