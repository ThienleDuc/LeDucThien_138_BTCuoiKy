using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class HocVi
    {
        public HocVi()
        {
        }

        [Key]
        public int MaHocVi { get; set; }

        [StringLength(50)]
        public string? TenHocVi { get; set; }

        public HocVi(int maHocVi, string? tenHocVi)
        {
            MaHocVi = maHocVi;
            TenHocVi = tenHocVi;
        }
    }

    public class HocViRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public HocViRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<HocVi> GetAllHocVi()
        {
            List<HocVi> list = new List<HocVi>();
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaHocVi, TenHocVi FROM HocVi;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new HocVi
                        {
                            MaHocVi = reader.GetInt32(reader.GetOrdinal("MaHocVi")),
                            TenHocVi = reader["TenHocVi"]?.ToString()
                        });
                    }
                }
            }

            return list;
        }
    }
}
