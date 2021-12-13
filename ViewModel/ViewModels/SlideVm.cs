using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class SlideVm
    {
        public int Id { get; set; }

        public string SlideName { get; set; }

        public string Alias { get; set; }

        public bool IsShow { get; set; }

        public DateTime DateUp { get; set; }
        public string FromFile { get; set; }
    }
}
