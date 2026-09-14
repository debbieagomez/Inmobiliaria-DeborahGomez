-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: dbinmobiliaria_deborahgomez
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `inmueble`
--
-- ORDER BY:  IdInmueble

INSERT INTO inmueble VALUES (2,'Av. Illia 450, San Luis',4,-33,-66,45000,30,1,NULL,4,2,NULL,NULL);
INSERT INTO inmueble VALUES (3,'Junin 780, San Luis',2,-33,-66,32000,25,1,NULL,5,3,NULL,NULL);
INSERT INTO inmueble VALUES (4,'Rivadavia 1120, San Luis',6,-33,-66,58000,35,1,NULL,6,4,NULL,NULL);
INSERT INTO inmueble VALUES (5,'Belgrano 650, San Luis',3,-33,-66,39000,30,1,NULL,7,2,NULL,NULL);
INSERT INTO inmueble VALUES (6,'Pringles 920, San Luis',5,-33,-66,52000,40,1,NULL,8,5,NULL,NULL);
INSERT INTO inmueble VALUES (7,'Mitre 540, San Luis',2,-33,-66,35000,25,1,NULL,9,3,NULL,NULL);
INSERT INTO inmueble VALUES (8,'25 de Mayo 870, San Luis',4,-33,-66,47000,30,1,NULL,10,4,NULL,NULL);
INSERT INTO inmueble VALUES (9,'Chacabuco 430, San Luis',6,-33,-66,61000,35,1,NULL,11,6,NULL,NULL);
INSERT INTO inmueble VALUES (10,'Maipu 710, San Luis',3,-33,-66,41000,30,1,NULL,12,2,NULL,NULL);
INSERT INTO inmueble VALUES (11,'Las Heras 980, San Luis',5,-33,-66,55000,35,1,NULL,13,5,NULL,NULL);
INSERT INTO inmueble VALUES (12,'Ayacucho 620, San Luis',2,-33,-66,30000,25,1,NULL,14,3,NULL,NULL);
INSERT INTO inmueble VALUES (13,'Colon 1150, San Luis',4,-33,-66,43000,30,1,NULL,15,4,NULL,NULL);
INSERT INTO inmueble VALUES (14,'Lavalle 760, San Luis',6,-33,-66,59000,40,1,NULL,16,6,NULL,NULL);
INSERT INTO inmueble VALUES (15,'Sarmiento 510, San Luis',3,-33,-66,38000,25,1,NULL,5,2,NULL,NULL);
INSERT INTO inmueble VALUES (16,'Pedernera 890, San Luis',5,-33,-66,50000,30,1,NULL,8,5,NULL,NULL);

--
-- Dumping data for table `inquilino`
--
-- ORDER BY:  IdInquilino

INSERT INTO inquilino VALUES (3,'20193222','Alfonsina Moro','1155592231','grzz443@hotmail.com');
INSERT INTO inquilino VALUES (4,'32113554','Rosina Quintos','','rosidx22@hotmail.com');
INSERT INTO inquilino VALUES (5,'34432133','Arbol Godoy','2655034221','hhtt123@gmail.com');
INSERT INTO inquilino VALUES (6,'94556212','Fernando Gil','1154328831','jacintosh20gil@gmail.com');
INSERT INTO inquilino VALUES (7,'44932341','Roma Saavedra','','');
INSERT INTO inquilino VALUES (8,'44343332','Ian Tolosa','2665035688','in9921@gmail.com');
INSERT INTO inquilino VALUES (9,'5332998','Don Santander','2345531322','santasanta50@hotmail.com');
INSERT INTO inquilino VALUES (10,'44003312','Zoe Gil','','zoy342@outlook.com');
INSERT INTO inquilino VALUES (11,'40000003','Uriel Aposito','3485033213','uri3485@gmail.com');
INSERT INTO inquilino VALUES (12,'90332341','Alfredo Alfonso','2665213421','');
INSERT INTO inquilino VALUES (13,'43552134','Rodrigo Toledo','255432123','rodri1997@gmail.com');
INSERT INTO inquilino VALUES (14,'50332134','Erica Rico','2665332123',NULL);

--
-- Dumping data for table `pago`
--
-- ORDER BY:  IdPago


--
-- Dumping data for table `propietario`
--
-- ORDER BY:  IdPropietario

INSERT INTO propietario VALUES (4,'Juan','Dominguez','32100300','2664768863','juandd24@gmail.com');
INSERT INTO propietario VALUES (5,'Candela','Gutierrez','35432324','2664221589','gugucande@hotmail.com');
INSERT INTO propietario VALUES (6,'Ernesto','Robusto','20331254','38425544367','erobusto31@hotmail.com');
INSERT INTO propietario VALUES (7,'Sofia','Baez','40552345','2665036623','casanova22@gmail.com');
INSERT INTO propietario VALUES (8,'Stefania','Sosa','21334994','2665443777','estfi40@gmail.com');
INSERT INTO propietario VALUES (9,'Romina','Gomez','40404351','1154399564','romi49831@gmail.com');
INSERT INTO propietario VALUES (10,'Berta','Tero','34445212','2665435579','boquita883@hotmail.com');
INSERT INTO propietario VALUES (11,'Gustavo','Pereyra','99341324','2345050676','gussss40@hotmail.com');
INSERT INTO propietario VALUES (12,'Hugo','Zapata','4556553','2665983245','');
INSERT INTO propietario VALUES (13,'Alfredo','Zapata','20443551','','afk205@gmail.com');
INSERT INTO propietario VALUES (14,'Pedro','Godoy','45332132','2665030303','pedritoclavo@gmail.com');
INSERT INTO propietario VALUES (15,'Jan','Godoy','45332132','','');
INSERT INTO propietario VALUES (16,'Nestor','Dorostiaga','42221335','','dorostiaga50@gmail.com');

--
-- Dumping data for table `reserva`
--
-- ORDER BY:  IdReserva


--
-- Dumping data for table `tipoinmueble`
--
-- ORDER BY:  IdTipoInmueble

INSERT INTO tipoinmueble VALUES (2,'Casa');
INSERT INTO tipoinmueble VALUES (3,'Departamento');
INSERT INTO tipoinmueble VALUES (4,'PH');
INSERT INTO tipoinmueble VALUES (5,'Cabania');
INSERT INTO tipoinmueble VALUES (6,'Quinta');

--
-- Dumping data for table `usuario`
--
-- ORDER BY:  IdUsuario


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-13 21:15:23
