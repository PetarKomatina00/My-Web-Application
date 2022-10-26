using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models.ViewModels
{
    public class PregledVM
    {
        [Key]
        public int PregledVMID { get; set; }

        public Pregled Pregled { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> listaOdeljenja { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> listaVremena { get; set; }
    }
}
