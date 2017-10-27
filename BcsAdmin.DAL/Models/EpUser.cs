using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Type { get; set; }
        public string Access { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public int? CountryId { get; set; }
        public string Street2 { get; set; }
        public string Zip2 { get; set; }
        public string City2 { get; set; }
        public int? Country2Id { get; set; }
        public string Company { get; set; }
        public string Ico { get; set; }
        public string Dic { get; set; }
        public string Icdph { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public int? LanguageId { get; set; }
        public string Remark { get; set; }
        public int? Active { get; set; }
        public string PasswordHash { get; set; }
    }
}
