package auth

import (
	"net/mail"
	"strings"
	"unicode"
)

func validateRegisterRequest(req RegisterRequest) error {
	if strings.TrimSpace(req.Username) == "" {
		return ErrUsernameRequired
	}

	if strings.TrimSpace(req.Email) == "" {
		return ErrEmailRequired
	}

	if _, err := mail.ParseAddress(req.Email); err != nil {
		return ErrEmailRequired
	}

	if strings.TrimSpace(req.Password) == "" {
		return ErrPasswordRequired
	}

	if !isStrongPassword(req.Password) {
		return ErrWeakPassword
	}

	return nil
}

func validateLoginRequest(req LoginRequest) error {
	if strings.TrimSpace(req.UsernameOrEmail) == "" {
		return ErrUsernameOrEmailNeeded
	}

	if strings.TrimSpace(req.Password) == "" {
		return ErrPasswordRequired
	}

	return nil
}

func isStrongPassword(password string) bool {
	if len(password) < 8 {
		return false
	}

	var hasUpper bool
	var hasLower bool
	var hasNumber bool

	for _, char := range password {
		switch {
		case unicode.IsUpper(char):
			hasUpper = true
		case unicode.IsLower(char):
			hasLower = true
		case unicode.IsNumber(char):
			hasNumber = true
		}
	}

	return hasUpper && hasLower && hasNumber
}