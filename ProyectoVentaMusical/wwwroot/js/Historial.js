// Sirve para cargar las funciones del datatable
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

// Se utiliza para filtrar las ventas por fecha
$(document).ready(function () {
	let table = $('#tbventa').DataTable({
		"processing": true,
		"serverSide": false, // No se usa servidor para cargar datos
		"paging": true, // Habilitar paginación
		"searching": true, // Habilitar búsqueda
		"ordering": true // Habilitar ordenamiento
	});

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
});

// Se utiliza para filtrar las ventas por usuario
$(document).ready(function () {
	let table = $('#tbventa').DataTable({
		"processing": true,
		"serverSide": false,
		"processing": true,
		"serverSide": false, 
		"paging": true, 
		"searching": true,
		"ordering": true 
		
	});

	// Filtros personalizados para usuario y rango de fechas
	$('#filtroUsuario').on('keyup change', function () {
		let usuario = $('#filtroUsuario').val().trim();
		table.column(2).search(usuario).draw(); 
	});
});

// Se utiliza para mostrar u ocultar los filtros de busqueda requeridos (Fecha/Usuario)
$(document).ready(function () {
	// Función para mostrar/ocultar campos según la selección
	function toggleFields() {
		var selectedOption = $('#cboBuscarPor').val();

		if (selectedOption === 'fecha') {
			$('.busqueda-fecha').show();
			$('.busqueda-usuario').hide();
		} else if (selectedOption === 'Usuario') {
			$('.busqueda-fecha').hide();
			$('.busqueda-usuario').show();
		}
	}

	// Ejecutar la función al cargar la página
	toggleFields();

	// Ejecutar la función cada vez que cambie la selección
	$('#cboBuscarPor').change(toggleFields);
});
