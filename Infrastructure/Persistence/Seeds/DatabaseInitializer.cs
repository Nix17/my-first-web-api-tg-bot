using Domain.Entities;
using Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        string str_ini_by = "Initializer";

        #region Users
        // if (await db.Users.CountAsync() == 0)
        // {
        //     var list = new List<UserEntity>();

        //     var user1 = new UserEntity(
        //                                 "vd@mail.ru",
        //                                 "vdvdvd",
        //                                 "admin",
        //                                 "+7(999)999-99-99",
        //                                 "",
        //                                 "",
        //                                 ""
        //                                 );// или admin

        //     list.Add(user1);
        //     // ------------------------

        //     //####################################
        //     await db.Users.AddRangeAsync(list);
        //     await db.SaveChangesAsync();
        // }
        #endregion
    }
}