/*
 Navicat Premium Data Transfer

 Source Server         : localhost
 Source Server Type    : MySQL
 Source Server Version : 80018
 Source Host           : localhost:3306
 Source Schema         : hospitalinformationsystem

 Target Server Type    : MySQL
 Target Server Version : 80018
 File Encoding         : 65001

 Date: 20/05/2020 22:18:29
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for bed
-- ----------------------------
DROP TABLE IF EXISTS `bed`;
CREATE TABLE `bed`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `departmentId` int(11) NULL DEFAULT NULL,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `isFree` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `recordId` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `departmentId`(`departmentId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 58 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci;

-- ----------------------------
-- Records of bed
-- ----------------------------
BEGIN;
INSERT INTO `bed` VALUES (12, 4, '外科——1号床', '空闲', NULL), (13, 4, '外科——2号床', '已占用', 10), (14, 4, '外科——3号床', '空闲', NULL), (15, 4, '外科——4号床', '空闲', NULL), (16, 4, '外科——5号床', '空闲', NULL), (17, 5, '呼吸科——1号床', '空闲', NULL), (18, 5, '呼吸科——2号床', '空闲', NULL), (19, 5, '呼吸科——3号床', '空闲', NULL), (20, 5, '呼吸科——4号床', '空闲', NULL), (21, 5, '呼吸科——5号床', '空闲', NULL), (22, 5, '呼吸科——6号床', '已占用', 7), (23, 5, '呼吸科——7号床', '空闲', NULL), (24, 5, '呼吸科——8号床', '空闲', NULL), (25, 6, '男科——1号床', '空闲', NULL), (26, 6, '男科——2号床', '已占用', 11), (27, 6, '男科——3号床', '空闲', NULL), (28, 7, '精神科——1号床', '空闲', NULL), (29, 7, '精神科——2号床', '空闲', NULL), (30, 7, '精神科——3号床', '空闲', NULL), (31, 7, '精神科——4号床', '空闲', NULL), (32, 7, '精神科——5号床', '空闲', NULL), (33, 7, '精神科——6号床', '空闲', NULL), (34, 7, '精神科——7号床', '空闲', NULL), (35, 7, '精神科——8号床', '空闲', NULL), (36, 7, '精神科——9号床', '空闲', NULL), (37, 7, '精神科——10号床', '空闲', NULL), (38, 7, '精神科——11号床', '空闲', NULL), (39, 7, '精神科——12号床', '空闲', NULL), (40, 7, '精神科——13号床', '空闲', NULL), (41, 7, '精神科——14号床', '空闲', NULL), (42, 7, '精神科——15号床', '空闲', NULL), (43, 7, '精神科——16号床', '空闲', NULL), (44, 7, '精神科——17号床', '空闲', NULL), (45, 7, '精神科——18号床', '空闲', NULL), (46, 7, '精神科——19号床', '空闲', NULL), (47, 7, '精神科——20号床', '空闲', NULL), (48, 8, '精神2——1号床', '空闲', NULL), (49, 8, '精神2——2号床', '已占用', 12), (50, 8, '精神2——3号床', '空闲', NULL), (51, 8, '精神2——4号床', '空闲', NULL), (52, 8, '精神2——5号床', '空闲', NULL), (53, 8, '精神2——6号床', '空闲', NULL), (54, 8, '精神2——7号床', '空闲', NULL), (55, 8, '精神2——8号床', '空闲', NULL), (56, 8, '精神2——9号床', '空闲', NULL), (57, 8, '精神2——10号床', '空闲', NULL);
COMMIT;

-- ----------------------------
-- Table structure for bill
-- ----------------------------
DROP TABLE IF EXISTS `bill`;
CREATE TABLE `bill`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `recordId` int(11) NULL DEFAULT NULL,
  `time` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `cost` double(10, 2) NULL DEFAULT NULL,
  `type` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `status` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `recordId`(`recordId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 32 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci;

-- ----------------------------
-- Records of bill
-- ----------------------------
BEGIN;
INSERT INTO `bill` VALUES (8, 6, '2020-04-12 23:29:07', 20.00, '医疗费', '已支付'), (9, 6, '2020-04-12 23:29:07', 20.00, '床位费', '已支付'), (12, 7, '2020-04-13 15:30:37', 50.00, '医疗费', '待支付'), (13, 7, '2020-04-13 15:30:37', 50.00, '床位费', '待支付'), (15, 8, '2020-04-13 16:02:24', 20.00, '医疗费', '已支付'), (16, 8, '2020-04-13 16:02:24', 20.00, '床位费', '已支付'), (17, 7, '2020-04-13 16:20:18', 25.00, '押金', '待支付'), (18, 6, '2020-04-13 16:20:43', 30.00, '押金', '已支付'), (19, 9, '2020-04-13 20:11:21', 100.00, '押金', '已支付'), (20, 9, '2020-04-13 20:11:21', 88.00, '医疗费', '已支付'), (21, 9, '2020-04-13 20:11:21', 88.00, '床位费', '已支付'), (22, 10, '2020-04-13 20:19:06', 100.00, '押金', '已支付'), (23, 10, '2020-04-13 20:19:06', 20.00, '医疗费', '已支付'), (24, 10, '2020-04-13 20:19:06', 20.00, '床位费', '已支付'), (25, 11, '2020-04-13 20:37:49', 100.00, '押金', '已支付'), (26, 11, '2020-04-13 20:37:49', 50.00, '医疗费', '待支付'), (27, 11, '2020-04-13 20:37:49', 50.00, '床位费', '待支付'), (28, 8, '2020-04-13 20:44:37', 15.00, '押金', '待支付'), (29, 12, '2020-04-13 20:51:12', 199.00, '押金', '待支付'), (30, 12, '2020-04-13 20:51:12', 60.00, '医疗费', '待支付'), (31, 12, '2020-04-13 20:51:12', 60.00, '床位费', '待支付');
COMMIT;

-- ----------------------------
-- Table structure for department
-- ----------------------------
DROP TABLE IF EXISTS `department`;
CREATE TABLE `department`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `cost` double(15, 2) NULL DEFAULT NULL,
  `freeBedNum` int(11) NULL DEFAULT NULL,
  `bedNum` int(11) NULL DEFAULT NULL,
  `freeDivTotal` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci;

-- ----------------------------
-- Records of department
-- ----------------------------
BEGIN;
INSERT INTO `department` VALUES (4, '外科', 88.00, 4, 5, NULL), (5, '呼吸科', 20.00, 7, 8, NULL), (6, '男科', 50.00, 2, 3, NULL), (7, '精神科', 60.00, 20, 20, NULL), (8, '精神2', 60.00, 9, 10, NULL);
COMMIT;

-- ----------------------------
-- Table structure for log
-- ----------------------------
DROP TABLE IF EXISTS `log`;
CREATE TABLE `log`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `time` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `content` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 42 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci;

-- ----------------------------
-- Records of log
-- ----------------------------
BEGIN;
INSERT INTO `log` VALUES (5, '2020-04-12 23:29:07', '病历号：6-姓名：蕉爸-办理入院登记'), (6, '2020-04-13 15:08:07', '病历号：6-姓名：蕉爸支付押金账单'), (7, '2020-04-13 15:17:12', '病历号：6-姓名：蕉爸的押金单据已作废'), (8, '2020-04-13 15:29:40', '病历号：6-姓名：蕉爸支付床位费账单'), (9, '2020-04-13 15:30:37', '病历号：7-姓名：亚双义-办理入院登记'), (10, '2020-04-13 15:36:14', '病历号：7-姓名：亚双义支付押金账单'), (11, '2020-04-13 16:02:24', '病历号：8-姓名：test办理入院登记'), (12, '2020-04-13 16:07:37', '病历号：7-姓名：亚双义转科调床-从男科——1号床转至呼吸科——1号床'), (13, '2020-04-13 16:07:55', '病历号：7-姓名：亚双义转科调床-从男科——1号床转至呼吸科——3号床'), (14, '2020-04-13 16:08:16', '病历号：7-姓名：亚双义转科调床-从男科——1号床转至呼吸科——5号床'), (15, '2020-04-13 16:12:08', '病历号：7-姓名：亚双义转科调床-从男科——1号床转至呼吸科——6号床'), (16, '2020-04-13 16:16:23', '病历号：6-姓名：蕉爸支付医疗费账单'), (17, '2020-04-13 16:16:27', '病历号：6-姓名：蕉爸支付押金账单'), (18, '2020-04-13 16:18:43', '病历号：6-姓名：蕉爸转科调床-从呼吸科——4号床转至外科——1号床'), (19, '2020-04-13 16:20:18', '病历号：7-姓名：亚双义的押金单据已作废'), (20, '2020-04-13 16:20:43', '病历号：6-姓名：蕉爸的押金单据已作废'), (21, '2020-04-13 16:20:57', '病历号：6-姓名：蕉爸支付押金账单'), (22, '2020-04-13 17:01:38', '病历号：6-姓名：蕉爸从外科——1号床出院'), (23, '2020-04-13 17:04:37', '病历号：8-姓名：test支付押金账单'), (24, '2020-04-13 17:04:45', '病历号：8-姓名：test支付医疗费账单'), (25, '2020-04-13 17:04:47', '病历号：8-姓名：test支付床位费账单'), (26, '2020-04-13 17:04:53', '病历号：8-姓名：test从呼吸科——2号床出院'), (27, '2020-04-13 20:11:21', '病历号：9-姓名：段海办理入院登记'), (28, '2020-04-13 20:16:59', '病历号：9-姓名：段海支付押金账单'), (29, '2020-04-13 20:17:20', '病历号：9-姓名：段海转科调床-从外科——1号床转至精神科——15号床'), (30, '2020-04-13 20:18:18', '病历号：9-姓名：段海转科调床-从精神科——15号床转至外科——1号床'), (31, '2020-04-13 20:19:06', '病历号：10-姓名：张宇办理入院登记'), (32, '2020-04-13 20:19:25', '病历号：10-姓名：张宇支付医疗费账单'), (33, '2020-04-13 20:19:58', '病历号：10-姓名：张宇支付床位费账单'), (34, '2020-04-13 20:20:13', '病历号：10-姓名：张宇支付押金账单'), (35, '2020-04-13 20:20:39', '病历号：10-姓名：张宇转科调床-从呼吸科——2号床转至外科——2号床'), (36, '2020-04-13 20:20:51', '病历号：9-姓名：段海支付床位费账单'), (37, '2020-04-13 20:20:56', '病历号：9-姓名：段海支付医疗费账单'), (38, '2020-04-13 20:37:49', '病历号：11-姓名：段海1办理入院登记'), (39, '2020-04-13 20:38:26', '病历号：11-姓名：段海1支付押金账单'), (40, '2020-04-13 20:38:40', '病历号：9-姓名：段海从外科——1号床出院'), (41, '2020-04-13 20:44:37', '病历号：8-姓名：test的押金单据已作废'), (42, '2020-04-13 20:51:12', '病历号：12-姓名：段海办理入院登记');
COMMIT;

-- ----------------------------
-- Table structure for record
-- ----------------------------
DROP TABLE IF EXISTS `record`;
CREATE TABLE `record`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `sex` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `age` int(11) NULL DEFAULT NULL,
  `phone` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `departmentId` int(11) NULL DEFAULT NULL,
  `bedId` int(11) NULL DEFAULT NULL,
  `medicalCost` double(15, 2) NULL DEFAULT NULL,
  `deposit` double(15, 2) NULL DEFAULT NULL,
  `status` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `inTime` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `outTime` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `account` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 13 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci;

-- ----------------------------
-- Records of record
-- ----------------------------
BEGIN;
INSERT INTO `record` VALUES (6, '蕉爸', '男', 20, '110', 4, 12, 20.00, 30.00, '已出院', '2020-04-12 23:29:07', '2020-04-13 17:01:38', 'HW9aN', 'HW9aN'), (7, '亚双义', '男', 18, '666', 5, 22, 50.00, 25.00, '待付押金', '2020-04-13 15:30:37', NULL, 'TvtjS', 'TvtjS'), (8, 'test', '女', 13, '222', 6, 25, 20.00, 15.00, '待付押金', '2020-04-13 16:02:24', '2020-04-13 17:04:53', 'pzOxP', '123456'), (9, '段海', '男', 21, '17726640038', 4, 12, 88.00, 100.00, '已出院', '2020-04-13 20:11:21', '2020-04-13 20:38:40', 'I6Fer', '123456'), (10, '张宇', '男', 21, '17726640099', 4, 13, 20.00, 100.00, '入院中(已付款)', '2020-04-13 20:19:06', NULL, 'jhfG4', '123456'), (11, '段海1', '男', 21, '17726640088', 6, 26, 50.00, 100.00, '入院中(未付款)', '2020-04-13 20:37:49', NULL, 'Ua6T3', '123456'), (12, '段海', '男', 21, '17724426743', 8, 49, 60.00, 199.00, '待付押金', '2020-04-13 20:51:12', NULL, '6QVmn', '123456');
COMMIT;

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `power` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `recordId` int(11) NULL DEFAULT NULL,
  `message` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 19 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci;

-- ----------------------------
-- Records of user
-- ----------------------------
BEGIN;
INSERT INTO `user` VALUES (1, 'admin', '123456', '帅哥', '超级管理员', 0, NULL), (3, 'test', '123456', '小管理员', '管理员', 0, NULL), (10, 'HW9aN', 'HW9aN', '蕉爸-家属', '家属', 6, NULL), (11, 'TvtjS', 'TvtjS', '亚双义-家属', '家属', 7, NULL), (12, 'pzOxP', '123456', 'test-家属', '家属', 8, NULL), (14, 'I6Fer', '123456', '段海-家属', '家属', 9, NULL), (15, 'jhfG4', '123456', '张宇-家属', '家属', 10, NULL), (16, 'wang', '123456', '王医生', '管理员', 0, NULL), (17, 'Ua6T3', '123456', '段海1-家属', '家属', 11, NULL), (18, '6QVmn', '123456', '段海-家属', '家属', 12, NULL);
COMMIT;

SET FOREIGN_KEY_CHECKS = 1;
