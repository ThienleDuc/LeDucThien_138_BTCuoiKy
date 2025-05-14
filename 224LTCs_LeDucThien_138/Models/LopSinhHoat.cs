using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _224LTCs_LeDucThien_138.Models
{
    public class LopSinhHoat
    {
        [Key]
        public string MaLSH { get; set; }

        public int? MaNganh { get; set; }

        [StringLength(10)]
        public string? TenLSH { get; set; }

        [ForeignKey("MaNK")]
        public NienKhoa NienKhoa { get; set; }

        [ForeignKey("MaNganh")]

        public ChuyenNganh ChuyenNganh { get; set; }

        [StringLength(4)]
        public string? MaNK { get; set; }

        public ICollection<SinhVien> SinhViens { get; set; }

        public LopSinhHoat()
        {
        }

        public LopSinhHoat(string maLSH, int? maNganh, string? tenLSH, NienKhoa nienKhoa, string? maNK, ICollection<SinhVien> sinhViens)
        {
            MaLSH = maLSH;
            MaNganh = maNganh;
            TenLSH = tenLSH;
            NienKhoa = nienKhoa;
            MaNK = maNK;
            SinhViens = sinhViens;
        }

        public LopSinhHoat(ChuyenNganh chuyenNganh)
        {
            ChuyenNganh = chuyenNganh;
        }
    }

    public class LopSinhHoatRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public LopSinhHoatRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<LopSinhHoat> GetLopSinhHoatById(int? maNganh, string? maNK)
        {
            List<LopSinhHoat> list = new List<LopSinhHoat>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaLSH, MaNganh, TenLSH, MaNK
                         FROM LopSinhHoat
                         WHERE MaNganh = @MaNganh AND MaNK = @MaNK;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNganh", maNganh.HasValue ? maNganh : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaNK", !string.IsNullOrEmpty(maNK) ? maNK : (object)DBNull.Value);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) {

                        list.Add(new LopSinhHoat
                        {
                            MaLSH = reader.GetString(reader.GetOrdinal("MaLSH")),
                            MaNganh = reader["MaNganh"] != DBNull.Value ? Convert.ToInt32(reader["MaNganh"]) : null,
                            TenLSH = reader["TenLSH"]?.ToString(),
                            MaNK = reader["MaNK"]?.ToString()
                        });
                    }
                }
            }

            return list;
        }

    }
}
