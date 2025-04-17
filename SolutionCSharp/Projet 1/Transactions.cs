using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompteBancaires;

namespace Transactions
{
   
    public class Transaction
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
