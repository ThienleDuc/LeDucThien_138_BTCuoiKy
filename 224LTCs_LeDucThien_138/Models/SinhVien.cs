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

        public string MaLSH { get; set; }  

        [StringLength(50)]
        public string TenSV { get; set; }

        public bool GioiTinh { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [StringLength(12)]
        public string Cccd { get; set; }

        public string DiaChi { get; set; }

        [StringLength(10)]
        public string Sdt { get; set; }

        public string Email { get; set; }

        [StringLength(10)]
        public string MatKhau { get; set; }

        public string Anh { get; set; }

        // Thêm navigation property để lấy TenLSH từ LopSinhHoat
        [ForeignKey("MaLSH")]
        public LopSinhHoat LopSinhHoat { get; set; }

        // Không ánh xạ sang DB, chỉ dùng để hiển thị TenLSH
        [NotMapped]
        public string TenLSH => LopSinhHoat?.TenLSH;

        public SinhVien(string maSV, string maLSH, string tenSV, bool gioiTinh, DateTime? ngaySinh, 
            string cccd, string diaChi, string sdt, string email, string matKhau, string anh, 
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

        public SinhVienRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<SinhVien> GetAllSinhVien()
        {
            List<SinhVien> list = new List<SinhVien>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @" SELECT sv.MaSV, sv.MaLSH, sv.TenSV, sv.GioiTinh, sv.NgaySinh, sv.Cccd, sv.DiaChi,
                            sv.Sdt, sv.Email, sv.MatKhau, sv.Anh,
                            lsh.TenLSH
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
                            MaLSH = reader.GetString(reader.GetOrdinal("MaLSH")),
                            TenSV = reader.GetString(reader.GetOrdinal("TenSV")),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader.GetDateTime("NgaySinh") : DateTime.Now,
                            Cccd = reader.GetString(reader.GetOrdinal("Cccd")),
                            DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                            Sdt = reader.GetString(reader.GetOrdinal("Sdt")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
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

        public List<SinhVien> GetSinhVienByNienKhoa(string maNK)
        {
            List<SinhVien> list = new List<SinhVien>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                        SELECT sv.*, lsh.TenLSH
                        FROM SinhVien sv
                        INNER JOIN LopSinhHoat lsh ON sv.MaLSH = lsh.MaLSH
                        WHERE lsh.MaNK = @MaNK;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNK", maNK);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SinhVien
                        {
                            MaSV = reader.GetString(reader.GetOrdinal("MaSV")),
                            MaLSH = reader.GetString(reader.GetOrdinal("MaLSH")),
                            TenSV = reader.GetString(reader.GetOrdinal("TenSV")),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader.GetDateTime("NgaySinh") : DateTime.Now,
                            Cccd = reader.GetString(reader.GetOrdinal("Cccd")),
                            DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                            Sdt = reader.GetString(reader.GetOrdinal("Sdt")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
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

        public List<SinhVien> GetSinhVienByKhoa(string maNK, int maKhoa)
        {
            List<SinhVien> list = new List<SinhVien>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                        SELECT sv.*, lsh.TenLSH
                        FROM SinhVien sv
                        INNER JOIN LopSinhHoat lsh ON sv.MaLSH = lsh.MaLSH
                        INNER JOIN ChuyenNganh cn ON lsh.MaNganh = cn.MaNganh
                        WHERE lsh.MaNK = @MaNK AND cn.MaKhoa = @MaKhoa;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNK", maNK);
                cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SinhVien
                        {
                            MaSV = reader.GetString(reader.GetOrdinal("MaSV")),
                            MaLSH = reader.GetString(reader.GetOrdinal("MaLSH")),
                            TenSV = reader.GetString(reader.GetOrdinal("TenSV")),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader.GetDateTime("NgaySinh") : DateTime.Now,
                            Cccd = reader.GetString(reader.GetOrdinal("Cccd")),
                            DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                            Sdt = reader.GetString(reader.GetOrdinal("Sdt")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
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

        public List<SinhVien> GetSinhVienByChuyenNganh(string maNK, int maKhoa, int maNganh)
        {
            List<SinhVien> list = new List<SinhVien>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                    SELECT sv.*, lsh.TenLSH
                    FROM SinhVien sv
                    INNER JOIN LopSinhHoat lsh ON sv.MaLSH = lsh.MaLSH
                    INNER JOIN ChuyenNganh cn ON cn.MaNganh = lsh.MaNganh
                    WHERE lsh.MaNK = @MaNK AND lsh.MaNganh = @MaNganh AND cn.MaKhoa = @MaKhoa;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNK", maNK);
                cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                cmd.Parameters.AddWithValue("@MaNganh", maNganh);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SinhVien
                        {
                            MaSV = reader.GetString(reader.GetOrdinal("MaSV")),
                            MaLSH = reader.GetString(reader.GetOrdinal("MaLSH")),
                            TenSV = reader.GetString(reader.GetOrdinal("TenSV")),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader.GetDateTime("NgaySinh") : DateTime.Now,
                            Cccd = reader.GetString(reader.GetOrdinal("Cccd")),
                            DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                            Sdt = reader.GetString(reader.GetOrdinal("Sdt")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
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

        public List<SinhVien> GetSinhVienByLSH(string maNK, int maKhoa, int maNganh, string maLSH)
        {
            List<SinhVien> list = new List<SinhVien>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                    SELECT sv.*, lsh.TenLSH
                    FROM SinhVien sv
                    INNER JOIN LopSinhHoat lsh ON sv.MaLSH = lsh.MaLSH
                    INNER JOIN ChuyenNganh cn ON cn.MaNganh = lsh.MaNganh
                    WHERE lsh.MaNK = @MaNK AND cn.MaKhoa = @MaKhoa AND cn.MaNganh = @MaNganh AND sv.MaLSH = @MalSH;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNK", maNK);
                cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                cmd.Parameters.AddWithValue("@MaNganh", maNganh);
                cmd.Parameters.AddWithValue("@MaLSH", maLSH);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SinhVien
                        {
                            MaSV = reader.GetString(reader.GetOrdinal("MaSV")),
                            MaLSH = reader.GetString(reader.GetOrdinal("MaLSH")),
                            TenSV = reader.GetString(reader.GetOrdinal("TenSV")),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader.GetDateTime("NgaySinh") : DateTime.Now,
                            Cccd = reader.GetString(reader.GetOrdinal("Cccd")),
                            DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                            Sdt = reader.GetString(reader.GetOrdinal("Sdt")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
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
                string query = @" SELECT sv.MaSV, sv.MaLSH, sv.TenSV, sv.GioiTinh, sv.NgaySinh, sv.Cccd, sv.DiaChi,
                            sv.Sdt, sv.Email, sv.MatKhau, sv.Anh,
                            lsh.TenLSH
                            FROM SinhVien sv
                            LEFT JOIN LopSinhHoat lsh ON sv.MaLSH = lsh.MaLSH;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSV", maSV);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sv = new SinhVien
                        {
                            MaSV = reader.GetString(reader.GetOrdinal("MaSV")),
                            MaLSH = reader.GetString(reader.GetOrdinal("MaLSH")),
                            TenSV = reader.GetString(reader.GetOrdinal("TenSV")),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader.GetDateTime("NgaySinh") : DateTime.Now,
                            Cccd = reader.GetString(reader.GetOrdinal("Cccd")),
                            DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                            Sdt = reader.GetString(reader.GetOrdinal("Sdt")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
                            Anh = reader["Anh"]?.ToString(),
                            LopSinhHoat = new LopSinhHoat
                            {
                                TenLSH = reader["TenLSH"]?.ToString()
                            }
                        };
                    }
                }
            }

            return sv;
        }

        public List<SinhVien> TimKiemSinhVien(string keyword)
        {
            List<SinhVien> danhSach = new List<SinhVien>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SearchSinhVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Keyword", keyword);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        danhSach.Add(new SinhVien
                        {
                            MaSV = reader.GetString(reader.GetOrdinal("MaSV")),
                            MaLSH = reader.GetString(reader.GetOrdinal("MaLSH")),
                            TenSV = reader.GetString(reader.GetOrdinal("TenSV")),
                            GioiTinh = Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                            Cccd = reader.GetString(reader.GetOrdinal("Cccd")),
                            DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                            Sdt = reader.GetString(reader.GetOrdinal("Sdt")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
                            Anh = reader.GetString(reader.GetOrdinal("Anh"))
                        });
                    }
                }
            }

            return danhSach;
        }

        public bool AddSinhVien(SinhVien sinhVien, string maNK, int maKhoa, int maNganh, string maLSH)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("AddSinhVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaNK", maNK);
                cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                cmd.Parameters.AddWithValue("@MaNganh", maNganh);
                cmd.Parameters.AddWithValue("@MaLSH", maLSH);
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
                cmd.Parameters.AddWithValue("@MaSV", maSV);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

    }
}
