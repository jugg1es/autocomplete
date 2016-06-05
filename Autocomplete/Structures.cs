/*
    Peter Roca

    Autocomplete Exercise
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocomplete
{
    
    /// <summary>
    /// Basic structure representing an autocomplete candidate.  
    /// </summary>
   public class Candidate
    {
        public string Suggestion { get; set; }    
        public double? Probability { get; set; }
        public int Likelihood { get; set; }
    }
   

}
