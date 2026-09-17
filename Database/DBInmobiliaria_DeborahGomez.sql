-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: dbinmobiliaria_deborahgomez
-- ------------------------------------------------------
-- Server version 8.0.46

SET NAMES utf8mb4;
SET CHARACTER SET utf8mb4;
SET COLLATION_CONNECTION = 'utf8mb4_0900_ai_ci';

SET UNIQUE_CHECKS = 0;
SET FOREIGN_KEY_CHECKS = 0;

DROP DATABASE IF EXISTS `dbinmobiliaria_deborahgomez`;

CREATE DATABASE `dbinmobiliaria_deborahgomez`
  DEFAULT CHARACTER SET utf8mb4
  COLLATE utf8mb4_0900_ai_ci;

USE `dbinmobiliaria_deborahgomez`;


-- ------------------------------------------------------
-- Table structure for table `propietario`
-- ------------------------------------------------------

DROP TABLE IF EXISTS `propietario`;

CREATE TABLE `propietario` (
  `IdPropietario` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(45) NOT NULL,
  `Apellido` varchar(45) NOT NULL,
  `Dni` varchar(45) NOT NULL,
  `Telefono` varchar(45) DEFAULT NULL,
  `Email` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`IdPropietario`),
  UNIQUE KEY `uq_Propietario_Dni` (`Dni`),
  UNIQUE KEY `uq_Propietario_Email` (`Email`)
) ENGINE=InnoDB
  AUTO_INCREMENT=17
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;


-- ------------------------------------------------------
-- Table structure for table `tipoinmueble`
-- ------------------------------------------------------

DROP TABLE IF EXISTS `tipoinmueble`;

CREATE TABLE `tipoinmueble` (
  `IdTipoInmueble` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(45) NOT NULL,
  PRIMARY KEY (`IdTipoInmueble`)
) ENGINE=InnoDB
  AUTO_INCREMENT=7
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;


-- ------------------------------------------------------
-- Table structure for table `usuario`
-- ------------------------------------------------------

DROP TABLE IF EXISTS `usuario`;

