const NAME = "ФИО";
const POSITION = "Должность";

function menuInit() {
    $(document).ready(function () {
        $("#sidebar").mCustomScrollbar({
            theme: "minimal"
        });
 
        $('#sidebarCollapse').on('click', function () {
            $('#sidebar, #content, .navbar').toggleClass('active');
            $('.collapse.in').toggleClass('in');
            $('a[aria-expanded=true]').attr('aria-expanded', 'true');
        });
    });
}

function activeMenuItem() {
    turnOffCurrentActiveMenuItem();

    var url = window.location.href.toLowerCase();
    if (url.indexOf('user') >= 0) {
        $('#user-page-menu-item').addClass('active');
    }
    else if (url.indexOf('login') >= 0) {
        $('#login-page-menu-item').addClass('active');
    }
    else if (url.indexOf('changeemail') >= 0) {
        $('#change-email-page-menu-item').addClass('active');
    }
    else if (url.indexOf('changepassword') >= 0) {
        $('#change-password-page-menu-item').addClass('active');
    }
    else {
        $('#main-page-menu-item').addClass('active');
    }  
}

function turnOffCurrentActiveMenuItem() {
    var menuItems = [];
    menuItems.push($('#user-page-menu-item'));
    menuItems.push($('#mix-page-menu-item'));
    menuItems.push($('#change-email-page-menu-item'));
    menuItems.push($('#change-password-page-menu-item'));
    menuItems.push($('#main-page-menu-item'));

    for (var i = 0; i < menuItems.length; i++) {
        if (menuItems[i].hasClass('active') >= 0) {
            menuItems[i].removeClass('active');
        }
    }
}

function searchEmployeesByEnter() {
    $(document).ready(function () {
        $("#search-input-value").keyup(function (event) {
            if (event.keyCode == 13) {
                searchEmployees('model');
            }
        });
    });
}

function clearSearch() {
    $("#found-items-area").empty();
    $("#search-input-value").val('');
}

function createTd(tdClass, value) {
    var td = document.createElement("td");
    td.className = tdClass;
    td.innerText = value;

    return td;
}

function searchEmployees(type) {
    var searchValue = $("#search-input-value").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var oldSearchingText = document.getElementById('searching').innerText;

    document.getElementById('searching').innerText = "Идет поиск...";

    $.ajax({
        url: "/Employee/FindEmployees",
        type: "Post",
        data: {
            __RequestVerificationToken: token,
            "value": searchValue,
            "type": type
        },
        success: function (html) {
            $("#found-items-area").empty();
            $("#found-items-area").append(html);
            document.getElementById('searching').innerText = oldSearchingText;
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
            document.getElementById('searching').innerText = oldSearchingText;
        }
    });
    return false;
}

function attachEmployee(employeeId) {
    if (document.body.contains(document.getElementById("pinned-" + employeeId))) {
        alert("Сотрудник уже в списке.");
        return false;
    }

    var employeeInfo = $("#" + employeeId);
    var name = employeeInfo.find("td.name")[0].innerText;
    var position = employeeInfo.find("td.position")[0].innerText;

    var inputId = document.createElement("input");
    inputId.type = "hidden";
    inputId.name = "employeeId[]";
    inputId.value = employeeId;

    var nameDiv = createDiv("name", NAME, name);
    var positionDiv = createDiv("position", POSITION, position);

    var buttonDiv = createButtonDiv(employeeId);

    var wrapDiv = document.createElement("div");
    wrapDiv.classList.add("col-md-8", "item");

    var newEmployee = document.createElement("div");
    newEmployee.className = "row";
    newEmployee.id = "pinned-" + employeeId;

    wrapDiv.appendChild(inputId);
    wrapDiv.appendChild(nameDiv);
    wrapDiv.appendChild(positionDiv);

    wrapDiv.appendChild(createElement("br"));
    wrapDiv.appendChild(buttonDiv);

    newEmployee.appendChild(wrapDiv);

    var attachedItems = document.getElementById("attached-items");
    attachedItems.appendChild(newEmployee);
}

function createElement(element) {
    return document.createElement(element);
}

