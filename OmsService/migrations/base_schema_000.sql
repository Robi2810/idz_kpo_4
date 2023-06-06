create table dish (
    id serial not null unique primary key,
    name text,
    description text,
    price decimal(10,2),
    quantity integer
);

alter table dish
    owner to "oms-service";

create unique index if not exists dish_id_uindex
    on dish (id);

create table "order" (
    id serial not null unique primary key,
    user_id integer,
    status text,
    special_requests text,
    created_at timestamp default now(),
    updated_at timestamp default now()
);

alter table "order"
    owner to "oms-service";

create unique index if not exists order_id_uindex
    on "order" (id);

create table order_dish (
    id serial not null unique primary key,
    order_id integer references "order" (id),
    dish_id integer references dish (id),
    quantity integer,
    price decimal(10,2)
);

alter table order_dish
    owner to "oms-service";

create unique index if not exists order_dish_id_uindex
    on order_dish (id);