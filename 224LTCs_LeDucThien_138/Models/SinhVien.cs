using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace _224LTCs_LeDucThien_138.Models
{
    public class SinhVien
    {
        public SinhVien()
        {
        }

        [Key]
        [StringLength(14)]
        public string MaSV { get; set; }

        public string? MaLSH { get; set; }  

        [StringLength(50)]
        public string? TenSV { get; set; }

        public bool GioiTinh { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [StringLength(12)]
        public string? Cccd { get; set; }

        public string? DiaChi { get; set; }

        [StringLength(10)]
        public string? Sdt { get; set; }

        public string? Email { get; set; }

        [StringLength(10)]
        public string? MatKhau { get; set; }

        public string? Anh { get; set; }

        public string? TenNganh => LopSinhHoat?.ChuyenNganh?.TenNganh;

        public string? TenKhoa => LopSinhHoat?.ChuyenNganh?.Khoa?.TenKhoa;

        public string? MaNK => LopSinhHoat?.MaNK;

        public int? MaKhoa => LopSinhHoat?.ChuyenNganh?.MaKhoa;

        // Thêm navigation property để lấy TenLSH từ LopSinhHoat
        [ForeignKey("MaLSH")]
        public LopSinhHoat LopSinhHoat { get; set; }

        [ForeignKey("MaSV")]
        public CT_LHP_SV CT_LHP_SV { get; set; }

        public List<HocPhan> DanhSachHocPhan { get; set; } = new();

        // Map MaHP -> danh sách lịch học
        public Dictionary<string, List<CT_LHP_SV>> LichHocTheoHocPhan { get; set; } = new();

        public List<CT_LHP_SV> LichHocMoiNhat { get; set; } = new ();

        public List<LopHocPhan> DanhSachLopHocPhanMoiNhat { get; set; } = new();

        public List<LopHocPhan> DanhSachLopHocPhanDaChon {  get; set; } = new();

        // Không ánh xạ sang DB, chỉ dùng để hiển thị TenLSH
        [NotMapped]
        public string? TenLSH => LopSinhHoat?.TenLSH;

        public SinhVien(string maSV, string? maLSH, string? tenSV, bool gioiTinh, DateTime? ngaySinh, 
            string? cccd, string? diaChi, string? sdt, string? email, string? matKhau, string? anh, 

            LopSinhHoat lopSinhHoat)
        {
            MaSV = maSV;
            MaLSH = maLSH;
            TenSV = tenSV;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
            Cccd = cccd;
            DiaChi = diaChi;
            Sdt = sdt;
            Email = email;
            MatKhau = matKhau;
            Anh = anh;
            LopSinhHoat = lopSinhHoat;
        }
    }


    public class SinhVienRepos
    {
        private ConnectionDatabase _connectionDatabase;
        private HocPhanRepos _hocPhanRepos;
        private CT_LHP_SVRepos _ctLHP_SVRepos;
        private LopHocPhanRepos _LopHocPhanRepos;

        public SinhVienRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
            _hocPhanRepos = new HocPhanRepos(_connectionDatabase);
            _ctLHP_SVRepos = new CT_LHP_SVRepos(_connectionDatabase);
            _LopHocPhanRepos = new LopHocPhanRepos(connectionDatabase);
        }

        public List<SinhVien> GetAllSinhVien()
        {
            List<SinhVien> list = new List<SinhVien>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @" SELECT sv.*, lsh.TenLSH
                            FROM SinhVien sv
                            LEFT JOIN LopSinhHoat lsh ON sv.MaLSH = lsh.MaLSH;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SinhVien
                        {
                            MaSV = reader.GetString(reader.GetOrdinal("MaSV")),
                            MaLSH = reader["MaLSH"]?.ToString(),
                            TenSV = reader["TenSV"]?.ToString(),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader["NgaySinh"] : null,
                            Cccd = reader["Cccd"]?.ToString(),
                            DiaChi = reader["DiaChi"]?.ToString(),
                            Sdt = reader["Sdt"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            MatKhau = reader["MatKhau"]?.ToString(),
                            Anh = reader["Anh"]?.ToString(),
                            LopSinhHoat = new LopSinhHoat
                            {       
                                TenLSH = reader["TenLSH"]?.ToString()
                            }
                        });
                    }
                }
            }

            return list;
        }

        public List<SinhVien> GetSinhVienFiltered(string? maNK = null, int? maKhoa = null, int? maNganh = null, string? maLSH = null, string? keyword = null)
        {
            List<SinhVien> list = new List<SinhVien>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                    SELECT sv.*, lsh.TenLSH
                    FROM SinhVien sv
                    INNER JOIN LopSinhHoat lsh ON sv.MaLSH = lsh.MaLSH
                    INNER JOIN ChuyenNganh cn ON lsh.MaNganh = cn.MaNganh
                    WHERE (@MaNK IS NULL OR lsh.MaNK = @MaNK)
                      AND (@MaKhoa IS NULL OR cn.MaKhoa = @MaKhoa)
                      AND (@MaNganh IS NULL OR cn.MaNganh = @MaNganh)
                      AND (@MaLSH IS NULL OR sv.MaLSH = @MaLSH)
                      AND (
                            @Keyword IS NULL OR 
                            sv.TenSV LIKE '%' + @Keyword + '%' OR 
                            sv.MaSV LIKE '%' + @Keyword + '%'
                          )";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MaNK", string.IsNullOrEmpty(maNK) ? DBNull.Value : (object)maNK);
                cmd.Parameters.AddWithValue("@MaKhoa", maKhoa.HasValue ? (object)maKhoa.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@MaNganh", maNganh.HasValue ? (object)maNganh.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@MaLSH", string.IsNullOrEmpty(maLSH) ? DBNull.Value : (object)maLSH);
                cmd.Parameters.AddWithValue("@Keyword", string.IsNullOrWhiteSpace(keyword) ? DBNull.Value : (object)keyword);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SinhVien
                        {
                            MaSV = reader.GetString(reader.GetOrdinal("MaSV")),
                            MaLSH = reader["MaLSH"]?.ToString(),
                            TenSV = reader["TenSV"]?.ToString(),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader["NgaySinh"] : null,
                            Cccd = reader["Cccd"]?.ToString(),
                            DiaChi = reader["DiaChi"]?.ToString(),
                            Sdt = reader["Sdt"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            MatKhau = reader["MatKhau"]?.ToString(),
                            Anh = reader["Anh"]?.ToString(),
                            LopSinhHoat = new LopSinhHoat
                            {
                                TenLSH = reader["TenLSH"]?.ToString()
                            }
                        });
                    }
                }
            }

            return list;
        }


        public SinhVien GetSinhVienById(string maSV)
        {
            SinhVien sv = null;

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @" 
                    SELECT sv.*, lsh.TenLSH, lsh.MaNK, cn.TenNganh, cn.MaKhoa, k.TenKhoa
                    FROM SinhVien sv
                    LEFT JOIN LopSinhHoat lsh ON sv.MaLSH = lsh.MaLSH
                    LEFT JOIN ChuyenNganh cn ON lsh.MaNganh = cn.MaNganh
                    LEFT JOIN Khoa k ON cn.MaKhoa = k.MaKhoa
                    WHERE sv.MaSV = @MaSV;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSV", !string.IsNullOrEmpty(maSV) ? maSV : (object)DBNull.Value);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sv = new SinhVien
                        {
                            MaSV = reader.GetString(reader.GetOrdinal("MaSV")),
                            MaLSH = reader["MaLSH"]?.ToString(),
                            TenSV = reader["TenSV"]?.ToString(),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader["NgaySinh"] : null,
                            Cccd = reader["Cccd"]?.ToString(),
                            DiaChi = reader["DiaChi"]?.ToString(),
                            Sdt = reader["Sdt"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            MatKhau = reader["MatKhau"]?.ToString(),
                            Anh = reader["Anh"]?.ToString(),
                            LopSinhHoat = new LopSinhHoat
                            {
                                TenLSH = reader["TenLSH"]?.ToString(),
                                MaNK = reader["MaNK"]?.ToString(),
                                ChuyenNganh = new ChuyenNganh
                                {
                                    MaKhoa = Convert.ToInt32(reader["MaKhoa"]),
                                    TenNganh = reader["TenNganh"]?.ToString(),
                                    Khoa = new Khoa
                                    {
                                        TenKhoa = reader["TenKhoa"]?.ToString()
                                    }
                                }
                            }
                        };
                    }
                }
            }

            if (sv != null)
            {
                sv.DanhSachHocPhan = _hocPhanRepos.GetHocPhanByMaxMaNK();

                foreach(var hp in sv.DanhSachHocPhan)
                {
                    var lichhoc = _ctLHP_SVRepos.GetLichHocOfSinhVien(maSV, hp.MaHP);
                    sv.LichHocTheoHocPhan[hp.MaHP] = lichhoc;
                }

                string maHP = _hocPhanRepos.GetMaHPMoiNhat();

                if (maHP != null) 
                { 
                    sv.LichHocMoiNhat = _ctLHP_SVRepos.GetLichHocOfSinhVien(maSV, maHP);
                    sv.DanhSachLopHocPhanMoiNhat = _LopHocPhanRepos.GetLopHocPhanFiltered(maKhoa: sv.MaKhoa, maHP: maHP);
                } else
                {
                    sv.LichHocMoiNhat = new List<CT_LHP_SV> { };
                    sv.DanhSachLopHocPhanMoiNhat = new List<LopHocPhan> { };
                }
            }
            return sv;
        }

        public bool AddSinhVien(SinhVien sinhVien, string maNK, int? maKhoa, int? maNganh, string maLSH)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("AddSinhVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaNK", !string.IsNullOrEmpty(maNK) ? maNK : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaKhoa", maKhoa.HasValue ? maKhoa : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaNganh", maNganh.HasValue ? maNganh : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaLSH", !string.IsNullOrEmpty(maLSH) ? maLSH : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TenSV", sinhVien.TenSV ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GioiTinh", sinhVien.GioiTinh);
                cmd.Parameters.AddWithValue("@NgaySinh", sinhVien.NgaySinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cccd", sinhVien.Cccd ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", sinhVien.DiaChi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Sdt", sinhVien.Sdt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", sinhVien.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MatKhau", sinhVien.MatKhau ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Anh", sinhVien.Anh ?? (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool UpdateSinhVien(SinhVien sinhVien)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("UpdateSinhVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaSV", sinhVien.MaSV ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaLSH", sinhVien.MaLSH ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TenSV", sinhVien.TenSV ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GioiTinh", sinhVien.GioiTinh);
                cmd.Parameters.AddWithValue("@NgaySinh", sinhVien.NgaySinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cccd", sinhVien.Cccd ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", sinhVien.DiaChi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Sdt", sinhVien.Sdt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", sinhVien.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MatKhau", sinhVien.MatKhau ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Anh", sinhVien.Anh ?? (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool DeleteSinhVien(string maSV)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = "DELETE FROM SinhVien WHERE MaSV = @MaSV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSV", !string.IsNullOrEmpty(maSV) ? maSV : (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool UpdateAvatarSinhVien(SinhVien sinhVien)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("UpdateAvatarSinhVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaSV", sinhVien.MaSV ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Anh", sinhVien.Anh ?? (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }


    }
}
