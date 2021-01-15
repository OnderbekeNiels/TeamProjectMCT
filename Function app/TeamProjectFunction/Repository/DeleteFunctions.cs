using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using TeamProjectFunction.Models;

namespace TeamProjectFunction.Repository
{
    public class DeleteFunctions
    {
        public static async Task<CustomResponse> DelLapTijdFunction(Guid lapTijd)
        {
            string connectionStringDel = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionDel = new SqlConnection(connectionStringDel))
            {
                await sqlConnectionDel.OpenAsync();
                using (SqlCommand sqlCommandDel = new SqlCommand())
                {
                    sqlCommandDel.Connection = sqlConnectionDel;
                    sqlCommandDel.CommandText = "DELETE FROM LapTijden WHERE LapTijdId = @LapTijdId";
                    sqlCommandDel.Parameters.AddWithValue("@LapTijdId", lapTijd);



                    await sqlCommandDel.ExecuteNonQueryAsync();
                    CustomResponse customResponse = new CustomResponse();
                    customResponse.Succes = true;
                    return customResponse;

                }
            }

        }

        public static async Task<CustomResponse> DelEtappeFunction(Guid etappe)
        {
            // eerst controleren als er nog laptijden betsaan van deze etappe
            List<Guid> lapTijden = new List<Guid>();
            string connectionStringDel = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionDel = new SqlConnection(connectionStringDel))
            {
                await sqlConnectionDel.OpenAsync();
                using (SqlCommand sqlCommandDel = new SqlCommand())
                {
                    sqlCommandDel.Connection = sqlConnectionDel;
                    sqlCommandDel.CommandText = "SELECT * FROM LapTijden WHERE EtappeId = @EtappeId";
                    sqlCommandDel.Parameters.AddWithValue("@EtappeId", etappe);

                    SqlDataReader reader = await sqlCommandDel.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        lapTijden.Add(Guid.Parse(reader["LapTijdId"].ToString()));
                    }

                }
                foreach (Guid lapTijd in lapTijden)
                {
                    await DelLapTijdFunction(lapTijd);
                }
            }
            using (SqlConnection sqlConnectionDel = new SqlConnection(connectionStringDel))
            {
                await sqlConnectionDel.OpenAsync();
                using (SqlCommand sqlCommandDel = new SqlCommand())
                {
                    sqlCommandDel.Connection = sqlConnectionDel;
                    sqlCommandDel.CommandText = "DELETE FROM Etappes WHERE EtappeId = @EtappeId";
                    sqlCommandDel.Parameters.AddWithValue("@EtappeId", etappe);


                    await sqlCommandDel.ExecuteNonQueryAsync();
                    CustomResponse customResponse = new CustomResponse();
                    customResponse.Succes = true;
                    return customResponse;
                }
     
            }
        }

        public static async Task<CustomResponse> DelDeelnemerFromRonde(Guid deelnemer)
        {
            string connectionStringDel = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionDel = new SqlConnection(connectionStringDel))
            {
                await sqlConnectionDel.OpenAsync();
                using (SqlCommand sqlCommandDel = new SqlCommand())
                {
                    sqlCommandDel.Connection = sqlConnectionDel;
                    sqlCommandDel.CommandText = "DELETE FROM Deelnemers WHERE deelnemerId = @deelnemerId";
                    sqlCommandDel.Parameters.AddWithValue("@deelnemerId", deelnemer);

                    await sqlCommandDel.ExecuteNonQueryAsync();
                    CustomResponse customResponse = new CustomResponse();
                    customResponse.Succes = true;
                    return customResponse;

                }
            }
        }

        public static async Task<CustomResponse> DelRondeFunction(Guid ronde)
        {
            // eerst controleren als er nog deelnemers betsaan in deze ronde
            // en controleren als er nog etappes zijn
            //List<Guid> lapTijden = new List<Guid>();

            List<Guid> deelnemerIds = new List<Guid>();
            string connectionStringDel = Environment.GetEnvironmentVariable("ConnectionString");
            //deelnemers verwijderen:
            using (SqlConnection sqlConnectionDel = new SqlConnection(connectionStringDel))
            {
                await sqlConnectionDel.OpenAsync();
                using (SqlCommand sqlCommandDel = new SqlCommand())
                {
                    sqlCommandDel.Connection = sqlConnectionDel;
                    sqlCommandDel.CommandText = "SELECT * FROM Deelnemers WHERE RondeId = @RondeId";
                    sqlCommandDel.Parameters.AddWithValue("@RondeId", ronde);

                    SqlDataReader reader = await sqlCommandDel.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        deelnemerIds.Add(Guid.Parse(reader["DeelnemerId"].ToString()));
                    }

                }
                foreach (Guid deelnemer in deelnemerIds)
                {
                    await DelDeelnemerFromRonde(deelnemer);
                }
            }
            //etappes verwijderen
            List<Guid> etappeIds = new List<Guid>();
            using (SqlConnection sqlConnectionDel = new SqlConnection(connectionStringDel))
            {
                await sqlConnectionDel.OpenAsync();
                using (SqlCommand sqlCommandDel = new SqlCommand())
                {
                    sqlCommandDel.Connection = sqlConnectionDel;
                    sqlCommandDel.CommandText = "SELECT * FROM Etappes WHERE RondeId = @RondeId";
                    sqlCommandDel.Parameters.AddWithValue("@RondeId", ronde);

                    SqlDataReader reader = await sqlCommandDel.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        etappeIds.Add(Guid.Parse(reader["EtappeId"].ToString()));
                    }

                }
                foreach (Guid etappe in etappeIds)
                {
                    await DelEtappeFunction(etappe);
                }
            }


            using (SqlConnection sqlConnectionDel = new SqlConnection(connectionStringDel))
            {
                await sqlConnectionDel.OpenAsync();
                using (SqlCommand sqlCommandDel = new SqlCommand())
                {
                    sqlCommandDel.Connection = sqlConnectionDel;
                    sqlCommandDel.CommandText = "DELETE FROM Rondes WHERE RondeId = @RondeId";
                    sqlCommandDel.Parameters.AddWithValue("@RondeId", ronde);


                    await sqlCommandDel.ExecuteNonQueryAsync();
                    CustomResponse customResponse = new CustomResponse();
                    customResponse.Succes = true;
                    return customResponse;
                }

            }
        }

    }
}
