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
    /// Implements IAutocompleteProvider, providing logic for the autocomplete system.  Underlying data structure
    /// is a tree - specifically a Trie, where the leaves contain the relevant data and the nodes just provide
    /// a pathway to get there, unless a node is both a word and a fragment.  
    /// </summary>
    public class AutocompleteProvider : IAutocompleteProvider
    {
        //Each letter will get it's own root node
        private List<TrieNode> _rootNodes;

        public AutocompleteProvider()
        {
            _rootNodes = new List<TrieNode>();

            //Generate the root node list for each letter of the alphabet 
            for (char c = 'a'; c <= 'z'; c++)
            {
                _rootNodes.Add(new TrieNode(c.ToString()));
            }
        }

        /// <summary>
        /// Returns a list of autocomplete candidates and their associated likelihoods from the provided string fragment
        /// </summary>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public List<Candidate> GetSuggestions(string fragment)
        {
            if (string.IsNullOrEmpty(fragment))
                return new List<Candidate>();

            //Remove punctuation using the char.IsPunctuation built-in method
            string stripped = new string(fragment.ToCharArray().Where(p => char.IsPunctuation(p) == false).ToArray());

            //Trim and knock down to lower-case
            string cleanedUpFragment = CleanupWord(fragment);

            //Find the appropriate root node
            var rootNode = _rootNodes.Single(p => p.Fragment.Equals(cleanedUpFragment[0].ToString())); 
            
            //Finally, query the root node for suggestions           
            List<Candidate> suggestions = rootNode.FindCandidates(cleanedUpFragment);

            //Order suggestions alphabetically and then high to low
            suggestions = suggestions.OrderBy(p => p.Suggestion).OrderByDescending(p => p.Likelihood).ToList();


            /*
            //Normalize list for probabilities (probably optional but might be easier to work with in a production implementation) 
            int maxLikelihood = suggestions.Sum(p => p.Likelihood);
            foreach (var cand in suggestions)
            {
                cand.Probability = (double)Math.Round((double)cand.Likelihood / (double)maxLikelihood, 5);
            }*/
                        
            return suggestions;
        }


        /// <summary>
        /// Add data to the autocomplete system.  Accepts single words and long strings.
        /// </summary>
        /// <param name="passage"></param>
        public void Train(string passage)
        {
            //Remove puncutation using the char.IsPunctuation built-in method
            string stripped = new string(passage.ToCharArray().Where(p => char.IsPunctuation(p) == false).ToArray());

            //Break passage apart into constiutent words
            List<string> passageWords = stripped.Split(' ').ToList();

            //Build the appropriate nodes for each word
            foreach(var word in passageWords)
            {
                if (string.IsNullOrEmpty(word))
                    continue;

                string cleanedUpWord = CleanupWord(word);

                //Find the appropriate node starting point
                var rootNode = _rootNodes.Single(p => p.Fragment.Equals(cleanedUpWord[0].ToString()));

                //Finally, construct the tree for the specified word
                rootNode.Train(cleanedUpWord);
            }
        }

       
      
        /// <summary>
        /// Returns a trimmed, lower-case word
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private string CleanupWord(string word)
        {
            return word.Trim().ToLower();
        }
    }
}
