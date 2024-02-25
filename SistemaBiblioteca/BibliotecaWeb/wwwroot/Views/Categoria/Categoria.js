let tablaData;
let idCategoriaEditar = 0;
$(document).ready(function () {

    tablaData = $('#tbCategoria').DataTable({
        responsive: true,
        "ajax": {
            "url": "/Categoria/Lista",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title:"Nombre", "data": "nombre" },
            { title: "Fecha Creacion", "data": "fechaCreacion" },
            { title: "", "data": "idCategoria", width:"100px", render: function (data, type, row) {
                    return `<button type="button" class="btn btn-sm btn-outline-primary me-1" onclick="tbEditarCategoria(${data});"><i class="fas fa-pen-to-square"></i></button>` + 
                        `<button type="button" class="btn btn-sm btn-outline-danger me-1" onclick="tbEliminarCategoria(${data});"><i class="fas fa-trash"></i></button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

function tbEditarCategoria(idCategoria) {

    fetch(`/Categoria/Obtener?IdCategoria=${idCategoria}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.idCategoria != 0) {
            idCategoriaEditar = responseJson.data.idCategoria;
            $("#txtNombre").val(responseJson.data.nombre);
            $('#mdCategoria').modal('show');
        }
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias.",
            icon: "warning"
        });
    })

    
}

$("#btnNuevaCategoria").on("click", function () {
    idCategoriaEditar = 0;
    $("#txtNombre").val("")
    $('#mdCategoria').modal('show');
})


function tbEliminarCategoria(id) {

    Swal.fire({
        text: "Desea eliminar la categoria?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/Categoria/Eliminar?IdCategoria=${id}`, {
                method: "DELETE",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == 1) {
                    Swal.fire({
                        title: "Eliminado!",
                        text: "La categoria fue eliminado.",
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
    if ($("#txtNombre").val().trim() =="") {
        Swal.fire({
            title: "Error!",
            text: "Debe ingresar el nombre.",
            icon: "warning"
        });
        return
    }

    let objeto = {
        IdCategoria: idCategoriaEditar,
        Nombre: $("#txtNombre").val().trim()
    }

    if (idCategoriaEditar != 0) {

        fetch(`/Categoria/Editar`, {
            method: "PUT",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto)
        }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.data == "") {
                idCategoriaEditar = 0;
                Swal.fire({
                    text: "Se guardaron los cambios!",
                    icon: "success"
                });
                $('#mdCategoria').modal('hide');
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
        fetch(`/Categoria/Guardar`, {
            method: "POST",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto)
        }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.data == "") {
                Swal.fire({
                    text: "Categoria registrada!",
                    icon: "success"
                });
                $('#mdCategoria').modal('hide');
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