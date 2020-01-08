using System;
using System.Linq;
using System.Collections.Generic;

namespace Viternus.Data.Interface
{
    interface IVideoRepository : IRepository<Video>
    {
        void AddVideoToUser(Video vid, string userName);
        List<Video> FindAllVideosByUser(string userName);
    }
}
