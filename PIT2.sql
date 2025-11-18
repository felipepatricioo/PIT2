create table Cupcake (
	ID uniqueidentifier primary key default newid(),
	Name varchar(100),
	Price decimal,
	Description text,
	Photo text,
	Flavor int,
	Topping int,
	Avaliable bit
)
create table Flavor (
	ID int identity,
	Name varchar(100),
)
create table Topping (
	ID int identity,
	Name varchar(100),
)
create table Cupom(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Cupom varchar(50),
	Avaliable bit,
	Desconto decimal
)
create table Pedido(
	Id uniqueidentifier primary key ,
	Total decimal,
	Email varchar(50),
	Customizacao text,
	Desconto decimal,
	DataPedido datetime
)

create table PedidoItens(
	IdPedido uniqueidentifier,
	IdItem uniqueidentifier
)
insert into Cupom (Cupom, Avaliable, Desconto)
values 
('MENOS10', 1, 10),
('MENOS20', 1, 20)
create table Cupcake (
	ID uniqueidentifier primary key default newid(),
	Name varchar(100),
	Price decimal,
	Description text,
	Photo text,
	Flavor int,
	Topping int,
	Avaliable bit
)

create table Flavor (
	ID int identity,
	Name varchar(100),
)

create table Topping (
	ID int identity,
	Name varchar(100),
)

insert into Cupcake (Name, Photo, Price, Flavor, Topping, Avaliable)
values
('Cupcake de Chocolate', 
'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSfAP32nCsIjqXD-6_N0rTTpO7PmEO6pAqp3Q&s', 
14.99,
1,
1,
1),
('Cupcake de Baunilha',
'https://static.itdg.com.br/images/360-240/23ec80134d4403cbc1f327586fc38c31/275202-original.jpg',
14.99,
2,
1,
1
)

create table Contato (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Email varchar(50),
	Nome varchar(100),
	Mensagem text
)
