CREATE TABLE public.trending
(
    id_movie integer NOT NULL,
    date date NOT NULL,
    "time" time without time zone NOT NULL,
    "position" integer NOT NULL,
    watchers integer,
    CONSTRAINT id_movie FOREIGN KEY (id_movie)
        REFERENCES public.movies (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE TABLE public.movies
(
    id integer NOT NULL,
    title character varying(255) COLLATE pg_catalog."default",
    year integer,
    overview character varying(1000) COLLATE pg_catalog."default",
    rating real,
    runtime integer,
    genres character varying(255)[] COLLATE pg_catalog."default",
    CONSTRAINT "Movies_pkey" PRIMARY KEY (id)
);