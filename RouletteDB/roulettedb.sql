-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 28, 2019 at 05:49 PM
-- Server version: 10.4.8-MariaDB
-- PHP Version: 7.1.32

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `roulettedb`
--

-- --------------------------------------------------------

--
-- Table structure for table `bets`
--

CREATE TABLE `bets` (
  `SpinId` char(36) NOT NULL,
  `UserId` int(10) UNSIGNED NOT NULL,
  `BetString` text NOT NULL,
  `BetAmount` decimal(15,2) NOT NULL,
  `WinningNumber` int(11) NOT NULL,
  `JackPotId` int(11) NOT NULL,
  `WonAmount` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `IpAddress` varchar(50) NOT NULL,
  `CreateDate` datetime NOT NULL,
  `IsActive` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `jackpot`
--

CREATE TABLE `jackpot` (
  `Id` int(11) NOT NULL,
  `JackPotAmount` decimal(15,2) NOT NULL,
  `CreateDate` datetime NOT NULL,
  `UpdateDate` datetime NOT NULL,
  `IsActive` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `jackpot`
--

INSERT INTO `jackpot` (`Id`, `JackPotAmount`, `CreateDate`, `UpdateDate`, `IsActive`) VALUES
(1, '1000.00', '2019-10-24 00:00:00', '2019-10-28 08:55:25', 1);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` int(10) UNSIGNED NOT NULL,
  `FirstName` varchar(250) NOT NULL,
  `LastName` varchar(250) NOT NULL,
  `UserName` text NOT NULL,
  `Password` text NOT NULL,
  `Salt` text NOT NULL,
  `Balance` decimal(15,2) NOT NULL,
  `CreateDate` datetime NOT NULL,
  `UpdateDate` datetime NOT NULL,
  `IsActive` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `FirstName`, `LastName`, `UserName`, `Password`, `Salt`, `Balance`, `CreateDate`, `UpdateDate`, `IsActive`) VALUES
(1, 'kakha', 'tezelashvili', 'admin', '81d77fd5637909ab7807a9685a294fdd3b2cbe109439a11d1fb89e868a80a948', '232ba493c391f32ad9da79a9de93b4e6b6f01eb8b99c282abb83e89ab19ea59b', '200.00', '2019-10-21 00:00:00', '2019-10-28 08:55:25', 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bets`
--
ALTER TABLE `bets`
  ADD KEY `FK_BetToJackPot` (`JackPotId`),
  ADD KEY `FK_BetToUser` (`UserId`);

--
-- Indexes for table `jackpot`
--
ALTER TABLE `jackpot`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `jackpot`
--
ALTER TABLE `jackpot`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `bets`
--
ALTER TABLE `bets`
  ADD CONSTRAINT `FK_BetToJackPot` FOREIGN KEY (`JackPotId`) REFERENCES `jackpot` (`Id`),
  ADD CONSTRAINT `FK_BetToUser` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
