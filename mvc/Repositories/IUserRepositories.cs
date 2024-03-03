using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;

namespace mvc.Repositories
{
    public interface IUserRepositories
    {

      public void Register(tblUser user);
      int Login(tblUser user);
      int LoginWithApi(tblLogin user);

  }
}