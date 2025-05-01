using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace _224LTCs_LeDucThien_138.Models
{
    public class SinhVien
    {
        [Key]
        [StringLength(14)]
        public string MaSV { get; set; }

        public int? MaLSH { get; set; }  

        [StringLength(50)]
        public string TenSV { get; set; }

        public bool GioiTinh { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

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
                string query = @"SELECT MaSV, MaLSH, TenSV, GioiTinh, NgaySinh, Cccd, DiaChi, Sdt, Email, MatKhau, Anh FROM SinhVien;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SinhVien
                        {
                            MaSV = reader.GetString(reader.GetOrdinal("MaSV")),
                            MaLSH = Convert.ToInt32(reader["MaLSH"]),
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

            return list;
        }

        public SinhVien GetSinhVienById(string maSV)
        {
            SinhVien sv = null;

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaSV, MaLSH, TenSV, GioiTinh, NgaySinh, Cccd, DiaChi, Sdt, Email, MatKhau, Anh
                         FROM SinhVien
                         WHERE MaSV = @MaSV";
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
                            MaLSH = Convert.ToInt32(reader["MaLSH"]),
                            TenSV = reader.GetString(reader.GetOrdinal("TenSV")),
                            GioiTinh = Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                            Cccd = reader.GetString(reader.GetOrdinal("Cccd")),
                            DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
                            Sdt = reader.GetString(reader.GetOrdinal("Sdt")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
                            Anh = reader.GetString(reader.GetOrdinal("Anh"))
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
                            MaLSH = Convert.ToInt32(reader["MaLSH"]),
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

    }
}
