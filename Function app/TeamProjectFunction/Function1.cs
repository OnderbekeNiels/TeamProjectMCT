using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TeamProjectFunction.Models;
using System.Data.SqlClient;
using System.Data;
using TeamProjectFunction.Repository;
using TeamProjectFunction.Models.GebruikerRelated;
using TeamProjectFunction.Models.Klassement;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using TeamProjectFunction.Models.Klassement.Graph;

namespace TeamProjectFunction
{
    public static class Function1
    {
        //[FunctionName("CreateAccount")]
        //public static async Task<IActionResult> CreateAccount(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "gebruikers/create")] HttpRequest req,
        //    ILogger log)
        //{

        //    //Account create:
        //    //naar api sturen email, ww, voornaam, achternaam via json, vb:
        //    //{
        //    //"naam": "test6",
        //    //"voornaam": "test1",
        //    //"email": "test1@email.com",
        //    //"wachtwoord": "test1efe"
        //    //}

        //    //mogelijke returns:
        //    //account aangemaakt gebruiker met alle params
        //    //gebruiker bestaat al: Gebruiker bestaat al
        //    //de return values kunnen aangepast worden natuurlijk, deze zijn als vb


        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    Gebruiker gebruiker = JsonConvert.DeserializeObject<Gebruiker>(requestBody);

        //    gebruiker.GebruikerId = Guid.NewGuid();         

        //    string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        //    // controleren als het email nog niet gebruikt is
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        await sqlConnection.OpenAsync();
        //        using (SqlCommand sqlCommand = new SqlCommand())
        //        {
        //            sqlCommand.Connection = sqlConnection;
        //            sqlCommand.CommandText = "SELECT * FROM Gebruiker WHERE Email = @Email";
        //            sqlCommand.Parameters.AddWithValue("@Email", gebruiker.Email);

        //            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
        //            Gebruiker gebruikerDb = new Gebruiker();
        //            while (reader.Read())
        //            {
        //                gebruikerDb.Email = reader["Email"].ToString();
        //                gebruikerDb.Wachtwoord = reader["Wachtwoord"].ToString();

        //            }

        //            if (gebruikerDb.Email != gebruiker.Email)
        //            {
        //                using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionString))
        //                {
        //                    await sqlConnectionInsert.OpenAsync();
        //                    using (SqlCommand sqlCommandInsert = new SqlCommand())
        //                    {
        //                        sqlCommandInsert.Connection = sqlConnectionInsert;
        //                        sqlCommandInsert.CommandText = "INSERT INTO Gebruiker VALUES (@GebruikerId, @Naam, @Voornaam,@Email, @Wachtwoord)";
        //                        sqlCommandInsert.Parameters.AddWithValue("@GebruikerId", gebruiker.GebruikerId);
        //                        sqlCommandInsert.Parameters.AddWithValue("@Naam", gebruiker.Naam);
        //                        sqlCommandInsert.Parameters.AddWithValue("@Voornaam", gebruiker.Voornaam);
        //                        sqlCommandInsert.Parameters.AddWithValue("@Email", gebruiker.Email);
        //                        sqlCommandInsert.Parameters.AddWithValue("@Wachtwoord", gebruiker.Wachtwoord);

        //                        await sqlCommandInsert.ExecuteNonQueryAsync();
        //                    }
        //                    // return gemaakte gebruiker
        //                    return new OkObjectResult(gebruiker);
        //                }
        //            }
        //            else
        //            {
        //                // gebruiker bestaat al
        //                return new OkObjectResult("Gebruiker bestaat al");
        //            }
        //        }
        //    }




        //}

        //[FunctionName("AccountLogin")]
        //public static async Task<IActionResult> AccountLogin(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gebruikers/login")] HttpRequest req,
        //    ILogger log)
        //{

        //    //Account login:
        //    //naar api sturen email en ww via json, vb:
        //    //{
        //    //"email": "test1@email.com",
        //    //"wachtwoord": "test1efe"
        //    //}

        //    //mogelijke returns:
        //    //ww correct: Wachtwoord correct
        //    //ww incorrect: Wachtwoord incorrect
        //    //geen account gevonde: Geen gebruiker gevonden
        //    //de return values kunnen aangepast worden natuurlijk, deze zijn als vb


        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    Gebruiker gebruikerLogin = JsonConvert.DeserializeObject<Gebruiker>(requestBody);


        //    string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        await sqlConnection.OpenAsync();
        //        using (SqlCommand sqlCommand = new SqlCommand())
        //        {
        //            sqlCommand.Connection = sqlConnection;
        //            sqlCommand.CommandText = "SELECT * FROM Gebruiker WHERE Email = @Email";
        //            sqlCommand.Parameters.AddWithValue("@Email", gebruikerLogin.Email);

        //            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
        //            Gebruiker gebruikerDb = new Gebruiker();
        //            while (reader.Read())
        //            {
        //                gebruikerDb.Email = reader["Email"].ToString();
        //                gebruikerDb.Wachtwoord = reader["Wachtwoord"].ToString();

        //            }

        //            if (gebruikerDb.Email != null)
        //            {
        //                if (gebruikerDb.Wachtwoord == gebruikerLogin.Wachtwoord)
        //                {
        //                    // psswd correct
        //                    return new OkObjectResult("Wachtwoord correct");
        //                }

        //                else
        //                {
        //                    // psswd incorrect
        //                    return new OkObjectResult("Wachtwoord onjuist");
        //                }
        //            }

        //            else
        //            {
        //                // no user in db with this email
        //                return new OkObjectResult("Geen gebruiker gevonden");
        //            }
        //        }
        //    }



        //}

        //[FunctionName("CreateAccount")]
        //public static async Task<IActionResult> CreateAccount(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "gebruikers/create")] HttpRequest req,
        //    ILogger log)
        //{

        //    //Account create:
        //    //naar api sturen email en gebruikersnaam via json gebruikersnaam is optioneel, vb:
        //    //{
        //    //"email": "test1@email.com",
        //    //"gebruikersnaam": "test1"
        //    //}

        //    //mogelijke returns:
        //    //account aangemaakt: gebruikerv2 met alle params
        //    //gebruiker bestaat al: custom response zie model
        //    //de return values kunnen aangepast worden natuurlijk, deze zijn als vb


        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    GebruikerV2 gebruiker = JsonConvert.DeserializeObject<GebruikerV2>(requestBody);

        //    gebruiker.GebruikerId = Guid.NewGuid();

        //    string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        //    // controleren als het email nog niet gebruikt is
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        await sqlConnection.OpenAsync();
        //        using (SqlCommand sqlCommand = new SqlCommand())
        //        {
        //            sqlCommand.Connection = sqlConnection;
        //            sqlCommand.CommandText = "SELECT * FROM Gebruiker WHERE Email = @Email";
        //            sqlCommand.Parameters.AddWithValue("@Email", gebruiker.Email);

        //            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
        //            Gebruiker gebruikerDb = new Gebruiker();
        //            while (reader.Read())
        //            {
        //                gebruikerDb.Email = reader["Email"].ToString();

        //            }

        //            if (gebruikerDb.Email != gebruiker.Email)
        //            {
        //                using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionString))
        //                {
        //                    await sqlConnectionInsert.OpenAsync();
        //                    using (SqlCommand sqlCommandInsert = new SqlCommand())
        //                    {
        //                        sqlCommandInsert.Connection = sqlConnectionInsert;
        //                        sqlCommandInsert.CommandText = "INSERT INTO Gebruiker VALUES (@GebruikerId, @GebruikersNaam, @Email)";
        //                        sqlCommandInsert.Parameters.AddWithValue("@GebruikerId", gebruiker.GebruikerId);
        //                        sqlCommandInsert.Parameters.AddWithValue("@GebruikersNaam", gebruiker.GebruikersNaam);
        //                        sqlCommandInsert.Parameters.AddWithValue("@Email", gebruiker.Email);

        //                        await sqlCommandInsert.ExecuteNonQueryAsync();
        //                    }
        //                    // return gemaakte gebruiker
        //                    return new OkObjectResult(gebruiker);
        //                }
        //            }
        //            else
        //            {
        //                // gebruiker bestaat al
        //                CustomResponse customResponse = new CustomResponse();
        //                customResponse.Succes = false;
        //                customResponse.Message = "Gebruiker bestaat al";
        //                return new OkObjectResult(customResponse);
        //            }
        //        }
        //    }




        //}



        //[FunctionName("AccountLogin")]
        //public static async Task<IActionResult> AccountLogin(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gebruikers/login")] HttpRequest req,
        //    ILogger log)
        //{

