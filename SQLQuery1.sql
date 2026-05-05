INSERT INTO Notifications (UserId, Title, Message, Type, IsRead, CreatedAt)
VALUES (1, 'Flash Sale!', '20% off all succulents today!', 'Promo', 0, GETDATE())

-- Insert notification para sa TANAN users
INSERT INTO dbo.Notifications (UserId, Title, Message, Type, IsRead, CreatedAt)
SELECT Id, 'Flash Sale!', '20% off all succulents today!', 'Promo', 0, GETDATE()
FROM dbo.Users