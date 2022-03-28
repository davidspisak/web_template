CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
--CREATE EXTENSION IF NOT EXISTS "unaccent";
update pg_database set encoding = pg_char_to_encoding('UTF8') where datname = 'mba';



