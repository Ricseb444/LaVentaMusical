var dataTable;
$(document).ready(function () {
    cargarDatatable();
});

function verVistaPrevia(id) {
    $.ajax({
        url: `/Identity/Account/Manage?handler=Video&id=${id}`,
        type: 'GET',
        success: function (data) {
            $('#nombreCancion').text(data.nombreCancion);
            $('#nombreAlbum').text(data.nombreAlbum);
            $('#nombreArtista').text(data.nombreArtista);

            let url = data.urlVideo;
            if (url.includes("watch?v=")) {
                url = url.replace("watch?v=", "embed/");
            }
            $('#iframeVideo').attr('src', url);
            $('#modalCancion').modal('show');
        },
        error: function () {
            alert("No se pudo cargar la canción.");
        }
    });
}

 $(document).ready(function () {
 	$('#tblSongs').DataTable({
 		ajax: {
 			url: '/Admin/Canciones/ObtenerCanciones',
 			dataSrc: ''
 		},
 		columns: [
 			{ data: 'codigoCancion' },
 			{ data: 'codigoGeneroNavigation.nombreGenero' },
 			{ data: 'codigoAlbumNavigation.nombreAlbum' },
 			{ data: 'nombreCancion' },
 			{
 				data: 'codigoCancion',
 				render: function (data) {
 					return `
 						<button class="btn btn-sm btn-danger" onclick="verVistaPrevia(${data})">
 							<i class="fab fa-youtube"></i> Video
 						</button>`;
 				}
 			},
 			{ data: 'precio' },
 			{ data: 'cantidadDisponible' },
 			{
 				data: 'fotoCancion',
 				render: function (data) {
 					return `<img src="${data.replace(/\\/g, "/")}" width="80"/>`;
 				}
 			},
 			{
 				data: 'codigoCancion',
 				render: function (data) {
 					return `
 						<a href="/Admin/Canciones/Edit/${data}" class="btn btn-sm btn-success">
 							<i class="fas fa-pencil-alt"></i>
 						</a>
 						<button class="btn btn-sm btn-danger" onclick="eliminarCancion(${data})">
 							<i class="fas fa-trash-alt"></i>
 						</button>`;
 				}
 			}
          ],
          "language": {
                      "decimal": "",
                      "emptyTable": "No hay registros",
                      "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
                      "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
                      "infoFiltered": "(Filtrado de _MAX_ total entradas)",
                      "infoPostFix": "",
                      "thousands": ",",
                      "lengthMenu": "Mostrar _MENU_ Entradas",
                      "loadingRecords": "Cargando...",
                      "processing": "Procesando...",
                      "search": "Buscar:",
                      "zeroRecords": "Sin resultados encontrados",
                      "paginate": {
                          "first": "Primero",
                          "last": "Ultimo",
                          "next": "Siguiente",
                          "previous": "Anterior"
                      }
          },
          "width": "100%"
 	});
 });

//function cargarDatatable() {
//    dataTable = $("#tblSongs").DataTable({  
//        "responsive" : true,
//        "ajax": {
//            "url": "/Admin/canciones/GetAll",
//            "type": "GET",
//            "datatype": "json"
//        },
//        "columns": [
//            { "data": "codigoCancion", "width": "10%" },
//            { "data": "codigoGenero", "width": "10%" },
//            { "data": "codigoAlbum", "width": "10%" },
//            { "data": "nombreCancion", "width": "20%" },
//            {
//                "data": "linkVideo",
//                "render": function (data) {
//                    if (!data) return "Sin enlace";

//                    return data
//                        ? `
//								<button class="btn btn-sm btn-danger" onclick="verVistaPrevia(${data})">
//									<i class="fab fa-youtube"></i> Video
//								</button>`
//                        : `<span class="text-muted">N/A</span>`;
//                },
//                "width": "15%", "orderable": false
//            },
//            { "data": "precio", "width": "10%" },
//            { "data": "cantidadDisponible", "width": "10%" },
//            {
//                "data": "fotoCancion",
//                "render": function (imagen) {
//                    if (imagen) {
//                        const rutaImagen = imagen.replace(/\\/g, "/");
//                        return `<img style="height:120px;" src="${rutaImagen}" class="rounded mx-auto d-block">`;
//                    }
//                    return "Sin imagen";
//                }, "orderable": false
//            },
//            {
//                "data": "codigoCancion",
//                "render": function (data) {
//                    return `<button onclick="window.location.href='/Admin/Canciones/Edit/${data}'" title="Editar" class="btn btn-success btn-sm" style="margin-right: 8px;"><i class="fas fa-pencil-alt"></i></button>` +
//                        `<button onclick="Delete('/Admin/Canciones/Delete/${data}')" title="Eliminar" class="btn btn-danger btn-sm ms-2"><i class="fas fa-trash-alt"></i></button>`;
//                }, "width": "15%", "orderable": false
//            }
//        ],
//        "language": {
//            "decimal": "",
//            "emptyTable": "No hay registros",
//            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
//            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
//            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
//            "infoPostFix": "",
//            "thousands": ",",
//            "lengthMenu": "Mostrar _MENU_ Entradas",
//            "loadingRecords": "Cargando...",
//            "processing": "Procesando...",
//            "search": "Buscar:",
//            "zeroRecords": "Sin resultados encontrados",
//            "paginate": {
//                "first": "Primero",
//                "last": "Ultimo",
//                "next": "Siguiente",
//                "previous": "Anterior"
//            }
//        },
//        "width": "100%"
//    });
//}

function Delete(url) {
    swal({
        title: "Esta seguro de borrar?",
        text: "Este contenido no se puede recuperar!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, borrar!",
        closeOnconfirm: false
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                swal.close();
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            },
            error: function (xhr) {
                swal.close();
                toastr.error("Error en la solicitud: " + xhr.responseText);
            }
        });
    });
}