        //    //Account login:
        //    //naar api sturen email json, vb:
        //    //{
        //    //"email": "test1@email.com"
        //    //}

        //    //mogelijke returns:
        //    //gebruiker gevonden: gebruikerv2 met alle params
        //    //geen account gevonden: custom response zie model
        //    //de return values kunnen aangepast worden natuurlijk, deze zijn als vb


        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    GebruikerV2 gebruikerLogin = JsonConvert.DeserializeObject<GebruikerV2>(requestBody);


        //    string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        await sqlConnection.OpenAsync();
        //        using (SqlCommand sqlCommand = new SqlCommand())
        //        {
        //            sqlCommand.Connection = sqlConnection;
        //            sqlCommand.CommandText = "SELECT * FROM Gebruiker WHERE Email = @Email";
        //            sqlCommand.Parameters.AddWithValue("@Email", gebruikerLogin.Email);

        //            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
        //            GebruikerV2 gebruikerDb = new GebruikerV2();
        //            while (reader.Read())
        //            {
        //                gebruikerDb.Email = reader["Email"].ToString();
        //                gebruikerDb.GebruikerId = Guid.Parse(reader["GebruikersId"].ToString());
        //                gebruikerDb.GebruikersNaam = reader["GebruikersNaam"].ToString();

        //            }

        //            if (gebruikerDb.Email != null)
        //            {
        //                gebruikerLogin.GebruikerId = gebruikerDb.GebruikerId;
        //                gebruikerLogin.GebruikersNaam = gebruikerDb.GebruikersNaam;

        //                return new OkObjectResult(gebruikerLogin);
        //            }

        //            else
        //            {
        //                // no user in db with this email
        //                CustomResponse customResponse = new CustomResponse();
        //                customResponse.Succes = false;
        //                customResponse.Message = "Geen gebruiker gevonden in de db";
        //                return new OkObjectResult(customResponse);
        //            }
        //        }
        //    }



        //}

        [FunctionName("AccountLogin")]
        public static async Task<IActionResult> AccountLogin(
            [HttpTrigger(AuthorizationLevel.Admin, "post", "get", Route = "gebruikers/login")] HttpRequest req,
            ILogger log)
        {

            //Account login:
            //naar api sturen email json gebruikersnaam verplicht, vb:
            //{
            //"email": "test1@email.com",
            //"name": "test1"
            //}

            //mogelijke returns:
            //gebruiker gevonden: gebruikerv2 met alle params
            //geen account gevonden: account wordt aangemaakt en returnd gebruikerv2 met alle params
            //de return values kunnen aangepast worden natuurlijk, deze zijn als vb

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                GebruikerV2 gebruikerLogin = JsonConvert.DeserializeObject<GebruikerV2>(requestBody);


                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "SELECT * FROM Gebruikers WHERE Email = @Email";
                        sqlCommand.Parameters.AddWithValue("@Email", gebruikerLogin.Email);

                        SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
                        GebruikerV2 gebruikerDb = new GebruikerV2();
                        while (reader.Read())
                        {
                            gebruikerDb.Email = reader["Email"].ToString();
                            gebruikerDb.GebruikerId = Guid.Parse(reader["GebruikersId"].ToString());
                            gebruikerDb.Name = reader["GebruikersNaam"].ToString();

                        }

                        if (gebruikerDb.Email != null)
                        {
                            //gebruiker bestaat al

                            //checken als username aangepast is
                            if (gebruikerLogin.Name != null && gebruikerLogin.Name != gebruikerDb.Name)
                            {
                                //gebruikersname moet geupdate worden
                                using (SqlConnection sqlConnectionUpdate = new SqlConnection(connectionString))
                                {
                                    await sqlConnectionUpdate.OpenAsync();
                                    using (SqlCommand sqlCommandUpdate = new SqlCommand())
                                    {
                                        sqlCommandUpdate.Connection = sqlConnection;
                                        sqlCommandUpdate.CommandText = "UPDATE Gebruikers SET GebruikersNaam = @GebruikersNaam  WHERE Email = @Email";
                                        sqlCommandUpdate.Parameters.AddWithValue("@GebruikersNaam", gebruikerLogin.Name);
                                        sqlCommandUpdate.Parameters.AddWithValue("@Email", gebruikerLogin.Email);
                                        await sqlCommandUpdate.ExecuteNonQueryAsync();

                                        gebruikerDb.Name = gebruikerLogin.Name;
                                    }
                                }
                            }


                            gebruikerLogin.GebruikerId = gebruikerDb.GebruikerId;
                            gebruikerLogin.Name = gebruikerDb.Name;

                            return new OkObjectResult(gebruikerLogin);
                        }

                        else
                        {
                            //gebruiker bestond nog niet
                            gebruikerLogin.GebruikerId = Guid.NewGuid();
                            using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionString))
                            {
                                await sqlConnectionInsert.OpenAsync();
                                using (SqlCommand sqlCommandInsert = new SqlCommand())
                                {
                                    sqlCommandInsert.Connection = sqlConnectionInsert;
                                    sqlCommandInsert.CommandText = "INSERT INTO Gebruikers VALUES (@GebruikerId, @GebruikersNaam, @Email)";
                                    sqlCommandInsert.Parameters.AddWithValue("@GebruikerId", gebruikerLogin.GebruikerId);
                                    sqlCommandInsert.Parameters.AddWithValue("@GebruikersNaam", gebruikerLogin.Name);
                                    sqlCommandInsert.Parameters.AddWithValue("@Email", gebruikerLogin.Email);

                                    await sqlCommandInsert.ExecuteNonQueryAsync();
                                    // return gemaakte gebruiker
                                    return new OkObjectResult(gebruikerLogin);
                                }

                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult(); ;
            }
        }


        [FunctionName("CreateRonde")]
        public static async Task<IActionResult> CreateRonde(
           [HttpTrigger(AuthorizationLevel.Admin, "post", Route = "rondes")] HttpRequest req,
           ILogger log)
        {

            //Account create:
            //naar api sturen email en gebruikersnaam via json gebruikersnaam is optioneel, vb:
            //{
            //"Admin": "ee6c128e-0b7e-4d8f-b139-6a40308e112d",
            //"naam": "testronde1",
            //"startdatum" : "2021-01-05T15:00:00"
            //}

            //mogelijke returns:
            //als ronde gemaakt is wordt het model ronde met alle params terug gestuurd

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Ronde ronde = JsonConvert.DeserializeObject<Ronde>(requestBody);

                // genereer nieuwe invite code
                ronde.RondeId = Guid.NewGuid();
                ronde.EindDatum = DateTime.ParseExact("9999-12-31 11:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                ronde.StartDatum = DateTime.Now;
                string inviteCode = RandomCharGen.RandomString(8);
                bool inviteIsValid = false;

                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                // check if invitecode doesn't already exist in db
                while (inviteIsValid is false)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        await sqlConnection.OpenAsync();
                        using (SqlCommand sqlCommand = new SqlCommand())
                        {
                            sqlCommand.Connection = sqlConnection;
                            sqlCommand.CommandText = "SELECT * FROM Rondes WHERE InviteCode = @InviteCode";
                            sqlCommand.Parameters.AddWithValue("@InviteCode", inviteCode);

                            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
                            Ronde rondeDb = new Ronde();
                            while (reader.Read())
                            {
                                rondeDb.InviteCode = reader["InviteCode"].ToString();

                            }

                            if (rondeDb.InviteCode == null)
                            {
                                ronde.InviteCode = inviteCode;
                                inviteIsValid = true;
                            }

                        }
                    }
                }



                string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
                using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
                {
                    await sqlConnectionInsert.OpenAsync();
                    using (SqlCommand sqlCommandInsert = new SqlCommand())
                    {
                        sqlCommandInsert.Connection = sqlConnectionInsert;
                        sqlCommandInsert.CommandText = "INSERT INTO Rondes VALUES(@RondeId, @Naam, @Admin , @InviteCode, @StartDatum, @EindDatum)";
                        sqlCommandInsert.Parameters.AddWithValue("@RondeId", ronde.RondeId);
                        sqlCommandInsert.Parameters.AddWithValue("@Naam", ronde.Naam);
                        sqlCommandInsert.Parameters.AddWithValue("@Admin", ronde.Admin);
                        sqlCommandInsert.Parameters.AddWithValue("@InviteCode", ronde.InviteCode);
                        sqlCommandInsert.Parameters.AddWithValue("@StartDatum", ronde.StartDatum);
                        sqlCommandInsert.Parameters.AddWithValue("@EindDatum", ronde.EindDatum);



                        await sqlCommandInsert.ExecuteNonQueryAsync();

                        // add admin to deelnemer van de ronde
                        Deelnemer deelnemer = new Deelnemer();
                        deelnemer.RondeId = ronde.RondeId;
                        deelnemer.GebruikerId = ronde.Admin;

                        sqlCommandInsert.Connection = sqlConnectionInsert;
                        sqlCommandInsert.CommandText = "INSERT INTO Deelnemers VALUES(@GebruikerId_2, @RondeId_2, 1)";
                        sqlCommandInsert.Parameters.AddWithValue("@RondeId_2", deelnemer.RondeId);
                        sqlCommandInsert.Parameters.AddWithValue("@GebruikerId_2", deelnemer.GebruikerId);

                        //deelnemer.RondeId = ronde.RondeId;

                        await sqlCommandInsert.ExecuteNonQueryAsync();


                        return new OkObjectResult(ronde);

                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult(); ;
            }
        }


