let totalP = 1, pageSet = 1, page = 1, numberOfRows;
function createPagination(noOfRows = 10) {
    numberOfRows = noOfRows;
    const totalRows = document.querySelectorAll("tbody>tr").length;
    let pagination = document.querySelector('.pagination');
    if(pagination)
        pagination.innerHTML = "";
    if (Math.ceil(totalRows / numberOfRows) < 1) return;

    pageSet = 1; page = 1;
    totalP = Math.ceil(totalRows / numberOfRows);
    if (totalP > 5) {
        pagination.innerHTML = `<button data-page="previous" class="btn rounded border border-2">
        <img src="/images/previous.png" alt="previous">
        </button>`;
    }
    if (totalP > 1) {
        pagination.innerHTML += `<button data-page="left" class="btn rounded border border-2">
        <img src="/images/left.png" alt="left"></button>`;
        for (let i = 1; i <= totalP; i++) {
            pagination.innerHTML += `<button data-page="${i}"
             class="btn rounded border border-2 shadow-sm color-darkgray fw-light">${i}</button>`;
        }
        pagination.innerHTML += `<button data-page="right" class="btn rounded border border-2">
        <img src="/images/right-arrow1.png" alt="right"></button>`;
    }
    if (totalP > 5) {
        pagination.innerHTML += `<button data-page="next" class="btn rounded border border-2">
        <img src="/images/next.png" alt="next">
        </button>`;
    }
    document.querySelectorAll('[data-page]').forEach((btn) => {
        $(btn).click(() => handlePagination($(btn).data('page')));

        if ($(btn).data('page') <= 5 * (pageSet - 1) || $(btn).data('page') > pageSet * 5)
            $(btn).addClass("d-none")
    });

    // css to current page
    $(`[data-page=${page}]`).addClass('active-page');
    displayRows();
}
function scrollPageSet() {
    document.querySelectorAll('[data-page]').forEach((btn) => {
        if ($(btn).data('page') <= 5 * (pageSet - 1) || $(btn).data('page') > pageSet * 5)
            $(btn).addClass("d-none")
        else
            $(btn).removeClass("d-none");
    });
}
function displayRows() {
    document.querySelectorAll("tbody > tr").forEach((row, index) => {
        if (index >= (page - 1) * numberOfRows && index < page * numberOfRows) {
            $(row).removeClass("d-none");
        }
        else {
            $(row).addClass("d-none");
        }
    })
}
function handlePagination(value) {
    if (value == 'next') {
        if (pageSet * 5 > totalP) return;
        pageSet += 1;
        scrollPageSet();
        return;
    }
    else if (value == 'previous') {
        if (pageSet == 1) return;
        pageSet -= 1;
        scrollPageSet();
        return;
    }
    else if (value == 'left') {
        if (page == 1) return;
        $(`[data-page=${page}]`).removeClass('active-page');
        page = page - 1;
        $(`[data-page=${page}]`).addClass('active-page');
        if (page <= (pageSet - 1) * 5) { pageSet -= 1; scrollPageSet(); }
    } else if (value == 'right') {
        if
            (page == totalP) return; $(`[data-page=${page}]`).removeClass('active-page'); page = page + 1;
        $(`[data-page=${page}]`).addClass('active-page'); if (page > pageSet * 5) {
            pageSet += 1;
            scrollPageSet();
        }
    }
    else {
        if (page == value) {
            window.scrollTo(0, 0);
            return;
        }
        $(`[data-page=${page}]`).removeClass('active-page');
        page = value;
        $(`[data-page=${page}]`).addClass('active-page');
    }
    displayRows();
    window.scrollTo(0, 0);
}