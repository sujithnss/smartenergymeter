using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartEnergyMeter.Models
{
    public class TariffViewModels
    {
        [Required]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Display(Name = "TOD")]
        public float TOD { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Display(Name = "Normal")]
        public float Normal { get; set; }
    }

    public class SmartEnergyMeterViewModels
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

      
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerId {get;set;}

    
        [Display(Name = "TariffType")]
        public string TariffType { get; set; }
    }
}