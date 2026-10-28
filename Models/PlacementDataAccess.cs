using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Practical_8_.Net.Models
{
    public class PlacementDataAccess
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["PlacementConn"]?.ConnectionString
            ?? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PlacementDB;Integrated Security=True;";

        public List<StudentPlacementRecord> GetPlacementData(string filter = "All")
        {
            var list = new List<StudentPlacementRecord>();

            string query = @"
                SELECT 
                    s.StudentId, 
                    s.EnrollmentNo, 
                    s.StudentName, 
                    s.Department, 
                    s.PassingYear, 
                    s.IsPlaced,
                    p.CompanyName, 
                    p.JobRole, 
                    p.PackageLPA
                FROM Students s
                LEFT JOIN Placements p ON s.StudentId = p.StudentId";

            if (filter == "Placed")
            {
                query += " WHERE s.IsPlaced = 1";
            }
            else if (filter == "Unplaced")
            {
                query += " WHERE s.IsPlaced = 0";
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new StudentPlacementRecord
                            {
                                StudentId = Convert.ToInt32(reader["StudentId"]),
                                EnrollmentNo = reader["EnrollmentNo"].ToString(),
                                StudentName = reader["StudentName"].ToString(),
                                Department = reader["Department"].ToString(),
                                PassingYear = Convert.ToInt32(reader["PassingYear"]),
                                IsPlaced = Convert.ToBoolean(reader["IsPlaced"]),
                                CompanyName = reader["CompanyName"] != DBNull.Value ? reader["CompanyName"].ToString() : "N/A",
                                JobRole = reader["JobRole"] != DBNull.Value ? reader["JobRole"].ToString() : "N/A",
                                PackageLPA = reader["PackageLPA"] != DBNull.Value ? Convert.ToDecimal(reader["PackageLPA"]) : (decimal?)null
                            });
                        }
                    }
                }
            }
            return list;
        }

        public PlacementSummary GetPlacementSummary()
        {
            var allRecords = GetPlacementData("All");
            var placedRecords = allRecords.Where(r => r.IsPlaced && r.PackageLPA.HasValue).ToList();

            int total = allRecords.Count;
            int placed = placedRecords.Count;

            return new PlacementSummary
            {
                TotalStudents = total,
                TotalPlaced = placed,
                TotalUnplaced = total - placed,
                PlacementPercentage = total > 0 ? Math.Round(((double)placed / total) * 100, 1) : 0,
                HighestPackage = placedRecords.Any() ? placedRecords.Max(r => r.PackageLPA.Value) : 0,
                AveragePackage = placedRecords.Any() ? Math.Round(placedRecords.Average(r => r.PackageLPA.Value), 2) : 0
            };
        }
    }
}