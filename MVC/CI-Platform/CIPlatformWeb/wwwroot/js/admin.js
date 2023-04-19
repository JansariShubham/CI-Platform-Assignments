document.getElementById("sidebarBtn").addEventListener('click', () => {
            document.getElementById("sidebar").classList.toggle("d-none");

        })
        document.getElementById("closeSidebar").addEventListener('click', () => {
            document.getElementById("sidebar").classList.add("d-none");
        })


userAjaxCall();

function userAjaxCall() {
    $.ajax({
        type: "GET",
        url: '/Admin/Dashboard/getAllUsers',

        success: function (data) {

            console.log("success in getting users data");
            $("#userPartial").html(data);
            showAddUserModal();
            editUserData();
            deleteUser();
            updateSidebarHeight();
            searchOperation();

            var search = document.getElementById("searchInput");
            search.focus();
            search.value = searchText;


        },
        error: (err) => {
            console.log("error in  getting users data");
        }
    });
}

function showAddUserModal() {
    
    var addUserBtn = document.getElementById("addUser");
    addUserBtn.addEventListener('click', () => {

        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getAddUserModal',

            success: function (data) {

                console.log("success in getting users modal");
                $("#addUserModalPartial").html(data);
                $('#addUserModal').modal('show');
                addUserData();
            },
            error: (err) => {
                console.log("error in  getting users modal");
            }
        });
       
    })
}

function addUserData() {
    var addUserForm = document.getElementById("addUserForm");
    addUserForm.addEventListener('submit', (e) => {
        e.preventDefault();
        if ($('#addUserForm').valid()) {

            var firstName = $('#firstName').val();
            var lastName = $('#lastName').val();
            var email = $("#email").val();
            var empId = $("#empId").val();
            var department = $("#department").val();
            var password = $("#password").val();

            console.log(firstName + lastName + email + empId + department);
            $('#addUserModal').modal('hide');
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/AddUser',
                data: {firstName : firstName, lastName : lastName, email : email , empId : empId, department : department, password: password},
                success: function (data) {
                    console.log("success adding user")
                    $("#userPartial").html(data);
                    Swal.fire(
                        'User Added Successfully!',
                        'You clicked the button!',
                        'success'
                    )

                },
                error: (err) => {
                    console.log("error in  getting users modal");
                }
            });


       }
        else {
            return;
        }
    })
}

function editUserData() {
    var editUserBtn = document.querySelectorAll(".edit-user");
    editUserBtn.forEach((user) => {
        user.addEventListener('click', () => {
            var userId = user.getAttribute("data-userid");
            //alert(userId);
            $.ajax({
                type: "GET",
                url: '/Admin/Dashboard/getUserDataByUserId',
                data: { userId: userId },
                success: function (data) {

                    //console.log("success in getting users modal");
                    $("#editUserModalPartial").html(data);
                    $('#editUserModal').modal('show');
                    editUser(userId);
                   
                },
                error: (err) => {
                    console.log("error in  getting users modal");
                }
            });
        })
    })
}

function editUser(userId) {
    var editUserForm = document.getElementById("editUserForm");
    editUserForm.addEventListener('submit', (e) => {
        e.preventDefault();
        if ($('#editUserForm').valid()) {

            var firstName = $('#firstName').val();
            var lastName = $('#lastName').val();
            var email = $("#email").val();
            var empId = $("#empId").val();
            var department = $("#department").val();
            var password = $("#password").val();
            
            console.log(firstName + lastName + email + empId + department);
            $('#editUserModal').modal('hide');
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/editUser',
                data: { firstName: firstName, lastName: lastName, email: email, empId: empId, department: department, password: password, userId: userId },
                success: function (data) {
                    console.log("success edit user")
                    $("#userPartial").html(data);
                    editUserData();
                    deleteUser();
                    Swal.fire(
                        'User Updated Successfully!',
                        'You clicked the button!',
                        'success'
                    )

                },
                error: (err) => {
                    console.log("error in  getting users modal");
                }
            });


        }
        else {
            return;
        }
    })
}

function deleteUser() {
    var deleteBtn = document.querySelectorAll(".delete-user");
    deleteBtn.forEach((delBtn) => {
        delBtn.addEventListener('click', () => {
            var userId = delBtn.getAttribute("data-userid");
            var status = delBtn.getAttribute("data-status");
            if (status == 0) {
                Swal.fire({
                    title: "Are you sure?",
                    text: "",
                    icon: "warning",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes",
                    cancelButtonText: "Cancle",
                    closeOnConfirm: false,
                    closeOnCancel: false
                })
                    .then((response) => {
                        if (response.isConfirmed) {
                            $.ajax({
                                type: "POST",
                                url: '/Admin/Dashboard/changeUserStatus',
                                data: { userId: userId, status: status },
                                success: function (data) {

                                    $("#userPartial").html(data);
                                    deleteUser();
                                    editUserData();
                                },
                                error: (err) => {
                                    console.log("error in  getting users modal");
                                }
                            });
                        }
                    });
            }
                        
            else {
                $.ajax({
                    type: "POST",
                    url: '/Admin/Dashboard/changeUserStatus',
                    data: { userId: userId, status: status },
                    success: function (data) {

                        $("#userPartial").html(data);
                        deleteUser();
                        editUserData();
                    },
                    error: (err) => {
                        console.log("error in  getting users modal");
                    }
                });
            }
            
        })
      })
}


var sidebar = document.getElementById("sidebar");
var rightDiv = document.getElementById("rightDiv");


const resizeObserver = new ResizeObserver(entries => {
    for (let entry of entries) {
        updateSidebarHeight();
    }
});

// Start observing the element
resizeObserver.observe(rightDiv);

function updateSidebarHeight() {
    // Get the height of the content
    const contentHeight = rightDiv.offsetHeight;
   // console.log(vhToPixels(100), contentHeight);
    let height = (vhToPixels(100) > contentHeight) ? vhToPixels(100) : contentHeight;
    sidebar.style.height = `${height}px`;
}

function vhToPixels(vh) {
    return Math.round(window.innerHeight / (100 / vh));
}
var searchText;

function searchOperation() {
    var search = document.getElementById("searchInput");
    search.addEventListener('input', () => {
         searchText = search.value;
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getSearchedUsers',
            data: { searchText: searchText },
            success: function (data) {

                $("#userPartial").html(data);
                deleteUser();
                editUserData();
                searchOperation();
            },
            error: (err) => {
                console.log("error in  getting users modal");
            }
        });
       
    })
}