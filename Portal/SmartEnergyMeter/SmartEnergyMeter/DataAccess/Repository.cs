using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEnergyMeter.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace SmartEnergyMeter.DataAccess
{
    public class Repository : IRepository
    {
        private string SmartEmConnectionString = ConfigurationManager.ConnectionStrings["SmartEmConnectionString"].ConnectionString;

        public bool AddCustomer(Customer customer)
        {
            Database db = new SqlDatabase(SmartEmConnectionString);
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "CustomerInsert";
                    cmd.Parameters.Add(new SqlParameter("@Id", customer.Id) { SqlDbType = SqlDbType.NVarChar });
                    cmd.Parameters.Add(new SqlParameter("@Email", customer.Email) { SqlDbType = SqlDbType.NVarChar });
                    cmd.Parameters.Add(new SqlParameter("@Password", customer.Password) { SqlDbType = SqlDbType.NVarChar });
                    cmd.Parameters.Add(new SqlParameter("@Name", customer.Name) { SqlDbType = SqlDbType.NVarChar });
                    cmd.Parameters.Add(new SqlParameter("@AddressLine1", customer.AddressLine1) { SqlDbType = SqlDbType.NVarChar });
                    cmd.Parameters.Add(new SqlParameter("@AddressLine2", customer.AddressLine2) { SqlDbType = SqlDbType.NVarChar });
                    cmd.Parameters.Add(new SqlParameter("@PinCode", customer.PinCode) { SqlDbType = SqlDbType.Int });

                   return cmd.ExecuteNonQuery() > 0;

                }
            }
        }

        public AdminUser AuthenticateAdminUser(string email, string password)
        {
            Database db = new SqlDatabase(SmartEmConnectionString);
            AdminUser admin = new AdminUser();

            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "AdminUserAuthenticate";
                    cmd.Parameters.Add(new SqlParameter("@Email", email) { SqlDbType = SqlDbType.NVarChar });
                    cmd.Parameters.Add(new SqlParameter("@Password", password) { SqlDbType = SqlDbType.NVarChar });
                    using (var reader = db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            admin.Id = Convert.ToInt32(reader["Id"]);
                            admin.Email = reader["Email"].ToString();

                        }
                    }

                    return admin;
                }
            }
        }

        public Customer AuthenticateCustomer(string email, string password)
        {
            Database db = new SqlDatabase(SmartEmConnectionString);
            Customer customer = new Customer();
            
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "CustomerAuthenticate";
                    cmd.Parameters.Add(new SqlParameter("@Email", email) { SqlDbType = SqlDbType.NVarChar });
                    cmd.Parameters.Add(new SqlParameter("@Password", password) { SqlDbType = SqlDbType.NVarChar });
                    using (var reader = db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            customer.Id = reader["Id"].ToString();
                            customer.Email = reader["Email"].ToString();
                            customer.Name = reader["Name"].ToString();
                            customer.AddressLine1 = reader["AddressLine1"].ToString();
                            customer.AddressLine2 = reader["AddressLine2"].ToString();
                            customer.PinCode = Convert.ToInt32(reader["PinCode"]);
                        }
                    }

                    return customer;
                }
            }

        }
    }
}