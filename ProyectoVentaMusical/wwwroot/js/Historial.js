$(document).ready(function () {
    // Verificar si la tabla ya está inicializada y destruirla si es necesario
    if ($.fn.dataTable.isDataTable('#tbventa')) {
        $('#tbventa').DataTable().destroy();
    }

    /************ Sección para la inicialización del DataTable ************/

    // Inicializa la tabla
    let table = $('#tbventa').DataTable({
        "paging": true,
        "lengthMenu": [5, 10, 25, 50],
        "pageLength": 5,
        "ordering": true,
        "info": true,
        "searching": true,
        "responsive": true,
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "zeroRecords": "No se encontraron resultados",
            "info": "Mostrando página _PAGE_ de _PAGES_",
            "infoEmpty": "No hay registros disponibles",
            "infoFiltered": "(filtrado de _MAX_ registros en total)",
            "search": "Buscar:",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });

    /************ Sección para los filtros ************/

    // Filtro por rango de fechas
    $('#fechaInicio, #fechaFin').on('keyup change', function () {
        let fechaInicio = $('#fechaInicio').val();
        let fechaFin = $('#fechaFin').val();

        // Limpiar filtros anteriores
        $.fn.dataTable.ext.search.length = 0;

        // Función personalizada para filtrar por rango de fechas
        $.fn.dataTable.ext.search.push(function (settings, data) {
            let fecha = data[0]; // Índice de la columna "FechaCompra"
            let fechaCompra = new Date(fecha);

            if (fechaInicio && new Date(fechaInicio) > fechaCompra) return false;
            if (fechaFin && new Date(fechaFin) < fechaCompra) return false;
            return true;
        });

        table.draw();
    });

    // Filtro por usuario
    $('#filtroUsuario').on('keyup change', function () {
        let usuario = $('#filtroUsuario').val().trim();
        table.column(2).search(usuario).draw();
    });

    /************ Sección para el botón toggle que cambia el tipo de filtro ************/

    function toggleFields() {
        var selectedOption = $('#cboBuscarPor').val();

        if (selectedOption === 'fecha') {
            $('.busqueda-usuario input').val('');
            $('.busqueda-fecha').show();
            $('.busqueda-usuario').hide();
        } else if (selectedOption === 'Usuario') {
            $.fn.dataTable.ext.search.length = 0;
            $('.busqueda-fecha input').val('');
            $('.busqueda-fecha').hide();
            $('.busqueda-usuario').show();
        }
        table.search('').columns().search('').draw();
        
    }

    if (userRole === "contador") {
        $('#cboBuscarPor').val('fecha');
        $('.busqueda-fecha').show();
        $('.busqueda-usuario').hide();
    } else if (userRole === "administrador") {
        $('#cboBuscarPor').val('Usuario');
        $('.busqueda-usuario').show();
        $('.busqueda-fecha').hide();
    }

    $('#cboBuscarPor').change(toggleFields);
});

// Sirve para mostrar los detalles de cada venta.
// Al presionar un botón que sale al final de cada fila
// se muestra un modal con los detalles de la venta
$(document).ready(function () {
	$(document).on("click", ".btn-info", function () {
		let idVenta = $(this).data("id");
		console.log("ID de venta seleccionado:", idVenta);

		$.get("/Admin/Historial/ObtenerDetalleVenta", { idVenta: idVenta }, function (data) {
			console.log("Datos recibidos:", data); // Muestra los datos en la consola

			let tbody = $("#detalleVentaBody");
			tbody.empty(); // Limpia la tabla antes de agregar nuevas filas

			if (!Array.isArray(data) || data.length === 0) {
				console.warn("El array de detalles está vacío o no es válido.");
				return;
			}

			data.forEach(function (detalle) {
				console.log("Procesando detalle:", detalle); // Muestra cada objeto en la consola

				let row = `<tr>
					<td>${detalle.idVenta}</td>
					<td>${detalle.cancion}</td>
					<td>${detalle.cantidad}</td>
					<td>${detalle.precioUnitario.toFixed(2)}</td>
					<td>${detalle.total.toFixed(2)}</td>
				</tr>`;

				tbody.append(row);
			});

			console.log("Tabla actualizada.");
		}).fail(function (xhr, status, error) {
			console.error("Error en la petición AJAX:", status, error);
		});
	});
});
