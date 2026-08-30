-- Esquema (Base de datos) para inicializar en MySQL Workbench

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema DBInmobiliaria_DeborahGomez
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema DBInmobiliaria_DeborahGomez
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `DBInmobiliaria_DeborahGomez` DEFAULT CHARACTER SET utf8 ;
USE `DBInmobiliaria_DeborahGomez` ;

-- -----------------------------------------------------
-- Table `DBInmobiliaria_DeborahGomez`.`Propietario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DBInmobiliaria_DeborahGomez`.`Propietario` (
  `IdPropietario` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  `Apellido` VARCHAR(45) NOT NULL,
  `Dni` VARCHAR(45) NOT NULL,
  `Telefono` VARCHAR(45) NULL,
  `Email` VARCHAR(45) NULL,
  PRIMARY KEY (`IdPropietario`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `DBInmobiliaria_DeborahGomez`.`TipoInmueble`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DBInmobiliaria_DeborahGomez`.`TipoInmueble` (
  `IdTipoInmueble` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`IdTipoInmueble`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `DBInmobiliaria_DeborahGomez`.`Inmueble`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DBInmobiliaria_DeborahGomez`.`Inmueble` (
  `IdInmueble` INT NOT NULL AUTO_INCREMENT,
  `Direccion` VARCHAR(45) NOT NULL,
  `Cupo` INT NOT NULL,
  `Latitud` DECIMAL(16) NULL,
  `Longitud` DECIMAL(16) NULL,
  `PrecioPorDia` DECIMAL(32) NOT NULL,
  `PorcentajeSenia` DECIMAL(32) NOT NULL,
  `Disponible` TINYINT NOT NULL DEFAULT 1,
  `ImagenPortadaUrl` VARCHAR(45) NULL,
  `PropietarioId` INT NOT NULL,
  `TipoInmuebleId` INT NOT NULL,
  `propietarioNombre` VARCHAR(45) NULL,
  `TipoNombre` VARCHAR(45) NULL,
  PRIMARY KEY (`IdInmueble`),
  INDEX `fk_PropietarioId_idx` (`PropietarioId` ASC) VISIBLE,
  INDEX `fk_TipoInmuebleId_idx` (`TipoInmuebleId` ASC) VISIBLE,
  CONSTRAINT `fk_PropietarioId`
    FOREIGN KEY (`PropietarioId`)
    REFERENCES `DBInmobiliaria_DeborahGomez`.`Propietario` (`IdPropietario`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_TipoInmuebleId`
    FOREIGN KEY (`TipoInmuebleId`)
    REFERENCES `DBInmobiliaria_DeborahGomez`.`TipoInmueble` (`IdTipoInmueble`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `DBInmobiliaria_DeborahGomez`.`Inquilino`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DBInmobiliaria_DeborahGomez`.`Inquilino` (
  `IdInquilino` INT NOT NULL AUTO_INCREMENT,
  `Dni` VARCHAR(45) NOT NULL,
  `NombreCompleto` VARCHAR(45) NOT NULL,
  `Telefono` VARCHAR(45) NULL,
  `Email` VARCHAR(45) NULL,
  PRIMARY KEY (`IdInquilino`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `DBInmobiliaria_DeborahGomez`.`Usuario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DBInmobiliaria_DeborahGomez`.`Usuario` (
  `IdUsuario` INT NOT NULL AUTO_INCREMENT,
  `Email` VARCHAR(45) NULL,
  `PasswordHash` VARCHAR(45) NOT NULL,
  `Rol` ENUM('Administrador', 'Empleado') NOT NULL,
  `Avatar` VARCHAR(45) NULL,
  PRIMARY KEY (`IdUsuario`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `DBInmobiliaria_DeborahGomez`.`Reserva`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DBInmobiliaria_DeborahGomez`.`Reserva` (
  `IdReserva` INT NOT NULL AUTO_INCREMENT,
  `FechaDesde` DATETIME NOT NULL,
  `FechaHasta` DATETIME NOT NULL,
  `FechaHastaOriginal` DATETIME NOT NULL,
  `MontoPorDia` DECIMAL(32) NOT NULL,
  `Finalizada` TINYINT NOT NULL,
  `FechaFinalizacionAnticipada` DATETIME NULL,
  `MontoMulta` DECIMAL(32) NULL,
  `InmuebleId` INT NOT NULL,
  `InquilinoId` INT NOT NULL,
  `UsuarioCreadorId` INT NOT NULL,
  `UsuarioFinalizadorId` INT NULL,
  PRIMARY KEY (`IdReserva`),
  INDEX `fk_InmuebleId_idx` (`InmuebleId` ASC) VISIBLE,
  INDEX `fk_InquilinoId_idx` (`InquilinoId` ASC) VISIBLE,
  INDEX `fk_UsuarioCreadorId_idx` (`UsuarioCreadorId` ASC) VISIBLE,
  CONSTRAINT `fk_InmuebleId`
    FOREIGN KEY (`InmuebleId`)
    REFERENCES `DBInmobiliaria_DeborahGomez`.`Inmueble` (`IdInmueble`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_InquilinoId`
    FOREIGN KEY (`InquilinoId`)
    REFERENCES `DBInmobiliaria_DeborahGomez`.`Inquilino` (`IdInquilino`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_R_UsuarioCreadorId`
    FOREIGN KEY (`UsuarioCreadorId`)
    REFERENCES `DBInmobiliaria_DeborahGomez`.`Usuario` (`IdUsuario`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `DBInmobiliaria_DeborahGomez`.`Pago`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DBInmobiliaria_DeborahGomez`.`Pago` (
  `IdPago` INT NOT NULL AUTO_INCREMENT,
  `Concepto` VARCHAR(45) NOT NULL,
  `FechaPago` DATETIME NOT NULL,
  `Importe` DECIMAL(32) NOT NULL,
  `Anulado` TINYINT NOT NULL,
  `ReservaId` INT NOT NULL,
  `UsuarioCreadorId` INT NOT NULL,
  `UsuarioAnuladorId` INT NULL,
  PRIMARY KEY (`IdPago`),
  INDEX `fk_ReservaId_idx` (`ReservaId` ASC) VISIBLE,
  INDEX `fk_UsuarioCreadorId_idx` (`UsuarioCreadorId` ASC) VISIBLE,
  CONSTRAINT `fk_ReservaId`
    FOREIGN KEY (`ReservaId`)
    REFERENCES `DBInmobiliaria_DeborahGomez`.`Reserva` (`IdReserva`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pago_UsuarioCreadorId`
    FOREIGN KEY (`UsuarioCreadorId`)
    REFERENCES `DBInmobiliaria_DeborahGomez`.`Usuario` (`IdUsuario`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
