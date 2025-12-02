-- 秒杀活动管理系统数据库脚本
-- 创建时间: 2024-01-01
-- 适用数据库: MySQL

-- ================================================
-- 秒杀活动表
-- ================================================
CREATE TABLE `seckill_activity` (
  `id` varchar(64) NOT NULL COMMENT '活动ID',
  `name` varchar(200) NOT NULL COMMENT '活动名称',
  `description` text COMMENT '活动描述',
  `start_time` datetime NOT NULL COMMENT '开始时间',
  `end_time` datetime NOT NULL COMMENT '结束时间',
  `status` tinyint NOT NULL DEFAULT 1 COMMENT '状态：1-待开始 2-进行中 3-已结束 4-已取消',
  `limit_per_user` int NOT NULL DEFAULT 1 COMMENT '每人限购数量',
  `is_valid` tinyint NOT NULL DEFAULT 1 COMMENT '是否有效',
  `create_user_id` varchar(64) NOT NULL COMMENT '创建人ID',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_user_id` varchar(64) NOT NULL COMMENT '更新人ID',
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`id`),
  KEY `idx_status` (`status`),
  KEY `idx_start_time` (`start_time`),
  KEY `idx_end_time` (`end_time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='秒杀活动表';

-- ================================================
-- 秒杀商品表
-- ================================================
CREATE TABLE `seckill_goods` (
  `id` varchar(64) NOT NULL COMMENT '商品ID',
  `activity_id` varchar(64) NOT NULL COMMENT '活动ID',
  `goods_id` varchar(64) NOT NULL COMMENT '商品ID',
  `sku_id` varchar(64) NOT NULL COMMENT 'SKU ID',
  `seckill_price` decimal(18,2) NOT NULL COMMENT '秒杀价格',
  `seckill_stock` int NOT NULL COMMENT '秒杀库存',
  `sold_quantity` int NOT NULL DEFAULT 0 COMMENT '已售数量',
  `display_order` int NOT NULL DEFAULT 0 COMMENT '显示顺序',
  `is_valid` tinyint NOT NULL DEFAULT 1 COMMENT '是否有效',
  `create_user_id` varchar(64) NOT NULL COMMENT '创建人ID',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_user_id` varchar(64) NOT NULL COMMENT '更新人ID',
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`id`),
  KEY `idx_activity_id` (`activity_id`),
  KEY `idx_goods_id` (`goods_id`),
  KEY `idx_sku_id` (`sku_id`),
  KEY `idx_is_valid` (`is_valid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='秒杀商品表';

-- ================================================
-- 秒杀订单表
-- ================================================
CREATE TABLE `seckill_order` (
  `id` varchar(64) NOT NULL COMMENT '订单ID',
  `order_no` varchar(50) NOT NULL COMMENT '订单号',
  `activity_id` varchar(64) NOT NULL COMMENT '活动ID',
  `goods_id` varchar(64) NOT NULL COMMENT '商品ID',
  `sku_id` varchar(64) NOT NULL COMMENT 'SKU ID',
  `user_id` varchar(64) NOT NULL COMMENT '用户ID',
  `quantity` int NOT NULL COMMENT '购买数量',
  `seckill_price` decimal(18,2) NOT NULL COMMENT '单价',
  `total_amount` decimal(18,2) NOT NULL COMMENT '总价',
  `status` tinyint NOT NULL DEFAULT 1 COMMENT '状态：1-待支付 2-已支付 3-已发货 4-已完成 5-已取消 6-已退款',
  `pay_time` datetime DEFAULT NULL COMMENT '支付时间',
  `pay_no` varchar(100) DEFAULT NULL COMMENT '支付单号',
  `ship_time` datetime DEFAULT NULL COMMENT '发货时间',
  `complete_time` datetime DEFAULT NULL COMMENT '完成时间',
  `cancel_time` datetime DEFAULT NULL COMMENT '取消时间',
  `refund_time` datetime DEFAULT NULL COMMENT '退款时间',
  `refund_amount` decimal(18,2) DEFAULT NULL COMMENT '退款金额',
  `refund_reason` varchar(500) DEFAULT NULL COMMENT '退款原因',
  `remark` varchar(500) DEFAULT NULL COMMENT '备注',
  `create_user_id` varchar(64) NOT NULL COMMENT '创建人ID',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_user_id` varchar(64) NOT NULL COMMENT '更新人ID',
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_order_no` (`order_no`),
  KEY `idx_user_id` (`user_id`),
  KEY `idx_activity_id` (`activity_id`),
  KEY `idx_goods_id` (`goods_id`),
  KEY `idx_status` (`status`),
  KEY `idx_create_time` (`create_time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='秒杀订单表';

-- ================================================
-- 秒杀商品预热表
-- ================================================
CREATE TABLE `seckill_goods_preheat` (
  `id` varchar(64) NOT NULL COMMENT 'ID',
  `activity_id` varchar(64) NOT NULL COMMENT '活动ID',
  `goods_id` varchar(64) NOT NULL COMMENT '商品ID',
  `sku_id` varchar(64) NOT NULL COMMENT 'SKU ID',
  `preheat_start_time` datetime NOT NULL COMMENT '预热开始时间',
  `preheat_end_time` datetime NOT NULL COMMENT '预热结束时间',
  `is_preheating` tinyint NOT NULL DEFAULT 0 COMMENT '是否预热中',
  `create_user_id` varchar(64) NOT NULL COMMENT '创建人ID',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_user_id` varchar(64) NOT NULL COMMENT '更新人ID',
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`id`),
  KEY `idx_activity_id` (`activity_id`),
  KEY `idx_goods_id` (`goods_id`),
  KEY `idx_is_preheating` (`is_preheating`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='秒杀商品预热表';

-- ================================================
-- 用户购买限制表
-- ================================================
CREATE TABLE `seckill_user_limit` (
  `id` varchar(64) NOT NULL COMMENT 'ID',
  `activity_id` varchar(64) NOT NULL COMMENT '活动ID',
  `goods_id` varchar(64) NOT NULL COMMENT '商品ID',
  `user_id` varchar(64) NOT NULL COMMENT '用户ID',
  `purchase_count` int NOT NULL DEFAULT 0 COMMENT '已购买数量',
  `last_purchase_time` datetime DEFAULT NULL COMMENT '最后购买时间',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_activity_goods_user` (`activity_id`, `goods_id`, `user_id`),
  KEY `idx_user_id` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户购买限制表';

-- ================================================
-- 秒杀请求限流表
-- ================================================
CREATE TABLE `seckill_request_limit` (
  `id` varchar(64) NOT NULL COMMENT 'ID',
  `user_id` varchar(64) NOT NULL COMMENT '用户ID',
  `activity_id` varchar(64) NOT NULL COMMENT '活动ID',
  `request_count` int NOT NULL DEFAULT 0 COMMENT '请求次数',
  `last_request_time` datetime NOT NULL COMMENT '最后请求时间',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`id`),
  KEY `idx_user_id` (`user_id`),
  KEY `idx_activity_id` (`activity_id`),
  KEY `idx_last_request_time` (`last_request_time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='秒杀请求限流表';

-- ================================================
-- 索引优化
-- ================================================

-- 秒杀商品表联合索引
CREATE INDEX idx_activity_goods_sku ON `seckill_goods` (`activity_id`, `goods_id`, `sku_id`);

-- 秒杀订单表联合索引
CREATE INDEX idx_user_activity_goods ON `seckill_order` (`user_id`, `activity_id`, `goods_id`);

-- 秒杀活动状态和时间联合索引
CREATE INDEX idx_status_time ON `seckill_activity` (`status`, `start_time`, `end_time`);

-- ================================================
-- 数据初始化
-- ================================================

-- 插入示例秒杀活动
INSERT INTO `seckill_activity` (`id`, `name`, `description`, `start_time`, `end_time`, `status`, `limit_per_user`, `is_valid`, `create_user_id`, `create_time`, `update_user_id`, `update_time`) VALUES 
('1', '元旦限时秒杀活动', '2024年元旦限时秒杀，多款商品5折起', '2024-01-01 10:00:00', '2024-01-01 22:00:00', 2, 2, 1, 'admin', NOW(), 'admin', NOW()),
('2', '春节前特惠秒杀', '春节前最后一波特惠，全场低至3折', '2024-02-01 09:00:00', '2024-02-10 23:59:59', 1, 1, 1, 'admin', NOW(), 'admin', NOW());

-- 插入示例用户购买限制
INSERT INTO `seckill_user_limit` (`id`, `activity_id`, `goods_id`, `user_id`, `purchase_count`, `last_purchase_time`, `create_time`, `update_time`) VALUES 
('1', '1', '1001', 'user1', 0, NULL, NOW(), NOW()),
('2', '1', '1002', 'user1', 0, NULL, NOW(), NOW()),
('3', '2', '2001', 'user1', 0, NULL, NOW(), NOW());

-- ================================================
-- 注释说明
-- ================================================
-- 秒杀活动表（seckill_activity）：存储秒杀活动的基本信息
-- 秒杀商品表（seckill_goods）：存储参与秒杀的商品信息
-- 秒杀订单表（seckill_order）：存储用户的秒杀订单信息
-- 秒杀商品预热表（seckill_goods_preheat）：存储秒杀商品的预热信息
-- 用户购买限制表（seckill_user_limit）：存储用户购买限制信息
-- 秒杀请求限流表（seckill_request_limit）：存储用户请求限流信息

-- 状态说明：
-- 秒杀活动状态：1-待开始 2-进行中 3-已结束 4-已取消
-- 秒杀订单状态：1-待支付 2-已支付 3-已发货 4-已完成 5-已取消 6-已退款

-- 索引优化说明：
-- 为高频查询字段添加索引，提高查询效率
-- 为联合查询字段添加联合索引，优化多条件查询