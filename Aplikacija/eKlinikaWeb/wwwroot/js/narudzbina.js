var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else {
        if (url.includes("cancelled")) {
            loadDataTable("cancelled");
        }
        else {
            if (url.includes("completed")) {
                loadDataTable("completed");
            }
            else {
                loadDataTable("all");
            }
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Narudzbina/GetAll?status=" + status
        },
        "columns": [
            { "data": "id", "width": "15%" },
            { "data": "ime", "width": "15%" },
            { "data": "brojTelefona", "width": "15%" },
            { "data": "applicationUser.userName", "width": "15%" },
            { "data": "statusIsporuke", "width": "15%" },
            { "data": "ukupnaSuma", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Narudzbina/Detalji?narudzbinaID=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i></a>
					</div>
                        `
                },
                "width": "15%"
            }
        ]
    });
}