using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GaleriUygulamasi.Models.Context
{
    public class GaleriYonetimContext:DbContext
    {
        public GaleriYonetimContext()
            : base("GaleriConnectionString")
        {

        }
        public DbSet<Dosya> Dosya { get; set; }
    }
}