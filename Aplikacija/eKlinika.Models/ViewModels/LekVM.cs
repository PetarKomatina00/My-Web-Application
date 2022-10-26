using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models.ViewModels
{
    public class LekVM
    {
        public Lek Lek { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ListaPakovanja { get; set; }
    }
}
