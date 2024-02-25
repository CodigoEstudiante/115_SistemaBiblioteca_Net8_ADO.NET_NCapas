
use DBBiblioteca

go
--- PROCEDIMIENTOS PARA ESTUDIANTES ---
create procedure sp_listaCategorias
as
begin
 select IdCategoria,Nombre,convert(char(10),FechaCreacion,103)[FechaCreacion] from Categoria
end
go

create procedure sp_obtenerCategoria(
@IdCategoria int)
as
begin
 select IdCategoria,Nombre,convert(char(10),FechaCreacion,103)[FechaCreacion] from Categoria
 where IdCategoria = @IdCategoria
end

go

create procedure sp_guardarCategoria
(
@Nombre varchar(100),
@msgError varchar(100) OUTPUT
)
as
begin
	set @msgError = ''

	if(not exists(select idcategoria from Categoria where Nombre = @Nombre))
		insert into Categoria(Nombre) values(@Nombre)
	else
		set @msgError = 'La categoria ya existe'
end

go

create procedure sp_editarCategoria
(
@IdCategoria int,
@Nombre varchar(100),
@msgError varchar(100) OUTPUT
)
as
begin
	set @msgError = ''
	if(not exists(select idcategoria from Categoria where Nombre = @Nombre and IdCategoria != @IdCategoria))
		update Categoria set
		Nombre = @Nombre
		where IdCategoria = @IdCategoria
	else
		set @msgError = 'La categoria ya existe'
end

go

create procedure sp_eliminarCategoria
(
@IdCategoria int
)
as
begin
 delete top (1) from Categoria
 where IdCategoria = @IdCategoria
end

go

--- PROCEDIMIENTOS PARA LIBROS ---
create procedure sp_listaLibros
as
begin
	select l.IdLibro,c.Nombre[NombreCategoria],l.Codigo,l.Titulo,l.Autor,
	convert(char(10),l.FechaPublicacion,103)FechaPublicacion,l.Cantidad,
	convert(char(10),l.FechaCreacion,103)FechaCreacion 
	from Libro l
	inner join Categoria c on c.IdCategoria = l.IdCategoria
end

go

create procedure sp_obtenerLibro(
@IdLibro int
)
as
begin
	select l.IdLibro,c.IdCategoria,l.Codigo,l.Titulo,l.Autor,
	convert(char(10),l.FechaPublicacion,103)FechaPublicacion,l.Cantidad,
	convert(char(10),l.FechaCreacion,103)FechaCreacion 
	from Libro l
	inner join Categoria c on c.IdCategoria = l.IdCategoria
	where l.IdLibro = @IdLibro
end

go

create procedure sp_guardarLibro
(
@IdCategoria int,
@Titulo varchar(100),
@Autor varchar(100),
@FechaPublicacion varchar(10),
@Cantidad int,
@msgError varchar(100) OUTPUT
)
as
begin
	set dateformat dmy
	declare @numeroCorrelativo int
	declare @codigo varchar(6)
	set @msgError = ''

	BEGIN TRY
		
		BEGIN TRAN
		if(not exists(select IdLibro from Libro where Titulo = @Titulo))
		begin
		
			update NumeroCorrelativo set
			@numeroCorrelativo = UltimoNumero = UltimoNumero + 1
			where Prefijo = 'LB'

			set @codigo = (select Prefijo + RIGHT(CONCAT(REPLICATE('0', Longitud),CONVERT(VARCHAR(10),@numeroCorrelativo)),Longitud - LEN(Prefijo)) from NumeroCorrelativo where Prefijo = 'LB')

			insert into Libro(IdCategoria,Codigo,Titulo,Autor,FechaPublicacion,Cantidad) values
			(@IdCategoria,@codigo,@Titulo,@Autor,convert(date,@FechaPublicacion),@Cantidad)
		end
		else
			set @msgError = 'El libro ya existe'

		COMMIT TRAN
	END TRY  
	BEGIN CATCH
		ROLLBACK TRAN
		set @msgError = 'Error al guardar el libro'
	END CATCH  

end

go

create procedure sp_editarLibro
(
@IdLibro int,
@IdCategoria int,
@Titulo varchar(100),
@Autor varchar(100),
@FechaPublicacion varchar(10),
@Cantidad int,
@msgError varchar(100) OUTPUT
)
as
begin
	set dateformat dmy
	set @msgError = ''

	BEGIN TRY
		
		BEGIN TRAN
		if(not exists(select IdLibro from Libro where Titulo = @Titulo and IdLibro != @IdLibro))
		begin
		
			update Libro set
			IdCategoria = @IdCategoria,
			Titulo = @Titulo,
			Autor = @Autor,
			FechaPublicacion = @FechaPublicacion,
			Cantidad = @Cantidad
			where IdLibro = @IdLibro
		end
		else
			set @msgError = 'El libro ya existe'

		COMMIT TRAN
	END TRY  
	BEGIN CATCH
		ROLLBACK TRAN
		set @msgError = 'Error al editar el libro'
	END CATCH  

