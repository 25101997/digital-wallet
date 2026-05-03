docker exec -it -u 0 database-container /bin/bash

psql -h localhost -p 5432 -U app_user -d digital-wallet-db

digital-wallet-db=# SHOW search_path;
digital-wallet-db=# SELECT current_schema();
digital-wallet-db=# CREATE SCHEMA IF NOT EXISTS dw;
digital-wallet-db=# ALTER ROLE app_user SET search_path TO dw, public;
digital-wallet-db=# \i database/schema/004_inserts/001_insert.sql 