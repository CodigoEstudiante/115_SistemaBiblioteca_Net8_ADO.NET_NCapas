let tablaData;
let idEstudianteEditar = 0;
$(document).ready(function () {
    
    tablaData = $('#tbEstudiante').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": "/Estudiante/Lista",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Codigo", "data": "codigo" },
            { title: "Nombres", "data": "nombres" },
            { title: "Apellidos", "data": "apellidos" },
            {
                title: "", "data": "idEstudiante", render: function (data, type, row) {
                    return `<button type="button" class="btn btn-sm btn-outline-primary me-1" onclick="tbEditarEstudiante(${data});"><i class="fas fa-pen-to-square"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-danger me-1" onclick="tbEliminarEstudiante(${data});"><i class="fas fa-trash"></i></button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

function tbEditarEstudiante(id) {

    fetch(`/Estudiante/Obtener?IdEstudiante=${id}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.idLibro != 0) {
            const estudiante = responseJson.data;
            idEstudianteEditar = estudiante.idEstudiante;
            $("#txtNombres").val(estudiante.nombres);
            $("#txtApellidos").val(estudiante.apellidos);
            $('#mdEstudiante').modal('show');
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

$("#btnNuevoEstudiante").on("click", function () {
    idEstudianteEditar = 0;
    $("#txtNombres").val("");
    $("#txtApellidos").val("");

    $('#mdEstudiante').modal('show');
})

function tbEliminarEstudiante(id) {

    Swal.fire({
        text: "Desea eliminar el estudiante?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/Estudiante/Eliminar?IdEstudiante=${id}`, {
                method: "DELETE",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == 1) {
                    Swal.fire({
                        title: "Eliminado!",
                        text: "El estudiante fue eliminado.",
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


    if ($("#txtNombres").val() == "" ||
        $("#txtApellidos").val().trim() == ""
    ) {
        Swal.fire({
            title: "Error!",
            text: "Faltan completar datos.",
            icon: "warning"
        });
        return;
    }

    const objeto = {
        IdEstudiante: idEstudianteEditar,
        Nombres: $("#txtNombres").val(),
        Apellidos: $("#txtApellidos").val()
    }

    if (idEstudianteEditar != 0) {

        fetch(`/Estudiante/Editar`, {
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
                $('#mdEstudiante').modal('hide');
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
        fetch(`/Estudiante/Guardar`, {
            method: "POST",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto)
        }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.data == "") {
                Swal.fire({
                    text: "Estudiante registrado!",
                    icon: "success"
                });
                $('#mdEstudiante').modal('hide');
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