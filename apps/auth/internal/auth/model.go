package auth

import "time"

type User struct {
	ID           string
	Username     string
	Email        string
	PasswordHash string
	FullName     string
	IsActive     bool
	CreatedAt    time.Time
	UpdatedAt    time.Time
}