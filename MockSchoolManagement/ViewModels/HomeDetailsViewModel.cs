using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.Models;

namespace MockSchoolManagement.ViewModels
{
    public class HomeDetailsViewModel
    {
        public string PageTitle { get; set; }

        public Student Student { get; set; }
    }
}
