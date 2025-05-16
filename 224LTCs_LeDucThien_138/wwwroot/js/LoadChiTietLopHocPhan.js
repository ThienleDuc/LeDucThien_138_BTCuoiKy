import { initPagination } from './paginationModule.js';

document.addEventListener("DOMContentLoaded", () => {
    initPagination({
        tableSelector: '.data-row',
        rowsPerPageSelector: '#rowsPerPage',
        paginationSelector: '#pagination',
        startRowSelector: '#startRow',
        endRowSelector: '#endRow'
    });
});
