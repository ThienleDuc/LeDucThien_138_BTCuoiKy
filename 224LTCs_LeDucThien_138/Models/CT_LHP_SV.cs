using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using System.Data;

namespace _224LTCs_LeDucThien_138.Models
{
    public class CT_LHP_SV
    {
        [Key, Column(Order = 0)]
        [StringLength(14)]
        public string MaSV { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(20)]
        public string MaLHP { get; set; }

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

        public int? SoTC => LopHocPhan?.MonHoc?.SoTC;

        public string? TenCB => LopHocPhan.TenCB;

        public string? TenPhong => LopHocPhan.TenPhong;

        public string? ThuNgay => LopHocPhan?.ThuNgay;

        public int? TietBatDau => LopHocPhan.TietBatDau;

        public int? TietKetThuc => LopHocPhan.TietKetThuc;

        public DateTime? NgayHoc => LopHocPhan?.NgayHoc;

        public string? LHP_GhiChu => LopHocPhan?.GhiChu;

        public CT_LHP_SV() { }

        public CT_LHP_SV(string maSV, string maLHP, string? ghiChu, SinhVien sinhVien, LopHocPhan lopHocPhan)
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
                            MaLHP = reader.GetString(reader.GetOrdinal("MaLHP")),
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
                    SELECT lhp.MaLHP, lhp.MaHP, mh.TenMH, mh.SoTC, 
                            cb.TenCB, p.TenPhong,lhp.ThuNgay, lhp.TietBatDau, 
                            lhp.TietKetThuc,lhp.NgayHoc, lhp.GhiChu
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
                            MaLHP = reader.GetString(reader.GetOrdinal("MaLHP")),
                            LopHocPhan = new LopHocPhan
                            {
                                MaHP = reader["MaHP"]?.ToString(),
                                MonHoc = new MonHoc
                                {
                                    TenMH = reader["TenMH"]?.ToString(),
                                    SoTC = Convert.ToInt32(reader["SoTC"])
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

        public bool DeleteLopHocPhanOfSinhVien(string maSV, string maLHP)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                DELETE FROM CT_LHP_SV
                WHERE MaSV = @MaSV AND MaLHP = @MaLHP";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSV", maSV ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaLHP", maLHP ?? (object)DBNull.Value);

                conn.Open();
                int result= cmd.ExecuteNonQuery();

                return result> 0;
            }
        }

        public bool SinhVienRegisterLopHocPhan(string maSV, string maLHP, string? ghiChu, List<string> errors)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("DangKyLopHocPhan", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MaSV", maSV ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaLHP", maLHP ?? (object)DBNull.Value);

                    // Giả sử stored procedure trả về 0 nếu đã tồn tại, 1 nếu đăng ký thành công
                    var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    int result = (int)returnParameter.Value;

                    if (result == 0)
                    {
                        errors.Add($"Lỗi: Lớp học phần [{maLHP}] đã được đăng ký.");
                        return false;
                    }
                    else if (result == 1)
                    {
                        // Thành công
                        return true;
                    }
                    else
                    {
                        errors.Add("Có lỗi không xác định xảy ra khi đăng ký lớp học phần.");
                        return false;
                    }
                }
            }
        }

    }

}
