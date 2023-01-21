using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories;

public class UserRepo : GenericRepository<UserEntity>, IUserRepo
{
    public UserRepo(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}