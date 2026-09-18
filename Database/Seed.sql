SET NAMES utf8mb4;
SET CHARACTER SET utf8mb4;
SET COLLATION_CONNECTION = 'utf8mb4_0900_ai_ci';

SET UNIQUE_CHECKS = 0;
SET FOREIGN_KEY_CHECKS = 0;

USE `dbinmobiliaria_deborahgomez`;

TRUNCATE TABLE `pago`;
TRUNCATE TABLE `reserva`;
TRUNCATE TABLE `imageninmueble`;
TRUNCATE TABLE `inmueble`;
TRUNCATE TABLE `inquilino`;
TRUNCATE TABLE `propietario`;
TRUNCATE TABLE `tipoinmueble`;
TRUNCATE TABLE `usuario`;

INSERT INTO `propietario` (`IdPropietario`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`) VALUES
(4, 'Juan', 'Dominguez', '32100300', '2664768863', 'juandd24@gmail.com'),
(5, 'Candela', 'Gutierrez', '35432324', '2664221589', 'gugucande@hotmail.com'),
(6, 'Ernesto', 'Robusto', '20331254', '38425544367', 'erobusto31@hotmail.com'),
(7, 'Sofia', 'Baez', '40552345', '2665036623', 'casanova22@gmail.com'),
(8, 'Stefania', 'Sosa', '21334994', '2665443777', 'estfi40@gmail.com'),
(9, 'Romina', 'Gomez', '40404351', '1154399564', 'romi49831@gmail.com'),
(10, 'Berta', 'Tero', '34445212', '2665435579', 'boquita883@hotmail.com'),
(11, 'Gustavo', 'Pereyra', '99341324', '2345050676', 'gussss40@hotmail.com'),
(12, 'Hugo', 'Zapata', '4556553', '2665983245', NULL),
(13, 'Alfredo', 'Zapata', '20443551', NULL, 'afk205@gmail.com'),
(14, 'Pedro', 'Godoy', '45332132', '2665030303', 'pedritoclavo@gmail.com'),
(15, 'Jan', 'Godoy', '45332133', NULL, NULL),
(16, 'Nestor', 'Dorostiaga', '42221335', NULL, 'dorostiaga50@gmail.com');

INSERT INTO `tipoinmueble` (`IdTipoInmueble`, `Nombre`) VALUES
(2, 'Casa'),
(3, 'Departamento'),
(4, 'PH'),
(5, 'Cabania'),
(6, 'Quinta');

INSERT INTO `inquilino` (`IdInquilino`, `Dni`, `NombreCompleto`, `Telefono`, `Email`) VALUES
(3, '20193222', 'Alfonsina Moro', '1155592231', 'grzz443@hotmail.com'),
(4, '32113554', 'Rosina Quintos', NULL, 'rosidx22@hotmail.com'),
(5, '34432133', 'Arbol Godoy', '2655034221', 'hhtt123@gmail.com'),
(6, '94556212', 'Fernando Gil', '1154328831', 'jacintosh20gil@gmail.com'),
(7, '44932341', 'Roma Saavedra', NULL, NULL),
(8, '44343332', 'Ian Tolosa', '2665035688', 'in9921@gmail.com'),
(9, '5332998', 'Don Santander', '2345531322', 'santasanta50@hotmail.com'),
(10, '44003312', 'Zoe Gil', NULL, 'zoy342@outlook.com'),
(11, '40000003', 'Uriel Aposito', '3485033213', 'uri3485@gmail.com'),
(12, '90332341', 'Alfredo Alfonso', '2665213421', NULL),
(13, '43552134', 'Rodrigo Toledo', '255432123', 'rodri1997@gmail.com'),
(14, '50332134', 'Erica Rico', '2665332123', NULL);

INSERT INTO `inmueble` (`IdInmueble`, `Direccion`, `Cupo`, `Latitud`, `Longitud`, `PrecioPorDia`, `PorcentajeSenia`, `Disponible`, `PropietarioId`, `TipoInmuebleId`) VALUES
(2, 'Av. Illia 450, San Luis', 4, -33.0000000, -66.0000000, 45000.00, 30.00, 1, 4, 2),
(3, 'Junin 780, San Luis', 2, -33.0000000, -66.0000000, 32000.00, 25.00, 1, 5, 3),
(4, 'Rivadavia 1120, San Luis', 6, -33.0000000, -66.0000000, 58000.00, 35.00, 1, 6, 4),
(5, 'Belgrano 650, San Luis', 3, -33.0000000, -66.0000000, 39000.00, 30.00, 1, 7, 2),
(6, 'Pringles 920, San Luis', 5, -33.0000000, -66.0000000, 52000.00, 40.00, 1, 8, 5),
(7, 'Mitre 540, San Luis', 2, -33.0000000, -66.0000000, 35000.00, 25.00, 1, 9, 3),
(8, '25 de Mayo 870, San Luis', 4, -33.0000000, -66.0000000, 47000.00, 30.00, 1, 10, 4),
(9, 'Chacabuco 430, San Luis', 6, -33.0000000, -66.0000000, 61000.00, 35.00, 1, 11, 6),
(10, 'Maipu 710, San Luis', 3, -33.0000000, -66.0000000, 41000.00, 30.00, 1, 12, 2),
(11, 'Las Heras 980, San Luis', 5, -33.0000000, -66.0000000, 55000.00, 35.00, 1, 13, 5),
(12, 'Ayacucho 620, San Luis', 2, -33.0000000, -66.0000000, 30000.00, 25.00, 1, 14, 3),
(13, 'Colon 1150, San Luis', 4, -33.0000000, -66.0000000, 43000.00, 30.00, 1, 15, 4),
(14, 'Lavalle 760, San Luis', 6, -33.0000000, -66.0000000, 59000.00, 40.00, 1, 16, 6),
(15, 'Sarmiento 510, San Luis', 3, -33.0000000, -66.0000000, 38000.00, 25.00, 1, 5, 2),
(16, 'Pedernera 890, San Luis', 5, -33.0000000, -66.0000000, 50000.00, 30.00, 1, 8, 5);

INSERT INTO `imageninmueble` (`IdImagenInmueble`, `InmuebleId`, `Url`, `EsPortada`) VALUES
(1, 2, 'https://images.unsplash.com/photo-1600585154340-be6161a56a0c', 1),
(2, 2, 'https://images.unsplash.com/photo-1600607687920-4e2a09cf159d', 0),
(3, 3, 'https://images.unsplash.com/photo-1505693416388-ac5ce068fe85', 1),
(4, 4, 'https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde', 1),
(5, 4, 'https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3', 0),
(6, 5, 'https://images.unsplash.com/photo-1605146769289-440113cc3d00', 1),
(7, 6, 'https://images.unsplash.com/photo-1600210492486-724fe5c67fb0', 1),
(8, 6, 'https://images.unsplash.com/photo-1600607688969-a5bfcd646154', 0),
(9, 7, 'https://images.unsplash.com/photo-1600585154526-990dced4db0d', 1),
(10, 8, 'https://images.unsplash.com/photo-1600566753086-00f18fb6b3ea', 1),
(11, 9, 'https://images.unsplash.com/photo-1600607687939-ce8a6c25118c', 1),
(12, 9, 'https://images.unsplash.com/photo-1600607687920-4e2a09cf159d', 0),
(13, 10, 'https://images.unsplash.com/photo-1600566753051-f0b89df2dd90', 1),
(14, 11, 'https://images.unsplash.com/photo-1600585154084-4e5fe7c39198', 1),
(15, 12, 'https://images.unsplash.com/photo-1600607687920-4e2a09cf159d', 1),
(16, 13, 'https://images.unsplash.com/photo-1600607688969-a5bfcd646154', 1),
(17, 14, 'https://images.unsplash.com/photo-1600047509358-9dc75507daeb', 1),
(18, 15, 'https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3', 1),
(19, 16, 'https://images.unsplash.com/photo-1600585154340-be6161a56a0c', 1);

SET FOREIGN_KEY_CHECKS = 1;
SET UNIQUE_CHECKS = 1;