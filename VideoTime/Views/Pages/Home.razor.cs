using Microsoft.Data.SqlClient;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Views.Pages
{
    public partial class Home
    {
        private List<VideoMetadata> videos = new List<VideoMetadata>();
        protected override void OnInitialized()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=VideoTimeDBCore;Trusted_Connection=True;MultipleActiveResultSets=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Title, BlobPath,Thubnail FROM VideoMetadatas"; // Select all videos

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            videos.Add(new VideoMetadata
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                BlobPath = reader.GetString(reader.GetOrdinal("BlobPath")),
                                Thubnail = reader.GetString(reader.GetOrdinal("Thubnail"))
                            });
                        }
                    }
                }
            }
        }
    }
}
