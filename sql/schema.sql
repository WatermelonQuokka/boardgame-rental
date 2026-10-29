DROP TABLE IF EXISTS rentals;
DROP TABLE IF EXISTS members;
DROP TABLE IF EXISTS boardgames;

CREATE TABLE boardgames
(
    id    int unsigned AUTO_INCREMENT
        PRIMARY KEY,
    name  varchar(64)     NOT NULL,
    stock int unsigned    NOT NULL,
    is_retired boolean    NOT NULL DEFAULT FALSE,
    CONSTRAINT boardgames_name_unique
        UNIQUE (name)
);

CREATE TABLE members
(
    id             int unsigned AUTO_INCREMENT
        PRIMARY KEY,
    name           varchar(64)  NOT NULL,
    street_address varchar(64)  NOT NULL,
    zip_code       varchar(10)  NOT NULL,
    city           varchar(32)  NOT NULL,
    phone_nr       varchar(20)  NOT NULL,
    email          varchar(255) NULL
);

CREATE TABLE rentals
(
    id           int unsigned AUTO_INCREMENT
        PRIMARY KEY,
    game_id      int unsigned NOT NULL,
    member_id    int unsigned NOT NULL,
    rental_start date         NOT NULL,
    rental_end   date         NOT NULL,
    CONSTRAINT rentals_boardgames_id_fk
        FOREIGN KEY (game_id) REFERENCES boardgames (id),
    CONSTRAINT rentals_members_id_fk
        FOREIGN KEY (member_id) REFERENCES members (id),
    CONSTRAINT rentals_end_not_before_start
        CHECK (`rental_end` >= `rental_start`)
);