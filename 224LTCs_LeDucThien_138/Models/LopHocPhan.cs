using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using System.Data;

namespace _224LTCs_LeDucThien_138.Models
{
    public class LopHocPhan
    {
        [Key]
        [StringLength(20)]
        public string MaLHP { get; set; }

        [StringLength(7)]
        public string? MaHP { get; set; }

        public string? MaMH { get; set; }

        public int? MaNganh => MonHoc?.MaNganh;

        public string? TenMH => MonHoc?.TenMH;

        public int? SoTC => MonHoc?.SoTC;

        [StringLength(10)]
        public string? MaCB { get; set; }

        [StringLength(2)]
        public string? ThuNgay {  get; set; }

        public int? TietBatDau { get; set; }

        public int? TietKetThuc { get; set; }

        public int? MaKhoa => CanBo?.MaKhoa;

        public string? TenCB => CanBo?.TenCB;

        public int? MaPhong { get; set; }

        public string? TenPhong => Phong?.TenPhong;

        public int? SLHienTai { get; set; }

        public int? SLToiDa { get; set; }

        public DateTime? NgayHoc { get; set; }

        public string? GhiChu { get; set; }

        public string? MaNK => HocPhan?.MaNK;

        public List<CT_LHP_SV> DanhSachSinhVienLopHocPhan { get; set; }
        
        // Navigation properties
        [ForeignKey("MaHP")]
        public HocPhan HocPhan { get; set; }

        [ForeignKey("MaMH")]
        public MonHoc MonHoc { get; set; }

        [ForeignKey("MaCB")]
        public CanBo CanBo { get; set; }

        [ForeignKey("MaPhong")]
        public PhongHoc Phong { get; set; }
        
        public ICollection<CT_LHP_SV> CT_LHP_SVs { get; set; }

        public LopHocPhan(string maLHP, string? maHP, string? maMH, string? maCB, string? thuNgay, int? tietBatDau, int? tietKetThuc, int? maPhong, 
            int? sLHienTai, int? sLToiDa, DateTime? ngayHoc, string? ghiChu, HocPhan hocPhan, MonHoc monHoc, 
            CanBo canBo, PhongHoc phong, ICollection<CT_LHP_SV> cT_LHP_SVs)
        {
            MaLHP = maLHP;
            MaHP = maHP;
            MaMH = maMH;
            MaCB = maCB;
            ThuNgay = thuNgay;
            TietBatDau = tietBatDau;
            TietKetThuc = tietKetThuc;
            MaPhong = maPhong;
            SLHienTai = sLHienTai;
            SLToiDa = sLToiDa;
            NgayHoc = ngayHoc;
            GhiChu = ghiChu;
            HocPhan = hocPhan;
            MonHoc = monHoc;
            CanBo = canBo;
            Phong = phong;
            CT_LHP_SVs = cT_LHP_SVs;
        }

        public LopHocPhan(List<CT_LHP_SV> danhSachSinhVienLopHocPhan)
        {
            DanhSachSinhVienLopHocPhan = danhSachSinhVienLopHocPhan;
        }

        public LopHocPhan()
        {
        }

        
    }

    public class LopHocPhanRepos
    {
        private ConnectionDatabase _connectionDatabase;
        private CT_LHP_SVRepos _cT_LHP_SVRepos;

        public LopHocPhanRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
            _cT_LHP_SVRepos  = new CT_LHP_SVRepos(_connectionDatabase);
        }

        public List<LopHocPhan> GetAllLopHocPhan()
        {
            List<LopHocPhan> list = new List<LopHocPhan>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                SELECT lhp.*, mh.TenMH, mh.SoTC, cb.TenCB, p.TenPhong
                FROM LopHocPhan lhp
                LEFT JOIN MonHoc mh ON lhp.MaMH = mh.MaMH
                LEFT JOIN CanBo cb ON lhp.MaCB = cb.MaCB
                LEFT JOIN Phong p ON lhp.MaPhong = p.MaPhong
                LEFT JOIN HocPhan hp ON lhp.MaHP = hp.MaHP
                ORDER BY lhp.MaMH, 
                CASE lhp.ThuNgay
                    WHEN '2'	THEN 2
                    WHEN '3'	THEN 3
                    WHEN '4'	THEN 4
                    WHEN '5'	THEN 5
                    WHEN '6'	THEN 6
                    WHEN '7'	THEN 7
                    WHEN 'CN'	THEN 8
                    ELSE 9
                END;";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new LopHocPhan
                        {
                            MaLHP = reader.GetString(reader.GetOrdinal("MaLHP")),
                            MaHP = reader["MaHP"]?.ToString(),
                            MaMH = reader["MaMH"]?.ToString(),
                            MaCB = reader["MaCB"]?.ToString(),
                            ThuNgay = reader["ThuNgay"]?.ToString(),
                            TietBatDau = reader["TietBatDau"] != DBNull.Value ? Convert.ToInt32(reader["TietBatDau"]) : null,
                            TietKetThuc = reader["TietKetThuc"] != DBNull.Value ? Convert.ToInt32(reader["TietKetThuc"]) : null,
                            MaPhong = reader["MaPhong"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MaPhong"]) : null,
                            SLHienTai = reader["SLHienTai"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SLHienTai"]) : 0,
                            SLToiDa = reader["SLToiDa"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SLToiDa"]) : null,
                            NgayHoc = reader["NgayHoc"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["NgayHoc"]) : null,
                            GhiChu = reader["GhiChu"]?.ToString(),
                            
                            MonHoc = new MonHoc
                            {
                                TenMH = reader["TenMH"]?.ToString(),
                                SoTC = reader["SoTC"] != DBNull.Value ? Convert.ToInt32(reader["SoTC"]) : null
                            },
                                
                            CanBo = new CanBo
                            {
                                TenCB = reader["TenCB"]?.ToString()
                            },
                            Phong = new PhongHoc
                            {
                                TenPhong = reader["TenPhong"]?.ToString()
                            }
                        });
                    }
                }
            }

