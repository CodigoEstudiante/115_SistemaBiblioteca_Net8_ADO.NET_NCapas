create database DBBiblioteca

go

use DBBiblioteca

go

create table Usuario(
IdUsuario int primary key identity,
NombreCompleto varchar(100),
NombreUsuario varchar(100),
Clave varchar(100),
FechaCreacion datetime default getdate()
)

go

create table Estudiante(
IdEstudiante int primary key identity,
Codigo varchar(100),
Nombres varchar(100),
Apellidos varchar(100),
FechaCreacion datetime default getdate()
)

go

create table Categoria(
IdCategoria int primary key identity,
Nombre varchar(100),
FechaCreacion datetime default getdate()
)
go

create table Libro(
IdLibro int primary key identity,
IdCategoria int references Categoria(IdCategoria),
Codigo varchar(100),
Titulo varchar(100),
Autor varchar(100),
FechaPublicacion date,
Cantidad int,
FechaCreacion datetime default getdate()
)

go

create table NumeroCorrelativo(
IdNumeroCorrelativo int primary key identity,
Prefijo varchar(2) not null,
Tipo varchar(100) not null,
UltimoNumero int not null,
Longitud int not null,
FechaCreacion datetime default getdate()
)

go

create table Prestamo(
IdPrestamo int primary key identity,
IdEstudiante int references Estudiante(IdEstudiante),
IdLibro int references Libro(IdLibro),
FechaPrestamo datetime default getdate(),
FechaDevolucion datetime,
EstadoPrestamo varchar(50)
)

go


insert into Usuario(NombreCompleto,NombreUsuario,Clave) values
('USER ADMIN','Admin','123')
go

insert into NumeroCorrelativo(Prefijo,Tipo,UltimoNumero,Longitud) values
('ST','ESTUDIANTE',3,6),
('LB','LIBRO',6,6)

go

SET IDENTITY_INSERT Categoria ON
insert into Categoria(IdCategoria, Nombre) values
(1,'Quimica'),
(2,'Psicologia'),
(3,'Historia Geografia'),
(4,'Novela'),
(5,'Ciencia'),
(6,'Biografias'),
(7,'Novela Gotica'),
(8,'Poema'),
(9,'Ciencia Ficcion'),
(10,'Fantasia')

SET IDENTITY_INSERT Categoria OFF

go
set dateformat dmy

insert into Libro(IdCategoria,Codigo,Titulo,Autor,FechaPublicacion,Cantidad) values
(9,'LB0001','Nieve en Marte','Pablo tebar','4/01/1998',30),
(2,'LB0002','Psicologia para principiantes','Max Krone','13/06/1996',30),
(4,'LB0003','La fuerza de sheccid','Carlos cauhtemoc sanchez','7/12/1996',30),
(2,'LB0004','El secreto de las siete semillas','David Fischman','21/04/1996',30),
(10,'LB0005','Imaginaria','Kristopher Rodas','10/09/1997',30),
(4,'LB0006','Almendra','won pyung sohn','25/06/1996',30)

go

insert into Estudiante(Codigo,Nombres,Apellidos) values
('ST0001','Rodrigo','Mendez Espinoza'),
('ST0002','Adriana','Suarez Sanchez'),
('ST0003','Rosa Maria','Hurtado Flores')

