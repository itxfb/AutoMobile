//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CarDetail
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string postalCode { get; set; }
        public string Doesitdrive { get; set; }
        public string Userstate { get; set; }
        public string Userzipcode { get; set; }
        public string transRemove { get; set; }
        public string HasKey { get; set; }
        public string odometerReadings { get; set; }
        public string SalePrice { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public string Title { get; set; }
        public string vin { get; set; }
        public string driveablestatus { get; set; }
        public string doesStart { get; set; }
        public string powerTrain { get; set; }
        public string FrontEndDamage { get; set; }
        public string RearEndDamage { get; set; }
        public string LeftSideDamage { get; set; }
        public string RightSideDamage { get; set; }
        public string FloodOrFireDamage { get; set; }
        public string AirbagDeployed { get; set; }
        public string AnyPartIsRemoved { get; set; }
        public string Usercity { get; set; }
        public string DamageSeverity { get; set; }
        public string RepairCost { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public Nullable<int> UserCarDetailId { get; set; }
        public Nullable<int> IsActive { get; set; }
    }
}
