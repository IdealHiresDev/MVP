﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IdealHires.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class IdealHiresDbContext : DbContext
    {
        public IdealHiresDbContext()
            : base("name=IdealHiresDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Academic> Academics { get; set; }
        public virtual DbSet<Action> Actions { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AddressEmployer> AddressEmployers { get; set; }
        public virtual DbSet<AuditTrail> AuditTrails { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyAddress> CompanyAddresses { get; set; }
        public virtual DbSet<CompanyJobCreditDetail> CompanyJobCreditDetails { get; set; }
        public virtual DbSet<CompanyJobCreditDetailHistory> CompanyJobCreditDetailHistories { get; set; }
        public virtual DbSet<CompanyLogo> CompanyLogoes { get; set; }
        public virtual DbSet<CompanyPackageDetail> CompanyPackageDetails { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<EmployerCompany> EmployerCompanies { get; set; }
        public virtual DbSet<Entity> Entities { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobCategory> JobCategories { get; set; }
        public virtual DbSet<JobCategoryJob> JobCategoryJobs { get; set; }
        public virtual DbSet<JobCategoryProfile> JobCategoryProfiles { get; set; }
        public virtual DbSet<JobCredit> JobCredits { get; set; }
        public virtual DbSet<JobType> JobTypes { get; set; }
        public virtual DbSet<JobTypeJob> JobTypeJobs { get; set; }
        public virtual DbSet<JobTypeProfile> JobTypeProfiles { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<KeywordsJob> KeywordsJobs { get; set; }
        public virtual DbSet<KeywordsProfile> KeywordsProfiles { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<NotificationTypeJob> NotificationTypeJobs { get; set; }
        public virtual DbSet<PayPeriodType> PayPeriodTypes { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PermissionType> PermissionTypes { get; set; }
        public virtual DbSet<PhoneFormat> PhoneFormats { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<ProfileJob> ProfileJobs { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<SaveSearch> SaveSearches { get; set; }
        public virtual DbSet<SortListedCandidate> SortListedCandidates { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Work> Works { get; set; }
    
        public virtual ObjectResult<Usp_Get_IHDashboard_Result_Result> Usp_Get_IHDashboard_Result()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Usp_Get_IHDashboard_Result_Result>("Usp_Get_IHDashboard_Result");
        }
    }
}
