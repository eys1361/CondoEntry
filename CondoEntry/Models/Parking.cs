using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondoEntry.Models
{
    public class Parking
    {
        public int ParkingId { get; set; }
         
        public bool IsOccupied { get; set; }
        
        public Visitor Visitor { get; set; }
    }
}
