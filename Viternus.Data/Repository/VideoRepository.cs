using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class VideoRepository : RepositoryBase<Video>, IVideoRepository
    {
        protected override ObjectQuery<Video> EntityQuery
        {
            get { return DataConnector.Context.Video; }
        }

        public List<Video> FindAllVideosByUser(string userName)
        {
            var result = from v in DataConnector.Context.Video.Include("AppUser")
                         where v.AppUser.UserName == userName
                            && v.CreationDate != null
                         select v;

            return result.ToList();
        }

        public void AddVideoToUser(Video vid, string userName)
        {
            AppUserRepository userRepository = new AppUserRepository();
            userRepository.GetByUserName(userName).Video.Add(vid);
        }
    }
}
