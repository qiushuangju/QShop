-- 创建秒杀活动表
CREATE TABLE `tb_SeckillActivity` (
  `Id` varchar(50) NOT NULL COMMENT '活动ID',
  `ActivityName` varchar(200) NOT NULL COMMENT '活动名称',
  `ActivityDesc` text COMMENT '活动描述',
  `StartTime` datetime NOT NULL COMMENT '开始时间',
  `EndTime` datetime NOT NULL COMMENT '结束时间',
  `LimitPerUser` int NOT NULL DEFAULT '1' COMMENT '用户限购数量',
  `MaxRequestPerSecond` int NOT NULL DEFAULT '100' COMMENT '每秒最大请求数',
  `Status` int NOT NULL DEFAULT '10' COMMENT '活动状态：10-待开始，20-进行中，30-已结束，40-已取消',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `UpdateTime` datetime NOT NULL COMMENT '更新时间',
  `Creator` varchar(50) NOT NULL COMMENT '创建人',
  PRIMARY KEY (`Id`),
  KEY `IX_Status` (`Status`),
  KEY `IX_StartTime` (`StartTime`),
  KEY `IX_EndTime` (`EndTime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='秒杀活动表';

-- 创建秒杀商品关联表
CREATE TABLE `tb_SeckillGoods` (
  `Id` varchar(50) NOT NULL COMMENT '主键ID',
  `ActivityId` varchar(50) NOT NULL COMMENT '活动ID',
  `GoodsId` varchar(50) NOT NULL COMMENT '商品ID',
  `SkuId` varchar(50) NOT NULL COMMENT 'SKU ID',
  `SeckillPrice` decimal(18,2) NOT NULL COMMENT '秒杀价格',
  `SeckillStock` int NOT NULL COMMENT '秒杀库存',
  `SoldCount` int NOT NULL DEFAULT '0' COMMENT '已售数量',
  `SortNo` int NOT NULL DEFAULT '0' COMMENT '排序编号',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`Id`),
  KEY `IX_ActivityId` (`ActivityId`),
  KEY `IX_GoodsId` (`GoodsId`),
  KEY `IX_SkuId` (`SkuId`),
  CONSTRAINT `FK_SeckillGoods_Activity` FOREIGN KEY (`ActivityId`) REFERENCES `tb_SeckillActivity` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='秒杀商品关联表';

-- 创建秒杀用户抢购记录表
CREATE TABLE `tb_SeckillUserRecord` (
  `Id` varchar(50) NOT NULL COMMENT '主键ID',
  `ActivityId` varchar(50) NOT NULL COMMENT '活动ID',
  `GoodsId` varchar(50) NOT NULL COMMENT '商品ID',
  `SkuId` varchar(50) NOT NULL COMMENT 'SKU ID',
  `UserId` varchar(50) NOT NULL COMMENT '用户ID',
  `BuyCount` int NOT NULL COMMENT '购买数量',
  `OrderId` varchar(50) COMMENT '订单ID',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`Id`),
  KEY `IX_ActivityId` (`ActivityId`),
  KEY `IX_UserId` (`UserId`),
  KEY `IX_GoodsId` (`GoodsId`),
  CONSTRAINT `UK_User_Activity_Goods` UNIQUE KEY (`ActivityId`, `UserId`, `GoodsId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='秒杀用户抢购记录表';