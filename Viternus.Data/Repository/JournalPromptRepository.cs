using System;
using System.Data.Objects;
using System.Linq;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class JournalPromptRepository : RepositoryBase<JournalPrompt>, IJournalPromptRepository
    {
        protected override ObjectQuery<JournalPrompt> EntityQuery
        {
            get { return DataConnector.Context.JournalPrompt; }
        }

        public JournalPrompt GetRandom()
        {
            var result = from jp in DataConnector.Context.JournalPrompt
                         where jp.Visible
                         orderby jp.Id
                         select jp;

            int count = result.Count();
            int index = new Random().Next(count);

            return result.Skip(index).FirstOrDefault();
        }
    }
}
