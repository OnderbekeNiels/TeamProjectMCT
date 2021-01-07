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
            string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
            {
                await sqlConnectionInsert.OpenAsync();
                using (SqlCommand sqlCommandInsert = new SqlCommand())
                {
                    sqlCommandInsert.Connection = sqlConnectionInsert;
                    sqlCommandInsert.CommandText = "DELETE FROM LapTijden WHERE LapTijdId = @LapTijdId";
                    sqlCommandInsert.Parameters.AddWithValue("@LapTijdId", lapTijd.LapTijdId);



                    await sqlCommandInsert.ExecuteNonQueryAsync();
                    CustomResponse customResponse = new CustomResponse();
                    customResponse.Succes = true;
                    return customResponse;

                }
            }

        }
    }
}
