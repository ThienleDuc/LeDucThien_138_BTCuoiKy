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
        const data_selected = document.getElementById('chooseKhoa')?.dataset.selected;
        data.forEach(k => {
            const option = new Option(k.tenKhoa, k.maKhoa);
            if (data_selected && k.maKhoa.toString() === data_selected.toString()) {
                option.selected = true;
            }
            document.getElementById('chooseKhoa').add(option);
        });
        LoadDataChuyenNganh();
        LoadDataGiangVien();
    });


    // Load Niên Khóa
    fetchData('/LopHocPhan/GetAllNienKhoa', function (data) {
        const data_selected = document.getElementById('chooseNK')?.dataset.selected;
        data.forEach(nk => {
            const option = new Option(nk.tenNK, nk.maNK);
            if (data_selected && nk.maNK.toString() === data_selected.toString()) {
                option.selected = true;
            }
            document.getElementById('chooseNK').add(option);
        });
        LoadDataHocPhan();
    });

    // Load Phòng Học
    fetchData('/LopHocPhan/GetAllPhongHoc', function (data) {
        const data_selected = document.getElementById('choosePH')?.dataset.selected;
        data.forEach(ph => {
            const option = new Option(ph.tenPhong, ph.maPhong);
            if (data_selected && ph.maPhong.toString() === data_selected.toString()) {
                option.selected = true;
            }
            document.getElementById('choosePH').add(option);
        });
    });

    // Load Môn Học
    function LoadDataMonHoc() {
        const maMHSelect = document.getElementById('chooseMH');
        const maNganh = parseInt(document.getElementById('chooseCN').value);
        const data_selected = document.getElementById('chooseMH')?.dataset.selected;

        maMHSelect.innerHTML = '';
        maMHSelect.innerHTML = '<option selected disabled>Chọn môn học</option>'

        fetchData(`/LopHocPhan/GetMonHocByChuyenNganh?maNganh=${maNganh}`, function (data) {
            data.forEach(mh => {
                const option = new Option(mh.tenMH, mh.maMH);
                if (data_selected && mh.maMH.toString() === data_selected.toString()) {
                    option.selected = true;
                }
                maMHSelect.add(option);
            });
        });


    }

    // Load Chuyên Ngành
    function LoadDataChuyenNganh() {
        const maNganhSelect = document.getElementById('chooseCN');
        const maKhoa = parseInt(document.getElementById('chooseKhoa').value);
        const data_selected = document.getElementById('chooseCN')?.dataset.selected;


        maNganhSelect.innerHTML = '';
        maNganhSelect.innerHTML = '<option selected disabled>Chọn chuyên ngành</option>'

        fetchData(`/LopHocPhan/GetChuyenNganhByKhoa?maKhoa=${maKhoa}`, function (data) {
            data.forEach(cn => {
                const option = new Option(cn.tenNganh, cn.maNganh);
                if (data_selected && cn.maNganh.toString() === data_selected.toString()) {
                    option.selected = true;
                }
                maNganhSelect.add(option);
            });

            if (data_selected) {
                LoadDataMonHoc();
            }
        });

        maNganhSelect.addEventListener('change', function () {
            LoadDataMonHoc();
        })
    }

    // Load Giảng Viên
    function LoadDataGiangVien() {
        const maCBSelect = document.getElementById('chooseCB');
        const maKhoa = parseInt(document.getElementById('chooseKhoa').value);
        const data_selected = document.getElementById('chooseCB')?.dataset.selected;

        maCBSelect.innerHTML = '';
        maCBSelect.innerHTML = '<option selected disabled>Chọn giảng viên</option>'

        fetchData(`/LopHocPhan/GetGiangVienByKhoa?maKhoa=${maKhoa}`, function (data) {
            data.forEach(cb => {
                const option = new Option(cb.tenCB, cb.maCB);
                if (data_selected && cb.maCB.toString() === data_selected.toString()) {
                    option.selected = true;
                }
                maCBSelect.add(option);
            });
        });
    }

    // Load Học Phần
    function LoadDataHocPhan() {
        const maHPSelect = document.getElementById('chooseHP');
        const maNK = document.getElementById('chooseNK').value;
        const data_selected = document.getElementById('chooseHP')?.dataset.selected;

        maHPSelect.innerHTML = '';
        maHPSelect.innerHTML = '<option selected disabled>Chọn học phần</option>'

        fetchData(`/LopHocPhan/GetHocPhanByNienKhoa?maNK=${maNK}`, function (data) {
            data.forEach(hp => {
                const option = new Option(hp.maHP, hp.maHP);
                if (data_selected && hp.maHP.toString() === data_selected.toString()) {
                    option.selected = true;
                }
                maHPSelect.add(option);
            });
        });
    }

    function LoadDaTaLichHoc() {
        const thuNgaySelect = document.getElementById('chooseThu');
        const tietBDSelect = document.getElementById('chooseTietBD');
        const tietKTSelect = document.getElementById('chooseTietKT');
        const thuNgay_data_selected = document.getElementById('chooseThu')?.dataset.selected;
        const tietBD_data_selected = document.getElementById('chooseTietBD')?.dataset.selected;
        const tietKT_data_selected = document.getElementById('chooseTietKT')?.dataset.selected;

        if (!thuNgaySelect || !tietBDSelect || !tietKTSelect || !thuNgay_data_selected || !tietBD_data_selected || !tietKT_data_selected) return;

        // Xoá các option cũ nếu có
        thuNgaySelect.innerHTML = '';
        thuNgaySelect.innerHTML = '<option selected disabled>Thứ</option>';

        for (let i = 2; i <= 8; i++) {
            const option = document.createElement("option");
            option.value = i != 8 ? i : "CN";
            option.text = i != 8 ? "Thứ " + i : "Chủ nhật";
            thuNgaySelect.appendChild(option);

            if (thuNgay_data_selected && i.toString() === thuNgay_data_selected.toString()) {
                option.selected = true;
            }
        }

        // Tạo tiết bắt đầu (tiết 1 -> 12)
        tietBDSelect.innerHTML = "";
        tietBDSelect.innerHTML = '<option selected disabled>Từ</option>';

        tietKTSelect.innerHTML = "";
        tietKTSelect.innerHTML = '<option selected disabled>Đến</option>';

        for (let i = 1; i <= 12; i++) {
            let option1 = document.createElement("option");
            option1.value = i;
            option1.text = i;
            tietBDSelect.appendChild(option1);

            if (tietBD_data_selected && i.toString() === tietBD_data_selected.toString()) {
                option1.selected = true;
            }

            let option2 = document.createElement("option");
            option2.value = i;
            option2.text = i;
            tietKTSelect.appendChild(option2);

            if (tietKT_data_selected && i.toString() === tietKT_data_selected.toString()) {
                option2.selected = true;
            }
        }
    }
    LoadDaTaLichHoc()

    // Khi chọn Niên Khóa → Hiển thị Học Phần
    document.getElementById('chooseNK').addEventListener('change', function () {
        LoadDataHocPhan();
    });

    document.getElementById('chooseKhoa').addEventListener('change', function () {
        LoadDataChuyenNganh();
        LoadDataGiangVien();
    });
});