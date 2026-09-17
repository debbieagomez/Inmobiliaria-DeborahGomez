-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: dbinmobiliaria_deborahgomez
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `imageninmueble`
--

DROP TABLE IF EXISTS `imageninmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `imageninmueble` (
  `IdImagenInmueble` int NOT NULL AUTO_INCREMENT,
  `InmuebleId` int NOT NULL,
  `Url` varchar(500) NOT NULL,
  `EsPortada` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`IdImagenInmueble`),
  KEY `fk_ImagenInmueble_Inmueble_idx` (`InmuebleId`),
  CONSTRAINT `fk_ImagenInmueble_Inmueble` FOREIGN KEY (`InmuebleId`) REFERENCES `inmueble` (`IdInmueble`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `inmueble`
--

DROP TABLE IF EXISTS `inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
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
  CONSTRAINT `fk_PropietarioId` FOREIGN KEY (`PropietarioId`) REFERENCES `propietario` (`IdPropietario`),
  CONSTRAINT `fk_TipoInmuebleId` FOREIGN KEY (`TipoInmuebleId`) REFERENCES `tipoinmueble` (`IdTipoInmueble`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `inquilino`
--

DROP TABLE IF EXISTS `inquilino`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inquilino` (
  `IdInquilino` int NOT NULL AUTO_INCREMENT,
  `Dni` varchar(45) NOT NULL,
  `NombreCompleto` varchar(45) NOT NULL,
  `Telefono` varchar(45) DEFAULT NULL,
  `Email` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`IdInquilino`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pago`
--

DROP TABLE IF EXISTS `pago`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
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
  CONSTRAINT `fk_Pago_UsuarioAnuladorId` FOREIGN KEY (`UsuarioAnuladorId`) REFERENCES `usuario` (`IdUsuario`),
  CONSTRAINT `fk_Pago_UsuarioCreadorId` FOREIGN KEY (`UsuarioCreadorId`) REFERENCES `usuario` (`IdUsuario`),
  CONSTRAINT `fk_ReservaId` FOREIGN KEY (`ReservaId`) REFERENCES `reserva` (`IdReserva`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `propietario`
--

DROP TABLE IF EXISTS `propietario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `propietario` (
  `IdPropietario` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(45) NOT NULL,
  `Apellido` varchar(45) NOT NULL,
  `Dni` varchar(45) NOT NULL,
  `Telefono` varchar(45) DEFAULT NULL,
  `Email` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`IdPropietario`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `reserva`
--

DROP TABLE IF EXISTS `reserva`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reserva` (
  `IdReserva` int NOT NULL AUTO_INCREMENT,
  `FechaDesde` datetime NOT NULL,
  `FechaHasta` datetime NOT NULL,
  `FechaHastaOriginal` datetime NOT NULL,
  `MontoPorDia` decimal(32,0) NOT NULL,
  `Finalizada` tinyint NOT NULL,
  `FechaFinalizacionAnticipada` datetime DEFAULT NULL,
  `MontoMulta` decimal(32,0) DEFAULT NULL,
  `InmuebleId` int NOT NULL,
  `InquilinoId` int NOT NULL,
  `UsuarioCreadorId` int NOT NULL,
  `UsuarioFinalizadorId` int DEFAULT NULL,
  PRIMARY KEY (`IdReserva`),
  KEY `fk_InmuebleId_idx` (`InmuebleId`),
  KEY `fk_InquilinoId_idx` (`InquilinoId`),
  KEY `fk_UsuarioCreadorId_idx` (`UsuarioCreadorId`),
  CONSTRAINT `fk_InmuebleId` FOREIGN KEY (`InmuebleId`) REFERENCES `inmueble` (`IdInmueble`),
  CONSTRAINT `fk_InquilinoId` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilino` (`IdInquilino`),
  CONSTRAINT `fk_R_UsuarioCreadorId` FOREIGN KEY (`UsuarioCreadorId`) REFERENCES `usuario` (`IdUsuario`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tipoinmueble`
--

DROP TABLE IF EXISTS `tipoinmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipoinmueble` (
  `IdTipoInmueble` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(45) NOT NULL,
  PRIMARY KEY (`IdTipoInmueble`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `IdUsuario` int NOT NULL AUTO_INCREMENT,
  `Email` varchar(100) DEFAULT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Rol` enum('Administrador','Empleado') NOT NULL,
  `Avatar` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`IdUsuario`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-17  0:12:46
