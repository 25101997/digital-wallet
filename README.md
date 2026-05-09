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
docker compose --env-file .env -f docker-compose.prod.yml up -d

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
docker compose --env-file .env -f docker-compose.dev.yml up -d --build
docker compose --env-file .env -f docker-compose.dev.yml up -d

mkdir -p deploy/nginx/ssl

openssl req -x509 -nodes -days 365 -newkey rsa:2048   -keyout deploy/nginx/ssl/key.pem   -out deploy/nginx/ssl/cert.pem


# produccion
docker save digital-wallet-api | gzip > api.tar.gz
docker save digital-wallet-web | gzip > web.tar.gz

scp api.tar.gz usuario@ip_del_servidor:/home/usuario/
scp web.tar.gz usuario@ip_del_servidor:/home/usuario/

gunzip -c api.tar.gz | docker load
gunzip -c web.tar.gz | docker load

docker compose --env-file .env -f docker-compose.prod.yml up -d

