export function initPagination({
    tableSelector = '.data-row',
    rowsPerPageSelector = '#rowsPerPage',
    paginationSelector = '#pagination',
    startRowSelector = '#startRow',
    endRowSelector = '#endRow'
}) {
    let rowsPerPageSelect = document.querySelector(rowsPerPageSelector);
    let pagination = document.querySelector(paginationSelector);
    let startRow = document.querySelector(startRowSelector);
    let endRow = document.querySelector(endRowSelector);

    if (rowsPerPageSelect && pagination && startRow && endRow) {
        let rows = Array.from(document.querySelectorAll(tableSelector));
        let totalRows = rows.length;

        let currentPage = 1;
        let rowsPerPage = parseInt(rowsPerPageSelect.value);

        function renderRows() {
            let start = (currentPage - 1) * rowsPerPage;
            let end = start + rowsPerPage;

            rows.forEach((row, index) => {
                row.classList.toggle("d-none", index < start || index >= end);
            });

            if (totalRows === 0) {
                startRow.textContent = 0;
                endRow.textContent = 0;
            } else {
                startRow.textContent = start + 1;
                endRow.textContent = Math.min(end, totalRows);
            }
        }

        function createPageButton(page, label, isActive = false, isDisabled = false) {
            let li = document.createElement('li');
            li.className = 'page-item';
            if (isActive) li.classList.add('active');
            if (isDisabled) li.classList.add('disabled');

            let a = document.createElement('a');
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
            if (totalRows === 0) return;

            let pageCount = Math.ceil(totalRows / rowsPerPage);
            let maxVisiblePages = 10;

            pagination.appendChild(createPageButton(currentPage - 1, '&laquo;', false, currentPage === 1));

            let startPage = Math.max(1, currentPage - Math.floor(maxVisiblePages / 2));
            let endPage = startPage + maxVisiblePages - 1;

            if (endPage > pageCount) {
                endPage = pageCount;
                startPage = Math.max(1, endPage - maxVisiblePages + 1);
            }

            if (startPage > 1) {
                pagination.appendChild(createPageButton(1, '1'));
                if (startPage > 2) {
                    let li = document.createElement('li');
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
                    let li = document.createElement('li');
                    li.className = 'page-item disabled';
                    li.innerHTML = `<span class="page-link">...</span>`;
                    pagination.appendChild(li);
                }
                pagination.appendChild(createPageButton(pageCount, pageCount));
            }

            pagination.appendChild(createPageButton(currentPage + 1, '&raquo;', false, currentPage === pageCount));
        }

        rowsPerPageSelect.addEventListener('change', function () {
            rowsPerPage = parseInt(this.value);
            let newPageCount = Math.ceil(totalRows / rowsPerPage);
            currentPage = Math.min(currentPage, newPageCount);
            renderRows();
            renderPagination();
        });

        renderRows();
        renderPagination();
    }
}
