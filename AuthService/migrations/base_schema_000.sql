create table users (
    id serial not null primary key,
    username text not null,
    email text not null,
    password_hash text not null,
    role text not null,
    created_at timestamp not null default now(),
    updated_at timestamp not null default now()
);

alter table users
    owner to "auth-service";

create index if not exists users_role_index
    on users (role);

create unique index if not exists users_id_uindex
    on users (id);

