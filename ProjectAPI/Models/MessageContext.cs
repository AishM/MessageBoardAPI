using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectAPI;

namespace ProjectAPI.Models
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options)
            : base(options)
        {
        }

        public DbSet<MessageBoard> Messages { get; set; }

        public DbSet<ProjectAPI.MessageBoard> MessageBoard { get; set; }


    }
}
