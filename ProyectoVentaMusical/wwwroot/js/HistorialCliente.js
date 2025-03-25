$(document).ready(function () {
	$('#tbventa').DataTable({		
		"paging": true,         // Habilita el paginado
		"lengthMenu": [5, 10, 25, 50],  // Opciones de registros por página
		"pageLength": 5,        // Cantidad inicial de registros por página
		"ordering": true,       // Habilita el ordenamiento
		"info": true,           // Muestra información de la tabla
		"searching": true,      // Habilita la búsqueda
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
});


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