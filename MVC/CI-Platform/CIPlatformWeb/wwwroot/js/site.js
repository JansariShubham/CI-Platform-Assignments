// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const drop = document.querySelector(".cms-dropdown");
if (drop) {
    $.ajax({
        url: "/Users/Home/GetCmsList",
        success: (result) => {
            for (var cms of result) {
                drop.innerHTML += `<li><a href="/Users/Home/CmsPage/${cms.cmsPageId}" class="dropdown-item">${cms.title}</a></li>`;
            }
        },
        error: error => {
            console.log(error);
        }
    });
}
$(document).ready(function () {
    const filterBtn = document.querySelector(".filter-btn");
    const sideBar = document.querySelector("#filter-menu");

    filterBtn.addEventListener("click", () => {
        sideBar.classList.toggle("hide-aside");
    });

    const gridBtn = document.querySelector(".grid-btn");
    const listBtn = document.querySelector(".list-btn");
    const listView = document.querySelector(".list-view");
    const gridView = document.querySelector(".grid-view");
    gridBtn.addEventListener("click", () => {
        listView.classList.add("d-none");
        gridView.classList.remove("d-none")

    });

    listBtn.addEventListener("click", () => {
        gridView.classList.add("d-none");
        listView.classList.remove("d-none")

    });
    const searchBtn = document.getElementById("search-btn");
    const searchBar = document.getElementById("search-bar");


    searchBtn.addEventListener("click", () => {
        searchBar.focus();
    });
});


var contactUsModal = document.getElementById("contactModal");
contactUsModal.addEventListener("click", () => {
    $.ajax({
        url: '/Users/Home/GetContactUsData',
        method: "GET",
        success: function (res) {
            
            $("#contactUsPartial").html(res);
            $("#contactUsModal").modal('show');
            contactForm();

        },
        error: (err) => {
            console.log("error in contact us");
            console.log(err);
        }
    })

})


/*$("#contactUsForm").on('submit', (e) => {
    e.preventDefault();
    alert("hello");
    if ($("#contactUsForm").valid) {
        var userId = $("#userId").val();
        console.log(userId);
    }
    else {
        return;
    }
})*/

function contactForm() {

    var contactForm = document.getElementById("contactUsForm");
    contactForm.addEventListener("submit", (e) => {
        e.preventDefault();
        if ($("#contactUsForm").valid()) {

            var userId = document.getElementById("userId").value;
            var subject = document.getElementById("subject").value;
            var message = document.getElementById("message").value;
             console.log( userId + subject + message)

            $.ajax({
                url: '/Users/Home/AddContactUsDetails',
                type: "POST",
                data: { userId: userId, subject: subject, message: message },
                success: (result) => {
                    console.log("success in adding data")
                    $("#contactUsModal").modal('hide');
                    

                },
                error: (err) => {
                    console.log("error in contact us");
                    console.log(err);
                }
            })
        }
        else {
            return;
        }



    


})
}







