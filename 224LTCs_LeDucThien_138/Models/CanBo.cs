using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace _224LTCs_LeDucThien_138.Models
{
    public class CanBo
    {
        public CanBo()
        {
        }

        [Key]
        [StringLength(14)]
        public string MaCB { get; set; }

        public int? MaHocVi { get; set; }

        public int? MaChucVu { get; set; }

        public int? MaKhoa { get; set; }

        [StringLength(50)]
        public string? TenCB { get; set; }

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

        // Navigation properties to get TenKhoa, TenHocVi, TenChucVu
        [ForeignKey("MaKhoa")]
        public Khoa Khoa { get; set; }

        [ForeignKey("MaHocVi")]
        public HocVi HocVi { get; set; }

        [ForeignKey("MaChucVu")]
        public ChucVu ChucVu { get; set; }

        // Non-mapped properties for displaying TenKhoa, TenHocVi, TenChucVu
        [NotMapped]
        public string? TenKhoa => Khoa?.TenKhoa;

        [NotMapped]
        public string? TenHocVi => HocVi?.TenHocVi;

        [NotMapped]
        public string? TenChucVu => ChucVu?.TenChucVu;

        public CanBo(string maCB, int? maHocVi, int? maChucVu, int? maKhoa, string? tenCB, bool gioiTinh, 
            DateTime? ngaySinh, string? cccd, string? diaChi, string? sdt, string? email, 
            string? matKhau, string? anh, Khoa khoa, HocVi hocVi, ChucVu chucVu)
        {
            MaCB = maCB;
            MaHocVi = maHocVi;
            MaChucVu = maChucVu;
            MaKhoa = maKhoa;
            TenCB = tenCB;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
            Cccd = cccd;
            DiaChi = diaChi;
            Sdt = sdt;
            Email = email;
            MatKhau = matKhau;
            Anh = anh;
            Khoa = khoa;
            HocVi = hocVi;
            ChucVu = chucVu;
        }
    }

    public class CanBoRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public CanBoRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<CanBo> GetAllCanBo()
        {
            List<CanBo> list = new List<CanBo>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                        SELECT cb.*, k.TenKhoa, hv.TenHocVi, cv.TenChucVu
                        FROM CanBo cb
                        LEFT JOIN Khoa k ON cb.MaKhoa = k.MaKhoa
                        LEFT JOIN HocVi hv ON cb.MaHocVi = hv.MaHocVi
                        LEFT JOIN ChucVu cv ON cb.MaChucVu = cv.MaChucVu;";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CanBo
                        {
                            MaCB = reader.GetString(reader.GetOrdinal("MaCB")),
                            MaKhoa = reader["MaKhoa"] != DBNull.Value ? Convert.ToInt32(reader["MaKhoa"]) : null,
                            MaHocVi = reader["MaHocVi"] != DBNull.Value ? Convert.ToInt32(reader["MaHocVi"]) : null,
                            MaChucVu = reader["MaChucVu"] != DBNull.Value ? Convert.ToInt32(reader["MaChucVu"]) : null,
                            TenCB = reader["TenCB"]?.ToString(),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader["NgaySinh"] : null,
                            Cccd = reader["Cccd"]?.ToString(),
                            DiaChi = reader["DiaChi"]?.ToString(),
                            Sdt = reader["Sdt"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            MatKhau = reader["MatKhau"]?.ToString(),
                            Anh = reader["Anh"]?.ToString(),
                            Khoa = new Khoa
                            {
                                TenKhoa = reader.GetString(reader.GetOrdinal("TenKhoa"))
                            },
                            HocVi = new HocVi
                            {
                                TenHocVi = reader.GetString(reader.GetOrdinal("TenHocVi"))
                            },
                            ChucVu = new ChucVu
                            {
                                TenChucVu = reader.GetString(reader.GetOrdinal("TenChucVu"))    
                            }
                        });
                    }
                }
            }

            return list;
        }

        public List<CanBo> GetCanBoFiltered(int? maKhoa = null, int? maHocVi = null, int? maChucVu = null, string? keyword = null)
        {
            List<CanBo> list = new List<CanBo>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                        SELECT cb.*, k.TenKhoa, hv.TenHocVi, cv.TenChucVu
                        FROM CanBo cb
                        LEFT JOIN Khoa k ON cb.MaKhoa = k.MaKhoa
                        LEFT JOIN HocVi hv ON cb.MaHocVi = hv.MaHocVi
                        LEFT JOIN ChucVu cv ON cb.MaChucVu = cv.MaChucVu
                        WHERE (@MaKhoa IS NULL OR cb.MaKhoa = @MaKhoa)
                          AND (@MaHocVi IS NULL OR cb.MaHocVi = @MaHocVi)
                          AND (@MaChucVu IS NULL OR cb.MaChucVu = @MaChucVu)
                          AND (
                                @Keyword IS NULL OR 
                                cb.TenCB LIKE '%' + @Keyword + '%' OR 
                                cb.MaCB LIKE '%' + @Keyword + '%'
                              )";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MaKhoa", maKhoa.HasValue? (object)maKhoa.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@MaHocVi", maHocVi.HasValue ? (object)maHocVi.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@MaChucVu", maChucVu.HasValue ? (object)maChucVu.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Keyword", string.IsNullOrWhiteSpace(keyword) ? DBNull.Value : (object)keyword);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CanBo
                        {
                            MaCB = reader.GetString(reader.GetOrdinal("MaCB")),
                            MaKhoa = reader["MaKhoa"] != DBNull.Value ? Convert.ToInt32(reader["MaKhoa"]) : null,
                            MaHocVi = reader["MaHocVi"] != DBNull.Value ? Convert.ToInt32(reader["MaHocVi"]) : null,
                            MaChucVu = reader["MaChucVu"] != DBNull.Value ? Convert.ToInt32(reader["MaChucVu"]) : null,
                            TenCB = reader["TenCB"]?.ToString(),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader["NgaySinh"] : null,
                            Cccd = reader["Cccd"]?.ToString(),
                            DiaChi = reader["DiaChi"]?.ToString(),
                            Sdt = reader["Sdt"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            MatKhau = reader["MatKhau"]?.ToString(),
                            Anh = reader["Anh"]?.ToString(),
                            Khoa = new Khoa
                            {
                                TenKhoa = reader.GetString(reader.GetOrdinal("TenKhoa"))
                            },
                            HocVi = new HocVi
                            {
                                TenHocVi = reader.GetString(reader.GetOrdinal("TenHocVi"))
                            },
                            ChucVu = new ChucVu
                            {
                                TenChucVu = reader.GetString(reader.GetOrdinal("TenChucVu"))
                            }
                        });
                    }
                }
            }

            return list;
        }

        public CanBo GetCanBoById(string maCB)
        {
            CanBo cb = null;

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                        SELECT cb.*, k.TenKhoa, hv.TenHocVi, cv.TenChucVu
                        FROM CanBo cb
                        LEFT JOIN Khoa k ON cb.MaKhoa = k.MaKhoa
                        LEFT JOIN HocVi hv ON cb.MaHocVi = hv.MaHocVi
                        LEFT JOIN ChucVu cv ON cb.MaChucVu = cv.MaChucVu
                        WHERE cb.MaCB = @MaCB;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaCB", maCB);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cb = new CanBo
                        {
                            MaCB = reader.GetString(reader.GetOrdinal("MaCB")),
                            MaKhoa = reader["MaKhoa"] != DBNull.Value ? Convert.ToInt32(reader["MaKhoa"]) : null,
                            MaHocVi = reader["MaHocVi"] != DBNull.Value ? Convert.ToInt32(reader["MaHocVi"]) : null,
                            MaChucVu = reader["MaChucVu"] != DBNull.Value ? Convert.ToInt32(reader["MaChucVu"]) : null,
                            TenCB = reader["TenCB"]?.ToString(),
                            GioiTinh = reader["GioiTinh"] != DBNull.Value && Convert.ToBoolean(reader["GioiTinh"]),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)reader["NgaySinh"] : null,
                            Cccd = reader["Cccd"]?.ToString(),
                            DiaChi = reader["DiaChi"]?.ToString(),
                            Sdt = reader["Sdt"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            MatKhau = reader["MatKhau"]?.ToString(),
                            Anh = reader["Anh"]?.ToString(),
                            Khoa = new Khoa
                            {
                                TenKhoa = reader.GetString(reader.GetOrdinal("TenKhoa"))
                            },
                            HocVi = new HocVi
                            {
                                TenHocVi = reader.GetString(reader.GetOrdinal("TenHocVi"))
                            },
                            ChucVu = new ChucVu
                            {
                                TenChucVu = reader.GetString(reader.GetOrdinal("TenChucVu"))
                            }
                        };
                    }
                }
            }

            return cb;
        }

        public bool AddCanBo(CanBo canBo)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("AddCanBo", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaHocVi", canBo.MaHocVi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaChucVu", canBo.MaChucVu ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaKhoa", canBo.MaKhoa ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TenCB", canBo.TenCB ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GioiTinh", canBo.GioiTinh);
                cmd.Parameters.AddWithValue("@NgaySinh", canBo.NgaySinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cccd", canBo.Cccd ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", canBo.DiaChi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Sdt", canBo.Sdt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", canBo.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MatKhau", canBo.MatKhau ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Anh", canBo.Anh ?? (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool UpdateCanBo(CanBo canBo)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("UpdateCanBo", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaHocVi", canBo.MaHocVi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaChucVu", canBo.MaChucVu ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaKhoa", canBo.MaKhoa ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaCB", canBo.MaCB ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TenCB", canBo.TenCB ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GioiTinh", canBo.GioiTinh);
                cmd.Parameters.AddWithValue("@NgaySinh", canBo.NgaySinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cccd", canBo.Cccd ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", canBo.DiaChi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Sdt", canBo.Sdt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", canBo.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MatKhau", canBo.MatKhau ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Anh", canBo.Anh ?? (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool DeleteCanBo(string maCB)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = "DELETE FROM CanBo WHERE MaCB = @MaCB";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaCB", maCB);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

    }
}
