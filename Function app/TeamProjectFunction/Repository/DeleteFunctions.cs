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
        public static async Task<CustomResponse> DelLapTijdFunction(LapTijd lapTijd)
        {
            string connectionStringDel = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionDel = new SqlConnection(connectionStringDel))
            {
                await sqlConnectionDel.OpenAsync();
                using (SqlCommand sqlCommandDel = new SqlCommand())
                {
                    sqlCommandDel.Connection = sqlConnectionDel;
                    sqlCommandDel.CommandText = "DELETE FROM LapTijden WHERE LapTijdId = @LapTijdId";
                    sqlCommandDel.Parameters.AddWithValue("@LapTijdId", lapTijd.LapTijdId);



                    await sqlCommandDel.ExecuteNonQueryAsync();
                    CustomResponse customResponse = new CustomResponse();
                    customResponse.Succes = true;
                    return customResponse;

                }
            }

        }

        public static async Task<CustomResponse> DelEtappeFunction(Etappe etappe)
        {
            // eerst controleren als er nog laptijden betsaan van deze etappe
            string connectionStringDel = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionDel = new SqlConnection(connectionStringDel))
            {
                await sqlConnectionDel.OpenAsync();
                using (SqlCommand sqlCommandDel = new SqlCommand())
                {
                    sqlCommandDel.Connection = sqlConnectionDel;
                    sqlCommandDel.CommandText = "SELECT * FROM LapTijden WHERE EtappeId = @EtappeId";
                    sqlCommandDel.Parameters.AddWithValue("@EtappeId", etappe.EtappeId);

                    SqlDataReader reader = await sqlCommandDel.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        LapTijd lapTijdDel = new LapTijd();
                        lapTijdDel.LapTijdId = Guid.Parse(reader["LapTijdId"].ToString());

                        await DelLapTijdFunction(lapTijdDel);
                    }


                    sqlCommandDel.Connection = sqlConnectionDel;
                    sqlCommandDel.CommandText = "DELETE FROM Etappes WHERE EtappeId = @EtappeId";
                    sqlCommandDel.Parameters.AddWithValue("@EtappeId", etappe.EtappeId);


                    await sqlCommandDel.ExecuteNonQueryAsync();
                    CustomResponse customResponse = new CustomResponse();
                    customResponse.Succes = true;
                    return customResponse;

                    
                }



                
            }
        }
    }
}
