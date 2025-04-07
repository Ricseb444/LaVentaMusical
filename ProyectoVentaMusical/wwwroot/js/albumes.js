var dataTable;

$(document).ready(function () {
    cargarDatatable();

    // Filtrado en tiempo real
    $('#filtroCodigoArtista').on('keyup', function () {
        dataTable.column(1).search(this.value).draw();
    });
});

function cargarDatatable() {
    dataTable = $("#tblAlbumes").DataTable({
        "responsive": true,
        "ajax": {
            "url": "/Admin/albumes/GetAll",
            "type": "GET",
            "datatype": "json",
            "dataSrc": function (json) {
                // Asegúrate de que la respuesta contiene la propiedad 'data' y es un array
                if (!json.data) {
                    return [];
                }
                return json.data; // Asegúrate de que los datos estén en la propiedad 'data'
            }
        },
        "columns": [
            { "data": "id", "width": "10%" },  // Asegúrate de que el 'id' está presente en la respuesta JSON
            { "data": "codigoArtista", "width": "20%" },
            { "data": "nombreAlbum", "width": "25%" },
            { "data": "añoLanzamiento", "width": "20%" },
            {
                "data": "imagenAlbum",
                "render": function (imagen) {
                    return `<img style="height:100px; width:100px;" src="../${imagen}" class="rounded mx-auto d-block">`
                }, "orderable": false
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<button onclick="window.location.href='/Admin/Albumes/Edit/${data}'" title="Editar" class="btn btn-success btn-sm" style="margin-right: 8px;"><i class="fas fa-pencil-alt"></i></button>` +
                        `<button onclick="Delete('/Admin/Albumes/Delete/${data}')" title="Eliminar" class="btn btn-danger btn-sm"><i class="fas fa-trash-alt"></i></button>`;
                }, "width": "25%", "orderable": false
            }
        ],
        "language": {
            "decimal": "",
            "emptyTable": "No hay registros",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ entradas",
            "infoEmpty": "Mostrando 0 a 0 de 0 entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "No se encontraron resultados",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "¿Está seguro de borrar?",
        text: "Este contenido no se puede recuperar.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function (xhr) {
                    toastr.error("Error en la solicitud: " + xhr.responseText);
                }
            });
        }
    });
}
