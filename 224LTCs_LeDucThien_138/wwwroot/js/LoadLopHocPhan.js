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
    fetchData('/LopHocPhan/GetAllKhoa', function (data) {
        data.forEach(k => {
            const option = new Option(k.tenKhoa, k.maKhoa);
            document.getElementById('chooseKhoa').add(option);
        });
    });


    // Load Niên Khóa
    fetchData('/LopHocPhan/GetAllNienKhoa', function (data) {
        data.forEach(nk => {
            const option = new Option(nk.tenNK, nk.maNK);
            document.getElementById('chooseNK').add(option);
        });
    });

    // Load Phòng Học
    fetchData('/LopHocPhan/GetAllPhongHoc', function (data) {
        data.forEach(ph => {
            const option = new Option(ph.tenPhong, ph.maPhong);
            document.getElementById('choosePH').add(option);
        });
    });

    // Load Lịch Học
    fetchData('/LopHocPhan/GetALLLichHoc', function (data) {
        data.forEach(lh => {
            const option = new Option(`Thứ: ${lh.thuNgay}, Từ tiết: ${lh.tietBatDau} → đến tiết: ${lh.tietKetThuc}`, lh.maLich);
            document.getElementById('chooseLH').add(option);
        });
    });


    // Load Môn Học
    function LoadDataMonHoc() {
        const maMHSelect = document.getElementById('chooseMH');
        const maNganh = parseInt(document.getElementById('chooseCN').value);

        maMHSelect.innerHTML = '';
        maMHSelect.innerHTML = '<option selected disabled>Chọn môn học</option>'

        fetchData(`/LopHocPhan/GetMonHocByChuyenNganh?maNganh=${maNganh}`, function (data) {
            data.forEach(mh => {
                const option = new Option(mh.tenMH, mh.maMH);
                maMHSelect.add(option);
            });
        });
    }

    // Load Chuyên Ngành
    function LoadDataChuyenNganh() {
        const maNganhSelect = document.getElementById('chooseCN');
        const maKhoa = parseInt(document.getElementById('chooseKhoa').value);

        maNganhSelect.innerHTML = '';
        maNganhSelect.innerHTML = '<option selected disabled>Chọn chuyên ngành</option>'

        fetchData(`/LopHocPhan/GetChuyenNganhByKhoa?maKhoa=${maKhoa}`, function (data) {
            data.forEach(cn => {
                const option = new Option(cn.tenNganh, cn.maNganh);
                maNganhSelect.add(option);
            });
        });

        maNganhSelect.addEventListener('change', function () {
            LoadDataMonHoc();
        })
    }

    // Load Giảng Viên
    function LoadDataGiangVien() {
        const maCBSelect = document.getElementById('chooseCB');
        const maKhoa = parseInt(document.getElementById('chooseKhoa').value);

        maCBSelect.innerHTML = '';
        maCBSelect.innerHTML = '<option selected disabled>Chọn giảng viên</option>'

        fetchData(`/LopHocPhan/GetGiangVienByKhoa?maKhoa=${maKhoa}`, function (data) {
            data.forEach(cb => {
                const option = new Option(cb.tenCB, cb.maCB);
                maCBSelect.add(option);
            });
        });
    }

    // Load Học Phần
    function LoadDataHocPhan() {
        const maHPSelect = document.getElementById('chooseHP');
        const maNK = document.getElementById('chooseNK').value;

        maHPSelect.innerHTML = '';
        maHPSelect.innerHTML = '<option selected disabled>Chọn học phần</option>'

        fetchData(`/LopHocPhan/GetHocPhanByNienKhoa?maNK=${maNK}`, function (data) {
            data.forEach(hp => {
                const option = new Option(hp.maHP, hp.maHP);
                maHPSelect.add(option);
            });
        });
    }

    // Khi chọn Khoa → Hiển thị chuyên ngành, Cán bộ - giảng viên
    document.getElementById('chooseKhoa').addEventListener('change', function () {
        LoadDataChuyenNganh();
        LoadDataGiangVien();
    });

    // Khi chọn Niên Khóa → Hiển thị Học Phần
    document.getElementById('chooseNK').addEventListener('change', function () {
        LoadDataHocPhan()
    });
});