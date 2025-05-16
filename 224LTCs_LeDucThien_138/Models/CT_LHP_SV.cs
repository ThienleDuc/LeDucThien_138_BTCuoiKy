using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace _224LTCs_LeDucThien_138.Models
{
    public class CT_LHP_SV
    {
        [Key, Column(Order = 0)]
        [StringLength(14)]
        public string MaSV { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(20)]
        public string? MaLHP { get; set; }

        public string? GhiChu { get; set; }

        // Navigation properties
        [ForeignKey("MaSV")]
        public SinhVien SinhVien { get; set; }

        [ForeignKey("MaLHP")]
        public LopHocPhan LopHocPhan { get; set; }

        // Read-only properties for convenience
        public string? TenSV => SinhVien?.TenSV;
        public string? Email => SinhVien?.Email;

        public CT_LHP_SV() { }

        public CT_LHP_SV(string maSV, string? maLHP, string? ghiChu, SinhVien sinhVien, LopHocPhan lopHocPhan)
        {
            MaSV = maSV;
            MaLHP = maLHP;
            GhiChu = ghiChu;
            SinhVien = sinhVien;
            LopHocPhan = lopHocPhan;
        }
    }

    public class CT_LHP_SVRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public CT_LHP_SVRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<CT_LHP_SV> GetCT_SV_LHP_ById(string maLHP)
        {
            List<CT_LHP_SV> danhSach = new List<CT_LHP_SV>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                    SELECT ct.MaLHP, ct.MaSV, ct.GhiChu, sv.TenSV, sv.Email
                    FROM CT_LHP_SV ct
                    JOIN SinhVien sv ON ct.MaSV = sv.MaSV
                    WHERE ct.MaLHP = @MaLHP";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaLHP", maLHP ?? (object)DBNull.Value);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CT_LHP_SV ct = new CT_LHP_SV
                        {
                            MaLHP = reader["MaLHP"]?.ToString(),
                            MaSV = reader.GetString(reader.GetOrdinal("MaSV")),
                            GhiChu = reader["GhiChu"]?.ToString(),
                            SinhVien = new SinhVien
                            {
                                TenSV = reader["TenSV"]?.ToString(),
                                Email = reader["Email"]?.ToString()
                            }
                        };

                        danhSach.Add(ct);
                    }
                }
            }

            return danhSach;
        }
    }
}
