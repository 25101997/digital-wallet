cd apps/auth

docker run -it --rm \
  --name auth-dev \
  -v "$PWD":/auth \
  -w /auth \
  -p 8081:8081 \
  --network digital-wallet-network \
  golang:1.25-alpine \
  sh

docker compose -f deploy/docker/docker-compose.dev.yml up -d db

go mod init github.com/tu-usuario/new-system-name/apps/auth

# 3. Librerías que vamos a usar: Instala estas dependencias:

go get github.com/go-chi/chi/v5
go get github.com/jackc/pgx/v5/pgxpool
go get github.com/golang-jwt/jwt/v5
go get golang.org/x/crypto/bcrypt


/auth # 
APP_ENV=development \
SERVER_PORT=8081 \
DB_HOST=digital-wallet-db \
DB_PORT=5432 \
DB_NAME=digital-wallet-db \
DB_USER=app_user \
DB_PASSWORD=AppPasswordSeguro123 \
DB_SSL_MODE=disable \
JWT_SECRET=change_this_super_secret_key_min_32_chars \
JWT_ISSUER=digital-wallet-auth \
JWT_AUDIENCE=digital-wallet-app \
JWT_ACCESS_MINUTES=15 \
go run ./cmd/auth-service
2026/05/09 20:27:39 auth-service running on port 8081

curl http://localhost:8081/auth/health