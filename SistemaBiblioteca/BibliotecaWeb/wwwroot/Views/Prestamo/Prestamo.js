let tablaData;
$(document).ready(function () {

    $('#cboEstudiante').select2({
        theme: 'bootstrap-5',
        dropdownParent: $('#mdPrestamo'),
        placeholder: 'Buscar estudiante',
        minimumInputLength: 1,
        allowClear: true,
        ajax: {
            url: '/Prestamo/BusquedaEstudiante',
            delay: 700,
            data: function (params) {
                var query = {
                    buscar: params.term
                }
                return query;
            },
            processResults: function (data) {
                let items = [];
                data.forEach((estudiante) => {
                    items.push({
                        id: estudiante.idEstudiante,
                        text: `${estudiante.codigo} - ${estudiante.nombres} ${estudiante.apellidos}`
                    })
                });

                return {
                    results: items
                };
            },
        }
    });

    $('#cboLibro').select2({
        theme: 'bootstrap-5',
        dropdownParent: $('#mdPrestamo'),
        placeholder: 'Buscar libro',
        minimumInputLength: 1,
        allowClear:true,
        ajax: {
            url: '/Prestamo/BusquedaLibro',
            delay: 700,
            data: function (params) {
                var query = {
                    buscar: params.term
                }
                return query;
            },
            processResults: function (data) {
                let items = [];
                data.forEach((libro) => {
                    items.push({
                        id: libro.idLibro,
                        text: `${libro.codigo} - ${libro.titulo}`
                    })
                });

                return {
                    results: items
                };
            },
        }
    });


    tablaData = $('#tbPrestamo').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": "/Prestamo/Lista",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Fecha Prestamo", "data": "fechaPrestamo" },
            {
                title: "Codigo Estudiante", "data": "oEstudiante", render: function (data, type, row) {
                    return data.codigo
                }
            },
            {
                title: "Estudiante", "data": "oEstudiante", render: function (data, type, row) {
                    return data.nombres + " " + data.apellidos
                }
            },
            {
                title: "Codigo Libro", "data": "oLibro", render: function (data, type, row) {
                    return data.codigo
                }
            },
            {
                title: "Titulo", "data": "oLibro", render: function (data, type, row) {
                    return data.titulo
                }
            }, 
            { title: "Fecha Devolucion", "data": "fechaDevolucion" },
            {
                title: "Estado Prestamo", "data": "estadoPrestamo", render: function (data, type, row) {
                    let etiqueta = "";
                    switch (data) {
                        case 'Pendiente':
                            etiqueta = `<span class="badge bg-warning">${data}</span>`
                            break;
                        case 'Anulado':
                            etiqueta = `<span class="badge bg-danger">${data}</span>`
                            break;
                        default:
                            etiqueta = `<span class="badge bg-success">${data}</span>`
                    }
                    return etiqueta;
                }
            },
            {
                title: "", "data": "idPrestamo", width: "100px", render: function (data, type, row) {
                    let disabled = row.estadoPrestamo == "Pendiente" ? "" : "disabled";
                    return `<button type="button" class="btn btn-sm btn-outline-primary me-1" ${disabled} onclick="devolverPrestamo(${data});"><i class="fas fa-rotate-left"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-danger me-1" ${disabled} onclick="anularPrestamo(${data});"><i class="fas fa-file-circle-minus"></i></button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});


$("#btnNuevoPrestamo").on("click", function () {
    $('#cboEstudiante').val(null).trigger('change');
    $('#cboLibro').val(null).trigger('change');
    $('#mdPrestamo').modal('show');
})


$("#btnGuardar").on("click", function () {

    if ($("#cboEstudiante").val() == null ||
        $("#cboLibro").val() == null
    ) {
        Swal.fire({
            title: "Error!",
            text: "Faltan completar datos.",
            icon: "warning"
        });
        return;
    }

    const objeto = {
        oEstudiante: {
            IdEstudiante: $("#cboEstudiante").val()
        },
        oLibro: {
            IdLibro: $("#cboLibro").val()
        }
    }

    fetch(`/Prestamo/Guardar`, {
        method: "POST",
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(objeto)
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data == "") {
            Swal.fire({
                text: "Prestamo registrado!",
                icon: "success"
            });
            $('#mdPrestamo').modal('hide');
            tablaData.ajax.reload();
        } else {
            Swal.fire({
                title: "Error!",
                text: responseJson.data,
                icon: "warning"
            });
        }
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se pudo registrar.",
            icon: "warning"
        });
    })
});

function devolverPrestamo(id) {
    Swal.fire({
        text: "Desea registrar la devolucion?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/Prestamo/Devolver?IdPrestamo=${id}`, {
                method: "GET",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == 1) {
                    Swal.fire({
                        text: "Devuelto!",
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: "No se pudo devolver.",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo devolver.",
                    icon: "warning"
                });
            })
        }
    });
}

function anularPrestamo(id) {

    Swal.fire({
        text: "Desea anular el prestamo?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/Prestamo/Anular?IdPrestamo=${id}`, {
                method: "GET",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == 1) {
                    Swal.fire({
                        text: "Anulado!",
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: "No se pudo anular.",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo anular.",
                    icon: "warning"
                });
            })
        }
    });
}