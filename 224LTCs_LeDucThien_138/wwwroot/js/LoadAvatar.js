// Xem trước ảnh đại diện khi người dùng chọn ảnh mới
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById('uploadAnh').addEventListener('change', function (e) {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (evt) {
                document.getElementById('anhDaiDien').src = evt.target.result;
            };
            reader.readAsDataURL(file);

            // Hiện nút tải ảnh lên
            document.getElementById('div-btnUploadAvatar').classList.remove('d-none');
        }
    });
});
