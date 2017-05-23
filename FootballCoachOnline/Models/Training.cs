using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballCoachOnline.Models
{
    public partial class Training
    {
        public int Id { get; set; }
        [Display(Name = "Tim")]
        public int TeamId { get; set; }

        [Display(Name = "Datum"), DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy. HH:mm}")]
        public DateTime Date { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Tim")]
        public virtual Team Team { get; set; }
    }
}
