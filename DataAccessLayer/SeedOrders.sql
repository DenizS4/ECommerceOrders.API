INSERT INTO `Orders` (
    OrderID, BuyerID, Currency,
    Subtotal, DiscountTotal, ShippingTotal, TaxTotal, GrandTotal,
    OrderStatus, PaymentStatus, FulfillmentStatus,
    BillingAddressJson, ShippingAddressJson, BuyerSnapshotJson,
    PlacedAt, PaidAt, FulfilledAt, CreatedAt, UpdatedAt, IsDeleted
) VALUES
      (
          '6f1c2f92-4b7f-4fa2-a347-8b3b7c71f021',
          '4d99e1bb-cc92-4b8f-91f3-9d1ad0b0a278',
          'USD',
          250.00, 20.00, 15.00, 22.50, 267.50,
          'Placed', 'Paid', 'Unfulfilled',
          JSON_OBJECT('name','John Doe','phone','+1-555-1234','city','New York','zip','10001','country','US'),
          JSON_OBJECT('name','John Doe','phone','+1-555-1234','city','New York','zip','10001','country','US'),
          JSON_OBJECT('email','john@example.com','name','John Doe'),
          NOW() - INTERVAL 2 DAY, NOW() - INTERVAL 2 DAY, NULL, NOW() - INTERVAL 2 DAY, NOW() - INTERVAL 1 DAY, 0
      ),
      (
          '5d2a7b30-91e9-4bbf-b6a5-6f4042a6df51',
          '9c3443b8-d19e-45f5-8ed7-fb56320f40b1',
          'EUR',
          120.00, 0.00, 10.00, 26.00, 156.00,
          'Confirmed', 'Authorized', 'Unfulfilled',
          JSON_OBJECT('name','Jane Smith','phone','+49-30-9999','city','Berlin','zip','10115','country','DE'),
          JSON_OBJECT('name','Jane Smith','phone','+49-30-9999','city','Berlin','zip','10115','country','DE'),
          JSON_OBJECT('email','jane@example.de','name','Jane Smith'),
          NOW() - INTERVAL 5 DAY, NULL, NULL, NOW() - INTERVAL 5 DAY, NOW() - INTERVAL 3 DAY, 0
      ),
      (
          'b81a1c6f-52f9-4ed3-9c32-44a6f1c63db8',
          '2f6d9c1e-991e-48db-85a7-b46b5a084e93',
          'TRY',
          5000.00, 250.00, 50.00, 950.00, 5750.00,
          'Closed', 'Refunded', 'Returned',
          JSON_OBJECT('name','Ali Yılmaz','phone','+90-212-444444','city','Istanbul','zip','34000','country','TR'),
          JSON_OBJECT('name','Ali Yılmaz','phone','+90-212-444444','city','Istanbul','zip','34000','country','TR'),
          JSON_OBJECT('email','ali@example.com','name','Ali Yılmaz'),
          NOW() - INTERVAL 20 DAY, NOW() - INTERVAL 19 DAY, NOW() - INTERVAL 15 DAY, NOW() - INTERVAL 20 DAY, NOW() - INTERVAL 1 DAY, 0
      );
INSERT INTO `OrderItems` (
    OrderItemID, OrderID, ProductRefID,
    Sku, ProductName, Category,
    UnitPrice, Quantity, LineDiscount, LineTax, LineTotal, CreatedAt
) VALUES
-- Order 1 (John Doe - USD)
(
    '1f6c7f22-3b6f-4f53-8a8a-ec5cddc88f10',
    '6f1c2f92-4b7f-4fa2-a347-8b3b7c71f021',
    'PROD-1001',
    'SKU-RED-TSHIRT-M',
    'Red T-Shirt (M)',
    'Apparel',
    50.00, 2, 5.00, 4.50, 99.50,
    NOW() - INTERVAL 2 DAY
),
(
    '2a4c93d7-739f-4b2d-95bb-2e7fef3b5f42',
    '6f1c2f92-4b7f-4fa2-a347-8b3b7c71f021',
    'PROD-1002',
    'SKU-BLK-JEANS-32',
    'Black Jeans (32)',
    'Apparel',
    80.00, 1, 0.00, 7.20, 87.20,
    NOW() - INTERVAL 2 DAY
),

-- Order 2 (Jane Smith - EUR)
(
    '3c8d54fa-2f41-41df-a2b9-1ecf446d9e89',
    '5d2a7b30-91e9-4bbf-b6a5-6f4042a6df51',
    'PROD-2001',
    'SKU-BLU-SHOES-42',
    'Blue Running Shoes (42)',
    'Footwear',
    60.00, 2, 0.00, 21.60, 141.60,
    NOW() - INTERVAL 5 DAY
),
(
    '4ed0b771-631c-4a77-a189-0df19c4b06de',
    '5d2a7b30-91e9-4bbf-b6a5-6f4042a6df51',
    'PROD-2002',
    'SKU-GRY-HOODIE-L',
    'Grey Hoodie (L)',
    'Apparel',
    45.00, 1, 5.00, 7.60, 47.60,
    NOW() - INTERVAL 5 DAY
),

-- Order 3 (Ali Yılmaz - TRY)
(
    '5f2a1cd3-14ff-47e7-8c0a-3a7d53aa4032',
    'b81a1c6f-52f9-4ed3-9c32-44a6f1c63db8',
    'PROD-3001',
    'SKU-GAMING-LAPTOP-15',
    'Gaming Laptop 15"',
    'Electronics',
    5000.00, 1, 250.00, 950.00, 5700.00,
    NOW() - INTERVAL 20 DAY
),
(
    '6a29ef2b-9f5c-4f64-8b1f-1db3026f5b4a',
    'b81a1c6f-52f9-4ed3-9c32-44a6f1c63db8',
    'PROD-3002',
    'SKU-GAMING-MOUSE',
    'Quantum Gaming Mouse',
    'Electronics',
    50.00, 2, 0.00, 18.00, 118.00,
    NOW() - INTERVAL 20 DAY
);
