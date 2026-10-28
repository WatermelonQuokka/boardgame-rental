INSERT INTO boardgames (name, stock)
VALUES ('Catan', 4),
       ('Ticket to Ride', 4),
       ('Carcassonne', 3),
       ('Pandemic', 3),
       ('Codenames', 3),
       ('Wingspan', 2),
       ('Azul', 2),
       ('Gloomhaven', 1),
       ('Twilight Imperium: Fourth Edition', 1);

INSERT INTO members (name, street_address, zip_code, city, phone_nr, email)
VALUES ('Anna Andersson', 'Annaregatan 25', '253 25', 'Malmö', '+461234567', 'a@example.com'),
       ('Bertil Bertilsson', 'Bertgatan 23', '253 24', 'Malmö', '+462134567', 'b@example.com'),
       ('Carl Carlsson', 'Koppargatan 10', '300 50', 'Lund', '+463124567', NULL);

INSERT INTO rentals (game_id, member_id, rental_start, rental_end)
VALUES (1, 1, '2026-10-24', '2026-10-27'),
       (2, 1, '2026-10-27', '2026-10-30'),
       (5, 2, '2026-10-30', '2026-11-02'),
       (8, 2, '2026-10-28', '2026-10-31'),
       (8, 3, '2026-10-31', '2026-11-03');