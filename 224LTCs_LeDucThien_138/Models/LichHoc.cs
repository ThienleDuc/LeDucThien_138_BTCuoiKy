using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace _224LTCs_LeDucThien_138.Models
{
    public class LichHoc
    {
        public LichHoc()
        {
        }

        [Key]
        public int? MaLich { get; set; }

        [StringLength(2)]
        public string ThuNgay { get; set; }

        public int? TietBatDau { get; set; }

        public int? TietKetThuc { get; set; }

        // Navigation property
        public ICollection<LopHocPhan> LopHocPhans { get; set; }

        public LichHoc(int? maLich, string thuNgay, int? tietBatDau, int? tietKetThuc, ICollection<LopHocPhan> lopHocPhans)
        {
            MaLich = maLich;
            ThuNgay = thuNgay;
            TietBatDau = tietBatDau;
            TietKetThuc = tietKetThuc;
            LopHocPhans = lopHocPhans;
        }
    }

    public class LichHocRepos
    {
        private readonly ConnectionDatabase _connectionDatabase;

        public LichHocRepos(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
        }

        public List<LichHoc> GetAllLichHoc()
        {
            List<LichHoc> list = new List<LichHoc>();

            using (SqlConnection conn = _connectionDatabase.GetConnection())
            {
                string query = @"SELECT MaLich, ThuNgay, TietBatDau, TietKetThuc FROM LichHoc;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new LichHoc
                        {
                            MaLich = reader.IsDBNull(reader.GetOrdinal("MaLich")) ? null : reader.GetInt32(reader.GetOrdinal("MaLich")),
                            ThuNgay = reader.IsDBNull(reader.GetOrdinal("ThuNgay")) ? null : reader.GetString(reader.GetOrdinal("ThuNgay")),
                            TietBatDau = reader.IsDBNull(reader.GetOrdinal("TietBatDau")) ? null : reader.GetInt32(reader.GetOrdinal("TietBatDau")),
                            TietKetThuc = reader.IsDBNull(reader.GetOrdinal("TietKetThuc")) ? null : reader.GetInt32(reader.GetOrdinal("TietKetThuc")),
                        });
                    }
                }
            }

            return list;
        }
    }

}
