document.addEventListener("DOMContentLoaded", function () {
    const rowsPerPageSelect = document.getElementById('rowsPerPage');
    const rows = Array.from(document.querySelectorAll('.data-row'));
    const pagination = document.getElementById('pagination');
    const startRow = document.getElementById('startRow');
    const endRow = document.getElementById('endRow');
    const totalRows = rows.length;

    let currentPage = 1;
    let rowsPerPage = parseInt(rowsPerPageSelect.value);

    function renderRows() {
        const start = (currentPage - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        rows.forEach((row, index) => {
            row.classList.toggle("d-none", index < start || index >= end);
        });

        if (totalRows === 0) {
            startRow.textContent = 0;
            endRow.textContent = 0;
        } else if (totalRows <= rowsPerPageSelect.value) {
            startRow.textContent = start + 1;
            endRow.textContent = totalRows
        } else
        {
            startRow.textContent = start + 1;
            endRow.textContent = end;
        }
    }

    function createPageButton(page, label, isActive = false, isDisabled = false) {
        const li = document.createElement('li');
        li.className = 'page-item';
        if (isActive) li.classList.add('active');
        if (isDisabled) li.classList.add('disabled');

        const a = document.createElement('a');
        a.className = 'page-link';
        a.href = '#';
        a.innerHTML = label;

        a.addEventListener('click', function (e) {
            e.preventDefault();
            if (!isDisabled && currentPage !== page) {
                currentPage = page;
                renderRows();
                renderPagination();
            }
        });

        li.appendChild(a);
        return li;
    }

    function renderPagination() {
        pagination.innerHTML = '';
        const pageCount = Math.ceil(totalRows / rowsPerPage);
        const maxVisiblePages = 10;

        // Previous button
        pagination.appendChild(createPageButton(currentPage - 1, '&laquo;', false, currentPage === 1));

        let startPage = Math.max(1, currentPage - Math.floor(maxVisiblePages / 2));
        let endPage = startPage + maxVisiblePages - 1;

        if (endPage > pageCount) {
            endPage = pageCount;
            startPage = Math.max(1, endPage - maxVisiblePages + 1);
        }

        if (startPage > 1) {
            pagination.appendChild(createPageButton(1, '1', false));
            if (startPage > 2) {
                const li = document.createElement('li');
                li.className = 'page-item disabled';
                li.innerHTML = `<span class="page-link">...</span>`;
                pagination.appendChild(li);
            }
        }

        for (let i = startPage; i <= endPage; i++) {
            pagination.appendChild(createPageButton(i, i, i === currentPage));
        }

        if (endPage < pageCount) {
            if (endPage < pageCount - 1) {
                const li = document.createElement('li');
                li.className = 'page-item disabled';
                li.innerHTML = `<span class="page-link">...</span>`;
                pagination.appendChild(li);
            }
            pagination.appendChild(createPageButton(pageCount, pageCount));
        }

        // Next button
        pagination.appendChild(createPageButton(currentPage + 1, '&raquo;', false, currentPage === pageCount));
    }

    rowsPerPageSelect.addEventListener('change', function () {
        const newRowsPerPage = parseInt(this.value);
        const newPageCount = Math.ceil(totalRows / newRowsPerPage);

        // Nếu trang hiện tại vượt quá tổng trang mới thì gán về trang cuối
        currentPage = Math.min(currentPage, newPageCount);
        rowsPerPage = newRowsPerPage;

        renderRows();
        renderPagination();
    });

    renderRows();
    renderPagination();
});
