using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CondoEntry.Models
{
    public class Visitor
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("First name")]       
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last name")]        
        public string LastName { get; set; }

        [Required]
        [DisplayName("Phone number")]        
        public string PhoneNumber { get; set; }
        
        [DisplayName("Time of entry")]
        public DateTime? TimeOfEntry { get; set; }

        [DisplayName("Time of exit")]
        public DateTime? TimeOfExit { get; set; }

        [Required]
        [DisplayName("Unit number")]        
        public int UnitNumber { get; set; }

        public Parking Parking { get; set; }

        public int ParkingId { get; set; }
    }
}