function createDiv(divClass, title, value, employeeId) {
    var buttonDiv = createButtonDiv(employeeId);

    var wrapDiv = document.createElement("div");
    wrapDiv.classList.add(divClass, "row", "item-info-row");

    var firstDiv = document.createElement("div");
    firstDiv.classList.add("col-md-3", "item-info");

    var bold = document.createElement("b");
    bold.innerText = title;

    firstDiv.appendChild(bold);

    var secondDiv = document.createElement("div");
    secondDiv.classList.add("col-md-6", "item-info");
    secondDiv.innerText = value;

    wrapDiv.appendChild(firstDiv);
    wrapDiv.appendChild(secondDiv);

    return wrapDiv;
}

function createButtonDiv(employeeId) {
    var removeBtn = createRemoveButton(employeeId);
    var detailsBtn = createDetailsButton(employeeId);

    var buttonDiv = document.createElement("div");
    buttonDiv.className = "btn-group";
    buttonDiv.classList.add("btn-group", "float-right");

    buttonDiv.appendChild(removeBtn);
    buttonDiv.appendChild(detailsBtn);

    return buttonDiv;
}

function createRemoveButton(employeeId) {
    var button = document.createElement("button");
    button.classList.add("btn", "btn-danger", "btn-sm");
    button.type = "button";
    button.setAttribute("onclick", "detachItem('" + employeeId + "')");
    button.innerText = "Убрать";

    return button;
}

function createDetailsButton(employeeId) {
    var button = document.createElement("a");
    button.setAttribute("href", "/Employee/Details?id=" + employeeId);
    button.classList.add("btn", "btn-primary", "btn-sm");
    button.setAttribute("target", "_blank");
    button.innerText = "Подробности";

    return button;
}

function detachItem(id) {
    var toRemove = $("#pinned-" + id);
    toRemove.remove();
}

function modalRemovalWindow(url) {
    $(document).ready(function () {
        var firstPaginationPage = $('.pagination li:nth-child(2)>a');
        var elementId;
        $('.delete-prompt').on('click', function () {
            elementId = $(this).attr('id');
            $('#myModal').modal('show');
        });

        $('.delete-confirm').on('click', function () {
            var token = $('input[name="__RequestVerificationToken"]').val();
            if (elementId != '') {
                $.ajax({
                    url: url,
                    type: 'Post',
                    data: {
                        __RequestVerificationToken: token,
                        'id': elementId
                    },
                    success: function (data) {
                        if (data == 'Удаление невозможно.') {
                            $('.delete-confirm').css('display', 'none');
                            $('.delete-cancel').html('Закрыть');
                            $('.success-message').html('Удаление невозможно, у записи есть связи!');
                        }
                        else if (data) {
                            $("#" + elementId).remove();
                            $('#myModal').modal('hide');
                            //$.notify("Запись удалена успешно!", "success");
                            location.reload();
                        }
                    }, error: function (err) {
                        if (!$('.modal-header').hasClass('alert-danger')) {
                            $('.modal-header').removeClass('alert-success').addClass('alert-danger');
                            $('.delete-confirm').css('display', 'none');
                        }
                        $('.success-message').html(err.statusText);
                    }
                });
            }
        });
        //function to reset bootstrap modal popups
        $("#myModal").on("hidden.bs.modal", function () {
            $('.delete-confirm').css('display', 'inline-block');
            $('.delete-cancel').html('Нет');
            $('.success-message').html('').html('Вы действительно хотите удалить запись?');
        });
    });
}

function hideAccordion() {
    $("#accordion").hide();
}

function removeListAndPagination() {
    $("#listTable").remove();
    $("#paginationToDelete").remove();
}

function toPrevMain(from = "") {
    if (from == "list") {
        $("#employee-list").empty();
    } else if (from == "admin") {
        $("#name").val("");
        $("#results").empty();
    } else {
        $("#results").empty();
    }
    $("#accordion").show();
}

function closeMessageDiv() {
    $("#message-div").remove();
}

function closeSuccessFormSaveMessage() {
    document.getElementById("form-saved-success").classList.add("d-none");
}

function closeFailedFormSaveMessage() {
    document.getElementById("form-saved-failed").classList.add("d-none");
}
