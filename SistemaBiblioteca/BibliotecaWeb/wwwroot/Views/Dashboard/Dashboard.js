$(document).ready(function () {

    fetch(`/Home/Dashboard`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        const data = responseJson.data;
        $("#hLibros").text(data.totalLibro);
        $("#hEstudiantes").text(data.totalEstudiante);
        $("#hPrestamos").text(data.totalPrestamos);
        $("#hDevoluciones").text(data.totalDevuelto);
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias.",
            icon: "warning"
        });
    })
})