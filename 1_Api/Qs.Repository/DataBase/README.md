# 秒杀活动管理系统

## 目录结构
```
Qs.Repository/
├─ Request/
│  ├─ ReqAuSeckillActivity.cs    # 秒杀活动新增/修改请求
│  ├─ ReqQuSeckill.cs            # 秒杀活动查询请求
│  └─ ReqQuSeckillActivity.cs    # 秒杀活动分页查询请求
├─ Response/
│  └─ ResSeckillActivity.cs      # 秒杀活动响应对象
├─ Interface/
│  └─ ISeckillRepository.cs      # 秒杀仓储接口
├─ DataBase/
│  ├─ sql/
│  │  └─ seckill.sql             # 数据库脚本
│  └─ README.md                  # 秒杀功能说明
└─ Domain/
   ├─ ModelSeckillActivity.cs    # 秒杀活动实体
   ├─ ModelSeckillGoods.cs       # 秒杀商品实体
   ├─ ModelSeckillOrder.cs       # 秒杀订单实体
   └─ Enums/
      └─ SeckillEnums.cs         # 秒杀相关枚举

Qs.App/
├─ AppSeckillActivity.cs         # 秒杀活动管理服务
├─ AppSeckillGoods.cs            # 秒杀商品展示服务
└─ AppSeckillPurchase.cs         # 秒杀抢购流程服务

Qs.Controllers/
└─ Seckill/
   ├─ SeckillActivityController.cs  # 秒杀活动管理API
   ├─ SeckillGoodsController.cs     # 秒杀商品展示API
   └─ SeckillPurchaseController.cs  # 秒杀抢购流程API
```

## 核心功能

### 1. 秒杀活动管理
- **活动创建**：设置活动名称、描述、开始/结束时间、每人限购数量
- **活动状态**：待开始、进行中、已结束、已取消
- **活动商品管理**：为活动添加/删除秒杀商品，设置秒杀价格和库存
- **活动预览**：活动开始前可进行预热
- **状态自动同步**：活动状态根据时间自动更新

### 2. 秒杀商品展示
- **秒杀专区**：分页展示所有秒杀商品
- **首页展示**：精选秒杀商品展示在首页
- **状态筛选**：可按进行中、待开始筛选活动
- **商品卡片信息**：显示倒计时、原价、秒杀价、已抢百分比、库存
- **实时状态更新**：支持商品状态实时更新

### 3. 秒杀抢购流程
- **用户验证**：必须登录才能参与秒杀
- **库存验证**：抢购时验证库存是否充足
- **购买限制**：限制用户购买次数（每人限购）
- **请求限流**：限制用户在规定时间内的请求频率
- **异步处理**：支持异步处理订单，提高响应速度
- **库存扣减**：原子操作，确保数据一致性
- **订单创建**：自动创建秒杀订单

## 数据库设计

### 主要表结构

#### 1. seckill_activity（秒杀活动表）
| 字段 | 类型 | 说明 |
|------|------|------|
| id | varchar(64) | 活动ID（主键） |
| name | varchar(200) | 活动名称 |
| description | text | 活动描述 |
| start_time | datetime | 开始时间 |
| end_time | datetime | 结束时间 |
| status | tinyint | 状态（1-待开始 2-进行中 3-已结束 4-已取消） |
| limit_per_user | int | 每人限购数量 |
| is_valid | tinyint | 是否有效 |
| create_time | datetime | 创建时间 |
| update_time | datetime | 更新时间 |

#### 2. seckill_goods（秒杀商品表）
| 字段 | 类型 | 说明 |
|------|------|------|
| id | varchar(64) | 商品ID（主键） |
| activity_id | varchar(64) | 活动ID |
| goods_id | varchar(64) | 商品ID |
| sku_id | varchar(64) | SKU ID |
| seckill_price | decimal(18,2) | 秒杀价格 |
| seckill_stock | int | 秒杀库存 |
| sold_quantity | int | 已售数量 |
| is_valid | tinyint | 是否有效 |

#### 3. seckill_order（秒杀订单表）
| 字段 | 类型 | 说明 |
|------|------|------|
| id | varchar(64) | 订单ID（主键） |
| order_no | varchar(50) | 订单号（唯一） |
| activity_id | varchar(64) | 活动ID |
| goods_id | varchar(64) | 商品ID |
| sku_id | varchar(64) | SKU ID |
| user_id | varchar(64) | 用户ID |
| quantity | int | 购买数量 |
| seckill_price | decimal(18,2) | 单价 |
| total_amount | decimal(18,2) | 总价 |
| status | tinyint | 状态（1-待支付 2-已支付...） |

## API接口说明

### 1. 秒杀活动管理接口
```
GET    /SeckillActivity/Load               # 分页查询秒杀活动
GET    /SeckillActivity/GetDetail          # 获取活动详情
POST   /SeckillActivity/Add                # 创建秒杀活动
POST   /SeckillActivity/Update             # 更新秒杀活动
POST   /SeckillActivity/Cancel             # 取消秒杀活动
POST   /SeckillActivity/End                # 结束秒杀活动
DELETE /SeckillActivity/Delete             # 删除秒杀活动
GET    /SeckillActivity/GetActivityGoods   # 获取活动商品
```