        [FunctionName("UpdateRonde")]
        public static async Task<IActionResult> UpdateRonde(
           [HttpTrigger(AuthorizationLevel.Admin, "put", Route = "rondes")] HttpRequest req,
           ILogger log)
        {

            //Account create:
            //naar api sturen email en gebruikersnaam via json gebruikersnaam is optioneel, vb:
            //{
            //"rondeId": "4dca58c9-7e62-4bcb-8b05-b81b15a1ba75",
            //"naam": "testronde1",
            //"startDatum": "2021-01-05T15:00:00",
            //"eindDatum": "2020-12-31T11:59:59"
            //}

            //mogelijke returns:
            //als ronde gemaakt is wordt het model ronde met alle params terug gestuurd

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Ronde ronde = JsonConvert.DeserializeObject<Ronde>(requestBody);


                string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
                using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
                {
                    await sqlConnectionInsert.OpenAsync();
                    using (SqlCommand sqlCommandInsert = new SqlCommand())
                    {
                        sqlCommandInsert.Connection = sqlConnectionInsert;
                        sqlCommandInsert.CommandText = "UPDATE Rondes SET Naam = @Naam,StartDatum = @StartDatum,EindDatum = @EindDatum WHERE RondeId = @RondeId";
                        sqlCommandInsert.Parameters.AddWithValue("@RondeId", ronde.RondeId);
                        sqlCommandInsert.Parameters.AddWithValue("@Naam", ronde.Naam);
                        sqlCommandInsert.Parameters.AddWithValue("@StartDatum", ronde.StartDatum);
                        sqlCommandInsert.Parameters.AddWithValue("@EindDatum", ronde.EindDatum);



                        await sqlCommandInsert.ExecuteNonQueryAsync();

                        return new OkObjectResult(ronde);

                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult(); ;
            }
        }

        [FunctionName("AddDeelnemerToRonde")]
        public static async Task<IActionResult> AddDeelnemerToRonde(
          [HttpTrigger(AuthorizationLevel.Admin, "post", Route = "deelnemer")] HttpRequest req,
          ILogger log)
        {

            //{
            //    "invitecode": "azertyui",
            //    "gebruikerId": "986c5f65-cafc-43ce-8fa5-bf4f97921e92"
            //}


            //mogelijke returns:
            //als ronde gemaakt is wordt het model deelnemer met alle params terug gestuurd

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Deelnemer deelnemer = JsonConvert.DeserializeObject<Deelnemer>(requestBody);
                Ronde ronde = JsonConvert.DeserializeObject<Ronde>(requestBody);

                //controleren als invitecode bestaat
                string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
                using (SqlConnection sqlConnection = new SqlConnection(connectionStringInsert))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "SELECT * FROM Rondes WHERE InviteCode = @InviteCode";
                        sqlCommand.Parameters.AddWithValue("@InviteCode", ronde.InviteCode);

                        SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
                        Ronde rondeDb = new Ronde();
                        while (reader.Read())
                        {

                            rondeDb.RondeId = Guid.Parse(reader["RondeId"].ToString());

                        }
                        if (rondeDb.RondeId != null && rondeDb.RondeId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                        {
                            //invitecode bestaat
                            deelnemer.RondeId = rondeDb.RondeId;

                            //controleren als gebruiker al mee doet aan de ronde
                            List<Deelnemer> deelnemers = new List<Deelnemer>();
                            using (SqlConnection sqlConnectionDeelnemerCheck = new SqlConnection(connectionStringInsert))
                            {
                                await sqlConnectionDeelnemerCheck.OpenAsync();
                                using (SqlCommand sqlCommandDeelnemerCheck = new SqlCommand())
                                {
                                    sqlCommandDeelnemerCheck.Connection = sqlConnectionDeelnemerCheck;
                                    sqlCommandDeelnemerCheck.CommandText = "SELECT * FROM Deelnemers WHERE RondeId = @RondeId and GebruikersId = @GebruikerId";
                                    sqlCommandDeelnemerCheck.Parameters.AddWithValue("@RondeId", deelnemer.RondeId);
                                    sqlCommandDeelnemerCheck.Parameters.AddWithValue("@GebruikerId", deelnemer.GebruikerId);

                                    SqlDataReader readerDeelnemerCheck = await sqlCommandDeelnemerCheck.ExecuteReaderAsync();

                                    while (readerDeelnemerCheck.Read())
                                    {
                                        Deelnemer data = new Deelnemer();
                                        data.GebruikerId = Guid.Parse(readerDeelnemerCheck["GebruikersId"].ToString());
                                        data.IsActief = bool.Parse(readerDeelnemerCheck["IsActief"].ToString());
                                        deelnemers.Add(data);
                                    }
                                }

                            }

                            List<LapTijd> laptijden = new List<LapTijd>();

                            //Controleren of er al een lap gereden is binnen een etappe
                            using (SqlConnection sqlConnectionEtappesCheck = new SqlConnection(connectionStringInsert))
                            {
                                await sqlConnectionEtappesCheck.OpenAsync();
                                using (SqlCommand sqlCommandEtappesCheck = new SqlCommand())
                                {
                                    sqlCommandEtappesCheck.Connection = sqlConnectionEtappesCheck;
                                    sqlCommandEtappesCheck.CommandText = "select l.LapTijdId from Etappes as e right join LapTijden as l on e.EtappeId = l.EtappeId join Rondes as r on r.RondeId = e.RondeId where r.RondeId = @RondeId";
                                    sqlCommandEtappesCheck.Parameters.AddWithValue("@RondeId", rondeDb.RondeId);

                                    SqlDataReader readerEtappesCheck = await sqlCommandEtappesCheck.ExecuteReaderAsync();

                                    while (readerEtappesCheck.Read())
                                    {

                                        LapTijd data = new LapTijd();
                                        data.LapTijdId = Guid.Parse(readerEtappesCheck["LapTijdId"].ToString());
                                        laptijden.Add(data);
                                    }
                                }
                            }

                            if(laptijden.Count() == 0)
                            {
                                

                                
                                //deelnemer doet nog niet mee --> je kan nog meedoen
                                if (deelnemers.Count() == 0)
                                {
                                    //Deelnemer toevoegen aan de ronde
                                    using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
                                    {
                                        await sqlConnectionInsert.OpenAsync();
                                        using (SqlCommand sqlCommandInsert = new SqlCommand())
                                        {
                                            sqlCommandInsert.Connection = sqlConnectionInsert;
                                            sqlCommandInsert.CommandText = "INSERT INTO Deelnemers VALUES( @GebruikerId, @RondeId, 1)";       
                                            sqlCommandInsert.Parameters.AddWithValue("@RondeId", deelnemer.RondeId);
                                            sqlCommandInsert.Parameters.AddWithValue("@GebruikerId", deelnemer.GebruikerId);

                                            //deelnemer.RondeId = ronde.RondeId;

                                            await sqlCommandInsert.ExecuteNonQueryAsync();
                                            deelnemer.IsActief = true;

                                            return new OkObjectResult(deelnemer);
                                        }
                                    }
                                }

                                // deelnemer heeft al eens meegedaan maar heeft de ronde al eens verlaten
                                if (deelnemers.Count() == 1 && deelnemers[0].IsActief == false)
                                {
                                    //deelnemer heeft al eens meegedaan, isactief = 0 en de deelnemer kan opnieuw meedoen:

                                    //deelnemer opnieuw toevoegen:
                                    using (SqlConnection sqlConnectionUpdate = new SqlConnection(connectionStringInsert))
                                    {
                                        await sqlConnectionUpdate.OpenAsync();
                                        using (SqlCommand sqlCommandUpdate = new SqlCommand())
                                        {
                                            sqlCommandUpdate.Connection = sqlConnectionUpdate;
                                            sqlCommandUpdate.CommandText = "UPDATE Deelnemers SET IsActief = 1 where RondeId = @RondeId and GebruikersId = @GebruikersId"; ;
                                            sqlCommandUpdate.Parameters.AddWithValue("@RondeId", deelnemer.RondeId);
                                            sqlCommandUpdate.Parameters.AddWithValue("@GebruikersId", deelnemer.GebruikerId);

                                            //deelnemer.RondeId = ronde.RondeId;

                                            await sqlCommandUpdate.ExecuteNonQueryAsync();
                                            deelnemer.IsActief = true;

                                            return new OkObjectResult(deelnemer);
                                        }
                                    }


                                }


                                else
                                {

                                    //Als de deelnemer al meedoet controleren als Is
                                    //Als de deelenemer meedoet: je kan maar 1 keer meedoen :p
                                    CustomResponse customResponse = new CustomResponse();
                                    customResponse.Succes = false;
                                    customResponse.Message = "Je doet al mee aan deze ronde.";
                                    return new OkObjectResult(customResponse);
                                    
                                }

                                
                            }

                            else
                            {
                                //Als er wel al een lap gereden is kan je niet meer meedoen.
                                CustomResponse customResponse = new CustomResponse();
                                customResponse.Succes = false;
                                customResponse.Message = "Invitecode is niet meer geldig.";
                                return new OkObjectResult(customResponse);
                            }


                            
                        }

                        else
                        {
                            CustomResponse customResponse = new CustomResponse();
                            customResponse.Succes = false;
                            customResponse.Message = "Invitecode betsaat niet";
                            return new OkObjectResult(customResponse);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult(); ;
            }
        }


        //[FunctionName("DelDeelnemerfromRondeIsActief")]
        //public static async Task<IActionResult> DelDeelnemerfromRondeIsActief(
        //  [HttpTrigger(AuthorizationLevel.Admin, "put", Route = "deelnemer/{DeelnemerId}")] HttpRequest req,
        //  ILogger log, Guid DeelnemerId)
        //{
        //    try
        //    {
                
        //        string connectionStringDel = Environment.GetEnvironmentVariable("ConnectionString");
        //        using (SqlConnection sqlConnectionDel = new SqlConnection(connectionStringDel))
        //        {
        //            await sqlConnectionDel.OpenAsync();
        //            using (SqlCommand sqlCommandDel = new SqlCommand())
        //            {
        //                sqlCommandDel.Connection = sqlConnectionDel;
        //                sqlCommandDel.CommandText = "UPDATE Deelnemers SET IsActief = 0 WHERE DeelnemerId = @DeelnemerId";
        //                sqlCommandDel.Parameters.AddWithValue("@DeelnemerId", DeelnemerId);




        //                await sqlCommandDel.ExecuteNonQueryAsync();

        //                CustomResponse customResponse = new CustomResponse();
        //                customResponse.Succes = true;
        //                return new OkObjectResult(customResponse);

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine($"Error: {ex}");
        //        return new BadRequestResult(); ;
        //    }


        //}


        [FunctionName("DelDeelnemerfromRondeIsActief")]
        public static async Task<IActionResult> DelDeelnemerfromRondeIsActief(
          [HttpTrigger(AuthorizationLevel.Admin, "put", Route = "deelnemer/{GebruikerId}/{RondeId}")] HttpRequest req,
          ILogger log, Guid GebruikerId, Guid RondeId)
        {
            try
            {
                string connectionStringDel = Environment.GetEnvironmentVariable("ConnectionString");
                //deelnemerid ophalen:
                using (SqlConnection sqlConnection = new SqlConnection(connectionStringDel))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "UPDATE Deelnemers SET IsActief = 0 where RondeId = @RondeId and GebruikersId = @GebruikersId";
                        sqlCommand.Parameters.AddWithValue("@RondeId", RondeId);
                        sqlCommand.Parameters.AddWithValue("@GebruikersId", GebruikerId);
                        await sqlCommand.ExecuteNonQueryAsync();

                        
                        CustomResponse customResponse = new CustomResponse();
                        customResponse.Succes = true;
                        return new OkObjectResult(customResponse);
                    }
                }


                
                
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult(); ;
            }


        }



