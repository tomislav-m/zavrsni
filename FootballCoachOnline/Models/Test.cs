using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballCoachOnline.Models
{
    public partial class Test
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }

        [Display(Name = "Test")]
        [Required(ErrorMessage = "Unos imena je obavezan")]
        [StringLength(30, ErrorMessage = "Dužina imena smije biti najviše 30 znakova")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        [Required(ErrorMessage = "Unos opisa je obavezan")]
        public string Description { get; set; }

        [Display(Name = "Datum")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual Player Player { get; set; }
    }
}
