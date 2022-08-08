using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopringNftSnapshotter.Models
{
    public class NftHolder
    {
        public string? dateRecieved { get; set; }
        public string? transactionId { get; set; }
        public string? transactionType { get; set; }
        public string? recieverAddress { get; set; }
        public string? fullNftId { get; set; }

        public string? balance { get; set; }


    }
}