end

go

create procedure sp_eliminarLibro
(
@IdLibro int
)
as
begin
 delete top (1) from Libro
 where IdLibro = @IdLibro
end

go

--- PROCEDIMIENTOS PARA ESTUDIANTES ---

create procedure sp_listaEstudiantes
as
begin
	select IdEstudiante,Codigo,Nombres,Apellidos,convert(char(10),FechaCreacion,103)[FechaCreacion] from Estudiante
end

go

create procedure sp_obtenerEstudiante(
@IdEstudiante int
)
as
begin
	select IdEstudiante,Codigo,Nombres,Apellidos,convert(char(10),FechaCreacion,103)[FechaCreacion] from Estudiante
	where IdEstudiante = @IdEstudiante
end

go

create procedure sp_guardarEstudiante
(
@Nombres varchar(100),
@Apellidos varchar(100),
@msgError varchar(100) OUTPUT
)
as
begin
	declare @numeroCorrelativo int
	declare @codigo varchar(6)
	set @msgError = ''

	BEGIN TRY
		
		BEGIN TRAN
		if(not exists(select IdEstudiante from Estudiante where Nombres = @Nombres and Apellidos = @Apellidos))
		begin
		
			update NumeroCorrelativo set
			@numeroCorrelativo = UltimoNumero = UltimoNumero + 1
			where Prefijo = 'ST'

			set @codigo = (select Prefijo + RIGHT(CONCAT(REPLICATE('0', Longitud),CONVERT(VARCHAR(10),@numeroCorrelativo)),Longitud - LEN(Prefijo)) from NumeroCorrelativo where Prefijo = 'ST')

			insert into Estudiante(Codigo,Nombres,Apellidos) values
			(@codigo,@Nombres,@Apellidos)
		end
		else
			set @msgError = 'El estudiante ya existe'

		COMMIT TRAN
	END TRY  
	BEGIN CATCH
		ROLLBACK TRAN
		set @msgError = 'Error al guardar el estudiante'
	END CATCH  

end

go

create procedure sp_editarEstudiante
(
@IdEstudiante int,
@Nombres varchar(100),
@Apellidos varchar(100),
@msgError varchar(100) OUTPUT
)
as
begin
	declare @numeroCorrelativo int
	declare @codigo varchar(6)
	set @msgError = ''

	BEGIN TRY
		
		BEGIN TRAN
		if(not exists(select IdEstudiante from Estudiante where Nombres = @Nombres and Apellidos = @Apellidos and IdEstudiante != @IdEstudiante))
		begin
		
			update Estudiante set
			Nombres = @Nombres,
			Apellidos = @Apellidos
			where IdEstudiante = @IdEstudiante

		end
		else
			set @msgError = 'El estudiante ya existe'

		COMMIT TRAN
	END TRY  
	BEGIN CATCH
		ROLLBACK TRAN
		set @msgError = 'Error al editar el estudiante'
	END CATCH  

end

go

create procedure sp_eliminarEstudiante
(
@IdEstudiante int
)
as
begin
 delete top (1) from Estudiante
 where IdEstudiante = @IdEstudiante
end

go

--- PROCEDIMIENTOS PARA PRESTAMO ---
create procedure sp_listaPrestamo
as
begin
	select p.IdPrestamo, convert(char(10),p.FechaPrestamo,103)[FechaPrestamo],
	e.Codigo[CodigoEstudiante],e.Nombres,e.Apellidos,
	l.Codigo[CodigoLibro],l.Titulo,
	convert(char(10),p.FechaDevolucion,103)[FechaDevolucion],
	p.EstadoPrestamo
	from Prestamo p
	inner join Estudiante e on e.IdEstudiante = p.IdEstudiante
	inner join Libro l on l.IdLibro = p.IdLibro
end

GO

create procedure sp_buscarEstudiante(
@Busqueda varchar(100)
)
as
begin
	select IdEstudiante,Codigo,Nombres,Apellidos from Estudiante where CONCAT(Codigo,Nombres,Apellidos) like '%'+@Busqueda+'%'
end

GO

create procedure sp_buscarLibro(
@Busqueda varchar(100)
)
as
begin
	select IdLibro,Codigo,Titulo from Libro where CONCAT(Codigo,Titulo) like '%'+@Busqueda+'%'
