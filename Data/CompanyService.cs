using Dapper;
using Microsoft.Extensions.Configuration;
using PatchNotes.Models;
using PatchNotes.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PatchNotes.Data
{
    public class CompanyService
    {
        private readonly IConfiguration _configuration;

        public CompanyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString(Constants.PatchNotesConnectionStringName));
            var sql = "SELECT c.*, p.ID, v.ID " +
                "FROM Companies c " +
                "LEFT OUTER JOIN Projects p ON p.CompanyID = c.ID " +
                "LEFT OUTER JOIN Versions v ON v.ProjectID = p.ID " +
                "ORDER BY c.Name";

            var lookup = new List<Company>();
            await conn.QueryAsync<Company, Project, Models.Version, Company>(sql, (company, project, version) =>
            {
                Company lookupCompany;
                if ((lookupCompany = lookup.FirstOrDefault(t => t.ID == company.ID)) == null)
                    lookup.Add(lookupCompany = company);

                if (project != null && project.ID != Guid.Empty && !lookupCompany.Projects.Any(p => p.ID == project.ID))
                    lookupCompany.Projects.Add(project);

                if (version != null && version.ID != Guid.Empty && !lookupCompany.Versions.Any(v => v.ID == version.ID))
                    lookupCompany.Versions.Add(version);

                return lookupCompany;
            }, splitOn: "ID, ID");

            return lookup;
        }

        public async Task SaveAsync(Company company)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString(Constants.PatchNotesConnectionStringName));
            var sql = "IF NOT EXISTS(SELECT TOP 1 ID FROM Companies WHERE ID = @ID) " +
                "BEGIN " +
                "INSERT INTO Companies (Name, Notes, Archived) " +
                "VALUES (@Name, @Notes, @Archived)" +
                "END " +
                "ELSE " +
                "BEGIN " +
                "UPDATE Companies SET " +
                "Name = @Name, " +
                "Notes = @Notes, " +
                "Archived = @Archived " +
                "WHERE ID = @ID " +
                "END ";

            await conn.ExecuteAsync(sql, company);
        }
    }
}
