create table boardgames
(
    id    int unsigned auto_increment
        primary key,
    name  varchar(64)  not null,
    stock int unsigned not null,
    constraint boardgames_name_unique
        unique (name)
);

create table members
(
    id       int unsigned auto_increment
        primary key,
    name     varchar(64)  not null,
    address  varchar(64)  not null,
    phone_nr varchar(20)  not null,
    email    varchar(255) null
);

create table rentals
(
    id           int unsigned auto_increment
        primary key,
    game_id      int unsigned not null,
    member_id    int unsigned not null,
    rental_start date         not null,
    rental_end   date         not null,
    constraint rentals_boardgames_id_fk
        foreign key (game_id) references boardgames (id),
    constraint rentals_members_id_fk
        foreign key (member_id) references members (id),
    constraint rentals_end_not_before_start
        check (`rental_end` >= `rental_start`)
);