CREATE TABLE `usuario` (
  `IdUsuario` int NOT NULL AUTO_INCREMENT,
  `Email` varchar(100) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Rol` enum('Administrador','Empleado') NOT NULL,
  `Avatar` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`IdUsuario`),
  UNIQUE KEY `uq_Usuario_Email` (`Email`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;


-- ------------------------------------------------------
-- Table structure for table `inquilino`
-- ------------------------------------------------------

DROP TABLE IF EXISTS `inquilino`;

CREATE TABLE `inquilino` (
  `IdInquilino` int NOT NULL AUTO_INCREMENT,
  `Dni` varchar(45) NOT NULL,
  `NombreCompleto` varchar(45) NOT NULL,
  `Telefono` varchar(45) DEFAULT NULL,
  `Email` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`IdInquilino`),
  UNIQUE KEY `uq_Inquilino_Dni` (`Dni`),
  UNIQUE KEY `uq_Inquilino_Email` (`Email`)
) ENGINE=InnoDB
  AUTO_INCREMENT=15
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;


-- ------------------------------------------------------
-- Table structure for table `inmueble`
-- ------------------------------------------------------

DROP TABLE IF EXISTS `inmueble`;

CREATE TABLE `inmueble` (
  `IdInmueble` int NOT NULL AUTO_INCREMENT,
  `Direccion` varchar(255) NOT NULL,
  `Cupo` int NOT NULL,
  `Latitud` decimal(10,7) DEFAULT NULL,
  `Longitud` decimal(10,7) DEFAULT NULL,
  `PrecioPorDia` decimal(12,2) NOT NULL,
  `PorcentajeSenia` decimal(5,2) NOT NULL,
  `Disponible` tinyint NOT NULL DEFAULT '1',
  `PropietarioId` int NOT NULL,
  `TipoInmuebleId` int NOT NULL,
  PRIMARY KEY (`IdInmueble`),
  KEY `fk_PropietarioId_idx` (`PropietarioId`),
  KEY `fk_TipoInmuebleId_idx` (`TipoInmuebleId`),
  CONSTRAINT `fk_PropietarioId`
    FOREIGN KEY (`PropietarioId`)
    REFERENCES `propietario` (`IdPropietario`),
  CONSTRAINT `fk_TipoInmuebleId`
    FOREIGN KEY (`TipoInmuebleId`)
    REFERENCES `tipoinmueble` (`IdTipoInmueble`)
) ENGINE=InnoDB
  AUTO_INCREMENT=17
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;


-- ------------------------------------------------------
-- Table structure for table `reserva`
-- ------------------------------------------------------

DROP TABLE IF EXISTS `reserva`;

CREATE TABLE `reserva` (
  `IdReserva` int NOT NULL AUTO_INCREMENT,
  `FechaDesde` datetime NOT NULL,
  `FechaHasta` datetime NOT NULL,
  `FechaHastaOriginal` datetime NOT NULL,
  `MontoPorDia` decimal(12,2) NOT NULL,
  `Finalizada` tinyint NOT NULL DEFAULT '0',
  `FechaFinalizacionAnticipada` datetime DEFAULT NULL,
  `MontoMulta` decimal(12,2) DEFAULT NULL,
  `InmuebleId` int NOT NULL,
  `InquilinoId` int NOT NULL,
  `UsuarioCreadorId` int NOT NULL,
  `UsuarioFinalizadorId` int DEFAULT NULL,
  PRIMARY KEY (`IdReserva`),
  KEY `fk_InmuebleId_idx` (`InmuebleId`),
  KEY `fk_InquilinoId_idx` (`InquilinoId`),
  KEY `fk_UsuarioCreadorId_idx` (`UsuarioCreadorId`),
  KEY `fk_UsuarioFinalizadorId_idx` (`UsuarioFinalizadorId`),
  CONSTRAINT `fk_InmuebleId`
    FOREIGN KEY (`InmuebleId`)
    REFERENCES `inmueble` (`IdInmueble`),
  CONSTRAINT `fk_InquilinoId`
    FOREIGN KEY (`InquilinoId`)
    REFERENCES `inquilino` (`IdInquilino`),
  CONSTRAINT `fk_R_UsuarioCreadorId`
    FOREIGN KEY (`UsuarioCreadorId`)
    REFERENCES `usuario` (`IdUsuario`),
  CONSTRAINT `fk_R_UsuarioFinalizadorId`
    FOREIGN KEY (`UsuarioFinalizadorId`)
    REFERENCES `usuario` (`IdUsuario`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;


-- ------------------------------------------------------
-- Table structure for table `imageninmueble`
-- ------------------------------------------------------

DROP TABLE IF EXISTS `imageninmueble`;

CREATE TABLE `imageninmueble` (
  `IdImagenInmueble` int NOT NULL AUTO_INCREMENT,
  `InmuebleId` int NOT NULL,
  `Url` varchar(500) NOT NULL,
  `EsPortada` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`IdImagenInmueble`),
  KEY `fk_ImagenInmueble_Inmueble_idx` (`InmuebleId`),
  CONSTRAINT `fk_ImagenInmueble_Inmueble`
    FOREIGN KEY (`InmuebleId`)
    REFERENCES `inmueble` (`IdInmueble`)
    ON DELETE CASCADE
) ENGINE=InnoDB
  AUTO_INCREMENT=20
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;


-- ------------------------------------------------------
-- Table structure for table `pago`
-- ------------------------------------------------------

DROP TABLE IF EXISTS `pago`;

CREATE TABLE `pago` (
  `IdPago` int NOT NULL AUTO_INCREMENT,
  `Concepto` varchar(100) NOT NULL,
  `FechaPago` datetime NOT NULL,
  `Importe` decimal(12,2) NOT NULL,
  `Anulado` tinyint NOT NULL DEFAULT '0',
  `FechaAnulacion` datetime DEFAULT NULL,
  `ReservaId` int NOT NULL,
  `UsuarioCreadorId` int NOT NULL,
  `UsuarioAnuladorId` int DEFAULT NULL,
  PRIMARY KEY (`IdPago`),
  KEY `fk_ReservaId_idx` (`ReservaId`),
  KEY `fk_UsuarioCreadorId_idx` (`UsuarioCreadorId`),
  KEY `fk_UsuarioAnuladorId_idx` (`UsuarioAnuladorId`),
  CONSTRAINT `fk_Pago_UsuarioAnuladorId`
    FOREIGN KEY (`UsuarioAnuladorId`)
    REFERENCES `usuario` (`IdUsuario`),
  CONSTRAINT `fk_Pago_UsuarioCreadorId`
    FOREIGN KEY (`UsuarioCreadorId`)
    REFERENCES `usuario` (`IdUsuario`),
  CONSTRAINT `fk_ReservaId`
    FOREIGN KEY (`ReservaId`)
    REFERENCES `reserva` (`IdReserva`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;


SET FOREIGN_KEY_CHECKS = 1;
SET UNIQUE_CHECKS = 1;

SET NAMES utf8mb4;
SET CHARACTER SET utf8mb4;
SET COLLATION_CONNECTION = 'utf8mb4_0900_ai_ci';

-- Dump completed
