package main

import (
	"log"
	"net/http"

	authModule "github.com/25101997/digital-wallet/apps/auth/internal/auth"
	"github.com/25101997/digital-wallet/apps/auth/internal/config"
	"github.com/25101997/digital-wallet/apps/auth/internal/database"
	"github.com/25101997/digital-wallet/apps/auth/internal/server"
)

func main() {
	cfg := config.Load()

	db, err := database.NewPostgresPool(cfg.DatabaseURL())
	if err != nil {
		log.Fatalf("error connecting to database: %v", err)
	}
	defer db.Close()

	authRepository := authModule.NewRepository(db)

	jwtService := authModule.NewJWTService(
		cfg.JWTSecret,
		cfg.JWTIssuer,
		cfg.JWTAudience,
		cfg.JWTAccessMinutes,
	)

	authService := authModule.NewService(authRepository, jwtService)
	authHandler := authModule.NewHandler(authService)

	router := server.NewRouter(authHandler)

	addr := ":" + cfg.ServerPort

	log.Printf("auth-service running on port %s", cfg.ServerPort)

	if err := http.ListenAndServe(addr, router); err != nil {
		log.Fatalf("server error: %v", err)
	}
}