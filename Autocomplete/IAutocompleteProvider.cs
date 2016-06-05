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
    /// I wouldn't normally create an interface here because of the size/scope of the project, but it was implied 
    /// to do so in the exercise instructions
    /// </summary>
    public interface IAutocompleteProvider
    {
        /// <summary>
        /// Returns a list of autocomplete candidates and their associated likelihoods from the provided string fragment
        /// </summary>
        /// <param name="fragment"></param>
        /// <returns></returns>
        List<Candidate> GetSuggestions(string fragment);

        /// <summary>
        /// Add data to the autocomplete system.  Accepts single words and long strings.
        /// </summary>
        /// <param name="passage"></param>
        void Train(string passage);
    }
}
