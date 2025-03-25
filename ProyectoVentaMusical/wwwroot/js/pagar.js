$(document).ready(function () {
    $("#paymentForm").submit(function (event) {
        event.preventDefault();

        $.ajax({
            url: $(this).attr("action"),
            type: "POST",
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: "Pago Exitoso",
                        text: response.message,
                        icon: "success",
                        confirmButtonText: "Ver Detalles"
                    }).then(() => {
                        window.location.href = "/Admin/Comprar/ConfirmacionCompra/" + response.idVenta;
                    });
                } else {
                    Swal.fire("Error", response.message, "error");
                }
            },
            error: function () {
                Swal.fire("Error", "Hubo un problema al procesar el pago.", "error");
            }
        });
    });
});



//$(document).ready(function () {
//    $('#paymentType').change(function () {
//        var selectedPaymentMethod = $(this).val();
//        $('#creditCardInfo, #sinpeInfo, #availableMoney').hide();

//        if (selectedPaymentMethod === 'Tarjeta de Crédito') {
//            $('#creditCardInfo').show();
//        } else if (selectedPaymentMethod === 'Sinpe') {
//            $('#sinpeInfo').show();
//        } else if (selectedPaymentMethod === 'Dinero Disponible') {
//            $('#availableMoney').show();
//        }
//    });

    //$(document).ready(function () {
    //    $('[name="VENTAS.TipoPago"]').on('change', function () {
    //        var selectedPaymentMethod = $(this).val();
    //        console.log("Método seleccionado: ", selectedPaymentMethod);

    //        $('#creditCardInfo, #sinpeInfo, #availableMoney').hide();

    //        if (selectedPaymentMethod === 'Tarjeta de Crédito') {
    //            $('#creditCardInfo').show();
    //        } else if (selectedPaymentMethod === 'Sinpe') {
    //            $('#sinpeInfo').show();
    //        } else if (selectedPaymentMethod === 'Dinero Disponible') {
    //            $('#availableMoney').show();
    //        }
    //    });
    //});

//$(document).ready(function () {
//    $('[name="VENTAS.TipoPago"]').on('change', function () {
//        var selectedPaymentMethod = $(this).val();

//        $('#creditCardInfo, #sinpeInfo, #availableMoney')
//            .hide()
//            .find('input').prop('disabled', true);

//        if (selectedPaymentMethod === 'Tarjeta de Crédito') {
//            $('#creditCardInfo').show()
//                .find('input').prop('disabled', false);
//        } else if (selectedPaymentMethod === 'Sinpe') {
//            $('#sinpeInfo').show();
//        } else if (selectedPaymentMethod === 'Dinero Disponible') {
//            $('#availableMoney').show();
//        }
//    });
//});



//    document.addEventListener("DOMContentLoaded", function () {
//        const form = document.getElementById("paymentForm");

//        form.addEventListener("submit", function (event) {
//            event.preventDefault(); // Evita el envío normal del formulario

//            // Crear objeto con los datos del formulario
//            let formData = new FormData(form);

//            // Enviar petición AJAX al servidor
//            fetch(form.action, {
//                method: "POST",
//                body: formData
//            })
//                .then(response => response.json())
//                .then(data => {
//                    if (data.success) {
//                        Swal.fire({
//                            title: "¡Éxito!",
//                            text: data.message,
//                            icon: "success",
//                            confirmButtonText: "OK"
//                        }).then(() => {
//                            window.location.href = "/Admin/Comprar/Index"; // Redireccionar después del pago
//                        });
//                    } else {
//                        Swal.fire({
//                            title: "Error",
//                            text: data.message,
//                            icon: "error",
//                            confirmButtonText: "OK"
//                        });
//                    }
//                })
//                .catch(error => {
//                    Swal.fire({
//                        title: "Error inesperado",
//                        text: "Hubo un problema con la transacción.",
//                        icon: "error",
//                        confirmButtonText: "OK"
//                    });
//                    console.error("Error en la petición:", error);
//                });
//        });
//    });



//$(document).ready(function () {
//    // Manejar el cambio de tipo de pago
//    $('#paymentType').change(function () {
//        var selectedPaymentMethod = $(this).val();
//        $('#creditCardInfo, #sinpeInfo, #availableMoney').hide();

//        if (selectedPaymentMethod === 'Tarjeta de Crédito') {
//            $('#creditCardInfo').show();
//        } else if (selectedPaymentMethod === 'Sinpe') {
//            $('#sinpeInfo').show();
//        } else if (selectedPaymentMethod === 'Dinero Disponible') {
//            $('#availableMoney').show();
//        }
//    });

//    // Manejar el envío del formulario con AJAX
//    $("#paymentForm").submit(function (event) {
//        event.preventDefault(); // Evita el envío normal del formulario

//        let formData = new FormData(this);

//        fetch(this.action, {
//            method: "POST",
//            body: formData
//        })
//            .then(response => response.json())
//            .then(data => {
//                if (data.success) {
//                    Swal.fire({
//                        title: "¡Éxito!",
//                        text: data.message,
//                        icon: "success",
//                        confirmButtonText: "OK"
//                    }).then(() => {
//                        window.location.href = "/Admin/Comprar/Index"; // Redireccionar después del pago
//                    });
//                } else {
//                    Swal.fire({
//                        title: "Error",
//                        text: data.message,
//                        icon: "error",
//                        confirmButtonText: "OK"
//                    });
//                }
//            })
//            .catch(error => {
//                Swal.fire({
//                    title: "Error inesperado",
//                    text: "Hubo un problema con la transacción.",
//                    icon: "error",
//                    confirmButtonText: "OK"
//                });
//                console.error("Error en la petición:", error);
//            });
//    });
//});
