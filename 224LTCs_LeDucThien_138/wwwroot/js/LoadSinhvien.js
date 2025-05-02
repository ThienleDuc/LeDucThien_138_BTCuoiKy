document.addEventListener("DOMContentLoaded", function () {
    // Hàm helper: gọi GET và parse JSON
    function fetchData(url, callback) {
        fetch(url)
            .then(response => response.json())
            .then(data => callback(data))
            .catch(err => console.error("Fetch error:", err));
    }

    const maNienKhoaSelect = document.getElementById('chooseNK');
    const maKhoaSelect = document.getElementById('chooseKhoa');
    const maNganhSelect = document.getElementById('chooseCN');
    const maLSHSelect = document.getElementById('chooseLSH');

    // Load Khoa
    fetchData('/SinhVien/GetAllKhoa', function (data) {
        data.forEach(k => {
            const option = new Option(k.tenKhoa, k.maKhoa);
            maKhoaSelect.add(option);
        });
    });

    // Load Niên Khóa
    fetchData('/SinhVien/GetAllNienKhoa', function (data) {
        data.forEach(nk => {
            const option = new Option(nk.tenNK, nk.maNK);
            maNienKhoaSelect.add(option);
        });
    });

    // Khi chọn Khoa → Hiển thị chuyên ngành
    document.getElementById('chooseKhoa').addEventListener('change', function () {
        const maKhoa = this.value;

        maNganhSelect.innerHTML = '';
        maNganhSelect.innerHTML = '<option selected disabled class="text-center">Chuyên ngành</option>'
        maLSHSelect.innerHTML = '';
        maLSHSelect.innerHTML = '<option selected disabled class="text-center">Lớp sinh hoạt</option>'

        fetchData(`/SinhVien/GetChuyenNganhByKhoa?maKhoa=${maKhoa}`, function (data) {
            data.forEach(cn => {

                const option = new Option(cn.tenNganh, cn.maNganh);
                maNganhSelect.add(option);
            });
        });
    });

    // Khi chọn Chuyên ngành hoặc Niên khóa → Hiển thị LSH
    function updateLopSinhHoat() {
        const maNganh = maNganhSelect.value;
        const maNK = maNienKhoaSelect.value;

        maLSHSelect.innerHTML = '';
        maLSHSelect.innerHTML = '<option selected disabled class="text-center">Lớp sinh hoạt</option>'

        fetchData(`/SinhVien/GetLopSinhHoat?maNganh=${maNganh}&maNK=${maNK}`, function (data) {
            data.forEach(lsh => {
                const option = new Option(lsh.tenLSH, lsh.maLSH);
                maLSHSelect.add(option);
            });
        });
    }

    function updateTableSinhVien() {
        const container = document.getElementById('TableSinhVien-container');

        if (!maNienKhoaSelect || !maKhoaSelect || !maNganhSelect || !maLSHSelect || !container) {
            console.error("Không tìm thấy phần tử DOM cần thiết.");
            return;
        }

        const maNienKhoa = maNienKhoaSelect.value;
        const maKhoa = parseInt(maKhoaSelect.value) || 0;
        const maNganh = parseInt(maNganhSelect.value) || 0;
        const maLSH = parseInt(maLSHSelect.value) || 0;

        const url = `/SinhVien/Index?makhoa=${maKhoa}&maNganh=${maNganh}&maLSH=${maLSH}&maNienKhoa=${encodeURIComponent(maNienKhoa)}`;

        fetch(url, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`Lỗi HTTP: ${response.status}`);
                }
                return response.text();
            })
            .then(html => {
                container.innerHTML = html;
            })
            .catch(error => {
                console.error('Lỗi khi tải bảng sinh viên:', error);
                container.innerHTML = "<p class='text-danger'>Không thể tải dữ liệu.</p>";
            });
    }

    const controls_ULSH = [maNienKhoaSelect, maNganhSelect];
    controls_ULSH.forEach(select => {
        select.addEventListener('change', updateLopSinhHoat);
    });

    const controls_UTSV = [maNienKhoaSelect, maKhoaSelect, maNganhSelect, maLSHSelect];
    controls_UTSV.forEach(select => {
        select.addEventListener('change', updateTableSinhVien);
    });
});