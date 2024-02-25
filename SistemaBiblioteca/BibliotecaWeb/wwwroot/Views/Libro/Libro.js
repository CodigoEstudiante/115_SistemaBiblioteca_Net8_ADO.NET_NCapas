let tablaData;
let idEstudianteEditar = 0;
$(document).ready(function () {
    
    obtenerCategorias();
    tablaData = $('#tbLibro').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": "/Libro/Lista",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Codigo", "data": "codigo" },
            { title: "Titulo", "data": "titulo" },
            { title: "Autor", "data": "autor" },
            {
                title: "Categoria", "data": "oCategoria", render: function (data, type, row) {
                    return data.nombre
                }
            },
            { title: "Fecha Publicacion", "data": "fechaPublicacion" },
            { title: "Cantidad", "data": "cantidad" },
            { title: "Fecha Creacion", "data": "fechaCreacion" },
            {
                title: "", "data": "idLibro", render: function (data, type, row) {
                    return `<button type="button" class="btn btn-sm btn-outline-primary me-1" onclick="tbEditarLibro(${data});"><i class="fas fa-pen-to-square"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-danger me-1" onclick="tbEliminarLibro(${data});"><i class="fas fa-trash"></i></button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

function obtenerCategorias() {
    fetch(`/Categoria/Lista`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.length > 0) {
            $("#cboCategoria").append($("<option>").val("").text(""));
            responseJson.data.forEach((item) => {
                $("#cboCategoria").append($("<option>").val(item.idCategoria).text(item.nombre));
            });
            $('#cboCategoria').select2({
                theme: 'bootstrap-5',
                dropdownParent: $('#mdLibro'),
                placeholder: "Seleccionar"
            });
        }
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias.",
            icon: "warning"
        });
    })
}

function tbEditarLibro(id) {

    fetch(`/Libro/Obtener?IdLibro=${id}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.idLibro != 0) {
            const libro = responseJson.data;
            idEstudianteEditar = libro.idLibro;
            $("#txtCodigo").val(libro.codigo);
            $("#txtTitulo").val(libro.titulo);
            $("#txtAutor").val(libro.autor);
            $("#cboCategoria").select2("val", libro.oCategoria.idCategoria.toString());
            $("#txtFechaPublicacion").val(moment(libro.fechaPublicacion, "DD/MM/YYYY").format('YYYY-MM-DD'));
            $("#txtCantidad").val(libro.cantidad);
            $('#mdLibro').modal('show');
        } else {
            Swal.fire({
                title: "Error!",
                text: "No se encontraron coincidencias.",
                icon: "warning"
            });
        }
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias.",
            icon: "warning"
        });
    })

    
}

$("#btnNuevoLibro").on("click", function () {
    idEstudianteEditar = 0;
    $("#txtCodigo").val("");
    $("#txtTitulo").val("");
    $("#txtAutor").val("");
    $("#cboCategoria").select2("val", "");
    $("#txtFechaPublicacion").val(moment().format("YYYY-MM-DD"));
    $("#txtCantidad").val(1);

    $('#mdLibro').modal('show');
})

function tbEliminarLibro(id) {

    Swal.fire({
        text: "Desea eliminar el libro?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/Libro/Eliminar?IdLibro=${id}`, {
                method: "DELETE",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == 1) {
                    Swal.fire({
                        title: "Eliminado!",
                        text: "El libro fue eliminado.",
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: "No se pudo eliminar.",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo eliminar.",
                    icon: "warning"
                });
            })
        }
    });
}

$("#btnGuardar").on("click", function () {


    if ($("#cboCategoria").val() == "" ||
        $("#txtTitulo").val().trim() == "" ||
        $("#txtAutor").val().trim() == "" 
    ) {
        Swal.fire({
            title: "Error!",
            text: "Faltan completar datos.",
            icon: "warning"
        });
        return;
    }

    const objeto = {
        IdLibro: idEstudianteEditar,
        oCategoria: {
            IdCategoria: $("#cboCategoria").val()
        },
        Titulo: $("#txtTitulo").val(),
        Autor: $("#txtAutor").val(),
        FechaPublicacion: moment($("#txtFechaPublicacion").val()).format("DD/MM/YYYY"),
        Cantidad: $("#txtCantidad").val(),
    }

    if (idEstudianteEditar != 0) {

        fetch(`/Libro/Editar`, {
            method: "PUT",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto)
        }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.data == "") {
                Swal.fire({
                    text: "Se guardaron los cambios!",
                    icon: "success"
                });
                $('#mdLibro').modal('hide');
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
                text: "No se pudo editar.",
                icon: "warning"
            });
        })
    } else {
        fetch(`/Libro/Guardar`, {
            method: "POST",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto)
        }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.data == "") {
                Swal.fire({
                    text: "Libro registrado!",
                    icon: "success"
                });
                $('#mdLibro').modal('hide');
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
    }
});