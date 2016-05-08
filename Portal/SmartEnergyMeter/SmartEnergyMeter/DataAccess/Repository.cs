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

        public bool ConfigureTariff(List<Tariff> tariff)
        {
            Database db = new SqlDatabase(SmartEmConnectionString);
            DataTable tariffData = new DataTable();
            tariffData.Columns.Add("TariffType");
            tariffData.Columns.Add("Rate");
            foreach (Tariff tem in tariff)
            {
                var dr = tariffData.NewRow();
                dr["TariffType"] = (int)tem.TariffType;
                dr["Rate"] = (decimal)tem.Rate;
                tariffData.Rows.Add(dr);
            }
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "TariffUpdate";
                    SqlParameter param = new SqlParameter();
                    param.SqlDbType = SqlDbType.Structured;
                    param.ParameterName = "@TariffInputData";
                    param.Value = tariffData;
                    cmd.Parameters.Add(param);
                    
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Tariff> GetTariff()
        {
            Database db = new SqlDatabase(SmartEmConnectionString);
            List<Tariff> tariffList = new List<Tariff>();

            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "TariffGet";
                    using (var reader = db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Tariff tempTariff = new Tariff();
                            tempTariff.Id = Convert.ToInt32(reader["Id"]);
                            tempTariff.Name = reader["Name"].ToString();
                            tempTariff.TariffType = (TariffType)Enum.Parse(typeof(TariffType), reader["TariffType"].ToString()); 
                            tempTariff.Rate = Convert.ToDouble(reader["Rate"]);

                            tariffList.Add(tempTariff);
                        }       
                    }

                    return tariffList;
                }
            }
        }

        public List<Customer> ListCustomer()
        {
            Database db = new SqlDatabase(SmartEmConnectionString);
            List<Customer> customers = new List<Customer>();

            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "CustomerGetAll";
                    using (var reader = db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Customer tempCustomer = new Customer();
                            tempCustomer.Id = (reader["Id"]).ToString();
                            tempCustomer.Name = reader["Name"].ToString();
                            tempCustomer.Email = reader["Email"].ToString();
                            tempCustomer.AddressLine1 = reader["AddressLine1"].ToString();
                            tempCustomer.AddressLine2 = reader["AddressLine2"].ToString();
                            tempCustomer.PinCode = Convert.ToInt32( reader["PinCode"]);

                            customers.Add(tempCustomer);
                        }
                    }

                    return customers;
                }
            }
        }

        public List<ConsumptionLog> ListConsumptionLog(string customerId)
        {
            Database db = new SqlDatabase(SmartEmConnectionString);
            List<ConsumptionLog> consumptionLogs = new List<ConsumptionLog>();

            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "ConsumptionLogGetById";
                    cmd.Parameters.Add(new SqlParameter("@CustomerId", customerId) { SqlDbType = SqlDbType.NVarChar });
                    using (var reader = db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            ConsumptionLog tempConsumptionLog = new ConsumptionLog();
                            tempConsumptionLog.Id = Convert.ToInt32((reader["Id"]));
                            tempConsumptionLog.CustomerId = reader["CustomerId"].ToString();
                            tempConsumptionLog.SmartEnergyMeterId = reader["SmartEnergyMeterId"].ToString();
                            tempConsumptionLog.Unit = Convert.ToDecimal( reader["Unit"]);
                            tempConsumptionLog.CreatedDateTime = Convert.ToDateTime( reader["CreatedDateTime"]);

                            consumptionLogs.Add(tempConsumptionLog);
                        }
                    }

                    return consumptionLogs;
                }
            }
        }

        public bool ConsumptionLogInsert(string smartEnergyMeterId, decimal unit)
        {
            Database db = new SqlDatabase(SmartEmConnectionString);
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "ConsumptionLogInsert";
                    cmd.Parameters.Add(new SqlParameter("@SmartEnergyMeterId", smartEnergyMeterId) { SqlDbType = SqlDbType.NVarChar });
                    cmd.Parameters.Add(new SqlParameter("@Unit", unit) { SqlDbType = SqlDbType.Decimal });
                    
                    return cmd.ExecuteNonQuery() > 0;

                }
            }
        }

        public List<Entities.SmartEnergyMeter> SmartEnergyMeterGet()
        {
            Database db = new SqlDatabase(SmartEmConnectionString);
            List<Entities.SmartEnergyMeter> smartEnergyMeters = new List<Entities.SmartEnergyMeter>();

            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "SmartEnergyMeterGet";
                    using (var reader = db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Entities.SmartEnergyMeter temp = new Entities.SmartEnergyMeter();
                            temp.Id = (reader["Id"]).ToString();
                            temp.CustomerId = reader["CustomerId"].ToString();
                            temp.TariffType = Convert.ToInt32( reader["TariffType"]);
                            
                           
                            smartEnergyMeters.Add(temp);
                        }
                    }

                    return smartEnergyMeters;
                }
            }
        }

        public bool InsertSmartEnergyMeter(Entities.SmartEnergyMeter smaertEnergyMeter)
        {
            Database db = new SqlDatabase(SmartEmConnectionString);
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "SmartEnergyMeterInsert";
                    cmd.Parameters.Add(new SqlParameter("@Id", smaertEnergyMeter.Id) { SqlDbType = SqlDbType.NVarChar });
                    cmd.Parameters.Add(new SqlParameter("@CustomerId", smaertEnergyMeter.CustomerId) { SqlDbType = SqlDbType.NVarChar });
                    cmd.Parameters.Add(new SqlParameter("@TariffType", smaertEnergyMeter.TariffType) { SqlDbType = SqlDbType.Int });
                   
                    return cmd.ExecuteNonQuery() > 0;

                }
            }
        }
    }
}