end

go

create procedure sp_guardarPrestamo(
@IdEstudiante int,
@IdLibro int,
@msgError varchar(100) OUTPUT
)
as
begin
	set @msgError = ''

	if((select Cantidad - 1 from Libro where IdLibro = @IdLibro) < 1)
	begin
		set @msgError = 'El libro no esta disponible'
	end

	if((select count(*) from Prestamo where IdEstudiante = @IdEstudiante and EstadoPrestamo = 'Pendiente') >= 3)
	begin
		set @msgError = 'El estudiante tiene 3 prestamos pendientes'
	end

	if(@msgError='')
	begin
		update Libro set
		Cantidad = Cantidad - 1
		where IdLibro = @IdLibro

		insert into Prestamo(IdEstudiante,IdLibro,EstadoPrestamo) values(@IdEstudiante,@IdLibro,'Pendiente')
	end
		
end

go

create procedure sp_devolverPrestamo(
@IdPrestamo int
)
as
begin
	declare @IdLibro int = (select IdLibro from Prestamo where IdPrestamo = @IdPrestamo)

	update Libro set
	Cantidad = Cantidad + 1
	where IdLibro = @IdLibro
	
	update Prestamo set 
	EstadoPrestamo = 'Devuelto',
	FechaDevolucion = GETDATE()
	where IdPrestamo = @IdPrestamo
end
go

create procedure sp_anularPrestamo(
@IdPrestamo int
)
as
begin
	declare @IdLibro int = (select IdLibro from Prestamo where IdPrestamo = @IdPrestamo)

	update Libro set
	Cantidad = Cantidad + 1
	where IdLibro = @IdLibro
	
	update Prestamo set 
	EstadoPrestamo = 'Anulado'
	where IdPrestamo = @IdPrestamo
end

go

--- PROCEDIMIENTOS PARA USUARIO ---
create procedure sp_listaUsuario
as
begin
	select u.IdUsuario,u.NombreCompleto,u.NombreUsuario,u.Clave,
	convert(char(10),u.FechaCreacion,103)[FechaCreacion]
	from Usuario u
end

go

create procedure sp_loginUsuario(
@NombreUsuario varchar(100),
@Clave varchar(100)
)
as
begin
	select u.IdUsuario,u.NombreCompleto,u.NombreUsuario,u.Clave,
	convert(char(10),u.FechaCreacion,103)[FechaCreacion]
	from Usuario u where u.NombreUsuario = @NombreUsuario and u.Clave = @Clave
end

go

create procedure sp_obtenerUsuario(
@IdUsuario int
)
as
begin
	select u.IdUsuario,u.NombreCompleto,u.NombreUsuario,u.Clave,
	convert(char(10),u.FechaCreacion,103)[FechaCreacion]
	from Usuario u where u.IdUsuario  =@IdUsuario
end

GO

create procedure sp_guardarUsuario
(
@NombreCompleto varchar(100),
@NombreUsuario varchar(100),
@Clave varchar(100),
@msgError varchar(100) OUTPUT
)
as
begin
	set @msgError = ''

	if(not exists(select IdUsuario from Usuario where NombreUsuario = @NombreUsuario))
		insert into Usuario(NombreCompleto,NombreUsuario,Clave) values(@NombreCompleto,@NombreUsuario,@Clave)
	else
		set @msgError = 'El usuario ya existe'
end

go

create procedure sp_editarUsuario
(
@IdUsuario int,
@NombreCompleto varchar(100),
@NombreUsuario varchar(100),
@Clave varchar(100),
@msgError varchar(100) OUTPUT
)
as
begin
	set @msgError = ''
	if(not exists(select IdUsuario from Usuario where NombreUsuario = @NombreUsuario and IdUsuario != @IdUsuario))
		update Usuario set
		NombreCompleto = @NombreCompleto,
		NombreUsuario = @NombreUsuario,
		Clave = @Clave
		where IdUsuario = @IdUsuario
	else
		set @msgError = 'El usuario ya existe'
end

go

create procedure sp_eliminarUsuario
(
@IdUsuario int
)
as
begin
 delete top (1) from Usuario
 where IdUsuario = @IdUsuario
end


go

--- PROCEDIMIENTOS PARA DASHBOARD ---

create procedure sp_obtenerDashboard
as
begin
	select
	(select count(*) from Libro)[TotalLibros],
	(select count(*) from Estudiante)[TotalEstudiante],
	(select count(*) from Prestamo where EstadoPrestamo != 'Anulado')[TotalPrestamos],
	(select count(*) from Prestamo where EstadoPrestamo = 'Devuelto')[TotalDevuelto]
end