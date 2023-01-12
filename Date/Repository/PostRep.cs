using Microsoft.EntityFrameworkCore;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class PostRep : IPost
    {
        private readonly AppDB _appDB;

        public PostRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public IEnumerable<Post> AllPosts => _appDB.DBPost;
    }
}
