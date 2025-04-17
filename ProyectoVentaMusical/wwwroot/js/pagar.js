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
                        confirmButtonText: "Continuar"
                    }).then(() => {
                        // Redirige a: /Admin/Comprar/ConfirmacionCompra/{idVenta}
                        window.location.href = "/Admin/Comprar/ConfirmacionCompra/" + response.idVenta;
                    });
                } else {
                    Swal.fire("Error", response.message, "error");
                }
            },
            error: function (xhr) {
                Swal.fire("Error", "Hubo un problema al procesar el pago.", "error");
                console.error("Error del servidor:", xhr.responseText);
            }
        });
    });
});

