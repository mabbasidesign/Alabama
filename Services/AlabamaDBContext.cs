using Alabama.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alabama.Services
{
    public class AlabamaDBContext: DbContext
    {
        public AlabamaDBContext(DbContextOptions<DbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Book> Books { get; set; }

    }
}
