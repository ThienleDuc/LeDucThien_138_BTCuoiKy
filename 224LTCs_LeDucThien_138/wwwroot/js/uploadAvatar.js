// Xử lý sự kiện upload ảnh
var uploadAnh = document.getElementById('uploadAnh');
uploadAnh.addEventListener('change', function (event) {
    var file = event.target.files[0]; // Lấy tệp người dùng chọn
    if (file) {
        var reader = new FileReader(); // Tạo một đối tượng FileReader để đọc tệp
        reader.onload = function (e) {
            var img = new Image(); // Tạo đối tượng Image để tải ảnh
            img.onload = function () {
                // Tạo một canvas để xử lý ảnh
                var canvas = document.createElement('canvas');
                var ctx = canvas.getContext('2d'); // Lấy ngữ cảnh 2D để vẽ ảnh

                // Tạo tỷ lệ mới cho ảnh (ví dụ resize xuống 150px chiều rộng)
                var newWidth = 150;
                var scale = newWidth / img.width; // Tính toán tỷ lệ phóng to/thu nhỏ
                canvas.width = newWidth;
                canvas.height = img.height * scale;

                // Vẽ lại ảnh đã resize lên canvas
                ctx.drawImage(img, 0, 0, canvas.width, canvas.height);

                // Kiểm tra định dạng của tệp để quyết định lưu dưới dạng JPEG hoặc PNG
                var fileType = file.type; // 'image/jpeg' hoặc 'image/png'
                var mimeType = (fileType === 'image/png') ? 'image/png' : 'image/jpeg'; // Nếu là PNG thì lưu dạng PNG, nếu không thì là JPEG

                // Chuyển canvas thành Blob (dữ liệu nhị phân của ảnh)
                canvas.toBlob(function (blob) {
                    var url = URL.createObjectURL(blob); // Tạo URL cho Blob ảnh
                    document.getElementById('anhDaiDien').src = url; // Đặt ảnh đã được resize vào thẻ <img> có id 'anhDaiDien'

                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var arrayBuffer = e.target.result; // Lấy mảng buffer của Blob
                        var bytes = new Uint8Array(arrayBuffer); // Chuyển đổi mảng buffer thành mảng byte

                        var binaryString = '';
                        for (var i = 0; i < bytes.length; i++) {
                            binaryString += bytes[i].toString(2).padStart(8, '0'); // Chuyển từng byte thành chuỗi nhị phân 8 bit
                        }
                        // In dãy bit ảnh ra console (nếu cần)
                    };

                    reader.readAsArrayBuffer(blob); // Đọc Blob dưới dạng ArrayBuffer
                }, mimeType); // Định dạng ảnh khi lưu
            };
            img.src = e.target.result; // Đọc ảnh từ Data URL
        };
        reader.readAsDataURL(file); // Đọc tệp dưới dạng Data URL để hiển thị ngay lập tức
    }
});