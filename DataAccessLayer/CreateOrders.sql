-- ===========================
-- Orders
-- ===========================
CREATE TABLE `Orders` (
                          `OrderID`              CHAR(36)        NOT NULL,                   -- GUID as string
                          `BuyerID`              CHAR(36)        NOT NULL,                   -- external user id

    -- Money
                          `Currency`             CHAR(3)         NOT NULL,                   -- ISO-4217 (e.g., 'USD','TRY')
                          `Subtotal`             DECIMAL(18,4)   NOT NULL DEFAULT 0.0000,
                          `DiscountTotal`        DECIMAL(18,4)   NOT NULL DEFAULT 0.0000,
                          `ShippingTotal`        DECIMAL(18,4)   NOT NULL DEFAULT 0.0000,
                          `TaxTotal`             DECIMAL(18,4)   NOT NULL DEFAULT 0.0000,
                          `GrandTotal`           DECIMAL(18,4)   NOT NULL DEFAULT 0.0000,

    -- Statuses (use VARCHAR to avoid coupling enum ordinals across services)
                          `OrderStatus`          VARCHAR(32)     NOT NULL,                   -- e.g., 'Draft','Placed','Confirmed','Cancelled','Closed'
                          `PaymentStatus`        VARCHAR(32)     NOT NULL,                   -- e.g., 'Unpaid','Authorized','Paid','Refunded', ...
                          `FulfillmentStatus`    VARCHAR(32)     NOT NULL,                   -- e.g., 'Unfulfilled','Fulfilled','Returned', ...

    -- Snapshots (JSON)
                          `BillingAddressJson`   JSON            NOT NULL,
                          `ShippingAddressJson`  JSON            NOT NULL,
                          `BuyerSnapshotJson`    JSON            NOT NULL,

    -- System log
                          `PlacedAt`             DATETIME(3)     NOT NULL,
                          `PaidAt`               DATETIME(3)     NULL,
                          `FulfilledAt`          DATETIME(3)     NULL,
                          `CreatedAt`            DATETIME(3)     NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
                          `UpdatedAt`            DATETIME(3)     NOT NULL DEFAULT CURRENT_TIMESTAMP(3) ON UPDATE CURRENT_TIMESTAMP(3),
                          `IsDeleted`            TINYINT(1)      NOT NULL DEFAULT 0,

                          PRIMARY KEY (`OrderID`),

    -- Common query patterns
                          KEY `ix_orders_buyer` (`BuyerID`),
                          KEY `ix_orders_status_date` (`OrderStatus`, `PlacedAt`),
                          KEY `ix_orders_payment_status` (`PaymentStatus`),
                          KEY `ix_orders_updated` (`UpdatedAt`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;


-- ===========================
-- OrderItems
-- ===========================
CREATE TABLE `OrderItems` (
                              `OrderItemID`          CHAR(36)        NOT NULL,                   -- GUID
                              `OrderID`              CHAR(36)        NOT NULL,                   -- logical link only (no FK)
                              `ProductRefID`         VARCHAR(64)     NOT NULL,                   -- external product id

    -- Snapshots
                              `Sku`                  VARCHAR(64)     NOT NULL,
                              `ProductName`          VARCHAR(256)    NOT NULL,
                              `Category`             VARCHAR(128)    NULL,

    -- Pricing per line
                              `UnitPrice`            DECIMAL(18,4)   NOT NULL,
                              `Quantity`             DECIMAL(18,4)   NOT NULL,                   -- supports fractional qty
                              `LineDiscount`         DECIMAL(18,4)   NOT NULL DEFAULT 0.0000,
                              `LineTax`              DECIMAL(18,4)   NOT NULL DEFAULT 0.0000,
                              `LineTotal`            DECIMAL(18,4)   NOT NULL,                   -- UnitPrice*Qty - Discount + Tax

                              `CreatedAt`            DATETIME(3)     NOT NULL DEFAULT CURRENT_TIMESTAMP(3),

                              PRIMARY KEY (`OrderItemID`),

    -- Common query patterns
                              KEY `ix_orderitems_order` (`OrderID`),
                              KEY `ix_orderitems_product_ref` (`ProductRefID`),
                              KEY `ix_orderitems_sku` (`Sku`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
