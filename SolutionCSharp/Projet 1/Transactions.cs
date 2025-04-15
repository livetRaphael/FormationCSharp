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
        public CompteBancaire _compteSrc;
        public CompteBancaire _compteDst;

        

        public Transaction(uint id, double montant, CompteBancaire compteSrc, CompteBancaire compteDst)
        {
            this._id = id;
            this._montant = montant;
            this._compteSrc = compteSrc;
            this._compteDst = compteDst;
        }


    }

}
