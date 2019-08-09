using System;
using System.Collections.Generic;

namespace SB014.API.DAL
{
    public class WordRepositoryFake:IWordRepository
    {
        public static List<string> Words {get;} = new List<string>
        {
            "abacus","abacuses","abalone","abalones",
            "abandon","abandoned","abandoning","abandonment",
            "abandons","abase","abased","abasement",
            "abases","abashed","abasing","abate",
            "abated","abatement","abates","genial",             
            "geniality","genially","genie","genies",
            "genital","genitalia","genitals","genitive",
            "genitives","genius","geniuses","genned",
            "genning","genocidal","genocide","genome",
            "genomes","genotype","genotypes","genre",
            "genres","gens","gent","genteel",
            "genteelly","gentian","gentians","gentile",
            "Gentile","gentiles","Gentiles","gentility",
            "gentle",
            
        };


        public IEnumerable<string> GetWords(int numberOfWords)
        {
            if(WordRepositoryFake.Words.Count < numberOfWords)
            {
                throw new ArgumentException("Too many words requested");
            }
            HashSet<string> words = new HashSet<string>();
            var random = new Random();
            
            int sanityCheck = 0;
            while (words.Count < numberOfWords && sanityCheck < (numberOfWords * 50))
            {
                int i = random.Next(WordRepositoryFake.Words.Count);
                words.Add(WordRepositoryFake.Words[i]);
                sanityCheck++;
            }

            return words;
        }
    }
        
}