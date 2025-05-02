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