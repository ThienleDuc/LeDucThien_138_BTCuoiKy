using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class ChucVu
    {
        public ChucVu()
        {
        }

        [Key]
        public int MaChucVu { get; set; }

        [StringLength(50)]
        public string? TenChucVu { get; set; }

        public ChucVu(int maChucVu, string? tenChucVu)
        {
            MaChucVu = maChucVu;
            TenChucVu = tenChucVu;
        }
    }

    public class ChucVuRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public ChucVuRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<ChucVu> GetAllChucVu()
        {
            List<ChucVu> list = new List<ChucVu>();
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaChucVu, TenChucVu FROM ChucVu;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ChucVu
                        {
                            MaChucVu = reader.GetInt32(reader.GetOrdinal("MaChucVu")),
                            TenChucVu = reader["TenChucVu"]?.ToString()
                        });
                    }
                }
            }

            return list;
        }

        public ChucVu GetChucVuByID(int maChucVu)
        {
            ChucVu cv = null;

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaChucVu, TenChucVu FROM ChucVu WHERE MaChucVu = @MaChucVu;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaChucVu", maChucVu);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cv = new ChucVu
                        {
                            MaChucVu = reader.GetInt32(reader.GetOrdinal("MaChucVu")),
                            TenChucVu = reader["TenChucVu"]?.ToString()
                        };
                    }
                }
            }

            return cv;
        }

    }
}
