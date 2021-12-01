using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Update_Transaction.Models
{
    public class DBInitializer
    {
        //    public async static void Initialize(IServiceProvider serviceProvider)
        //    {
        //        using var context = new TransactionAppContext(serviceProvider.GetRequiredService<DbContextOptions<TransactionAppContext>>());
        //        context.Database.EnsureCreated();
        //        if (await context.ROLE.AnyAsync())
        //        {
        //            return;   // DB has been seeded
        //        }
        //        var roles = new Role[]
        //       {
        //                new Role{Id=1, Active = true, Name = "Super Admin",Slug="superadmin"},
        //                new Role{Id=2, Active = true, Name = "Transcript Head",Slug="transcripthead"},
        //                new Role{Id=3, Active = true, Name = "Transcript Officer",Slug="transcriptofficer"},
        //       };
        //        foreach (Role role in roles)
        //        {

        //            context.Add(role);
        //        }
        //        context.Database.OpenConnection();
        //        context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT dbo.ROLE ON;");
        //        await context.SaveChangesAsync();
        //        context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT dbo.ROLE OFF;");
        //        context.Database.CloseConnection();



        //}

    }
}
