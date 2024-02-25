let tablaData;
let idUsuarioEditar = 0;
$(document).ready(function () {

    tablaData = $('#tbUsuario').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": "/Usuario/Lista",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Nombre Completo", "data": "nombreCompleto" },
            { title: "Nombre Usuario", "data": "nombreUsuario" },
            { title: "FechaCreacion", "data": "fechaCreacion" },
            {
                title: "", "data": "idUsuario", render: function (data, type, row) {
                    return `<button type="button" class="btn btn-sm btn-outline-primary me-1" onclick="tbEditarUsuario(${data});"><i class="fas fa-pen-to-square"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-danger me-1" onclick="tbEliminarUsuario(${data});"><i class="fas fa-trash"></i></button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

function tbEditarUsuario(id) {

    fetch(`/Usuario/Obtener?IdUsuario=${id}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.idUsuario != 0) {
            const usuario = responseJson.data;
            idUsuarioEditar = usuario.idUsuario;
            $("#txtNombreCompleto").val(usuario.nombreCompleto);
            $("#txtNombreUsuario").val(usuario.nombreUsuario);
            $("#txtContrasenia").val(usuario.clave);
            $("#txtRepetirContrasenia").val(usuario.clave);
            $('#mdUsuario').modal('show');
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

$("#btnNuevoUsuario").on("click", function () {
    idUsuarioEditar = 0;
    $("#txtNombreCompleto").val("");
    $("#txtNombreUsuario").val("");
    $("#txtContrasenia").val("");
    $("#txtRepetirContrasenia").val("");

    $('#mdUsuario').modal('show');
})
function tbEliminarUsuario(id) {

    Swal.fire({
        text: "Desea eliminar el usuario?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/Usuario/Eliminar?IdUsuario=${id}`, {
                method: "DELETE",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == 1) {
                    Swal.fire({
                        title: "Eliminado!",
                        text: "El usuario fue eliminado.",
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


    if ($("#txtNombreCompleto").val() == "" ||
        $("#txtNombreUsuario").val().trim() == "" ||
        $("#txtContrasenia").val().trim() == ""
    ) {
        Swal.fire({
            title: "Error!",
            text: "Faltan completar datos.",
            icon: "warning"
        });
        return;
    }

    if ($("#txtContrasenia").val().trim() != $("#txtRepetirContrasenia").val().trim())
    {
        Swal.fire({
            title: "Error!",
            text: "Las contraseñas no coinciden.",
            icon: "warning"
        });
        return;
    }

    const objeto = {
        IdUsuario: idUsuarioEditar,
        NombreCompleto: $("#txtNombreCompleto").val().trim(),
        NombreUsuario: $("#txtNombreUsuario").val().trim(),
        Clave: $("#txtContrasenia").val().trim()
    }

    if (idUsuarioEditar != 0) {

        fetch(`/Usuario/Editar`, {
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
                $('#mdUsuario').modal('hide');
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
        fetch(`/Usuario/Guardar`, {
            method: "POST",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto)
        }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.data == "") {
                Swal.fire({
                    text: "Usuario registrado!",
                    icon: "success"
                });
                $('#mdUsuario').modal('hide');
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