            return list;
        }

        public LopHocPhan GetLopHocPhanById(string maLHP)
        {
            LopHocPhan lhp = null;

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @" 
                    SELECT lhp.*, mh.MaNganh, mh.TenMH, mh.SoTC, 
                            cb.TenCB, p.TenPhong, hp.MaNK, cb.MaKhoa
                    FROM LopHocPhan lhp
                    LEFT JOIN MonHoc mh ON lhp.MaMH = mh.MaMH
                    LEFT JOIN CanBo cb ON lhp.MaCB = cb.MaCB
                    LEFT JOIN Phong p ON lhp.MaPhong = p.MaPhong
                    LEFT JOIN HocPhan hp ON lhp.MaHP = hp.MaHP
                    WHERE lhp.MaLHP = @MaLHP;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaLHP", maLHP);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lhp = new LopHocPhan
                        {
                            MaLHP = reader.GetString(reader.GetOrdinal("MaLHP")),
                            MaHP = reader["MaHP"]?.ToString(),
                            MaMH = reader["MaMH"]?.ToString(),
                            MaCB = reader["MaCB"]?.ToString(),
                            ThuNgay = reader["ThuNgay"]?.ToString(),
                            TietBatDau = reader["TietBatDau"] != DBNull.Value ? Convert.ToInt32(reader["TietBatDau"]) : null,
                            TietKetThuc = reader["TietKetThuc"] != DBNull.Value ? Convert.ToInt32(reader["TietKetThuc"]) : null,
                            MaPhong = reader["MaPhong"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MaPhong"]) : null,
                            SLHienTai = reader["SLHienTai"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SLHienTai"]) : 0,
                            SLToiDa = reader["SLToiDa"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SLToiDa"]) : null,
                            NgayHoc = reader["NgayHoc"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["NgayHoc"]) : null,
                            GhiChu = reader["GhiChu"]?.ToString(),

                            MonHoc = new MonHoc
                            {
                                MaNganh = reader["MaNganh"] != DBNull.Value ? Convert.ToInt32(reader["MaNganh"]) : null,
                                TenMH = reader["TenMH"]?.ToString(),
                                SoTC = reader["SoTC"] != DBNull.Value ? Convert.ToInt32(reader["SoTC"]) : null
                            },

                            CanBo = new CanBo
                            {
                                MaKhoa = reader["MaKhoa"] != DBNull.Value ? Convert.ToInt32(reader["MaKhoa"]) : null,
                                TenCB = reader["TenCB"]?.ToString()
                            },
                            Phong = new PhongHoc
                            {
                                TenPhong = reader["TenPhong"]?.ToString()
                            },
                            HocPhan = new HocPhan
                            {
                                MaNK = reader["MaNK"]?.ToString()
                            }
                        }; 
                    }
                }
            }

            if (lhp != null)
            {
                lhp.DanhSachSinhVienLopHocPhan = _cT_LHP_SVRepos.GetCT_SV_LHP_ById(maLHP);
            }

            return lhp;
        }

        public List<LopHocPhan> GetLopHocPhanFiltered(string? maNK = null, int? maKhoa = null, int? maNganh = null, string? maHP = null, int? maPhong = null, string? maMH = null, string? maCB = null, string? keyword = null)
        {
            List<LopHocPhan> list = new List<LopHocPhan>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"
                    SELECT lhp.*, mh.TenMH, mh.SoTC, cb.TenCB, p.TenPhong
                    FROM LopHocPhan lhp
                    LEFT JOIN MonHoc mh ON lhp.MaMH = mh.MaMH
                    LEFT JOIN CanBo cb ON lhp.MaCB = cb.MaCB
                    LEFT JOIN Phong p ON lhp.MaPhong = p.MaPhong
                    LEFT JOIN HocPhan hp ON lhp.MaHP = hp.MaHP
                    LEFT JOIN ChuyenNganh cn ON mh.MaNganh = cn.MaNganh
                    WHERE 
                        (@MaNK IS NULL OR hp.MaNK = @MaNK) AND
                        (@MaKhoa IS NULL OR cn.MaKhoa = @MaKhoa) AND
                        (@MaNganh IS NULL OR mh.MaNganh = @MaNganh) AND
                        (@MaHP IS NULL OR lhp.MaHP = @MaHP) AND
                        (@MaPhong IS NULL OR lhp.MaPhong = @MaPhong) AND
                        (@MaMH IS NULL OR lhp.MaMH = @MaMH) AND
                        (@MaCB IS NULL OR lhp.MaCB = @MaCB) AND
                        (@Keyword IS NULL OR lhp.MaLHP LIKE '%' + @Keyword + '%' OR mh.TenMH LIKE '%' + @Keyword + '%')
                    ORDER BY lhp.MaMH,
                        CASE lhp.ThuNgay
                            WHEN '2' THEN 2
                            WHEN '3' THEN 3
                            WHEN '4' THEN 4
                            WHEN '5' THEN 5
                            WHEN '6' THEN 6
                            WHEN '7' THEN 7
                            WHEN 'CN' THEN 8
                            ELSE 9
                        END;";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MaNK", string.IsNullOrEmpty(maNK) ? DBNull.Value : (object)maNK);
                cmd.Parameters.AddWithValue("@MaKhoa", maKhoa.HasValue ? (object)maKhoa.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@MaNganh", maNganh.HasValue ? (object)maNganh.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@MaHP", string.IsNullOrEmpty(maHP) ? DBNull.Value : (object)maHP);
                cmd.Parameters.AddWithValue("@MaPhong", maPhong.HasValue ? (object)maPhong.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@MaMH", string.IsNullOrEmpty(maMH) ? DBNull.Value : (object)maMH);
                cmd.Parameters.AddWithValue("@MaCB", string.IsNullOrEmpty(maCB) ? DBNull.Value : (object)maCB);
                cmd.Parameters.AddWithValue("@Keyword", string.IsNullOrWhiteSpace(keyword) ? DBNull.Value : (object)keyword);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new LopHocPhan
                        {
                            MaLHP = reader.GetString(reader.GetOrdinal("MaLHP")),
                            MaHP = reader["MaHP"]?.ToString(),
                            MaMH = reader["MaMH"]?.ToString(),
                            MaCB = reader["MaCB"]?.ToString(),
                            ThuNgay = reader["ThuNgay"]?.ToString(),
                            TietBatDau = reader["TietBatDau"] != DBNull.Value ? Convert.ToInt32(reader["TietBatDau"]) : null,
                            TietKetThuc = reader["TietKetThuc"] != DBNull.Value ? Convert.ToInt32(reader["TietKetThuc"]) : null,
                            MaPhong = reader["MaPhong"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MaPhong"]) : null,
                            SLHienTai = reader["SLHienTai"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SLHienTai"]) : 0,
                            SLToiDa = reader["SLToiDa"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SLToiDa"]) : null,
                            NgayHoc = reader["NgayHoc"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["NgayHoc"]) : null,
                            GhiChu = reader["GhiChu"]?.ToString(),

                            MonHoc = new MonHoc
                            {
                                TenMH = reader["TenMH"]?.ToString(),
                                SoTC = reader["SoTC"] != DBNull.Value ? Convert.ToInt32(reader["SoTC"]) : null
                            },
                            CanBo = new CanBo
                            {
                                TenCB = reader["TenCB"]?.ToString()
                            },
                            Phong = new PhongHoc
                            {
                                TenPhong = reader["TenPhong"]?.ToString()
                            }
                        });
                    }
                }
            }

            return list;
        }

        public bool AddLopHocPhan(LopHocPhan lopHocPhan)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("AddLopHocPhan", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaHP", lopHocPhan.MaHP ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaMH", lopHocPhan.MaMH ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaCB", lopHocPhan.MaCB ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ThuNgay", lopHocPhan.ThuNgay ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TietBatDau", lopHocPhan.TietBatDau ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TietKetThuc", lopHocPhan.TietKetThuc ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaPhong", lopHocPhan.MaPhong ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SLHienTai", lopHocPhan.SLHienTai ?? 0);
                cmd.Parameters.AddWithValue("@SLToiDa", lopHocPhan.SLToiDa ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayHoc", lopHocPhan.NgayHoc ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GhiChu", lopHocPhan.GhiChu ?? (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool UpdateLopHocPhan(LopHocPhan lopHocPhan)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("UpdateLopHocPhan", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaLHP", lopHocPhan.MaLHP ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaHP", lopHocPhan.MaHP ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaMH", lopHocPhan.MaMH ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaCB", lopHocPhan.MaCB ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ThuNgay", lopHocPhan.ThuNgay ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TietBatDau", lopHocPhan.TietBatDau ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TietKetThuc", lopHocPhan.TietKetThuc ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MaPhong", lopHocPhan.MaPhong ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SLHienTai", lopHocPhan.SLHienTai ?? 0);
                cmd.Parameters.AddWithValue("@SLToiDa", lopHocPhan.SLToiDa ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayHoc", lopHocPhan.NgayHoc ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GhiChu", lopHocPhan.GhiChu ?? (object)DBNull.Value);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool DeleteLopHocPhan(string maLHP)
        {
            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = "DELETE FROM LopHocPhan WHERE MaLHP = @MaLHP";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaLHP", maLHP);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }
    }
}
