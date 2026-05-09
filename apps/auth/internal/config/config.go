package config

import (
	"fmt"
	"os"
	"strconv"
)

type Config struct {
	AppEnv string

	ServerPort string

	DBHost     string
	DBPort     string
	DBName     string
	DBUser     string
	DBPassword string
	DBSSLMode  string

	JWTSecret        string
	JWTIssuer        string
	JWTAudience      string
	JWTAccessMinutes int
}

func Load() Config {
	return Config{
		AppEnv: getEnv("APP_ENV", "development"),

		ServerPort: getEnv("SERVER_PORT", "8081"),

		DBHost:     getEnv("DB_HOST", "localhost"),
		DBPort:     getEnv("DB_PORT", "5432"),
		DBName:     getEnv("DB_NAME", "money_flow_system"),
		DBUser:     getEnv("DB_USER", "postgres"),
		DBPassword: getEnv("DB_PASSWORD", "postgres"),
		DBSSLMode:  getEnv("DB_SSL_MODE", "disable"),

		JWTSecret:        getEnv("JWT_SECRET", "change_this_super_secret_key_min_32_chars"),
		JWTIssuer:        getEnv("JWT_ISSUER", "money-flow-auth"),
		JWTAudience:      getEnv("JWT_AUDIENCE", "money-flow-app"),
		JWTAccessMinutes: getEnvAsInt("JWT_ACCESS_MINUTES", 15),
	}
}

func (c Config) DatabaseURL() string {
	return fmt.Sprintf(
		"postgres://%s:%s@%s:%s/%s?sslmode=%s",
		c.DBUser,
		c.DBPassword,
		c.DBHost,
		c.DBPort,
		c.DBName,
		c.DBSSLMode,
	)
}

func getEnv(key string, fallback string) string {
	value := os.Getenv(key)
	if value == "" {
		return fallback
	}
	return value
}

func getEnvAsInt(key string, fallback int) int {
	value := os.Getenv(key)
	if value == "" {
		return fallback
	}

	result, err := strconv.Atoi(value)
	if err != nil {
		return fallback
	}

	return result
}