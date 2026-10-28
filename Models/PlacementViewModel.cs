using System.Collections.Generic;

namespace Practical_8_.Net.Models
{
    // Individual student row joined with placement data
    public class StudentPlacementRecord
    {
        public int StudentId { get; set; }
        public string EnrollmentNo { get; set; }
        public string StudentName { get; set; }
        public string Department { get; set; }
        public int PassingYear { get; set; }
        public bool IsPlaced { get; set; }
        public string CompanyName { get; set; }
        public string JobRole { get; set; }
        public decimal? PackageLPA { get; set; }
    }

    // Top summary KPI metrics
    public class PlacementSummary
    {
        public int TotalStudents { get; set; }
        public int TotalPlaced { get; set; }
        public int TotalUnplaced { get; set; }
        public double PlacementPercentage { get; set; }
        public decimal HighestPackage { get; set; }
        public decimal AveragePackage { get; set; }
    }

    // Consolidated ViewModel for the Dashboard View
    public class PlacementDashboardViewModel
    {
        public PlacementSummary Summary { get; set; }
        public List<StudentPlacementRecord> StudentsList { get; set; }
        public string CurrentFilter { get; set; }
    }
}