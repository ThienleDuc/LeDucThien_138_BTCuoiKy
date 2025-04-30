document.addEventListener("DOMContentLoaded", function () {
    // Gắn sự kiện cho nút bấm ẩn hiện mật khẩu
    var togglePassword = document.getElementById('togglePassword');
    togglePassword.addEventListener('click', function () {
        var pwInput = document.getElementById("password");
        var eyeIcon = document.getElementById("eyeIcon");

        if (pwInput.type === "password") {
            pwInput.type = "text";
            eyeIcon.classList.remove("fa-eye");
            eyeIcon.classList.add("fa-eye-slash");
        } else {
            pwInput.type = "password";
            eyeIcon.classList.remove("fa-eye-slash");
            eyeIcon.classList.add("fa-eye");
        }
    })
})

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
