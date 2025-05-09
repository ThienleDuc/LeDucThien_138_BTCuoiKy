import { initPagination } from './paginationModule.js';

document.addEventListener("DOMContentLoaded", function () {
    // Hàm helper: gọi GET và parse JSON
    function fetchData(url, callback) {
        fetch(url)
            .then(response => response.json())
            .then(data => callback(data))
            .catch(err => console.error("Fetch error:", err));
    }

    initPagination({
        tableSelector: '.data-row',
        rowsPerPageSelector: '#rowsPerPage',
        paginationSelector: '#pagination',
        startRowSelector: '#startRow',
        endRowSelector: '#endRow'
    });

    // Load Khoa
    fetchData('/SinhVien/GetAllKhoa', function (data) {
        data.forEach(k => {
            const option = new Option(k.tenKhoa, k.maKhoa);
            document.getElementById('chooseKhoa').add(option);
        });
    });

    // Load Niên Khóa
    fetchData('/SinhVien/GetAllNienKhoa', function (data) {
        data.forEach(nk => {
            const option = new Option(nk.tenNK, nk.maNK);
            document.getElementById('chooseNK').add(option);
        });
    });


    // Khi chọn Khoa → Hiển thị chuyên ngành
    document.getElementById('chooseKhoa').addEventListener('change', function () {
        const maNganhSelect = document.getElementById('chooseCN');
        const maLSHSelect = document.getElementById('chooseLSH');
        const maKhoa = parseInt(this.value);

        maNganhSelect.innerHTML = '';
        maNganhSelect.innerHTML = '<option selected disabled>Chuyên ngành</option>'
        maLSHSelect.innerHTML = '';
        maLSHSelect.innerHTML = '<option selected disabled>Lớp sinh hoạt</option>'

        fetchData(`/SinhVien/GetChuyenNganhByKhoa?maKhoa=${maKhoa}`, function (data) {
            data.forEach(cn => {
                const option = new Option(cn.tenNganh, cn.maNganh);
                maNganhSelect.add(option);
            });
        });

    });

    // Khi chọn niên khóa → Hiển thị lớp sinh hoạt
    document.getElementById('chooseNK').addEventListener('change', function () {
        const maNK = this.value;
        const maNganhValue = document.getElementById('chooseCN').value
        const maNganh = maNganhValue !== "" ? maNganhValue : null;
        const maLSHSelect = document.getElementById('chooseLSH');

        maLSHSelect.innerHTML = '';
        maLSHSelect.innerHTML = '<option selected disabled>Lớp sinh hoạt</option>';

        fetchData(`/SinhVien/GetLopSinhHoat?maNganh=${maNganh}&maNK=${maNK}`, function (data) {
            data.forEach(lsh => {
                const option = new Option(lsh.tenLSH, lsh.maLSH);
                maLSHSelect.add(option);
            });
        });
    })
});