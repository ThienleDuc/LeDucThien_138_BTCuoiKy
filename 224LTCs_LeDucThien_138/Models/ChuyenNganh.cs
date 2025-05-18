using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace _224LTCs_LeDucThien_138.Models
{
    public class ChuyenNganh
    {
        public ChuyenNganh()
        {
        }

        [Key]
        public int MaNganh { get; set; }

        // FK
        public int? MaKhoa { get; set; }

        [StringLength(50)]
        public string? TenNganh { get; set; }

        // Navigation properties
        [ForeignKey("MaKhoa")]
        public Khoa Khoa { get; set; }

        public ChuyenNganh(int maNganh, int? maKhoa, string? tenNganh, Khoa khoa)
        {
            MaNganh = maNganh;
            MaKhoa = maKhoa;
            TenNganh = tenNganh;
            Khoa = khoa;
        }
    }

    public class ChuyenNganhRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public ChuyenNganhRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<ChuyenNganh> GetChuyenNganhByKhoa(int? maKhoa)
        {
            List<ChuyenNganh> list = new List<ChuyenNganh>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaNganh, MaKhoa, TenNganh 
                         FROM ChuyenNganh
                         WHERE MaKhoa = @MaKhoa;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKhoa", maKhoa.HasValue ? maKhoa : (object)DBNull.Value);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ChuyenNganh
                        {
                            MaNganh = Convert.ToInt32(reader["MaNganh"]),
                            MaKhoa = reader["MaKhoa"] != DBNull.Value ? Convert.ToInt32(reader["MaKhoa"]) : null,
                            TenNganh = reader["TenNganh"].ToString()
                        });
                    }
                }
            }

            return list;
        }
    }
}