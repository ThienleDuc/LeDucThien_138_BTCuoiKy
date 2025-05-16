using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class Khoa
    {
        public Khoa()
        {
        }

        [Key]
        public int MaKhoa { get; set; }

        [StringLength(50)]
        public string TenKhoa { get; set; }

        // Navigation property
        public ICollection<ChuyenNganh> ChuyenNganhs { get; set; }

        public Khoa(int maKhoa, string tenKhoa, ICollection<ChuyenNganh> chuyenNganhs)
        {
            MaKhoa = maKhoa;
            TenKhoa = tenKhoa;
            ChuyenNganhs = chuyenNganhs;
        }
    }

    public class KhoaRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public KhoaRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<Khoa> GetAllKhoa()
        {
            List<Khoa> list = new List<Khoa>();
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaKhoa, TenKhoa FROM Khoa;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Khoa
                        {
                            MaKhoa = reader.GetInt32(reader.GetOrdinal("MaKhoa")),
                            TenKhoa = reader.GetString(reader.GetOrdinal("TenKhoa"))
                        });
                    }
                }
            }

            return list;
        }
    }
}
