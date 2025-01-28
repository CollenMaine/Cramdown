using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace CramDown.Models
{
    public class FirestoreHelper
    {
        private string fireConfig = @"{
          ""type"": ""service_account"",
          ""project_id"": ""#################"",
          ""private_key_id"": ""#########################################"",
          ""private_key"": ""##################"",
          ""client_email"": ""#############################"",
          ""client_id"": ""#############################"",
          ""auth_uri"": ""##########################################"",
          ""token_uri"": ""#################################"",
          ""auth_provider_x509_cert_url"": ""########################"",
          ""client_x509_cert_url"": ""################################"",
          ""universe_domain"": ""googleapis.com""
        }";
        private FirestoreDb db;

        public FirestoreHelper()
        {
            GoogleCredential credentials = GoogleCredential.FromJson(fireConfig);
            db = FirestoreDb.Create("##########################3", new FirestoreClientBuilder 
            {
                Credential = credentials
            }.Build());
        }

        public FirestoreDb GetDatabase() => db;
    }
}
