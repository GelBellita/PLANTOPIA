-- Enable identity insert
SET IDENTITY_INSERT dbo.Plants ON;

-- Clear existing data first
DELETE FROM dbo.Plants;

-- Insert all plants
INSERT INTO dbo.Plants (Id, Name, Category, Badge, ImageUrl, Price, Description) VALUES
(1,  'Succulent Trio Set', 'Indoor Plants',   'New', '/images/succulent.jpg',    280, 'A beautiful set of three succulents perfect for your desk or shelf.'),
(2,  'Snake Plant',        'Indoor Plants',   'Hot', '/images/snake.jpg',        260, 'Low-maintenance plant that purifies air and thrives in low light.'),
(3,  'Monstera Plant',     'Indoor Plants',   'New', '/images/monstera.jpg',     280, 'Iconic tropical plant with large, glossy split leaves.'),
(11, 'Peace Lily',         'Indoor Plants',   'New', '/images/peace-lily.jpg',   320, 'Elegant white flowers with dark green leaves, great for indoors.'),
(4,  'Santan',             'Outdoor Plants',  'New', '/images/santan.jpg',       280, 'Colorful Filipino flowering shrub perfect for gardens.'),
(5,  'Bougainvillea',      'Outdoor Plants',  'New', '/images/Bougainvillea.jpg',320, 'Vibrant climbing plant with bright papery flowers.'),
(6,  'Ylang-Ylang',        'Outdoor Plants',  'New', '/images/ylang-ylang.jpg',  450, 'Fragrant tropical tree famous for its sweet-scented flowers.'),
(7,  'Gumamela',           'Outdoor Plants',  'Hot', '/images/gumamela.jpeg',    180, 'Classic Filipino hibiscus with large colorful blooms.'),
(8,  'Dwarf Ixora',        'Balcony / Patio', 'New', '/images/dwarf-ixora.jpg',  220, 'Compact flowering shrub ideal for pots and balconies.'),
(9,  'Caladium',           'Balcony / Patio', 'Hot', '/images/caladium.jpg',     260, 'Stunning heart-shaped leaves with vibrant color patterns.'),
(14, 'Sampaguita',         'Balcony / Patio', 'New', '/images/sampaguita.jpg',   175, 'Philippines national flower, known for its sweet fragrance.'),
(10, 'Pandan Plant',       'Balcony / Patio', 'New', '/images/pandan.jpg',       150, 'Aromatic plant used in Filipino cooking and as air freshener.');

-- Disable identity insert after
SET IDENTITY_INSERT dbo.Plants OFF;
