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
    /// This problem is best solved using a tree structure where the nodes are prefixes and the leaves are words
    /// Only the leaves have likelihood values associated with them.  The nodes just contain the string prefixes for all
    /// of the words in their children.  For example:
    /// 
    /// word:  the
    /// 
    /// t (value: 0)
    ///  > th (value: 0)
    ///    > the (value: 1)
    /// 
    /// </summary>
    public class TrieNode
    {
        /// <summary>
        /// Children nodes containing fragements one character longer than the fragment in this node
        /// </summary>
        public List<TrieNode> Children { get; set; }


        /// <summary>
        /// This is either a string fragment or a full word.  It is a full word if the RawLikelihood value is greater than zero
        /// </summary>
        public string Fragment { get; set; }

        /// <summary>
        /// The raw number of times this word has been trained
        /// </summary>
        public int RawLikelihood { get; set; }
        

        public TrieNode(string fragment)
        {
            this.Fragment = fragment;
            this.RawLikelihood = 0;
            this.Children = new List<TrieNode>();
        }

        /// <summary>
        /// Returns all of the words that start with the provided string fragment 
        /// </summary>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public List<Candidate> FindCandidates(string fragment)
        {
            if (fragment.IndexOf(this.Fragment) == -1)
                return new List<Candidate>();

            List<Candidate> candidates = new List<Candidate>();


            //We've found the fragment node 
            if (this.Fragment.Equals(fragment))
            {
                //If RawLikelihood is greater than zero, then it's a word 
                if(this.RawLikelihood > 0)
                {
                    candidates.Add(new Candidate()
                    {
                        Suggestion = this.Fragment,
                        Likelihood = this.RawLikelihood
                    });
                }

                //Now that we've found the fragment node, get all of the words below 
                candidates.AddRange(GetAllChildCandidates(this));
            }
            else 
            { 
                //Scan through the children nodes if the prefix has not yet been found
                foreach (var child in this.Children)
                {
                    candidates.AddRange(child.FindCandidates(fragment));
                }
            }
          

            return candidates;
        }

        /// <summary>
        /// Gets all of the words beneath a specific node.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        private List<Candidate> GetAllChildCandidates(TrieNode parentNode)
        {
            List<Candidate> candidates = new List<Candidate>();

            foreach(var child in parentNode.Children)
            {
                //If likelihood is greater than zero, then it's a word - include it in the list
                if(child.RawLikelihood > 0)
                {
                    candidates.Add(new Candidate()
                    {
                        Suggestion = child.Fragment,
                        Likelihood = child.RawLikelihood
                    });
                }
               
                //Continue traversing tree in case the node was both a word and a fragment
                candidates.AddRange(GetAllChildCandidates(child));
                
            }

            return candidates;

        }



        /// <summary>
        /// Takes a word and generates a tree structure where each node is associated with a string.
        /// If the string is the whole word, then increment it's likelihood, otherwise
        /// generate children for each substring. 
        /// </summary>
        /// <param name="word"></param>
        public void Train(string word)
        {
            if (word.IndexOf(this.Fragment) == -1)
                return;

            //We've found the leaf!  Add and return.
            if(this.Fragment.Equals(word))
            {
                this.RawLikelihood += 1;
                return;
            }

            //Extract the next substring
            string childFragment = word.Substring(0, this.Fragment.Length + 1);
            
            //Look for an existing child with the same fragment
            var existingChild = this.Children.SingleOrDefault(p => p.Fragment.Equals(childFragment));
            if(existingChild == null)
            {
                //No child node was found, so create one
                existingChild = new TrieNode(childFragment);
                this.Children.Add(existingChild);
            }
           
            //Keep traversing down the tree, creating nodes as we go
            existingChild.Train(word);
        }






       
    }
  
}
