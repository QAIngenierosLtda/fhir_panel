using System;
using System.Collections.Generic;
using System.Text;
using AspStudio.Models;
using Microsoft.EntityFrameworkCore;

namespace AspStudio.Data
{
    public class ApplicationDbContext : DbContext
    {
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Device> Devices {get; set;}
        public DbSet<Person> Persons {get; set;}
        public DbSet<Employee> Empleados { get; set; }
        public DbSet<DeviceSite> DeviceSites { get; set; }
        public DbSet<DeviceEmployee> DeviceEmployees { get; set; }
        public DbSet<FhirData> FhirDatas { get; set; }

        public DbSet<Tipodoc> TiposDoc { get; set; }

        public DbSet<Empresa> Empresas { get; set; }

        public DbSet<Regional> Regionales { get; set; }

        public DbSet<Instalacion> Instalaciones { get; set; }

        public DbSet<TextosEnrol> TextosEnrolam { get; set; }

        public DbSet<EnrolData> EnrolDatas { get; set; }

        public DbSet<Reconocimiento> Reconocimientos { get; set;}
        public DbSet<RecoDia> RecoDia { get; set;}
        public DbSet<SopoRecoDia> SopoRecoDias { get; set;}
        public DbSet<SopoEvRecoDia> SopoEvRecoDias { get; set;}
        public DbSet<RepoEnrolamiento> RepoEnrolamientos { get; set;}
        public DbSet<RepoEnrollDevice> RepoEnrollDevices { get; set;}
        public DbSet<SopoRecoPersona> SopoRecoPersonas { get; set;}
        public DbSet<SopoRecoPersonaFecha> SopoRecoPersonaFechas { get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Reconocimiento>().HasNoKey();
            builder.Entity<RecoDia>().HasNoKey();
            builder.Entity<SopoRecoDia>().HasNoKey();
            builder.Entity<SopoEvRecoDia>().HasNoKey();
            builder.Entity<RepoEnrolamiento>().HasNoKey();
            builder.Entity<RepoEnrollDevice>().HasNoKey();
            builder.Entity<SopoRecoPersona>().HasNoKey();
            builder.Entity<SopoRecoPersonaFecha>().HasNoKey();
            //builder.Entity<Person>().
            //    HasOne(p => p.Employee);
        }


    }
}
