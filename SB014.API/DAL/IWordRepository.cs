using System.Collections.Generic;

namespace SB014.API.DAL
{
    public interface IWordRepository
    {
        IEnumerable<string> GetWords(int numberOfWords);
    }
}