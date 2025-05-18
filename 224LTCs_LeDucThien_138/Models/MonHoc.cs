using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _224LTCs_LeDucThien_138.Models
{
    public class MonHoc
    {
        public MonHoc()
        {
        }

        [Key]
        public string MaMH { get; set; }

        public int? MaNganh{ get; set; }

        [StringLength(50)]
        public string? TenMH { get; set; }

        public int? SoTC { get; set; }

        [ForeignKey("MaNganh")]
        public ChuyenNganh ChuyenNganh { get; set; }

        public string? TenNganh => ChuyenNganh?.TenNganh;
        // Navigation property
        public ICollection<LopHocPhan> LopHocPhans { get; set; }

        public MonHoc(string maMH, int? maNganh, string? tenMH, int? soTC, ChuyenNganh chuyenNganh, ICollection<LopHocPhan> lopHocPhans)
        {
            MaMH = maMH;
            MaNganh = maNganh;
            TenMH = tenMH;
            SoTC = soTC;
            ChuyenNganh = chuyenNganh;
            LopHocPhans = lopHocPhans;
        }
    }

    public class MonHocRepos
    {
        private ConnectionDatabase _connectionDatabase;

        public MonHocRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<MonHoc> GetMonHocByNganh(int? maNganh)
        {
            List<MonHoc> list = new List<MonHoc>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT * FROM MonHoc WHERE MaNganh = @MaNganh;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNganh", maNganh);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new MonHoc
                        {
                            MaMH = reader.GetString(reader.GetOrdinal("MaMH")),
                            MaNganh = reader.IsDBNull(reader.GetOrdinal("MaNganh")) ? null : reader.GetInt32(reader.GetOrdinal("MaNganh")),
                            TenMH = reader.IsDBNull(reader.GetOrdinal("TenMH")) ? null : reader.GetString(reader.GetOrdinal("TenMH")),
                            SoTC = reader.IsDBNull(reader.GetOrdinal("SoTC")) ? null : reader.GetInt32(reader.GetOrdinal("SoTC")),
                        });
                    }
                }
            }

            return list;
        }

    }
}
