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
    function LoadDataKhoa() {
        const selectedKhoa = document.getElementById('chooseKhoa')?.dataset.selected;
        fetchData('/CanBo/GetAllKhoa', function (data) {
            data.forEach(k => {
                const option = new Option(k.tenKhoa, k.maKhoa);
                if (selectedKhoa && k.maKhoa.toString() === selectedKhoa.toString()) {
                    option.selected = true;
                }
                document.getElementById('chooseKhoa').add(option);
            });
        });
    }

    // Load Chức Vụ
    fetchData('/CanBo/GetAllChucVu', function (data) {
        const chooseCV = document.getElementById('chooseCV');
        const khoaSelect = document.getElementById('chooseKhoa');
        const isAddPage = document.getElementById('SelectWrapper') !== null;
        const isFilterPage = document.getElementById('selectWrapperFilter') !== null;

        const selectedCV = chooseCV?.dataset.selected;

        if (!chooseCV || !khoaSelect) return;

        // Thêm các option chức vụ
        data.forEach(cv => {
            const option = new Option(cv.tenChucVu, cv.maChucVu);
            if (selectedCV && cv.maChucVu.toString() === selectedCV.toString()) {
                option.selected = true;
            }
            chooseCV.add(option);
        });

        // Gắn sự kiện khi thay đổi chức vụ
        chooseCV.addEventListener('change', function () {
            const selectedText = this.options[this.selectedIndex].text.trim().toLowerCase();

            // Với trang thêm cán bộ
            if (isAddPage) {
                if (selectedText === 'giảng viên' || selectedText === 'trưởng khoa' || selectedText === 'phó khoa') {
                    khoaSelect.classList.remove('d-none');
                    LoadDataKhoa();
                } else {
                    khoaSelect.classList.add('d-none');
                }
            }

            // Với trang lọc
            if (isFilterPage) {
                khoaSelect.innerHTML = '';
                if (selectedText === 'giảng viên' || selectedText === 'trưởng khoa' || selectedText === 'phó khoa') {
                    khoaSelect.innerHTML = '<option selected disabled>Chọn khoa</option>';
                    LoadDataKhoa();
                } else {
                    khoaSelect.innerHTML = '<option selected disabled>Chọn khoa</option>';
                }
            }
        });

        chooseCV.dispatchEvent(new Event('change'));
    });


    // Load Học Vị
    fetchData('/CanBo/GetAllHocVi', function (data) {
        const selectedHV = document.getElementById('chooseHV')?.dataset.selected;
        data.forEach(hv => {
            const option = new Option(hv.tenHocVi, hv.maHocVi);
            if (selectedHV && hv.maHocVi.toString() === selectedHV.toString()) {
                option.selected = true;
            }
            document.getElementById('chooseHV').add(option);
        });
    });
    
});