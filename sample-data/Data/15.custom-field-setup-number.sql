-- Story Points has no decimals and rounds, Budget and Transfer Fee have a currency and 2 decimals (Budget also has a default value)
INSERT INTO CustomFieldSetup_Number
    (Id, CurrencyCode, Decimals, Rounding, DefaultNumber)
VALUES
    ('70000003-0000-0000-0000-000000000000', null, 0, 1, null),
    ('70000004-0000-0000-0000-000000000000', 'USD', 2, 0, 1000.00),
    ('70000011-0000-0000-0000-000000000000', 'EUR', 2, 0, null);