        [FunctionName("CreateEtappe")]
        public static async Task<IActionResult> CreateEtappe(
           [HttpTrigger(AuthorizationLevel.Admin, "post", Route = "etappes")] HttpRequest req,
           ILogger log)
        {

            //Account create:
            //naar api sturen email en gebruikersnaam via json gebruikersnaam is optioneel, vb:
            //{
            //"laps" : 3,
            //"rondeid": "55c83dc3-8bc6-43de-b1ae-50fdf9a10590",
            //"lapafstand": 102.33,
            //"starttijd": "2021-01-05T15:00:00"
            //}

            //mogelijke returns:
            //als etappe gemaakt is wordt het model etappe met alle params terug gestuurd

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Etappe etappe = JsonConvert.DeserializeObject<Etappe>(requestBody);
                etappe.EtappeId = Guid.NewGuid();


                string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
                using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
                {
                    await sqlConnectionInsert.OpenAsync();
                    using (SqlCommand sqlCommandInsert = new SqlCommand())
                    {
                        sqlCommandInsert.Connection = sqlConnectionInsert;
                        sqlCommandInsert.CommandText = "INSERT INTO Etappes VALUES(@EtappeId, @Laps, @RondeId, @StartTijd, 333.33, 1)";
                        sqlCommandInsert.Parameters.AddWithValue("@EtappeId", etappe.EtappeId);
                        sqlCommandInsert.Parameters.AddWithValue("@Laps", etappe.Laps);
                        sqlCommandInsert.Parameters.AddWithValue("@RondeId", etappe.RondeId);
                        sqlCommandInsert.Parameters.AddWithValue("@StartTijd", etappe.StartTijd);

                        etappe.LapAfstand = float.Parse("333,33".ToString());
                        await sqlCommandInsert.ExecuteNonQueryAsync();

                        return new OkObjectResult(etappe);

                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult(); ;
            }

        }


        [FunctionName("StopEtappe")]
        public static async Task<IActionResult> StopEtappe(
          [HttpTrigger(AuthorizationLevel.Admin, "put", Route = "etappe/{etappeId}")] HttpRequest req,
          ILogger log, Guid etappeId)
        {
            try
            {
                CustomResponse customResponse = new CustomResponse();
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "UPDATE Etappes SET IsActief = 0 WHERE EtappeId = @EtappeId";
                        sqlCommand.Parameters.AddWithValue("@EtappeId", etappeId);
                        await sqlCommand.ExecuteNonQueryAsync();

                        List<Gebruiker> gebruikers = await GetDeelnemersRonde(etappeId, connectionString);

                        foreach(Gebruiker g in gebruikers)
                        {
                            DisableDeelnemer(g.GebruikerId, connectionString);
                        }

                        customResponse.Succes = true;

                    }

                }
                return new OkObjectResult(customResponse);



            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult(); ;
            }
        }


        public static async Task<List<Gebruiker>> GetDNFDeelnemers(Guid etappeId, string connString)
        {
            List<Gebruiker> gebruikers = new List<Gebruiker>();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    string sql = "select d.IsActief, l.GebruikerId, l.EtappeId from LapTijden as l join Gebruikers as g on g.GebruikersId = l.GebruikerId join Etappes as e on e.EtappeId = l.EtappeId join Rondes as r on r.RondeId = e.RondeId join Deelnemers as d on d.GebruikersId = g.GebruikersId and d.RondeId = r.RondeId where l.EtappeId = @etappeId group by l.GebruikerId, g.GebruikersNaam, l.EtappeId, e.Laps, d.IsActief having count(l.TijdLap) < e.Laps;";
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@etappeId", etappeId);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Gebruiker data = new Gebruiker();
                        data.GebruikerId = Guid.Parse(reader["GebruikerId"].ToString());
                        gebruikers.Add(data);
                    }
                }
            }
            return gebruikers;
        }

        public static async Task<int> DisableDeelnemer(Guid userId, string connString)
        {

            int response = 0;
            using (SqlConnection connection = new SqlConnection(connString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    string sql = "update Deelnemers set IsActief = 0 where GebruikersId = @userId";
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@userId", userId);

                     response = await command.ExecuteNonQueryAsync();
                }
            }
            return response;
        }



        [FunctionName("AddLaptijd")]
        public static async Task<IActionResult> AddLaptijd(
           [HttpTrigger(AuthorizationLevel.Admin, "post", Route = "laptijden")] HttpRequest req,
           ILogger log)
        {

            //Account create:
            //naar api sturen email en gebruikersnaam via json gebruikersnaam is optioneel, vb:
            //{
            //"etappeId": "b3a5f165-3011-4fdb-bba3-8c888e7a15d9",
            //"gebruikerid": "f35d476e-55c8-484a-8b12-fb52da1c1413",
            //"tijdlap": 95,
            //"lapnummer": 1
            //}

            //mogelijke returns:
            //als laptijd gemaakt is wordt het model laptijd met alle params terug gestuurd
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                //var jarray = JArray.Parse(requestBody);
                List<LapTijd> lapTijden = JsonConvert.DeserializeObject<List<LapTijd>>(requestBody);
                foreach (LapTijd lapTijd in lapTijden)
                {
                    lapTijd.LapTijdId = Guid.NewGuid();
                    Console.WriteLine(lapTijd.EtappeId);

                    string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
                    using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
                    {
                        await sqlConnectionInsert.OpenAsync();
                        using (SqlCommand sqlCommandInsert = new SqlCommand())
                        {
                            sqlCommandInsert.Connection = sqlConnectionInsert;
                            sqlCommandInsert.CommandText = "INSERT INTO LapTijden VALUES(@LapTijdId, @EtappeId, @GebruikerId, @TijdLap, @LapNummer)";
                            sqlCommandInsert.Parameters.AddWithValue("@LapTijdId", lapTijd.LapTijdId);
                            sqlCommandInsert.Parameters.AddWithValue("@EtappeId", lapTijd.EtappeId);
                            sqlCommandInsert.Parameters.AddWithValue("@GebruikerId", lapTijd.GebruikerId);
                            sqlCommandInsert.Parameters.AddWithValue("@TijdLap", lapTijd.TijdLap);
                            sqlCommandInsert.Parameters.AddWithValue("@LapNummer", lapTijd.LapNummer);



                            await sqlCommandInsert.ExecuteNonQueryAsync();

                        }
                    }
                }


                CustomResponse customResponse = new CustomResponse();
                customResponse.Succes = true;
                customResponse.Message = "Laptijden succesvol opgeslagen.";
                return new OkObjectResult(customResponse);
                
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult(); ;
            }
        }

        [FunctionName("DelLapTijd")]
        public static async Task<IActionResult> DelLapTijd(
          [HttpTrigger(AuthorizationLevel.Admin, "delete", Route = "laptijden/{LaptijdId}")] HttpRequest req,
          ILogger log, Guid LaptijdId)
        {
            try
            {
                CustomResponse customResponse = await DeleteFunctions.DelLapTijdFunction(LaptijdId);

                return new OkObjectResult(customResponse);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult(); ;
            }

        }


        //[FunctionName("DelEtappeIsActief")]
        //public static async Task<IActionResult> DelEtappeIsActief(
        //  [HttpTrigger(AuthorizationLevel.Admin, "put", Route = "etappe/{EtappeId}")] HttpRequest req,
        //  ILogger log, Guid EtappeId)
        //{
        //    try
        //    {
        //        string connectionStringDel = Environment.GetEnvironmentVariable("ConnectionString");
        //        using (SqlConnection sqlConnectionDel = new SqlConnection(connectionStringDel))
        //        {
        //            await sqlConnectionDel.OpenAsync();
        //            using (SqlCommand sqlCommandDel = new SqlCommand())
        //            {
        //                sqlCommandDel.Connection = sqlConnectionDel;
        //                sqlCommandDel.CommandText = "UPDATE Etappes SET IsActief = 0 WHERE EtappeId = @EtappeId";
        //                sqlCommandDel.Parameters.AddWithValue("@EtappeId", EtappeId);




        //                await sqlCommandDel.ExecuteNonQueryAsync();

        //                CustomResponse customResponse = new CustomResponse();
        //                customResponse.Succes = true;
        //                return new OkObjectResult(customResponse);

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine($"Error: {ex}");
        //        return new BadRequestResult(); ;
        //    }

        //}


        [FunctionName("DelRonde")]
        public static async Task<IActionResult> DelRonde(
         [HttpTrigger(AuthorizationLevel.Admin, "delete", Route = "rondes/{RondeId}")] HttpRequest req,
         ILogger log, Guid RondeId)
        {
            try
            {
                CustomResponse customResponse = await DeleteFunctions.DelRondeFunction(RondeId);
                return new OkObjectResult(customResponse);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult(); ;
            }

        }

        [FunctionName("GetDeelnemersRonde")]
        public static async Task<IActionResult> GetDeelnemersRonde(
       [HttpTrigger(AuthorizationLevel.Admin, "get", Route = "ronde/deelnemers/{RondeId}")] HttpRequest req, Guid RondeId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                List<DeelnemerRonde> deelnemers = new List<DeelnemerRonde>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "select d.GebruikersId, g.GebruikersNaam from Deelnemers as d join Rondes as r on r.RondeId = d.RondeId join Gebruikers as g on d.GebruikersId = g.GebruikersId where d.RondeId = @rondeId and r.Admin != d.GebruikersId group by d.GebruikersId, g.GebruikersNaam;";
                        command.Parameters.AddWithValue("@rondeId", RondeId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            DeelnemerRonde data = new DeelnemerRonde();
                            data.GebruikersId = Guid.Parse(reader["GebruikersId"].ToString());
                            data.GebruikersNaam = reader["GebruikersNaam"].ToString();

                            deelnemers.Add(data);

                        }
                    }
                }
                return new OkObjectResult(deelnemers);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        //Toont alle rondes van een gebruiker - voor in xamarin app
        [FunctionName("GetRondesUser")]
        public static async Task<IActionResult> GetRondesUser(
       [HttpTrigger(AuthorizationLevel.Admin, "get", Route = "gebruiker/rondes/{UserId}")] HttpRequest req, Guid UserId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                List<RondesGebruiker> rondes = new List<RondesGebruiker>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select g.GebruikersId,r.Admin, r.InviteCode, r.RondeId, r.Naam as 'RondeNaam', r.StartDatum, count(e.etappeId) as 'AantalEtappes' from  Gebruikers as g join Deelnemers as d on d.GebruikersId = g.GebruikersId join Rondes as r on r.RondeId = d.RondeId left join Etappes as e on e.RondeId = r.RondeId where d.GebruikersId = @userId and d.IsActief = 1 group by g.GebruikersId, r.Admin, g.GebruikersNaam, g.Email, r.RondeId, r.Naam, r.StartDatum, r.InviteCode order by r.StartDatum desc";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@userId", UserId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            RondesGebruiker data = new RondesGebruiker();
                            data.GebruikersId = Guid.Parse(reader["GebruikersId"].ToString());
                            data.StartDatum = DateTime.Parse(reader["StartDatum"].ToString());
                            data.InviteCode = reader["InviteCode"].ToString();
                            data.RondeNaam = reader["RondeNaam"].ToString();
                            data.RondeId = Guid.Parse(reader["RondeId"].ToString());
                            data.AantalEtappes = int.Parse(reader["AantalEtappes"].ToString());
                            data.Admin = Guid.Parse(reader["Admin"].ToString());

                            //Ophalen andere query die per ronde de totaaltijd in een ronde ophaalt. Zo kunnen we per ronde van de gebruiker ook zijn tijd en positie weergeven.

                            List<RondesGebruiker> rondeKlassement = await GetRondeKlassementAsync(data.RondeId, connectionString);
                            RondesGebruiker obj = rondeKlassement.Where(obj => obj.GebruikersId == data.GebruikersId).FirstOrDefault();

                            if (obj != null)
                            {
                                data.TotaalTijd = obj.TotaalTijd;
                                data.Plaats = obj.Plaats;
                            }

                            rondes.Add(data);
                        }
                    }
                }
                return new OkObjectResult(rondes);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        [FunctionName("GetRondesAdmin")]
        public static async Task<IActionResult> GetRondesAdmin(
[HttpTrigger(AuthorizationLevel.Admin, "get", Route = "site/admin/rondes/{UserId}")] HttpRequest req, Guid UserId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                List<RondesAdmin> rondes = new List<RondesAdmin>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select r.RondeId, r.Naam as 'RondeNaam', r.Admin, r.StartDatum, Count(e.EtappeId) as 'AantalEtappes' from Rondes as r left join Gebruikers as g on g.GebruikersId = r.Admin left join Etappes as e on e.RondeId = r.RondeId where r.Admin = @userId and e.IsActief = 0 group by r.RondeId, r.Naam, r.Admin, r.StartDatum order by r.StartDatum desc;";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@userId", UserId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            RondesAdmin data = new RondesAdmin();
                            data.RondeId = Guid.Parse(reader["RondeId"].ToString());
                            data.RondeNaam = reader["RondeNaam"].ToString();
                            data.Admin = Guid.Parse(reader["Admin"].ToString());
                            data.StartDatum = DateTime.Parse(reader["StartDatum"].ToString());
                            data.AantalEtappes = int.Parse(reader["AantalEtappes"].ToString());                          

                            rondes.Add(data);
                        }
                    }
                }
                return new OkObjectResult(rondes);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }




        //Toont alle rondes van een gebruiker, telt enkel de niet actieve etappes mee - voor website
        [FunctionName("GetRondesUserSite")]
        public static async Task<IActionResult> GetRondesUserSite(
[HttpTrigger(AuthorizationLevel.Admin, "get", Route = "site/gebruiker/rondes/{UserId}")] HttpRequest req, Guid UserId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                List<RondesGebruiker> rondes = new List<RondesGebruiker>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select g.GebruikersId,r.Admin, r.InviteCode, r.RondeId, r.Naam as 'RondeNaam', r.StartDatum, count(e.etappeId) as 'AantalEtappes' from  Gebruikers as g join Deelnemers as d on d.GebruikersId = g.GebruikersId join Rondes as r on r.RondeId = d.RondeId left join Etappes as e on e.RondeId = r.RondeId where d.GebruikersId = @userId and d.IsActief = 1 and e.IsActief = 0 and r.Admin != @userId group by g.GebruikersId, r.Admin, g.GebruikersNaam, g.Email, r.RondeId, r.Naam, r.StartDatum, r.InviteCode order by r.StartDatum desc";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@userId", UserId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            RondesGebruiker data = new RondesGebruiker();
                            data.GebruikersId = Guid.Parse(reader["GebruikersId"].ToString());
                            data.StartDatum = DateTime.Parse(reader["StartDatum"].ToString());
                            data.InviteCode = reader["InviteCode"].ToString();
                            data.RondeNaam = reader["RondeNaam"].ToString();
                            data.RondeId = Guid.Parse(reader["RondeId"].ToString());
                            data.AantalEtappes = int.Parse(reader["AantalEtappes"].ToString());
                            data.Admin = Guid.Parse(reader["Admin"].ToString());

                            //Ophalen andere query die per ronde de totaaltijd in een ronde ophaalt. Zo kunnen we per ronde van de gebruiker ook zijn tijd en positie weergeven.

                            List<RondesGebruiker> rondeKlassement = await GetRondeKlassementAsync(data.RondeId, connectionString);
                            RondesGebruiker obj = rondeKlassement.Where(obj => obj.GebruikersId == data.GebruikersId).FirstOrDefault();

                            if (obj != null)
                            {
                                data.TotaalTijd = obj.TotaalTijd;
                                data.Plaats = obj.Plaats;
                            }

                            rondes.Add(data);
                        }
                    }
                }
                return new OkObjectResult(rondes);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        public static async Task<List<RondesGebruiker>> GetRondeKlassementAsync(Guid RondeId, string connString)
        {
            List<RondesGebruiker> rondeKlassement = new List<RondesGebruiker>();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    string sql = "select Row_number() OVER (order by Sum(l.TijdLap)) as 'Plaats', l.GebruikerId, g.GebruikersNaam, e.RondeId, r.Naam as 'RondeNaam', Sum(l.TijdLap) as 'TotaalTijd' from LapTijden as l join Gebruikers as g on g.GebruikersId = l.GebruikerId join Etappes as e on e.EtappeId = l.EtappeId join Rondes as r on r.RondeId = e.RondeId where e.RondeId = @rondeId group by l.GebruikerId, g.GebruikersNaam, e.RondeId, r.Naam order by Sum(l.TijdLap)";
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@rondeId", RondeId);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        RondesGebruiker data = new RondesGebruiker();
                        data.GebruikersId = Guid.Parse(reader["GebruikerId"].ToString());
                        data.Plaats = int.Parse(reader["Plaats"].ToString());
                        data.TotaalTijd = int.Parse(reader["TotaalTijd"].ToString());
                        rondeKlassement.Add(data);
                    }
                }
            }
            return rondeKlassement;
        }


        //Toont alle etappes van een ronde - voor in xamarin app en website
        [FunctionName("GetEtappesRonde")]
        public static async Task<IActionResult> GetEtappesRonde(
        [HttpTrigger(AuthorizationLevel.Admin, "get", Route = "gebruiker/ronde/etappes/{RondeId}/{UserId}")] HttpRequest req, Guid RondeId, Guid UserId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                List<EtappesRonde> etappes = new List<EtappesRonde>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select r.RondeId,r.Naam as 'RondeNaam',e.EtappeId, e.Laps, e.StartTijd, e.LapAfstand, r.Admin, e.IsActief as 'EtappeActief' from Rondes as r left join Deelnemers as d on d.RondeId = r.RondeId left join Etappes as e on r.RondeId = e.RondeId where e.RondeId = @rondeId and d.GebruikersId= @userId group by r.RondeId, r.Naam, e.Laps, e.EtappeId, e.StartTijd, e.LapAfstand, r.Admin, e.IsActief order by e.StartTijd desc;";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@userId", UserId);
                        command.Parameters.AddWithValue("@rondeId", RondeId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            EtappesRonde data = new EtappesRonde();
                            data.GebruikersId = UserId;
                            data.RondId = Guid.Parse(reader["RondeId"].ToString());
                            data.EtappeId = Guid.Parse(reader["EtappeId"].ToString());
                            data.Laps = int.Parse(reader["Laps"].ToString());
                            data.StartTijd = DateTime.Parse(reader["StartTijd"].ToString());
                            data.LapAfstand = double.Parse(reader["LapAfstand"].ToString());
                            data.Admin = Guid.Parse(reader["Admin"].ToString());
                            data.EtappeActief = bool.Parse(reader["EtappeActief"].ToString());
                            data.RondeNaam = reader["RondeNaam"].ToString();

                            //Ophalen andere query die per ronde de totaaltijd in een ronde ophaalt. Zo kunnen we per ronde van de gebruiker ook zijn tijd en positie weergeven.

                            List<EtappesRonde> etappeKlassement = await GetEtappeKlassement(data.EtappeId, connectionString);
                            EtappesRonde obj = etappeKlassement.Where(obj => obj.GebruikersId == data.GebruikersId).FirstOrDefault();

                            if (obj != null)
                            {
                                data.Plaats = obj.Plaats;
                                data.TotaalTijd = obj.TotaalTijd;
                                int snelsteTijd = etappeKlassement[0].TotaalTijd;
                                data.SnelsteTijd = snelsteTijd;
                            }

                            etappes.Add(data);
                        }
                    }
                }
                return new OkObjectResult(etappes);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        [FunctionName("GetEtappesRondeAdmin")]
        public static async Task<IActionResult> GetEtappesRondeAdmin(
        [HttpTrigger(AuthorizationLevel.Admin, "get", Route = "site/admin/ronde/etappes/{RondeId}")] HttpRequest req, Guid RondeId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                List<EtappesRonde> etappes = new List<EtappesRonde>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select r.RondeId,r.Naam as 'RondeNaam',e.EtappeId, e.Laps, e.StartTijd, e.LapAfstand, r.Admin, e.IsActief as 'EtappeActief' from Rondes as r left join Etappes as e on r.RondeId = e.RondeId where e.RondeId = @rondeId and e.IsActief = 0 group by r.RondeId, r.Naam, e.Laps, e.EtappeId, e.StartTijd, e.LapAfstand, r.Admin, e.IsActief order by e.StartTijd desc;";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@rondeId", RondeId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            EtappesRonde data = new EtappesRonde();
                            data.RondId = Guid.Parse(reader["RondeId"].ToString());
                            data.EtappeId = Guid.Parse(reader["EtappeId"].ToString());
                            data.Laps = int.Parse(reader["Laps"].ToString());
                            data.StartTijd = DateTime.Parse(reader["StartTijd"].ToString());
                            data.LapAfstand = double.Parse(reader["LapAfstand"].ToString());
                            data.Admin = Guid.Parse(reader["Admin"].ToString());
                            data.EtappeActief = bool.Parse(reader["EtappeActief"].ToString());
                            data.RondeNaam = reader["RondeNaam"].ToString();

                            //Ophalen andere query die per ronde de totaaltijd in een ronde ophaalt. Zo kunnen we per ronde van de gebruiker ook zijn tijd en positie weergeven.

                            List<EtappesRonde> etappeKlassement = await GetEtappeKlassement(data.EtappeId, connectionString);
                            EtappesRonde obj = etappeKlassement.FirstOrDefault();

                            if (obj != null)
                            {
                                data.SnelsteTijd = obj.TotaalTijd;
                            }

                            etappes.Add(data);
                        }
                    }
                }
                return new OkObjectResult(etappes);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        public static async Task<List<EtappesRonde>> GetEtappeKlassement(Guid EtappeId, string connString)
        {
            List<EtappesRonde> etappeKlassement = new List<EtappesRonde>();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    string sql = "select Row_number() OVER (order by Sum(l.TijdLap)) as 'Plaats', l.GebruikerId, g.GebruikersNaam, l.EtappeId,  Sum(l.TijdLap) as 'TotalTime' from LapTijden as l join Gebruikers as g on g.GebruikersId = l.GebruikerId where l.EtappeId = @etappeId group by  l.GebruikerId, g.GebruikersNaam, l.EtappeId order by Sum(l.TijdLap);";
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@etappeId", EtappeId);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        EtappesRonde data = new EtappesRonde();
                        data.GebruikersId = Guid.Parse(reader["GebruikerId"].ToString());
                        data.Plaats = int.Parse(reader["Plaats"].ToString());
                        data.EtappeId = Guid.Parse(reader["EtappeId"].ToString());
                        data.TotaalTijd = int.Parse(reader["TotalTime"].ToString());
                        etappeKlassement.Add(data);
                    }
                }
            }
            return etappeKlassement;
        }

        //[FunctionName("GetTotaalTijdRonde")]
        //public static async Task<IActionResult> GetTotaalTijdRonde(
        //[HttpTrigger(AuthorizationLevel.Admin, "get", Route = "gebruikers/ronde/{UserId}/{RondeId}/Totaaltijd")] HttpRequest req, Guid UserId, Guid RondeId, ILogger log)
        //{
        //    try
        //    {
        //        TotaalTijdRonde tijd = new TotaalTijdRonde();
        //        string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");

        //        using (SqlConnection connection = new SqlConnection(connectionStringInsert))
        //        {
        //            await connection.OpenAsync();
        //            using (SqlCommand command = new SqlCommand())
        //            {
        //                command.Connection = connection;
        //                string sql = "select Sum(l.TijdLap) as 'TotaalTijd' from LapTijden as l join Gebruikers as g on g.GebruikersId = l.GebruikerId join Etappes as e on e.EtappeId = l.EtappeId join Rondes as r on r.RondeId = e.RondeId where g.GebruikersId = @userId and e.RondeId = @rondeId  group by  g.GebruikersId, g.GebruikersNaam, e.RondeId, r.Naam order by Sum(l.TijdLap)";
        //                ;
        //                command.CommandText = sql;
        //                command.Parameters.AddWithValue("@userId", UserId);
        //                command.Parameters.AddWithValue("@rondeId", RondeId);
        //                SqlDataReader reader = command.ExecuteReader();

        //                while (reader.Read())
        //                {
        //                    tijd.TotaalTijd = int.Parse(reader["TotaalTijd"].ToString());

        //                }
        //            }
        //        }
        //        return new OkObjectResult(tijd);
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine($"Error: {ex}");
        //        return new OkObjectResult(ex);
        //    }

        //}

        
        [FunctionName("GetKlassementRonde")]
        public static async Task<IActionResult> GetKlassementRonde(
        [HttpTrigger(AuthorizationLevel.Admin, "get", Route = "klassement/rondes/{rondeId}")] HttpRequest req, Guid rondeId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                List<KlassementRonde> klassement = new List<KlassementRonde>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select Row_number() OVER (order by Sum(l.TijdLap)) as 'Plaats', l.GebruikerId, g.GebruikersNaam, e.RondeId, r.Naam as 'RondeNaam', Sum(l.TijdLap) as 'TotaalTijd' from LapTijden as l join Gebruikers as g on g.GebruikersId = l.GebruikerId join Etappes as e on e.EtappeId = l.EtappeId join Rondes as r on r.RondeId = e.RondeId join Deelnemers as d on d.GebruikersId = g.GebruikersId and d.RondeId = r.RondeId where e.RondeId = @rondeId and d.IsActief = 1 group by l.GebruikerId, g.GebruikersNaam, e.RondeId, r.Naam order by Sum(l.TijdLap);";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@rondeId", rondeId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            KlassementRonde data = new KlassementRonde();
                            data.Plaats = int.Parse(reader["Plaats"].ToString());
                            data.GebruikersId = Guid.Parse(reader["GebruikerId"].ToString());
                            data.GebruikersNaam = reader["GebruikersNaam"].ToString();
                            data.RondeId = Guid.Parse(reader["RondeId"].ToString());
                            data.RondeNaam = reader["RondeNaam"].ToString();
                            data.TotaalTijd = int.Parse(reader["TotaalTijd"].ToString());
                            klassement.Add(data);
                        }
                    }
                }
                return new OkObjectResult(klassement);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        [FunctionName("GetKlassementEtappe")]
        public static async Task<IActionResult> GetKlassementEtappe(
[HttpTrigger(AuthorizationLevel.Admin, "get", Route = "klassement/etappes/{etappeId}")] HttpRequest req, Guid etappeId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                List<KlassementEtappe> klassement = new List<KlassementEtappe>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select Row_number() OVER (order by Sum(l.TijdLap)) as 'Plaats', l.GebruikerId, g.GebruikersNaam, l.EtappeId,  Sum(l.TijdLap) as 'TotaalTijd' from LapTijden as l join Gebruikers as g on g.GebruikersId = l.GebruikerId join Etappes as e on e.EtappeId = l.EtappeId where l.EtappeId = @etappeId group by  l.GebruikerId, g.GebruikersNaam, l.EtappeId, e.Laps having count(l.TijdLap) = e.Laps order by Sum(l.TijdLap);";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@etappeId", etappeId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            KlassementEtappe data = new KlassementEtappe();
                            data.Plaats = int.Parse(reader["Plaats"].ToString());
                            data.GebruikersId = Guid.Parse(reader["GebruikerId"].ToString());
                            data.GebruikersNaam = reader["GebruikersNaam"].ToString();
                            data.EtappeId = Guid.Parse(reader["EtappeId"].ToString());
                            data.TotaalTijd = int.Parse(reader["TotaalTijd"].ToString());
                            klassement.Add(data);
                        }
                    }
                }
                return new OkObjectResult(klassement);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        [FunctionName("GetInfoEtappe")]
        public static async Task<IActionResult> GetInfoEtappe(
[HttpTrigger(AuthorizationLevel.Admin, "get", Route = "info/etappes/{etappeId}")] HttpRequest req, Guid etappeId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                EtappeInfo etappeInfo = new EtappeInfo();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    //Opvragen etappe info vd winnaar
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select top 1 r.Naam as 'RondeNaam', r.RondeId, l.EtappeId,  Sum(l.TijdLap) as 'TotaalTijd', (Sum(l.TijdLap)/CONVERT(FLOAT,(e.Laps))) as 'GemiddeldeLapTijd', (e.Laps * (e.LapAfstand / 1000)) as 'Afstand', e.Laps from LapTijden as l left join Etappes as e on e.EtappeId = l.EtappeId left join Rondes as r on r.RondeId = e.RondeId where l.EtappeId = @etappeId and e.IsActief = 0 group by  r.Naam, r.rondeId, l.GebruikerId, l.EtappeId, e.Laps, e.LapAfstand order by Sum(l.TijdLap);";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@etappeId", etappeId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            etappeInfo.RondeId = Guid.Parse(reader["RondeId"].ToString());
                            etappeInfo.RondeNaam = reader["RondeNaam"].ToString();
                            etappeInfo.EtappeId = Guid.Parse(reader["EtappeId"].ToString());
                            etappeInfo.TotaalTijd = int.Parse(reader["TotaalTijd"].ToString());
                            etappeInfo.GemiddeldeLapTijd = double.Parse(reader["GemiddeldeLapTijd"].ToString());
                            etappeInfo.Afstand = double.Parse(reader["Afstand"].ToString());
                            etappeInfo.Laps = int.Parse(reader["Laps"].ToString());
                        }
                    }
                    
                    //Opvragen aantal deelnemers in de etappe
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select l.GebruikerId as 'Deelnemers' from LapTijden as l where l.EtappeId = @etappeId group by l.EtappeId, l.GebruikerId;";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@etappeId", etappeId);
                        SqlDataReader reader = command.ExecuteReader();

                        List<Guid> Deelnemers = new List<Guid>();

                        while (reader.Read())
                        {
                            Guid data = new Guid();
                            data = Guid.Parse(reader["Deelnemers"].ToString());
                            Deelnemers.Add(data);
                        }

                        etappeInfo.AantalDeelnemers = Deelnemers.Count;
                    }
                }

                return new OkObjectResult(etappeInfo);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        [FunctionName("GetInfoEtappeGraphUser")]
        public static async Task<IActionResult> GetInfoEtappeGraphUser(
[HttpTrigger(AuthorizationLevel.Admin, "get", Route = "info/etappe/users/{etappeId}/{userId}")] HttpRequest req, Guid etappeId, Guid userId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                EtappeInfoUser etappeInfoUser = new EtappeInfoUser();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select g.GebruikersNaam, l.GebruikerId, r.Naam as 'RondeNaam', r.RondeId, l.EtappeId,  Sum(l.TijdLap) as 'TotaalTijd', (Sum(l.TijdLap)/CONVERT(FLOAT,(e.Laps))) as 'GemiddeldeLapTijd', min(l.TijdLap) as 'SnelsteLapTijd', max(l.TijdLap) as 'TraagsteLaptijd' from LapTijden as l left join Etappes as e on e.EtappeId = l.EtappeId left join Gebruikers as g on g.GebruikersId = l.GebruikerId left join Rondes as r on r.RondeId = e.RondeId where l.EtappeId = @etappeId and l.GebruikerId = @userId and e.IsActief = 0 group by g.GebruikersNaam, r.Naam, r.rondeId, l.GebruikerId, l.EtappeId, e.Laps, e.LapAfstand order by Sum(l.TijdLap)";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@etappeId", etappeId);
                        command.Parameters.AddWithValue("@userId", userId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            etappeInfoUser.GebruikersNaam = reader["GebruikersNaam"].ToString();
                            etappeInfoUser.GebruikerId = Guid.Parse(reader["GebruikerId"].ToString());
                            etappeInfoUser.RondeNaam = reader["RondeNaam"].ToString();
                            etappeInfoUser.RondeId = Guid.Parse(reader["RondeId"].ToString());
                            etappeInfoUser.EtappeId = Guid.Parse(reader["EtappeId"].ToString());
                            etappeInfoUser.TotaalTijd = int.Parse(reader["TotaalTijd"].ToString());
                            etappeInfoUser.GemiddeldeLaptijd = double.Parse(reader["GemiddeldeLapTijd"].ToString());
                            etappeInfoUser.SnelsteLapTijd = int.Parse(reader["SnelsteLapTijd"].ToString());
                            etappeInfoUser.TraagsteLapTijd = int.Parse(reader["TraagsteLapTijd"].ToString());
                        }
                    }
                }
                return new OkObjectResult(etappeInfoUser);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        [FunctionName("GetInfoEtappeGraphTimes")]
        public static async Task<IActionResult> GetInfoEtappeGraphTimes(
[HttpTrigger(AuthorizationLevel.Admin, "get", Route = "info/etappe/times/{etappeId}")] HttpRequest req, Guid etappeId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                EtappeInfoUser etappeInfoUser = new EtappeInfoUser();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select l.EtappeId, min(l.TijdLap) as 'SnelsteLapTijd', max(l.TijdLap) as 'TraagsteLaptijd' from LapTijden as l where l.EtappeId = @etappeId group by l.EtappeId;";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@etappeId", etappeId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            etappeInfoUser.EtappeId = Guid.Parse(reader["EtappeId"].ToString());
                            etappeInfoUser.SnelsteLapTijd = int.Parse(reader["SnelsteLapTijd"].ToString());
                            etappeInfoUser.TraagsteLapTijd = int.Parse(reader["TraagsteLapTijd"].ToString());
                        }
                    }
                }
                return new OkObjectResult(etappeInfoUser);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        [FunctionName("GetLapTijdenEtappeGraphUser")]
        public static async Task<IActionResult> GetLapTijdenEtappeGraphUser(
[HttpTrigger(AuthorizationLevel.Admin, "get", Route = "info/etappe/laptijden/users/{etappeId}/{userId}")] HttpRequest req, Guid etappeId, Guid userId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                List<EtappeLapTijdUser> etappeLapTijdUser = new List<EtappeLapTijdUser>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select l.EtappeId, e.StartTijd, e.Laps, l.GebruikerId, l.LapNummer, l.TijdLap from LapTijden as l left join Etappes as e on e.EtappeId = l.EtappeId where e.EtappeId = @etappeId and l.GebruikerId = @userId and e.IsActief = 0 group by l.EtappeId, e.Laps, l.GebruikerId , l.LapNummer, l.TijdLap, e.StartTijd order by l.GebruikerId, e.StartTijd, l.LapNummer";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@etappeId", etappeId);
                        command.Parameters.AddWithValue("@userId", userId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            EtappeLapTijdUser data = new EtappeLapTijdUser();
                            data.EtappeId = Guid.Parse(reader["EtappeId"].ToString());
                            data.GebruikerId = Guid.Parse(reader["GebruikerId"].ToString());
                            data.Laps = int.Parse(reader["Laps"].ToString());
                            data.LapNummer = int.Parse(reader["LapNummer"].ToString());
                            data.TijdLap = int.Parse(reader["TijdLap"].ToString());
                            etappeLapTijdUser.Add(data);
                        }
                    }
                }
                return new OkObjectResult(etappeLapTijdUser);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        [FunctionName("GetLapTijdenEtappeGraphAdmin")]
        public static async Task<IActionResult> GetLapTijdenEtappeGraphAdmin(
[HttpTrigger(AuthorizationLevel.Admin, "get", Route = "info/admin/etappe/laptijden/users/{etappeId}")] HttpRequest req, Guid etappeId, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                List<EtappeLapTijdAdmin> lapTijden = new List<EtappeLapTijdAdmin>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "select top 5 Row_number() OVER (order by Sum(l.TijdLap)) as 'Plaats', l.GebruikerId, g.GebruikersNaam, l.EtappeId,  Sum(l.TijdLap) as 'TotaalTijd' from LapTijden as l join Gebruikers as g on g.GebruikersId = l.GebruikerId join Etappes as e on e.EtappeId = l.EtappeId where l.EtappeId = @etappeId group by  l.GebruikerId, g.GebruikersNaam, l.EtappeId, e.Laps having count(l.TijdLap) = e.Laps order by Sum(l.TijdLap);";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@etappeId", etappeId);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            EtappeLapTijdAdmin data = new EtappeLapTijdAdmin();
                            data.Plaats = int.Parse(reader["Plaats"].ToString());
                            data.GebruikersId = Guid.Parse(reader["GebruikerId"].ToString());
                            data.GebruikersNaam = reader["GebruikersNaam"].ToString();
                            data.EtappeId = Guid.Parse(reader["EtappeId"].ToString());
                            data.LapTijden = await GetLapTijden(data.GebruikersId, etappeId, connectionString);

                            lapTijden.Add(data);
                        }
                    }
                }
              
                return new OkObjectResult(lapTijden);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                return new BadRequestResult();
            }

        }

        public static async Task<List<EtappeLapTijdUser>> GetLapTijden(Guid userdId, Guid etappeId, string connString)
        {
            List<EtappeLapTijdUser> etappeLapTijdUser = new List<EtappeLapTijdUser>();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    string sql = "select l.EtappeId, l.GebruikerId, g.GebruikersNaam, l.LapNummer, l.TijdLap from Laptijden as l join Gebruikers as g on g.GebruikersId = l.GebruikerId where g.GebruikersId = @userId and l.EtappeId = @etappeId group by l.EtappeId, l.GebruikerId, g.GebruikersNaam, g.GebruikersId, l.LapNummer, l.TijdLap order by l.LapNummer;";
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@etappeId", etappeId);
                    command.Parameters.AddWithValue("@userId", userdId);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        EtappeLapTijdUser data = new EtappeLapTijdUser();
                        data.EtappeId = Guid.Parse(reader["EtappeId"].ToString());
                        data.GebruikerId = Guid.Parse(reader["GebruikerId"].ToString());
                        data.LapNummer = int.Parse(reader["LapNummer"].ToString());
                        data.TijdLap = int.Parse(reader["TijdLap"].ToString());
                        etappeLapTijdUser.Add(data);
                    }
                }
            }
            return etappeLapTijdUser;
        }

    }
}


