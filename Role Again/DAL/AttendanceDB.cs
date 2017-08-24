using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Role_Again.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Role_Again.DAL
{
    public class AttendanceDB : DbContext
    {
        public DbSet<FileUpload> FileUploads { get; set; }
    }
}