### 2. 秒杀商品展示接口
```
GET    /SeckillGoods/Load                  # 分页查询秒杀商品
GET    /SeckillGoods/GetHomePageSeckillGoods  # 首页秒杀商品
GET    /SeckillGoods/GetOngoingSeckillGoods   # 进行中的秒杀商品
GET    /SeckillGoods/GetPendingSeckillGoods   # 待开始的秒杀商品
GET    /SeckillGoods/GetDetail             # 获取秒杀商品详情
GET    /SeckillGoods/GetByGoodsId          # 按商品ID查询秒杀活动
GET    /SeckillGoods/UpdateStatus          # 实时更新商品状态
```

### 3. 秒杀抢购流程接口
```
POST   /SeckillPurchase/Purchase           # 秒杀抢购
POST   /SeckillPurchase/PurchaseAsync      # 异步秒杀抢购
GET    /SeckillPurchase/GetUserOrders      # 查询用户秒杀订单
GET    /SeckillPurchase/GetOrderDetail     # 获取订单详情
POST   /SeckillPurchase/CancelOrder       # 取消订单
GET    /SeckillPurchase/GetUserPurchasedCount  # 获取用户已购买数量
POST   /SeckillPurchase/SyncActivityStatus     # 同步活动状态
```

## 技术特点

### 1. 并发控制
- 使用锁机制确保库存扣减的原子性
- 实现用户购买限制，防止超卖
- 请求限流，防止恶意刷请求

### 2. 性能优化
- 异步处理订单，提高接口响应速度
- 数据库索引优化，提高查询效率
- 实时状态更新，支持高并发场景

### 3. 数据安全
- 用户登录验证，防止未授权访问
- 权限控制，确保数据安全
- 事务处理，保证数据一致性

## 使用示例

### 1. 创建秒杀活动
```csharp
var req = new ReqAuSeckillActivity
{
    Name = "限时秒杀活动",
    Description = "限时秒杀，5折起",
    StartTime = DateTime.Now.AddHours(1),
    EndTime = DateTime.Now.AddHours(24),
    LimitPerUser = 2,
    Status = (int)SeckillActivityStatus.Pending,
    SeckillGoods = new List<ReqAuSeckillGoods>
    {
        new ReqAuSeckillGoods
        {
            GoodsId = "1001",
            SkuId = "2001",
            SeckillPrice = 99.99m,
            SeckillStock = 100,
            DisplayOrder = 1
        }
    }
};

var result = await _appSeckillActivity.Add(req);
```

### 2. 秒杀商品抢购
```csharp
var req = new ReqSeckillPurchase
{
    ActivityId = "1",
    GoodsId = "1001",
    SkuId = "2001",
    Quantity = 1
};

var result = await _appSeckillPurchase.PurchaseAsync(req);

if (result.Success)
{
    Console.WriteLine("抢购成功！订单号：" + result.OrderNo);
}
else
{
    Console.WriteLine("抢购失败：" + result.Message);
}
```

## 部署说明

### 1. 数据库初始化
1. 执行 `seckill.sql` 脚本创建数据库表
2. 根据需要插入初始化数据

### 2. 项目配置
1. 在 `appsettings.json` 中配置数据库连接字符串
2. 配置日志记录
3. 配置CORS等中间件

### 3. 启动服务
1. 编译并启动API服务
2. 确保服务正常运行
3. 可以使用API文档工具（如Swagger）测试接口

## 注意事项

1. **数据库性能**：秒杀活动期间会有大量并发请求，建议对数据库进行性能优化，使用读写分离等技术。
2. **缓存策略**：对秒杀商品信息可以使用缓存，减轻数据库压力。
3. **消息队列**：对于高并发场景，可以考虑使用消息队列处理订单，提高系统稳定性。
4. **监控告警**：添加监控和告警机制，及时发现和处理问题。
5. **限流配置**：根据实际情况调整请求限流参数，防止系统被恶意攻击。
6. **事务管理**：确保关键操作的事务完整性，特别是库存扣减和订单创建。

## 扩展功能

1. **秒杀提醒**：为用户添加秒杀提醒功能，活动开始前推送通知。
2. **秒杀排行**：展示秒杀商品的销量排行。
3. **秒杀统计**：添加秒杀活动的统计功能，如参与人数、销售额等。
4. **秒杀分享**：添加秒杀分享功能，用户可以分享活动链接。
5. **秒杀优惠券**：结合优惠券功能，增加秒杀活动的多样性。

## 版本历史

- **v1.0.0**：
  - 实现基础的秒杀活动管理功能
  - 实现秒杀商品展示功能
  - 实现秒杀抢购流程功能
  - 支持异步处理订单
  - 实现库存扣减和购买限制