-- Roles
INSERT INTO "Roles" ("Name") VALUES
    ('user'),
    ('manager'),
    ('admin'),
    ('director')
ON CONFLICT DO NOTHING;

-- Users
INSERT INTO "Users" ("Name", "Email", "Password", "RoleId") VALUES
    ('user', 'user@test.com', 'useruser', 1),
    ('man', 'manager@test.com', 'manman', 2),
    ('adm',  'adm@test.com', 'admadm', 3),
    ('dir',  'dir@test.com', 'dirdir', 4)
ON CONFLICT DO NOTHING;

-- Directions
INSERT INTO "Directions" ("Name", "IsActive") VALUES
    ('Программирование', true),
    ('Дизайн', true),
    ('Менеджмент', true),
    ('Аналитика данных', true),
    ('Кибербезопасность', true)
ON CONFLICT DO NOTHING;

-- TrainingFormats
INSERT INTO "TrainingFormats" ("Name", "IsActive") VALUES
    ('Онлайн', true),
    ('Очно', true),
    ('Гибрид', true),
    ('Самостоятельно', true)
ON CONFLICT DO NOTHING;

-- Statuses
INSERT INTO "Statuses" ("Name", "IsActive") VALUES
    ('Новая', true),
    ('В работе', true),
    ('Требуется уточнение', true),
    ('Согласована', true),
    ('Завершена', true),
    ('Отклонена', true)
ON CONFLICT DO NOTHING;

-- Requests
INSERT INTO "Requests" ("Topic", "Description", "CreatedAt", "UpdatedAt", "ContactInfo", "DirectionId", "TrainingFormatId", "AuthorId", "StatusId") VALUES
    ('Курс по Python',           'Хочу научиться программировать на Python с нуля', NOW(), NOW(), 'python@test.com',  1, 1, 3, 1),
    ('Основы UX/UI',             'Нужен курс по проектированию интерфейсов',        NOW(), NOW(), 'ux@test.com',      2, 2, 3, 1),
    ('Agile и Scrum',            'Обучение методологиям гибкой разработки',         NOW(), NOW(), 'agile@test.com',   3, 3, 3, 2),
    ('SQL для аналитиков',       'Курс по работе с базами данных',                  NOW(), NOW(), 'sql@test.com',     4, 1, 3, 1),
    ('Основы кибербезопасности', 'Введение в информационную безопасность',          NOW(), NOW(), 'sec@test.com',     5, 4, 3, 2)
ON CONFLICT DO NOTHING;
