# si apps/api$ aun no esta iniciado un proyecto .net

docker run -it --rm -v "$PWD":/api -u $(id -u):$(id -g) -w /api -e DOTNET_CLI_HOME=/api -p 8080:8080 mcr.microsoft.com/dotnet/sdk:8.0 sh

dotnet new webapi --output .

# si apps/web$ aun no esta iniciado un proyecto angular

docker run -it --rm -v "$PWD":/web -u $(id -u):$(id -g) -w /web -p 4200:4200 node:18-alpine sh

npx @angular/cli@16.2.0 new web --routing --style=css --directory .

# -------
cd deploy/docker
cp .env.example .env

# comando para pruebas de produccion en desarrollo
docker compose --env-file .env -f docker-compose.prod.yml down -v

# si se modifico algo en api web deploy archivos que afecten la imagen.
docker compose --env-file .env -f docker-compose.prod.yml up -d --build

# si solo se quiere levantar 
docker compose --env-file ../../.env -f docker-compose.prod.yml up -d

docker compose --env-file .env -f docker-compose.prod.yml ps
docker compose --env-file .env -f docker-compose.prod.yml logs -f

# comando para pruebas de produccion real en servidores publicos
docker compose --env-file .env -f docker-compose.prod.yml down

# comandos para desarrollo 
docker compose -f docker-compose.dev.yml down

# si se quiere ver logs 
docker compose -f docker-compose.dev.yml up --build

docker compose -f docker-compose.dev.yml up -d
docker compose -f docker-compose.dev.yml up

# si se quiere ocultar logs 
docker compose --env-file ../../.env -f docker-compose.dev.yml up -d --build
docker compose --env-file ../../.env -f docker-compose.dev.yml up -d

# solo para levantar sin contruir y ver logs en el momento.
docker compose --env-file ../../.env -f docker-compose.dev.yml up

mkdir -p deploy/nginx/ssl

openssl req -x509 -nodes -days 365 -newkey rsa:2048   -keyout deploy/nginx/ssl/key.pem   -out deploy/nginx/ssl/cert.pem


# Restaurar bdd de produccion en desarrollo
  # Paso 1: crear el backup
    docker ps
    ver nombre del contenedor de bdd

    docker exec -t digital-wallet-db \
    pg_dump \
    --no-owner \
    --no-privileges \
    -U usuario \
    -d digital-wallet-db \
    > digital-wallet-prod.sql

  # Paso 2: desde el servidor de desarrollo
    dlopez@dlopez:~/Repos/digital-wallet/deploy/docker$ docker compose --env-file ../../.env -f docker-compose.dev.yml down

    docker volume rm digital-wallet-postgres-data

    docker volume ls | grep digital-wallet

    dlopez@dlopez:~/Repos/digital-wallet/deploy/docker$ docker compose --env-file ../../.env -f docker-compose.dev.yml up

    dlopez@dlopez:~/Repos/digital-wallet/deploy/docker$ cat digital-wallet-prod.sql | docker exec -i digital-wallet-db   psql -U app_user -d digital-wallet-db


