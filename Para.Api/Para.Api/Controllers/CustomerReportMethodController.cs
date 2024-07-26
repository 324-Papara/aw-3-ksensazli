using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Para.Data.Domain;

namespace Para.Api.Controllers;

public class CustomerReportMethodController : ControllerBase
{
    private readonly string _connectionString;

    public CustomerReportMethodController(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Customer> GetCustomerReportAsync(long customerId)
    {
        using var connection = new SqlConnection(_connectionString);

        var sql = @"
            SELECT 
                c.Id, c.FirstName, c.LastName, c.IdentityNumber, c.Email, c.CustomerNumber, c.DateOfBirth
            FROM 
                dbo.Customer c
            WHERE 
                c.Id = @CustomerId;

            SELECT 
                cd.CustomerId, cd.FatherName, cd.MotherName, cd.EducationStatus, cd.MonthlyIncome, cd.Occupation
            FROM 
                dbo.CustomerDetail cd
            WHERE 
                cd.CustomerId = @CustomerId;

            SELECT 
                ca.CustomerId, ca.Country, ca.City, ca.AddressLine, ca.ZipCode, ca.IsDefault
            FROM 
                dbo.CustomerAddress ca
            WHERE 
                ca.CustomerId = @CustomerId;

            SELECT 
                cp.CustomerId, cp.CountryCode, cp.Phone, cp.IsDefault
            FROM 
                dbo.CustomerPhone cp
            WHERE 
                cp.CustomerId = @CustomerId;
        ";

        using var multi = await connection.QueryMultipleAsync(sql, new { CustomerId = customerId });

        var customer = await multi.ReadSingleOrDefaultAsync<Customer>();

        if (customer == null)
        {
            return null;
        }

        customer.CustomerDetail = await multi.ReadSingleOrDefaultAsync<CustomerDetail>();

        customer.CustomerAddresses = (await multi.ReadAsync<CustomerAddress>()).AsList();

        customer.CustomerPhones = (await multi.ReadAsync<CustomerPhone>()).AsList();

        return customer;
    }
}