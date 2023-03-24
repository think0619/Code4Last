using System;
using System.Collections.Generic; 
using System.Text; 
using Microsoft.EntityFrameworkCore;

using Entities; 

namespace TextVoiceServer.DBContext
{
    public class DataConfigContext : DbContext
    {
        public DataConfigContext(DbContextOptions<DataConfigContext> options) : base(options)
        {  
        } 
         
        //public DbSet<VoiceView> view_voice { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
         
            //modelBuilder.Entity<VoiceView>().ToView(nameof(view_voice)).HasKey(t => t.recid); 

            //modelBuilder.Entity<NewGold>(entity =>
            //{
            //    entity.HasKey(e => e.TestNo);
            //});

            

        }
    }
}
