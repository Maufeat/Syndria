-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               10.1.25-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             10.3.0.5771
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for game
DROP DATABASE IF EXISTS `game`;
CREATE DATABASE IF NOT EXISTS `game` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */;
USE `game`;

-- Dumping structure for table game.hero_data
DROP TABLE IF EXISTS `hero_data`;
CREATE TABLE IF NOT EXISTS `hero_data` (
  `id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL,
  `description` varchar(255) NOT NULL,
  `type` int(11) NOT NULL DEFAULT '1',
  `rarity` int(11) NOT NULL DEFAULT '1',
  `aptitude` int(11) NOT NULL DEFAULT '1',
  `health` int(11) NOT NULL DEFAULT '1',
  `attack` int(11) NOT NULL DEFAULT '1',
  `attack_range` int(11) NOT NULL DEFAULT '1',
  `movement` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table game.hero_data: ~3 rows (approximately)
/*!40000 ALTER TABLE `hero_data` DISABLE KEYS */;
INSERT IGNORE INTO `hero_data` (`id`, `name`, `description`, `type`, `rarity`, `aptitude`, `health`, `attack`, `attack_range`, `movement`) VALUES
	(1, 'Boruto', 'No Descrption', 1, 1, 7, 52, 15, 5, 3),
	(2, 'Sarada', 'No Description', 2, 1, 8, 55, 12, 4, 3),
	(3, 'Mitsuki', 'No Description', 3, 1, 6, 55, 17, 5, 2),
	(4, 'Nasdasd', 'asdad', 1, 1, 1, 1, 1, 1, 1),
	(5, 'Test', 'test', 1, 1, 1, 1, 1, 1, 1);
/*!40000 ALTER TABLE `hero_data` ENABLE KEYS */;

-- Dumping structure for table game.users
DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nickname` varchar(16) NOT NULL,
  `fb_id` bigint(20) NOT NULL,
  `last_login` date NOT NULL,
  `tutorial_done` int(1) NOT NULL DEFAULT '0',
  `last_daily` date NOT NULL,
  `daily_count` int(11) DEFAULT '0',
  `level` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `energy` int(11) NOT NULL,
  `last_used_energy` date NOT NULL,
  `gold` int(11) NOT NULL,
  `diamonds` int(11) NOT NULL,
  `create_time` date NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=latin1;

-- Dumping data for table game.users: ~8 rows (approximately)
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT IGNORE INTO `users` (`id`, `nickname`, `fb_id`, `last_login`, `tutorial_done`, `last_daily`, `daily_count`, `level`, `exp`, `energy`, `last_used_energy`, `gold`, `diamonds`, `create_time`) VALUES
	(23, 'Maufeat', 3180195068675868, '2020-02-22', 0, '0000-00-00', 0, 1, 0, 0, '2020-02-22', 0, 0, '2020-02-22'),
	(24, 'Mazfeatt', 3180195068675868, '2020-04-07', 0, '0000-00-00', 0, 1, 0, 0, '2020-04-07', 0, 0, '2020-04-07'),
	(25, 'Maufeat', 3180195068675868, '2020-04-07', 0, '0000-00-00', 0, 1, 0, 0, '2020-04-07', 0, 0, '2020-04-07'),
	(26, 'Maufeat', 3180195068675868, '2020-04-07', 0, '0000-00-00', 0, 1, 0, 0, '2020-04-07', 0, 0, '2020-04-07'),
	(27, 'Mazfeat', 3180195068675868, '2020-04-07', 0, '0000-00-00', 0, 1, 0, 0, '2020-04-07', 0, 0, '2020-04-07'),
	(28, 'Maufeat', 3180195068675868, '2020-04-07', 0, '0000-00-00', 0, 1, 0, 0, '2020-04-07', 0, 0, '2020-04-07'),
	(29, 'Maufeat', 3180195068675868, '2020-04-07', 0, '0000-00-00', 0, 1, 0, 0, '2020-04-07', 0, 0, '2020-04-07'),
	(30, 'Maufeat', 3180195068675868, '2020-04-07', 0, '0000-00-00', 0, 1, 0, 0, '2020-04-07', 0, 0, '2020-04-07'),
	(31, 'Maufeat', 3180195068675868, '2020-04-08', 0, '0000-00-00', 0, 1, 0, 0, '2020-04-08', 0, 0, '2020-04-08');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;

-- Dumping structure for table game.user_hero
DROP TABLE IF EXISTS `user_hero`;
CREATE TABLE IF NOT EXISTS `user_hero` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `hero_id` int(11) NOT NULL,
  `aptitude` int(11) NOT NULL,
  `level` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=latin1;

-- Dumping data for table game.user_hero: ~1 rows (approximately)
/*!40000 ALTER TABLE `user_hero` DISABLE KEYS */;
INSERT IGNORE INTO `user_hero` (`id`, `user_id`, `hero_id`, `aptitude`, `level`, `exp`) VALUES
	(30, 31, 5, 0, 1, 0);
/*!40000 ALTER TABLE `user_hero` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
