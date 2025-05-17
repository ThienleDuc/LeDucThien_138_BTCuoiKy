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

        public string? MaHP => LopHocPhan?.MaHP;

        public string? TenMH => LopHocPhan.TenMH;

        public string? TenCB => LopHocPhan.TenCB;

        public string? TenPhong => LopHocPhan.TenPhong;

        public string? ThuNgay => LopHocPhan?.ThuNgay;

        public int? TietBatDau => LopHocPhan.TietBatDau;

        public int? TietKetThuc => LopHocPhan.TietKetThuc;

        public DateTime? NgayHoc => LopHocPhan?.NgayHoc;

        public string? LHP_GhiChu => LopHocPhan?.GhiChu;

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
        
        public List<CT_LHP_SV> GetLichHocOfSinhVien(string maSV, string maHP)
        {
            List<CT_LHP_SV> danhSach = new List<CT_LHP_SV>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                    SELECT lhp.MaLHP, lhp.MaHP, mh.TenMH, cb.TenCB, p.TenPhong,
		                    lhp.ThuNgay, lhp.TietBatDau, lhp.TietKetThuc, 
		                    lhp.NgayHoc, lhp.GhiChu
                    FROM CT_LHP_SV ct
                    LEFT JOIN LopHocPhan lhp ON ct.MaLHP = lhp.MaLHP
                    LEFT JOIN MonHoc mh ON mh.MaMH = lhp.MaMH
                    LEFT JOIN CanBo cb ON cb.MaCB = lhp.MaCB
                    LEFT JOIN Phong p ON p.MaPhong = lhp.MaPhong
                    LEFT JOIN HocPhan hp ON hp.MaHP = lhp.MaHP
                    WHERE ct.MaSV = @MaSV
                      AND lhp.MaHP = @MaHP;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSV", !string.IsNullOrEmpty(maSV) ? maSV : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaHP", !string.IsNullOrEmpty (maHP) ? maHP : (object)DBNull.Value);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ct = new CT_LHP_SV
                        {
                            MaLHP = reader["MaLHP"]?.ToString(),
                            LopHocPhan = new LopHocPhan
                            {
                                MaHP = reader["MaHP"]?.ToString(),
                                MonHoc = new MonHoc
                                {
                                    TenMH = reader["TenMH"]?.ToString(),
                                },
                                CanBo = new CanBo
                                {
                                    TenCB = reader["TenCB"]?.ToString(),
                                },
                                Phong = new PhongHoc
                                {
                                    TenPhong = reader["TenPhong"]?.ToString(),
                                },
                                ThuNgay = reader["ThuNgay"]?.ToString(),
                                TietBatDau = Convert.ToInt32(reader["TietBatDau"]),
                                TietKetThuc = Convert.ToInt32(reader["TietKetThuc"]),
                                NgayHoc = reader["NgayHoc"] != DBNull.Value ? (DateTime?)reader["NgayHoc"] : null,
                                GhiChu = reader["GhiChu"]?.ToString(),
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
