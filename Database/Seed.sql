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
-- Dumping data for table `imageninmueble`
--

LOCK TABLES `imageninmueble` WRITE;
/*!40000 ALTER TABLE `imageninmueble` DISABLE KEYS */;
INSERT INTO `imageninmueble` VALUES (1,2,'https://images.unsplash.com/photo-1600585154340-be6161a56a0c',1),(2,2,'https://images.unsplash.com/photo-1600607687920-4e2a09cf159d',0),(3,3,'https://images.unsplash.com/photo-1505693416388-ac5ce068fe85',1),(4,4,'https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde',1),(5,4,'https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3',0),(6,5,'https://images.unsplash.com/photo-1605146769289-440113cc3d00',1),(7,6,'https://images.unsplash.com/photo-1600210492486-724fe5c67fb0',1),(8,6,'https://images.unsplash.com/photo-1600607688969-a5bfcd646154',0),(9,7,'https://images.unsplash.com/photo-1600585154526-990dced4db0d',1),(10,8,'https://images.unsplash.com/photo-1600566753086-00f18fb6b3ea',1),(11,9,'https://images.unsplash.com/photo-1600607687939-ce8a6c25118c',1),(12,9,'https://images.unsplash.com/photo-1600607687920-4e2a09cf159d',0),(13,10,'https://images.unsplash.com/photo-1600566753051-f0b89df2dd90',1),(14,11,'https://images.unsplash.com/photo-1600585154084-4e5fe7c39198',1),(15,12,'https://images.unsplash.com/photo-1600607687920-4e2a09cf159d',1),(16,13,'https://images.unsplash.com/photo-1600607688969-a5bfcd646154',1),(17,14,'https://images.unsplash.com/photo-1600047509358-9dc75507daeb',1),(18,15,'https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3',1),(19,16,'https://images.unsplash.com/photo-1600585154340-be6161a56a0c',1);
/*!40000 ALTER TABLE `imageninmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `inmueble`
--

LOCK TABLES `inmueble` WRITE;
/*!40000 ALTER TABLE `inmueble` DISABLE KEYS */;
INSERT INTO `inmueble` VALUES (2,'Av. Illia 450, San Luis',4,-33.0000000,-66.0000000,45000.00,30.00,1,4,2),(3,'Junin 780, San Luis',2,-33.0000000,-66.0000000,32000.00,25.00,1,5,3),(4,'Rivadavia 1120, San Luis',6,-33.0000000,-66.0000000,58000.00,35.00,1,6,4),(5,'Belgrano 650, San Luis',3,-33.0000000,-66.0000000,39000.00,30.00,1,7,2),(6,'Pringles 920, San Luis',5,-33.0000000,-66.0000000,52000.00,40.00,1,8,5),(7,'Mitre 540, San Luis',2,-33.0000000,-66.0000000,35000.00,25.00,1,9,3),(8,'25 de Mayo 870, San Luis',4,-33.0000000,-66.0000000,47000.00,30.00,1,10,4),(9,'Chacabuco 430, San Luis',6,-33.0000000,-66.0000000,61000.00,35.00,1,11,6),(10,'Maipu 710, San Luis',3,-33.0000000,-66.0000000,41000.00,30.00,1,12,2),(11,'Las Heras 980, San Luis',5,-33.0000000,-66.0000000,55000.00,35.00,1,13,5),(12,'Ayacucho 620, San Luis',2,-33.0000000,-66.0000000,30000.00,25.00,1,14,3),(13,'Colon 1150, San Luis',4,-33.0000000,-66.0000000,43000.00,30.00,1,15,4),(14,'Lavalle 760, San Luis',6,-33.0000000,-66.0000000,59000.00,40.00,1,16,6),(15,'Sarmiento 510, San Luis',3,-33.0000000,-66.0000000,38000.00,25.00,1,5,2),(16,'Pedernera 890, San Luis',5,-33.0000000,-66.0000000,50000.00,30.00,1,8,5);
/*!40000 ALTER TABLE `inmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `inquilino`
--

LOCK TABLES `inquilino` WRITE;
/*!40000 ALTER TABLE `inquilino` DISABLE KEYS */;
INSERT INTO `inquilino` VALUES (3,'20193222','Alfonsina Moro','1155592231','grzz443@hotmail.com'),(4,'32113554','Rosina Quintos',NULL,'rosidx22@hotmail.com'),(5,'34432133','Arbol Godoy','2655034221','hhtt123@gmail.com'),(6,'94556212','Fernando Gil','1154328831','jacintosh20gil@gmail.com'),(7,'44932341','Roma Saavedra',NULL,NULL),(8,'44343332','Ian Tolosa','2665035688','in9921@gmail.com'),(9,'5332998','Don Santander','2345531322','santasanta50@hotmail.com'),(10,'44003312','Zoe Gil',NULL,'zoy342@outlook.com'),(11,'40000003','Uriel Aposito','3485033213','uri3485@gmail.com'),(12,'90332341','Alfredo Alfonso','2665213421',NULL),(13,'43552134','Rodrigo Toledo','255432123','rodri1997@gmail.com'),(14,'50332134','Erica Rico','2665332123',NULL);
/*!40000 ALTER TABLE `inquilino` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `pago`
--

LOCK TABLES `pago` WRITE;
/*!40000 ALTER TABLE `pago` DISABLE KEYS */;
/*!40000 ALTER TABLE `pago` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `propietario`
--

LOCK TABLES `propietario` WRITE;
/*!40000 ALTER TABLE `propietario` DISABLE KEYS */;
INSERT INTO `propietario` VALUES (4,'Juan','Dominguez','32100300','2664768863','juandd24@gmail.com'),(5,'Candela','Gutierrez','35432324','2664221589','gugucande@hotmail.com'),(6,'Ernesto','Robusto','20331254','38425544367','erobusto31@hotmail.com'),(7,'Sofia','Baez','40552345','2665036623','casanova22@gmail.com'),(8,'Stefania','Sosa','21334994','2665443777','estfi40@gmail.com'),(9,'Romina','Gomez','40404351','1154399564','romi49831@gmail.com'),(10,'Berta','Tero','34445212','2665435579','boquita883@hotmail.com'),(11,'Gustavo','Pereyra','99341324','2345050676','gussss40@hotmail.com'),(12,'Hugo','Zapata','4556553','2665983245',NULL),(13,'Alfredo','Zapata','20443551',NULL,'afk205@gmail.com'),(14,'Pedro','Godoy','45332132','2665030303','pedritoclavo@gmail.com'),(15,'Jan','Godoy','45332133',NULL,NULL),(16,'Nestor','Dorostiaga','42221335',NULL,'dorostiaga50@gmail.com');
/*!40000 ALTER TABLE `propietario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `reserva`
--

LOCK TABLES `reserva` WRITE;
/*!40000 ALTER TABLE `reserva` DISABLE KEYS */;
/*!40000 ALTER TABLE `reserva` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tipoinmueble`
--

LOCK TABLES `tipoinmueble` WRITE;
/*!40000 ALTER TABLE `tipoinmueble` DISABLE KEYS */;
INSERT INTO `tipoinmueble` VALUES (2,'Casa'),(3,'Departamento'),(4,'PH'),(5,'Cabania'),(6,'Quinta');
/*!40000 ALTER TABLE `tipoinmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (1,'0108.facultad@gmail.com','AQAAAAIAAYagAAAAEN6EgVbs+N11NloNwriqzxBeDh28KTyk2SaqgJ1By9aKK2cMJhKfc6p8vbhQRfF9Wg==','Administrador',NULL);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-17  0:48:30
