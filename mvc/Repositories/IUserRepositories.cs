using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Repositories
{
    public interface IUserRepositories
    {
    
      tblUser Login(tblUser data);

    }
}